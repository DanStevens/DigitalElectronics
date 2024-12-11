using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    private static IEnumerable<T> CreateExpectedEnumerableArg<T>(IEnumerable<T> expectedValue)
    {
        return Arg.Is<IEnumerable<T>>(p => p.SequenceEqual(expectedValue));
    }

    [Test]
    public void InitialState()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);
        objUT.RegisterA.Should().BeSameAs(mocks.registerAVM);
        objUT.RegisterB.Should().BeSameAs(mocks.registerBVM);
        objUT.ALU.Should().BeSameAs(mocks.aluVM);
    }

    [Test]
    public void Ctor_ShouldSyncAluWithRegisters()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);

        // Register A
        var expectedArgA = CreateExpectedEnumerableArg(mocks.registerAVM.Probe);
        mocks.aluVM.Received(1).SetInputA(expectedArgA);

        // Register B
        var expectedArgB = CreateExpectedEnumerableArg(mocks.registerBVM.Probe);
        mocks.aluVM.Received(1).SetInputB(expectedArgB);
    }

    [Test]
    public void BusState_ShouldMatchOutputOfRegisterA_WhenRegisterAIsEnabled()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);
        mocks.registerAVM.Enable.Should().Be(false);
        var binary42 = _bitConverter.GetBits((byte)42);
        mocks.registerAVM.Output.Returns((IReadOnlyList<bool>?) binary42.ToArray<bool>());
        objUT.BusState.Should().BeNull();

        mocks.registerAVM.Enable = true;
        mocks.registerAVM.EnableChanged += Raise.Event();
        objUT.BusState.Should().BeEquivalentTo(mocks.registerAVM.Output);

        mocks.registerAVM.Enable = false;
        mocks.registerAVM.EnableChanged += Raise.Event();
        objUT.BusState.Should().BeNull();
    }

    [Test]
    public void BusState_ShouldMatchOutputOfRegisterB_WhenRegisterBIsEnabled()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);
        mocks.registerBVM.Enable.Should().Be(false);
        var binary42 = _bitConverter.GetBits((byte)42);
        mocks.registerBVM.Output.Returns((IReadOnlyList<bool>?) binary42.ToArray<bool>());
        objUT.BusState.Should().BeNull();

        mocks.registerBVM.Enable = true;
        mocks.registerBVM.EnableChanged += Raise.Event();
        objUT.BusState.Should().BeEquivalentTo(mocks.registerBVM.Output);

        mocks.registerBVM.Enable = false;
        mocks.registerBVM.EnableChanged += Raise.Event();
        objUT.BusState.Should().BeNull();
    }

    [Test]
    public void BusState_ShouldMatchOutputEOfAlu_WhenAluIsEnabled()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);
        var binary42 = _bitConverter.GetBits((byte)42);
        objUT.BusState.Should().BeNull();

        mocks.aluVM.OutputE.Returns((IReadOnlyList<bool>?) binary42.ToArray<bool>());
        mocks.aluVM.Enable = true;
        mocks.aluVM.EnableChanged += Raise.Event();
        objUT.BusState.Should().BeEquivalentTo(binary42.ToArray<bool>());

        mocks.aluVM.Enable = false;
        mocks.aluVM.EnableChanged += Raise.Event();
        objUT.BusState.Should().BeNull();
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

    [TestCase(false, false, false, false)]
    [TestCase(true, false, false, false)]
    [TestCase(false, true, false, false)]
    [TestCase(false, false, true, false)]
    [TestCase(true, true, true, true)]
    [TestCase(false, true, true, true)]
    [TestCase(true, true, true, true)]
    public void Clock_ShouldBusCollisionException_WhenMoreThanOneComponentIsEnabled(
        bool registerAEnabled,
        bool registerBEnabled,
        bool aluEnabled,
        bool busCollisionExpected)
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);

        mocks.registerAVM.Enable = registerAEnabled;
        mocks.registerBVM.Enable = registerBEnabled;
        mocks.aluVM.Enable = aluEnabled;

        if (busCollisionExpected)
        {
            var ex = Assert.Throws<BusContentionException>(() => objUT.Clock());
            ex!.Message.Should().Be(BusContentionException.DefaultMessage);
        }
        else
        {
            Assert.DoesNotThrow(() => objUT.Clock());
        }
    }

    [Test]
    public void Clock_ShouldSyncInputAOnAluWithRegisterA()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);
        var binary42 = _bitConverter.GetBits((byte)42);
        objUT.RegisterA.Data = binary42.AsEnumerable<Bit>().ToList();

        objUT.Clock();

        var expectedArgs = CreateExpectedEnumerableArg(objUT.RegisterA.Probe);
        mocks.aluVM.Received(1).SetInputA(expectedArgs);
    }

    [Test]
    public void Clock_ShouldSyncInputBOnAluWithRegisterB()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);
        var binary42 = _bitConverter.GetBits((byte)42);
        objUT.RegisterB.Data = binary42.AsEnumerable<Bit>().ToList();

        objUT.Clock();

        var expectedArgs = CreateExpectedEnumerableArg(objUT.RegisterB.Probe);
        mocks.aluVM.Received(1).SetInputB(expectedArgs);
    }

    [Test]
    public void Clock_ShouldSyncRegisterAWithAlu_WhenAluOutputIsEnabled_AndRegisterAIsLoading()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);
        var binary42 = _bitConverter.GetBits((byte)42);
        mocks.aluVM.OutputE.Returns(binary42.ToArray().AsEnumerable());
        mocks.aluVM.Enable = true;
        mocks.aluVM.EnableChanged += Raise.Event();
        mocks.registerAVM.Load = true;

        objUT.Clock();

        mocks.registerAVM.Data.Should().BeEquivalentTo(binary42.AsEnumerable());
    }

    [Test]
    public void Clock_ShouldTriggerBusTransferFromRegisterAtoAluShould_WhenAluOutputIsEnabled_AndRegisterAIsNotLoading()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);
        var current = mocks.registerAVM.Data;
        var binary42 = _bitConverter.GetBits((byte)42);
        mocks.aluVM.OutputE.Returns(binary42.ToArray());
        mocks.aluVM.Enable = true;
        mocks.aluVM.EnableChanged += Raise.Event();
        mocks.registerAVM.Load = false;

        objUT.Clock();

        mocks.registerAVM.Data.Should().BeSameAs(current);
    }

    [Test]
    public void Clock_ShouldTriggerBusTransferFromRegisterBToAlu_WhenAluOutputIsEnabled_AndRegisterBIsLoading()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);
        var binary42 = _bitConverter.GetBits((byte)42);
        mocks.registerBVM.Data = Substitute.For<IList<Bit>>();
        mocks.registerBVM.Data.Count.Returns(8);
        mocks.aluVM.OutputE.Returns(binary42.ToArray());
        mocks.aluVM.Enable = true;
        mocks.aluVM.EnableChanged += Raise.Event();
        mocks.registerBVM.Load = true;

        objUT.Clock();

        for (int i = 0; i < binary42.Length; i++)
        {
            mocks.registerBVM.Data.Received(1)[i] = binary42[i];
        }
    }

    [Test]
    public void Clock_ShouldNotTriggerBusTransferFromRegisterBToAlu_WhenAluOutputIsEnabled_AndRegisterBIsNotLoading()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);
        var current = mocks.registerBVM.Data;
        var binary42 = _bitConverter.GetBits((byte)42);
        mocks.aluVM.OutputE.Returns(binary42.ToArray());
        mocks.aluVM.Enable = true;
        mocks.aluVM.EnableChanged += Raise.Event();
        mocks.registerBVM.Load = false;

        objUT.Clock();

        mocks.registerBVM.Data.Should().BeSameAs(current);
    }

    [Test]
    public void Clock_ShouldSyncBusWithAlu_WhenALUIsEnabled()
    {
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);
        var binary1 = _bitConverter.GetBits((byte)1);
        mocks.aluVM.Enable = true;
        mocks.aluVM.EnableChanged += Raise.Event();
        mocks.aluVM.OutputE.Returns(binary1.ToArray());

        objUT.Clock();

        objUT.BusState.Should().BeEquivalentTo(binary1.AsEnumerable());
    }

    [Test]
    public void PropertyChanged_ShouldBeRaisedForBusState_WhenBusStateChanges()
    {
        bool raised = false;
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);
        objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.BusState);
        mocks.registerAVM.Enable.Should().Be(false);
        var binary42 = _bitConverter.GetBits((byte)42);
        mocks.registerAVM.Output.Returns((IReadOnlyList<bool>?) binary42.ToArray<bool>());
        objUT.BusState.Should().BeNull();

        mocks.registerAVM.Enable = true;
        mocks.registerAVM.EnableChanged += Raise.Event();
        raised.Should().Be(true);
        raised = false;

        mocks.registerAVM.Enable = true;
        mocks.registerAVM.EnableChanged += Raise.Event();
        raised.Should().Be(true);
    }

    [Test]
    public void Addition()
    {
        for (int a = 0; a < 25; a++)
            for (int b = 0; b < 25; b++)
                DoTest(a, b, a + b);

        void DoTest(int a, int b, int expectedSum)
        {
            using var objUT = new AluBoard();
            var binaryA = new BitArray((byte) a);
            var binaryB = new BitArray((byte) b);
            var binaryExpectedSum = new BitArray((byte) expectedSum);

            LoadRegisterA(binaryB);
            TransferRegisterAToRegisterB();
            objUT.RegisterB.Probe.Should().BeEquivalentTo(binaryB.ToArray<bool>());
            LoadRegisterA(binaryA);
            objUT.RegisterA.Probe.Should().BeEquivalentTo(binaryA.ToArray<bool>());
            objUT.ALU.Probe.Should().BeEquivalentTo(binaryExpectedSum.ToArray<bool>());

            void LoadRegisterA(BitArray value)
            {
                objUT.RegisterA.Load = true;
                objUT.RegisterA.Data = value.AsEnumerable<Bit>().ToList();
                objUT.Clock();
                objUT.RegisterA.Load = false;
                objUT.RegisterA.Probe.Should().BeEquivalentTo(value.ToArray<bool>());
            }

            void TransferRegisterAToRegisterB()
            {
                objUT.RegisterA.Enable = true;
                objUT.RegisterB.Load = true;
                objUT.Clock();
                objUT.RegisterB.Load = false;
                objUT.RegisterA.Enable = false;
            }
        }
    }

    #region Helpers

    private static AluBoard CreateObjectUnderTest(Mocks mocks)
    {
        _ = mocks ?? throw new ArgumentNullException(nameof(mocks));
        return new AluBoard(mocks.aluVM, mocks.registerAVM, mocks.registerBVM);
    }

    private class Mocks
    {
        public readonly IAluViewModel aluVM = Substitute.For<IAluViewModel>();
        public readonly IRegisterViewModel registerAVM = Substitute.For<IRegisterViewModel>();
        public readonly IRegisterViewModel registerBVM = Substitute.For<IRegisterViewModel>();

        public Mocks()
        {
            var zero = Enumerable.Repeat(new Bit(), 8);
            registerAVM.Data = zero.ToList();
            registerBVM.Data = zero.ToList();

            registerAVM.Probe.Returns(_ => new ReadOnlyCollection<bool>(registerAVM.Data.Select(bit => bit.Value).ToList()));
            registerBVM.Probe.Returns(_ => new ReadOnlyCollection<bool>(registerBVM.Data.Select(bit => bit.Value).ToList()));
        }
    }

    #endregion
}
