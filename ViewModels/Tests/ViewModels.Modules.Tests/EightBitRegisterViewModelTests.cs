﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;
using DigitalElectronics.Utilities;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using BitArray = DigitalElectronics.Concepts.BitArray;
using BitConverter = DigitalElectronics.Utilities.BitConverter;

namespace DigitalElectronics.ViewModels.Modules.Tests
{
    public class EightBitRegisterViewModelTests
    {
        private const int NumberOfBits = 8;

        private static readonly BitConverter BitConverter = new ();
        private static readonly BitArray minByte = BitConverter.GetBits(byte.MinValue);
        private static readonly BitArray maxByte = BitConverter.GetBits(byte.MaxValue);
        private static readonly ObservableCollection<Bit> BitCollectionFor255 = CreateObservableBitCollection(byte.MaxValue);
        private static readonly ObservableCollection<Bit> BitCollectionFor0 = CreateObservableBitCollection((byte)0);
        private static readonly ObservableCollection<bool> BoolCollectionFor255 = CreateObservableBoolCollection(byte.MaxValue);
        private static readonly ObservableCollection<bool> BoolCollectionFor0 = CreateObservableBoolCollection((byte)0);

        private readonly BitArrayComparer _baComparer = new();

        private static IRegister CreateRegisterMock()
        {
            var registerMock = Substitute.For<IRegister>();
            registerMock.ProbeState().Returns(maxByte);
            return registerMock;
        }

        private static ObservableCollection<Bit> CreateObservableBitCollection(byte value)
        {
            return new ObservableCollection<Bit>(BitConverter.GetBits(value));
        }

        private static ObservableCollection<bool> CreateObservableBoolCollection(byte value)
        {
            return new ObservableCollection<bool>(BitConverter.GetBits(value));
        }


        private BitArray CreateExpectedBitArrayArg(BitArray expectedValue)
        {
            return Arg.Is<BitArray>(arg => _baComparer.Compare(arg, expectedValue) == 0);
        }

        [Test]
        public void InitialState()
        {
            var objUT = new EightBitRegisterViewModel();
            objUT.Enable.Should().Be(false);
            objUT.Load.Should().Be(false);
            objUT.Data.Should().BeOfType<ObservableCollection<Bit>>();

            objUT.Data.Should().BeEquivalentTo(BitCollectionFor255);
            objUT.Probe.Should().BeEquivalentTo(BoolCollectionFor255);
            objUT.Output.Should().BeNull();
        }


        [Test]
        public void Data_ShouldCallSetInputDMethodOnRegister_WhenSet()
        {
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);

            // Set to zero
            objUT.Data = BitCollectionFor0;
            AssertSetInputDWasCalled(minByte);

            // Set to 255
            objUT.Data = BitCollectionFor255;
            AssertSetInputDWasCalled(maxByte);

            void AssertSetInputDWasCalled(BitArray bitArray)
            {
                var expectedArg = CreateExpectedBitArrayArg(bitArray);
                registerMock.Received(1).SetInputD(expectedArg);
            }
        }

