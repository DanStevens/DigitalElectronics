using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Memory;
using DigitalElectronics.Utilities;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace DigitalElectronics.ViewModels.Modules.Tests
{
    public class SixteenByteRAMViewModelTests
    {
        private static readonly BitConverter BitConverter = new();
        private static readonly BitArrayComparer BitArrayComparer = new();
        private static readonly BitArray minByte = BitConverter.GetBits(byte.MinValue);
        private static readonly BitArray maxByte = BitConverter.GetBits(byte.MaxValue);
        private static readonly BitArray[] initialRamState = Enumerable.Range(0, 16).Select(_ => new BitArray(byte.MaxValue)).ToArray();

        #region Helper methods

        private static ObservableCollection<Bit> CreateObservableBitCollection(byte value)
        {
            return new ObservableCollection<Bit>(BitConverter.GetBits(value).Select(b => new Bit(b)).ToList());
        }

        private static ObservableCollection<Bit> CreateObservableBitCollection(int length, bool value = false)
        {
            return new ObservableCollection<Bit>(Enumerable.Range(0, length).Select(b => new Bit(value)));
        }

        private static BitArray CreateExpectedBitArrayArg(BitArray expectedValue)
        {
            return Arg.Is<BitArray>(arg => BitArrayComparer.Compare(arg, expectedValue) == 0);
        }

        private static IRAM CreateRamMock(BitArray? output = null)
        {
            var ramMock = Substitute.For<IRAM>();
            ramMock.WordSize.Returns(8);
            ramMock.Capacity.Returns(16);
            ramMock.ProbeState(Arg.Any<BitArray>()).Returns(new BitArray(byte.MaxValue));
            ramMock.ProbeState().Returns(initialRamState);
            ramMock.Output.Returns(output);

            ramMock.When(_ => _.SetInputA(Arg.Is<BitArray>(ba => ba.Length > 4)))
                .Throw<System.ArgumentOutOfRangeException>();
            //Assert.Throws<System.ArgumentOutOfRangeException>(() => ramMock.SetInputA(new BitArray(length: 5)));
            //Assert.DoesNotThrow(() => ramMock.SetInputA(new BitArray(length: 4)));

            ramMock.ClearReceivedCalls();
            return ramMock;
        }

        #endregion

        [Test]
        public void InitialState()
        {
            var objUT = new SixteenByteRAMViewModel();
            objUT.Enable.Should().Be(false);
            objUT.Load.Should().Be(false);

            objUT.Data.Should().BeEquivalentTo(CreateObservableBitCollection((byte)255));
            objUT.Address.Should().BeEquivalentTo(CreateObservableBitCollection(length: 4));

            var expectedProbe = new ObservableCollection<BitArray>(
                Enumerable.Range(0, 16).Select(_ => new BitArray((byte)255)));
            objUT.ProbeAll.Should().BeOfType<ReadOnlyObservableCollection<BitArray>>();
            objUT.ProbeAll.Should().BeEquivalentTo(expectedProbe);
        }

        [Test]
        public void Ctor_CallsSetInputAMethodPassingInitialAddressArgument()
        {
            var initialAddress = 6;
            var ramMock = CreateRamMock();
           _ = new SixteenByteRAMViewModel(ramMock, initialAddress);
            ramMock.Received(1).SetInputA(CreateExpectedBitArrayArg(new BitArray((byte)initialAddress).Trim(4)));
        }

        [Test]
        public void Ctor_ClassSetInputAMethodPassingZero_WhenInitialAddressArgumentIsNegative()
        {
            var ramMock = CreateRamMock();
            _ = new SixteenByteRAMViewModel(ramMock, -1);
            ramMock.Received(1).SetInputA(CreateExpectedBitArrayArg(new BitArray((byte)0).Trim(4)));
        }

        [Test]
        public void Ctor_ClassSetInputAMethodPassingCapacityOfRamMinus1_WhenInitialAddressArgumentIsGreaterThanOrEqualToCapacity()
        {
            var ramMock = CreateRamMock();
            _ = new SixteenByteRAMViewModel(ramMock, 16);
            ramMock.Received(1).SetInputA(CreateExpectedBitArrayArg(new BitArray((byte)15).Trim(4)));
            ramMock.ClearReceivedCalls();
            _ = new SixteenByteRAMViewModel(ramMock, 17);
            ramMock.Received(1).SetInputA(CreateExpectedBitArrayArg(new BitArray((byte)15).Trim(4)));
        }

        [Test]
        public void Address_ShouldCallSetInputAMethodOnRam_WhenSet()
        {
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);

            // Set to 15
            objUT.Address = CreateObservableBitCollection((byte)15);
            AssertSetInputDWasCalled(new BitArray((byte)15));

            // Set to zero
            objUT.Address = CreateObservableBitCollection((byte)0);
            AssertSetInputDWasCalled(new BitArray((byte)0));

            void AssertSetInputDWasCalled(BitArray bitArray)
            {
                var expectedArg = CreateExpectedBitArrayArg(bitArray);
                ramMock.Received(1).SetInputA(expectedArg);
                ramMock.ClearReceivedCalls();
            }
        }

        [Test]
        public void Address_ShouldThrowArgumentOutOfRangeException_WhenAddressIsNegative()
        {
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);

            Assert.Throws<System.ArgumentOutOfRangeException>(() => objUT.Address = CreateObservableBitCollection((byte)16));
            Assert.Throws<System.ArgumentOutOfRangeException>(() => objUT.Address = CreateObservableBitCollection((byte)255));
        }

        [Test]
        public void ProbeAll_ShouldBeEntireMemory()
        {
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);
            objUT.ProbeAll.Should().BeOfType<ReadOnlyObservableCollection<BitArray>>();
            objUT.ProbeAll.Should().BeEquivalentTo(initialRamState);
        }

        //[Test]
        //public void ProbeCurrent_ShouldBeCurrentlyAddressMemory()
        //{
        //    var ramMock = CreateRamMock();
        //    var objUT = new SixteenByteRAMViewModel(ramMock);
        //    objUT.ProbeCurrent.Should().BeOfType<ReadOnlyObservableCollection<BitArray>>();
        //    objUT.ProbeCurrent.Should().BeEquivalentTo(initialRamState[0]);
        //}

        [Test]
        public void Data_ShouldCallSetInputDMethodOnRAM_WhenSet()
        {
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);

            // Set to zero
            objUT.Data = CreateObservableBitCollection((byte)0);
            AssertSetInputDWasCalled(minByte);

            // Set to 255
            objUT.Data = CreateObservableBitCollection((byte)255);
            AssertSetInputDWasCalled(maxByte);

            void AssertSetInputDWasCalled(BitArray bitArray)
            {
                var expectedArg = CreateExpectedBitArrayArg(bitArray);
                ramMock.Received(1).SetInputD(expectedArg);
            }
        }

        [Test]
        public void Load_ShouldCallSetInputLMethodOnRegister_WhenSet()
        {
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);

            // Set to true
            objUT.Load = true;
            ramMock.Received(1).SetInputL(true);

            // Set to false;
            objUT.Load = false;
            ramMock.Received(1).SetInputL(false);
        }

        [Test]
        public void Enable_ShouldCallSetInputLMethodOnRegister_WhenSet()
        {
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);

            // Set to true
            objUT.Enable = true;
            ramMock.Received(1).SetInputE(true);

            // Set to false;
            objUT.Enable = false;
            ramMock.Received(1).SetInputE(false);
        }


        [Test]
        public void Output_ShouldBeNull_WhenEnableIsFalse()
        {
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);

            objUT.Enable.Should().Be(false);
            objUT.Output.Should().BeNull();
        }

        [Test]
        public void Output_ShouldBeFirstMemoryLocation_WhenEnableIsTrue()
        {
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);

            objUT.Enable = true;
            objUT.Output.Should().NotBeNull();
            objUT.Output.Should().BeEquivalentTo(initialRamState[0]);
        }

        [Test]
        public void Clock_ShouldCallClockORam_WhenCalled()
        {
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);
            objUT.Clock();
            ramMock.Received(1).Clock();
        }

        [Test]
        public void Clock_ThrowsInvalidOperationException_WhenLoadAndEnableAreTrue()
        {
            var objUT = new SixteenByteRAMViewModel { Enable = true, Load = true };
            var ex = Assert.Throws<System.InvalidOperationException>(() => objUT.Clock());
            ex.Message.Should().Be("Load and Enable should not both be set high at the same time");
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForAddressProperty_WhenAddressPropertyIsChanged()
        {
            bool raised = false;
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Address);

            // Set to 15
            objUT.Address = new ObservableCollection<Bit>(new Bit[] { new(true), new(true), new(true), new(true) });
            raised.Should().Be(true);
            raised = false;

            // Set to 0 again (using different `BitArray` object representing the same value)
            objUT.Address = new ObservableCollection<Bit>(new Bit[] { new(true), new(true), new(true), new(true) });
            raised.Should().Be(false);
            raised = false;

            // Set to 0
            objUT.Address = new ObservableCollection<Bit>(new Bit[] { new(), new(), new(), new() });
            raised.Should().Be(true);
            raised = false;

            // Set to 0 again (using different `BitArray` object representing the same value)
            objUT.Address = new ObservableCollection<Bit>(new Bit[] { new(), new(), new(), new() });
            raised.Should().Be(false);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForDataProperty_WhenDataPropertyIsChanged()
        {
            bool raised = false;
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Data);

            objUT.Data = CreateObservableBitCollection((byte)0);
            raised.Should().Be(true);
            raised = false;

            // Set to 0 again (using different `BitArray` object representing the same value)
            objUT.Data = CreateObservableBitCollection((byte)0);
            raised.Should().Be(false);
            raised = false;

            // Set to 255
            objUT.Data = CreateObservableBitCollection((byte)255);
            raised.Should().Be(true);
            raised = false;

            // Set to 255 again (using different `BitArray` object representing the same value)
            objUT.Data = CreateObservableBitCollection((byte)255);
            raised.Should().Be(false);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForLoadProperty_WhenLoadPropertyIsSet()
        {
            bool raised = false;
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);
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
        public void PropertyChanged_ShouldBeRaisedForEnableProperty_WhenEnablePropertyIsSet()
        {
            bool raised = false;
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);
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
        public void PropertyChanged_ShouldBeRaisedForProbeAllProperty_UponClockCalled_WhenLoadIsTrue()
        {
            bool raisedForProbe = false;
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock) { Load = true };
            objUT.PropertyChanged += (s, e) => raisedForProbe |= e.PropertyName == nameof(objUT.ProbeAll);

            objUT.Data.Should().BeEquivalentTo(CreateObservableBitCollection((byte)255));
            objUT.Clock();
            raisedForProbe.Should().Be(true);

            raisedForProbe = false;
            objUT.Data = CreateObservableBitCollection((byte)0);
            raisedForProbe.Should().Be(false);
            objUT.Clock();
            raisedForProbe.Should().Be(true);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForProbeAllProperty_UponClockCalled_WhenLoadIsFalse()
        {
            bool raisedForProbe = false;
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock) { Load = false };
            objUT.PropertyChanged += (s, e) => raisedForProbe |= e.PropertyName == nameof(objUT.ProbeAll);

            objUT.Data.Should().BeEquivalentTo(CreateObservableBitCollection((byte)255));
            objUT.Clock();
            raisedForProbe.Should().Be(false);

            raisedForProbe = false;
            objUT.Data = CreateObservableBitCollection((byte)0);
            raisedForProbe.Should().Be(false);
            objUT.Clock();
            raisedForProbe.Should().Be(false);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForOutput_WhenEnableIsChanged()
        {
            bool raised = false;
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Output);

            objUT.Enable = true;
            raised.Should().Be(true);

            raised = false;
            objUT.Enable = false;
            raised.Should().Be(true);
        }

        [Test]
        public void PropertyChanged_ShouldBeNotRaisedForOutput_WhenDataIsChangedAndEnableIsFalse()
        {
            bool raised = false;
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock) { Enable = false };
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Output);

            objUT.Data = CreateObservableBitCollection((byte)0);
            raised.Should().Be(false);

            raised = false;
            objUT.Data = CreateObservableBitCollection((byte)0);
            raised.Should().Be(false);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForOutput_WhenDataIsChangedAndEnableIsTrue()
        {
            bool raised = false;
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock) { Enable = true };
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Output);

            objUT.Data = CreateObservableBitCollection((byte)0);
            raised.Should().Be(true);

            raised = false;
            objUT.Data = CreateObservableBitCollection((byte)255);
            raised.Should().Be(true);
        }
    }
}
