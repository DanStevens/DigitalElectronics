using System;
using System.Linq;
using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.ALUs;
using DigitalElectronics.Utilities;
using DigitalElectronics.ViewModels.Modules;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using BitConverter = DigitalElectronics.Utilities.BitConverter;

namespace DigitalElectronics.Boards.Tests;

public class AluBoardTests
{
    private static readonly BitConverter _bitConverter = new ();
    private static readonly BitArrayComparer _baComparer = new();

    private BitArray CreateExpectedBitArrayArg(BitArray expectedValue)
    {
        return Arg.Is<BitArray>(arg => _baComparer.Compare(arg, expectedValue) == 0);
    }

    [Test]
    public void InitialState()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);
        objUT.RegisterAVM.Should().BeSameAs(mocks.registerAVM);
        objUT.RegisterBVM.Should().BeSameAs(mocks.registerBVM);
        objUT.ALU.Should().BeSameAs(mocks.alu);
    }


    [Test]
    public void Clock_ShouldCallClockOnRegisterAAndRegisterB()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);

        objUT.Clock();

        mocks.registerAVM.Received(1).Clock();
        mocks.registerBVM.Received(1).Clock();
    }

    [Test]
    public void Clock_ShouldBusCollisionException_WhenRegisterAAndRegisterBAreBothEnabled()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);

        mocks.registerAVM.Enable = true;
        mocks.registerBVM.Enable = true;

        var ex = Assert.Throws<BusCollisionException>(() => objUT.Clock());
        ex.Message.Should().Be("Register A and Register B should not be enabled at the same time.");
    }

    [Test]
    public void WhenRegisterAViewModelRaisesDataChanged_SetInputAIsCalledOnALU()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);

        var binary42 = _bitConverter.GetBits((byte)42);
        objUT.RegisterAVM.Data = binary42.AsEnumerable<Bit>().ToList();
        mocks.registerAVM.DataChanged += Raise.Event();

        var expectedArgs = CreateExpectedBitArrayArg(binary42);
        objUT.ALU.Received(1).SetInputA(expectedArgs);
    }

    [Test]
    public void WhenRegisterBViewModelRaisesDataChanged_SetInputBIsCalledOnALU()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);

        var binary42 = _bitConverter.GetBits((byte)42);
        objUT.RegisterBVM.Data = binary42.AsEnumerable<Bit>().ToList();
        mocks.registerBVM.DataChanged += Raise.Event();

        var expectedArgs = CreateExpectedBitArrayArg(binary42);
        objUT.ALU.Received(1).SetInputB(expectedArgs);
    }

    #region Helpers

    private static AluBoard CreateObjectUnderTest(Mocks mocks)
    {
        _ = mocks ?? throw new ArgumentNullException(nameof(mocks));
        return new AluBoard(
            mocks.registerAVM,
            mocks.registerA,
            mocks.registerBVM,
            mocks.registerB,
            mocks.alu);
    }

    private class Mocks
    {
        public readonly IRegisterViewModel registerAVM = Substitute.For<IRegisterViewModel>();
        public readonly IRegister registerA = Substitute.For<IRegister>();
        public readonly IRegisterViewModel registerBVM = Substitute.For<IRegisterViewModel>();
        public readonly IRegister registerB = Substitute.For<IRegister>();
        public readonly IArithmeticLogicUnit alu = Substitute.For<IArithmeticLogicUnit>();

        public Mocks()
        {
            registerA.ProbeState().Returns(_ => registerAVM.Data.ToBitArray());
            registerB.ProbeState().Returns(_ => registerBVM.Data.ToBitArray());
        }
    }

    #endregion
}
