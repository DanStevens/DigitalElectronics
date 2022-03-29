using System;
using System.Linq;
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
    private readonly static BitConverter _bitConverter = new BitConverter();

    [Test]
    public void InitialState()
    {
        var mocks = new Mocks();
        var objUT = CreateObjectUnderTest(mocks);
        objUT.RegisterA.Should().BeSameAs(mocks.registerA);
        objUT.RegisterB.Should().BeSameAs(mocks.registerB);
        objUT.ALU.Should().BeSameAs(mocks.alu);
    }


    [Test]
    public void Clock_ShouldCallClockOnRegisterAAndRegisterB()
    {
        var mocks = new Mocks();
        var objUT = CreateObjectUnderTest(mocks);

        objUT.Clock();

        mocks.registerA.Received(1).Clock();
        mocks.registerB.Received(1).Clock();
    }

    [Test]
    public void Clock_ShouldBusCollisionException_WhenRegisterAAndRegisterBAreBothEnabled()
    {
        var mocks = new Mocks();
        var objUT = CreateObjectUnderTest(mocks);

        mocks.registerA.Enable = true;
        mocks.registerB.Enable = true;

        var ex = Assert.Throws<BusCollisionException>(() => objUT.Clock());
        ex.Message.Should().Be("Register A and Register B should not be enabled at the same time.");
    }

    [Test]
    public void Test()
    {
        var mocks = new Mocks();
        var objUT = CreateObjectUnderTest(mocks);

        ////objUT.RegisterA.Data = _bitConverter.GetBits(42).AsEnumerable().ToList();
    }

    #region Helpers

    private static AluBoard CreateObjectUnderTest(Mocks mocks)
    {
        _ = mocks ?? throw new ArgumentNullException(nameof(mocks));
        return new AluBoard(mocks.registerA, mocks.registerB, mocks.alu);
    }

    private class Mocks
    {
        public readonly IRegisterViewModel registerA = Substitute.For<IRegisterViewModel>();
        public readonly IRegisterViewModel registerB = Substitute.For<IRegisterViewModel>();
        public readonly IArithmeticLogicUnit alu = Substitute.For<IArithmeticLogicUnit>();
    }

    #endregion
}
