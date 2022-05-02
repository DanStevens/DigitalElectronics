using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Memory;
using DigitalElectronics.Utilities;
using DigitalElectronics.ViewModels.Utilities;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace DigitalElectronics.ViewModels.Modules.Tests
{
    public class SixteenByteRAMViewModelTests
    {
        private static readonly BitConverter BitConverter = new();
        private static readonly BitArrayComparer BitArrayComparer = new();
        private static readonly BitArray zeroByte = BitConverter.GetBits((byte)0);
        private static readonly BitArray maxByte = BitConverter.GetBits(byte.MaxValue);
        private static readonly BitArray[] initialRamState = Enumerable.Range(0, 16).Select(_ => new BitArray(byte.MaxValue)).ToArray();
        private static readonly ReadOnlyObservableCollection<bool> BoolCollectionFor255 = new(CreateObservableBoolCollection(byte.MaxValue));

        #region Helper methods

        private static IEnumerable<Bit> CreateBits(byte value)
        {
            return BitConverter.GetBits(value).Select(b => new Bit(b));
        }

        private static IEnumerable<Bit> CreateBits(int length, bool value)
        {
            return Enumerable.Range(0, length).Select(b => new Bit(value));
        }

        private static ObservableCollection<Bit> CreateObservableBitCollection(byte value)
        {
            return new ObservableCollection<Bit>(CreateBits(value));
        }
        private static ObservableCollection<Bit> CreateObservableBitCollection(int length, bool value)
        {
            return new ObservableCollection<Bit>(CreateBits(length, value));
        }

        private static FullyObservableCollection<Bit> CreateFullyObservableBitCollection(byte value)
        {
            return new FullyObservableCollection<Bit>(CreateBits(value));
        }
        private static FullyObservableCollection<Bit> CreateFullyObservableBitCollection(int length, bool value = false)
        {
            return new FullyObservableCollection<Bit>(CreateBits(length, value));
        }

        private static ObservableCollection<bool> CreateObservableBoolCollection(byte value)
        {
            return new ObservableCollection<bool>(BitConverter.GetBits(value));
        }

        private static BitArray CreateExpectedBitArrayArg(BitArray expectedValue)
        {
            return Arg.Is<BitArray>(arg => BitArrayComparer.Compare(arg, expectedValue) == 0);
        }

        private static IRAM CreateRamMock(BitArray? output = null)
        {
            var ramMock = Substitute.For<IRAM, IDARAM>();
            ramMock.WordSize.Returns(8);
            ramMock.Capacity.Returns(16);
            ramMock.ProbeState(Arg.Any<BitArray>()).Returns(new BitArray(byte.MaxValue));
            ramMock.ProbeState().Returns(initialRamState);
            ramMock.Output.Returns(output);

            ((IDARAM)ramMock).When(_ => _.SetInputA(Arg.Is<BitArray>(ba => ba.Length > 4)))
                .Throw<System.ArgumentOutOfRangeException>();
            //Assert.Throws<System.ArgumentOutOfRangeException>(() => ramMock.SetInputA(new BitArray(length: 5)));
            //Assert.DoesNotThrow(() => ramMock.SetInputA(new BitArray(length: 4)));

            ramMock.ClearReceivedCalls();
            return ramMock;
        }

        private static void AssertSetInputAWasCalled(IDARAM ramMock, BitArray bitArray)
        {
            var expectedArg = CreateExpectedBitArrayArg(bitArray);
            ramMock.Received(1).SetInputA(expectedArg);
            ramMock.ClearReceivedCalls();
        }

        #endregion

        [Test]
        public void InitialState()
        {
            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel();
            objUT.Enable.Should().Be(false);
            objUT.Load.Should().Be(false);

            objUT.Data.Should().BeEquivalentTo(CreateObservableBitCollection((byte)255));
            objUT.Address.Should().BeEquivalentTo(CreateObservableBitCollection(length: 4, false));

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
            AssertRamMockReceivedSetInputAOnce((IDARAM)ramMock, (byte)initialAddress);
        }

        private static void AssertRamMockReceivedSetInputAOnce(IDARAM ramMock, byte address)
        {
            ramMock.Received(1).SetInputA(CreateExpectedBitArrayArg(new BitArray(address).Trim(4)));
        }

        [Test]
        public void Ctor_ClassSetInputAMethodPassingZero_WhenInitialAddressArgumentIsNegative()
        {
            var ramMock = CreateRamMock();
            _ = new SixteenByteRAMViewModel(ramMock, -1);
            AssertRamMockReceivedSetInputAOnce((IDARAM)ramMock, 0);
        }

        [Test]
        public void Ctor_ClassSetInputAMethodPassingCapacityOfRamMinus1_WhenInitialAddressArgumentIsGreaterThanOrEqualToCapacity()
        {
            var ramMock = CreateRamMock();
            _ = new SixteenByteRAMViewModel(ramMock, 16);
            AssertRamMockReceivedSetInputAOnce((IDARAM)ramMock, 15);
            ramMock.ClearReceivedCalls();
            _ = new SixteenByteRAMViewModel(ramMock, 17);
            AssertRamMockReceivedSetInputAOnce((IDARAM)ramMock, 15);
        }

        [Test]
        public void Address_ShouldCallSetInputAMethodOnRam_WhenSet()
        {
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);

            // Set to 15
            objUT.Address = CreateFullyObservableBitCollection((byte)15);
            AssertSetInputAWasCalled((IDARAM)ramMock, new BitArray((byte)15));

            // Set to zero
            objUT.Address = CreateFullyObservableBitCollection((byte)0);
            AssertSetInputAWasCalled((IDARAM)ramMock, new BitArray((byte)0));
        }

        [Test]
        public void Address_ShouldCalSetInputAMethodOnRam_WhenIndividualBitIsSet()
        {
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);

            objUT.Address[0].Value = true;

            AssertSetInputAWasCalled((IDARAM)ramMock, new BitArray((byte)1));
        }

        [Test]
        public void Address_ShouldThrowArgumentOutOfRangeException_WhenAddressIsNegative()
        {
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);

            Assert.Throws<System.ArgumentOutOfRangeException>(() => objUT.Address = CreateFullyObservableBitCollection((byte)16));
            Assert.Throws<System.ArgumentOutOfRangeException>(() => objUT.Address = CreateFullyObservableBitCollection((byte)255));
        }

        [Test]
        public void Address_ShouldUpdateOutput_WhenChangedAndEnableIsTrue()
        {
            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel();

            // Set address 0 to 0
            objUT.Address = CreateFullyObservableBitCollection((byte)0);
            objUT.Load = true;
            objUT.Data = CreateObservableBitCollection((byte)0);
            objUT.Clock();
            objUT.ProbeAll[0].Should().BeEquivalentTo(zeroByte);

            // set address 1 to 1
            objUT.Address = CreateFullyObservableBitCollection((byte)1);
            objUT.Data = CreateObservableBitCollection((byte)1);
            objUT.Clock();
            objUT.ProbeAll[1].Should().BeEquivalentTo(new BitArray((byte)1));
            objUT.Load = false;

            // Output current address 1
            objUT.Enable = true;
            objUT.Output.Should().BeEquivalentTo(new BitArray((byte)1));

            // Switch address to 0
            objUT.Address = CreateFullyObservableBitCollection((byte)0);
            objUT.Output.Should().BeEquivalentTo(new BitArray((byte)0));

            // Switch address to 1 by modifying `Address` property (rather than replacing)
            objUT.Address[0].Value = true;
            objUT.Output.Should().BeEquivalentTo(new BitArray((byte)1));
        }

        [Test]
        public void Address_ShouldAlwaysBeLength4_WhenAssignedCollectionWithLength8()
        {
            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel();
            objUT.Address.Count.Should().Be(4);
            var newAddress = CreateFullyObservableBitCollection((byte)0);
            newAddress.Count.Should().Be(8);
            objUT.Address = newAddress;
            objUT.Address.Count.Should().Be(4);
        }

        [Test]
        public void Address_ShouldAlwaysBeLength4_WhenAssignedCollectionWithLength5()
        {
            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel();
            objUT.Address.Count.Should().Be(4);
            var newAddress = CreateFullyObservableBitCollection(length: 5);
            newAddress.Count.Should().Be(5);
            objUT.Address = newAddress;
            objUT.Address.Count.Should().Be(4);
        }


        [Test]
        public void Address_ShouldAlwaysBeLength4_WhenAssignedCollectionWithLength3()
        {
            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel();
            objUT.Address.Count.Should().Be(4);
            var newAddress = CreateFullyObservableBitCollection(length: 3);
            newAddress.Count.Should().Be(3);
            objUT.Address = newAddress;
            objUT.Address.Count.Should().Be(4);
        }

        [Test]
        public void ProbeAll_ShouldBeEntireMemory()
        {
            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel();
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
            AssertSetInputDWasCalled(zeroByte);

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
            ramMock.Received(1).SetInputLD(true);

            // Set to false;
            objUT.Load = false;
            ramMock.Received(1).SetInputLD(false);
        }

        [Test]
        public void Enable_ShouldCallSetInputEMethodOnRegister_WhenSet()
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
            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel();

            objUT.Enable.Should().Be(false);
            objUT.Output.Should().BeNull();
        }

        [Test]
        public void Output_ShouldBeFirstMemoryLocation_WhenEnableIsTrue()
        {
            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel();

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
        public void Clock_ShouldUpdateProbeAll_WhenCalledAndLoadIsTrue()
        {
            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel();
            objUT.ProbeAll[6].ToByte().Should().Be(byte.MaxValue);

            objUT.Address = CreateFullyObservableBitCollection((byte)6);
            objUT.Data = CreateObservableBitCollection((byte)42);
            objUT.Load = true;

            objUT.Clock();

            objUT.ProbeAll[6].ToByte().Should().Be(42);
        }

        [Test]
        public void Clock_ShouldNotUpdateProbeAll_WhenCalledAndLoadIsFalse()
        {
            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel();
            objUT.ProbeAll[6].ToByte().Should().Be(byte.MaxValue);

            objUT.Address = CreateFullyObservableBitCollection(6);
            objUT.Data = CreateObservableBitCollection((byte)42);
            objUT.Load = false;

            objUT.Clock();

            objUT.ProbeAll[6].ToByte().Should().Be(byte.MaxValue);
        }

        [Test]
        public void Clock_ShouldCallSetInputDMethodOnRam_WhenCalled()
        {
            var ramMock = CreateRamMock();
            var objUT = new SixteenByteRAMViewModel(ramMock);
            objUT.Data[0].Value = false;

            objUT.Clock();

            var expectedArg = CreateExpectedBitArrayArg(new BitArray((byte)254));
            ramMock.Received(1).SetInputD(expectedArg);
        }


        [Test]
        public void Output_ShouldUpdateToMatchAddressedMemoryLocation_WhenClockIsCalledAndLoadIsTrue()
        {
            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel();

            objUT.Enable = true;
            objUT.Output.Should().BeEquivalentTo(BoolCollectionFor255);
            objUT.Enable = false;

            objUT.Data = CreateObservableBitCollection(42);
            objUT.Load = true;
            objUT.Clock();
            objUT.Load = false;

            objUT.Enable = true;
            objUT.Output.Should().BeEquivalentTo(CreateObservableBoolCollection(42));
        }

        [Test]
        public void Clock_ThrowsInvalidOperationException_WhenLoadAndEnableAreTrue()
        {
            var objUT = new SixteenByteRAMViewModel { Enable = true, Load = true };
            var ex = Assert.Throws<System.InvalidOperationException>(() => objUT.Clock());
            ex!.Message.Should().Be("Load and Enable should not both be set high at the same time");
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForAddressProperty_WhenAddressPropertyIsChanged()
        {
            bool raised = false;

            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel();
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Address);

            // Set to 15
            objUT.Address = new FullyObservableCollection<Bit>(new Bit[] { new(true), new(true), new(true), new(true) });
            raised.Should().Be(true);
            raised = false;

            // Set to 0 again (using different `BitArray` object representing the same value)
            objUT.Address = new FullyObservableCollection<Bit>(new Bit[] { new(true), new(true), new(true), new(true) });
            raised.Should().Be(false);
            raised = false;

            // Set to 0
            objUT.Address = new FullyObservableCollection<Bit>(new Bit[] { new(), new(), new(), new() });
            raised.Should().Be(true);
            raised = false;

            // Set to 0 again (using different `BitArray` object representing the same value)
            objUT.Address = new FullyObservableCollection<Bit>(new Bit[] { new(), new(), new(), new() });
            raised.Should().Be(false);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForDataProperty_WhenDataPropertyIsChanged()
        {
            bool raised = false;

            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel();
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

            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel();
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

            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel();
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

            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel() { Load = true };
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

            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel() { Load = false };
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

            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel();
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

            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel() { Enable = false };
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

            // Use real `SixteenByteRAM` object
            var objUT = new SixteenByteRAMViewModel() { Enable = true };
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Output);

            objUT.Data = CreateObservableBitCollection((byte)0);
            raised.Should().Be(true);

            raised = false;
            objUT.Data = CreateObservableBitCollection((byte)255);
            raised.Should().Be(true);
        }
    }
}