        [Test]
        public void Data_ShouldBeSetToZeroBitArray_WhenSetToNull()
        {
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);
            objUT.Data = null;
            objUT.Data.Should().BeEquivalentTo(BitCollectionFor0);
        }

        [Test]
        public void Data_WhenChangingABit_ShouldCallSetInputDMethodOnRegister()
        {
            var binary254 = BitConverter.GetBits((byte)254);
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);
            objUT.Data.Should().BeEquivalentTo(BitCollectionFor255);

            objUT.Data[0] = false;

            var expectedArg = CreateExpectedBitArrayArg(binary254);
            registerMock.Received().SetInputD(expectedArg);
        }

        [Test]
        public void Data_WhenChangingABit_AfterAssigningNewValue_ShouldCallSetInputDMethdOnRegister()
        {
            var binary1 = BitConverter.GetBits((byte)1);
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);
            objUT.Data.Should().BeEquivalentTo(BitCollectionFor255);

            objUT.Data = BitCollectionFor0;
            objUT.Data[0] = true;

            var expectedArg = CreateExpectedBitArrayArg(binary1);
            registerMock.Received().SetInputD(expectedArg);
        }

        [Test]
        public void Load_ShouldCallSetInputLMethodOnRegister_WhenSet()
        {
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);

            // Set to true
            objUT.Load = true;
            registerMock.Received(1).SetInputL(true);

            // Set to false;
            objUT.Load = false;
            registerMock.Received(1).SetInputL(false);
        }

        [Test]
        public void Output_ShouldBeNull_WhenEnableIsFalse()
        {
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);
            
            objUT.Enable.Should().Be(false);
            objUT.Output.Should().BeNull();
        }

        [Test]
        public void Output_ShouldBeProbe_WhenEnableIsTrue()
        {
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);

            objUT.Enable = true;
            objUT.Output.Should().BeEquivalentTo(objUT.Probe);
        }


        [Test]
        public void Enable_ShouldCallSetInputLMethodOnRegister_WhenSet()
        {
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);

            // Set to true
            objUT.Enable = true;
            registerMock.Received(1).SetInputE(true);

            // Set to false;
            objUT.Enable = false;
            registerMock.Received(1).SetInputE(false);
        }

        [Test]
        public void Clock_ShouldCallClockOnRegister_WhenCalled()
        {
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);
            objUT.Clock();
        }

        [Test]
        public void Clock_ThrowsInvalidOperationException_WhenLoadAndEnableAreTrue()
        {
            var objUT = new EightBitRegisterViewModel { Enable = true, Load = true };
            var ex = Assert.Throws<InvalidOperationException>(() => objUT.Clock());
            ex.Message.Should().Be("Load and Enable should not both be set high at the same time");
        }

        [Test]
        [Ignore("TODO come back to this")]
        public void PropertyChanged_ShouldBeRaisedChangedForDataProperty_WhenDataPropertyIsChanged()
        {
            bool raised = false;
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Data);

            objUT.Data = BitCollectionFor0;
            raised.Should().Be(true);
            raised = false;

            // Set to 0 again (using different `BitArray` object representing the same value)
            objUT.Data = CreateObservableBitCollection((byte)0);
            raised.Should().Be(false);
            raised = false;

            // Set to 255
            objUT.Data = BitCollectionFor255;
            raised.Should().Be(true);
            raised = false;

            // Set to 255 again (using different `BitArray` object representing the same value)
            objUT.Data = CreateObservableBitCollection((byte)255);
            raised.Should().Be(false);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedChangedForLoadProperty_WhenLoadPropertyIsSet()
        {
            bool raised = false;
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Load);

            // Set to true
            objUT.Load = true;
            raised.Should().Be(true);
            raised = false;

            // Set to true again
            objUT.Load = true;
            raised.Should().Be(false);

            // Set to false
            objUT.Load = false;
            raised.Should().Be(true);
            raised = false;

            // Set to false again
            objUT.Load = false;
            raised.Should().Be(false);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedChangedForEnableProperty_WhenEnablePropertyIsSet()
        {
            bool raised = false;
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Enable);

            // Set to true
            objUT.Enable = true;
            raised.Should().Be(true);
            raised = false;

            // Set to true again
            objUT.Enable = true;
            raised.Should().Be(false);

            // Set to false
            objUT.Enable = false;
            raised.Should().Be(true);
            raised = false;

            // Set to false again
            objUT.Enable = false;
            raised.Should().Be(false);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForProbe_UponClockCalled_WhenLoadIsTrue()
        {
            bool raised = false;
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock) { Load = true };
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Probe);

            objUT.Data.Should().BeEquivalentTo(BitCollectionFor255);
            objUT.Clock();
            raised.Should().Be(true);

            raised = false;
            objUT.Data = BitCollectionFor0;
            raised.Should().Be(false);
            objUT.Clock();
            raised.Should().Be(true);
        }

        [Test]
        public void PropertyChanged_ShouldNotBeRaisedForProbe_UponClockCalled_WhenLoadIsFalse()
        {
            bool raised = false;
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock) { Load = false };
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Probe);

            objUT.Data.Should().BeEquivalentTo(BitCollectionFor255);
            objUT.Clock();
            raised.Should().Be(false);

            raised = false;
            objUT.Data = BitCollectionFor0;
            raised.Should().Be(false);
            objUT.Clock();
            raised.Should().Be(false);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForOutput_UponEnableSetToTrue()
        {
            bool raised = false;
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Output);

            objUT.Enable = true;

            raised.Should().Be(true);
        }

        [Test]
        public void PropertyChanged_ShouldBeNotRaisedForOutput_WhenEnableIsFalse()
        {
            bool raised = false;
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock) { Enable = false };
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Output);

            objUT.Data = BitCollectionFor0;
            raised.Should().Be(false);

            raised = false;
            objUT.Data = BitCollectionFor255;
            raised.Should().Be(false);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForOutput_WhenEnableIsTrue()
        {
            bool raised = false;
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock) { Enable = true };
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Output);

            objUT.Data = BitCollectionFor0;
            raised.Should().Be(true);

            raised = false;
            objUT.Data = BitCollectionFor255;
            raised.Should().Be(true);
        }

        [Test]
        public void DataChanged_ShouldBeRaised_WhenDataPropertyChanges()
        {
            var raised = false;
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);
            objUT.DataChanged += (s, e) => raised = true;

            objUT.Data = BitCollectionFor0;

            raised.Should().Be(true);

            raised = false;
            objUT.Data[0] = true;

            raised.Should().Be(true);
        }
    }
}
