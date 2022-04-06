﻿using System;
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
    private static readonly BitArrayComparer _baComparer = new();

    private static BitArray CreateExpectedBitArrayArg(BitArray expectedValue)
    {
        return Arg.Is<BitArray>(arg => _baComparer.Compare(arg, expectedValue) == 0);
    }

    private static IEnumerable<T> CreateExpectedEnumerableArg<T>(IReadOnlyList<T> expectedValue)
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
        mocks.registerAVM.Output.Returns(binary42.ToList<bool>());
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
        mocks.registerBVM.Output.Returns(binary42.ToList<bool>());
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

        mocks.aluVM.OutputE.Returns(binary42.ToList<bool>());
        mocks.aluVM.Enable = true;
        mocks.aluVM.EnableChanged += Raise.Event();
        objUT.BusState.Should().BeEquivalentTo(binary42.ToList<bool>());

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
            ex.Message.Should().Be(BusContentionException.DefaultMessage);
        }
        else
        {
            Assert.DoesNotThrow(() => objUT.Clock());
        }
    }

    [Test]
    public void Clock_ShouldCallSetInputAWithRegisterAProbeStateAsArg_WhenCalled()
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
    public void Clock_ShouldCallSetInputBWithRegisterBProbeStateAsArg_WhenCalled()
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
    public void PropertyChanged_ShouldBeRaisedForBusState_WhenBusStateChanges()
    {
        bool raised = false;
        var mocks = new Mocks();
        using var objUT = CreateObjectUnderTest(mocks);
        objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.BusState);
        mocks.registerAVM.Enable.Should().Be(false);
        var binary42 = _bitConverter.GetBits((byte)42);
        mocks.registerAVM.Output.Returns(binary42.ToList<bool>());
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
            objUT.RegisterB.Probe.Should().BeEquivalentTo(binaryB.AsReadOnlyList<bool>());
            LoadRegisterA(binaryA);
            objUT.RegisterA.Probe.Should().BeEquivalentTo(binaryA.AsReadOnlyList<bool>());
            objUT.ALU.Probe.Should().BeEquivalentTo(binaryExpectedSum.AsReadOnlyList<bool>());

            void LoadRegisterA(BitArray value)
            {
                objUT.RegisterA.Load = true;
                objUT.RegisterA.Data = value.AsEnumerable<Bit>().ToList();
                objUT.Clock();
                objUT.RegisterA.Load = false;
                objUT.RegisterA.Probe.Should().BeEquivalentTo(value.AsReadOnlyList<bool>());
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
            registerAVM.Probe.Returns(_ => new ReadOnlyCollection<bool>(registerAVM.Data.Select(bit => bit.Value).ToList()));
            registerBVM.Probe.Returns(_ => new ReadOnlyCollection<bool>(registerBVM.Data.Select(bit => bit.Value).ToList()));
        }
    }

    #endregion
}
