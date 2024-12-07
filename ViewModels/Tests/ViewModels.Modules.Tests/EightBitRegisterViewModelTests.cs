using System.Collections.ObjectModel;
using System.Linq;
using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;
using DigitalElectronics.Utilities;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace DigitalElectronics.ViewModels.Modules.Tests
{
    public class EightBitRegisterViewModelTests
    {
        private static readonly BitConverter BitConverter = new ();
        private static readonly BitArray minByte = new BitArray(byte.MinValue);
        private static readonly BitArray maxByte = new BitArray(byte.MaxValue);
        private static readonly ReadOnlyObservableCollection<Bit> BitCollectionFor255 =   new (CreateObservableBitCollection(byte.MaxValue));
        private static readonly ReadOnlyObservableCollection<bool> BoolCollectionFor255 = new (CreateObservableBoolCollection(byte.MaxValue));

        #region Helper methods

        private static IReadWriteRegister CreateRegisterMock()
        {
            var registerMock = Substitute.For<IReadWriteRegister>();
            registerMock.ProbeState().Returns(maxByte);
            return registerMock;
        }

        private static ObservableCollection<Bit> CreateObservableBitCollection(byte value)
        {
            return new ObservableCollection<Bit>(new BitArray(value).AsEnumerable<Bit>().ToList());
        }

        private static ObservableCollection<bool> CreateObservableBoolCollection(byte value)
        {
            return new ObservableCollection<bool>(new BitArray(value).AsEnumerable());
        }

        private static void AssertSetInputDWasCalled(IReadWriteRegister registerMock, BitArray bitArray)
        {
            registerMock.Received(1).SetInputD(bitArray);
        }

        #endregion

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
            registerMock.ClearReceivedCalls();

            // Set to zero
            objUT.Data = CreateObservableBitCollection(0);
            AssertSetInputDWasCalled(registerMock, minByte);

            // Set to 255
            objUT.Data = CreateObservableBitCollection(255);
            AssertSetInputDWasCalled(registerMock, maxByte);
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

            registerMock.Output.Returns(maxByte);
            objUT.Enable = true;
            objUT.Output.Should().NotBeNull();
            objUT.Output.Should().BeEquivalentTo(objUT.Probe);
        }
        
        [Test]
        public void Clock_ShouldCallClockOnRegister_WhenCalled()
        {
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);
            objUT.Clock();
            registerMock.Received(1).Clock();
        }

        [Test]
        public void Clock_ShouldCallSetInputDOnRegister_WhenCalled()
        {
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);
            registerMock.ClearReceivedCalls();
            objUT.Clock();
            AssertSetInputDWasCalled(registerMock, maxByte);
        }

        [Test]
        public void Output_ShouldUpdateToMatchData_WhenClockIsCalledAndLoadIsTrue()
        {
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);

            registerMock.Output.Returns(maxByte);
            objUT.Enable = true;
            objUT.Output.Should().BeEquivalentTo(BoolCollectionFor255);
            objUT.Enable = false;

            objUT.Data = CreateObservableBitCollection(42);
            objUT.Load = true;
            objUT.Clock();
            objUT.Load = false;

            registerMock.Output.Returns(new BitArray((byte)42));
            objUT.Enable = true;
            objUT.Output.Should().BeEquivalentTo(CreateObservableBoolCollection(42));
        }

        [Test]
        public void Clock_ThrowsInvalidOperationException_WhenLoadAndEnableAreTrue()
        {
            var objUT = new EightBitRegisterViewModel { Enable = true, Load = true };
            var ex = Assert.Throws<System.InvalidOperationException>(() => objUT.Clock());
            ex!.Message.Should().Be("Load and Enable should not both be set high at the same time");
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForDataProperty_WhenDataPropertyIsChanged()
        {
            bool raised = false;
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Data);

            objUT.Data = CreateObservableBitCollection(0);
            raised.Should().Be(true);
            raised = false;

            // Set to 0 again (using different `BitArray` object representing the same value)
            objUT.Data = CreateObservableBitCollection(0);
            raised.Should().Be(false);
            raised = false;

            // Set to 255
            objUT.Data = CreateObservableBitCollection(255);
            raised.Should().Be(true);
            raised = false;

            // Set to 255 again (using different `BitArray` object representing the same value)
            objUT.Data = CreateObservableBitCollection(255);
            raised.Should().Be(false);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForLoadProperty_WhenLoadPropertyIsSet()
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
        public void PropertyChanged_ShouldBeRaisedForEnableProperty_WhenEnablePropertyIsSet()
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
        public void PropertyChanged_ShouldBeRaisedForProbeProperty_UponClockCalled_WhenLoadIsTrue()
        {
            bool raisedForProbe = false;
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock) { Load = true };
            objUT.PropertyChanged += (s, e) => raisedForProbe |= e.PropertyName == nameof(objUT.Probe);

            objUT.Data.Should().BeEquivalentTo(BitCollectionFor255);
            objUT.Clock();
            raisedForProbe.Should().Be(true);

            raisedForProbe = false;
            objUT.Data = CreateObservableBitCollection(0);
            raisedForProbe.Should().Be(false);
            objUT.Clock();
            raisedForProbe.Should().Be(true);
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
            objUT.Data = CreateObservableBitCollection(0);
            raised.Should().Be(false);
            objUT.Clock();
            raised.Should().Be(false);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForOutput_WhenEnableIsChanged()
        {
            bool raised = false;
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);
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
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock) { Enable = false };
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Output);

            objUT.Data = CreateObservableBitCollection(0);
            raised.Should().Be(false);

            raised = false;
            objUT.Data = CreateObservableBitCollection(0);
            raised.Should().Be(false);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForOutput_WhenDataIsChangedAndEnableIsTrue()
        {
            bool raised = false;
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock) { Enable = true };
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Output);

            objUT.Data = CreateObservableBitCollection(0);
            raised.Should().Be(true);

            raised = false;
            objUT.Data = CreateObservableBitCollection(255);
            raised.Should().Be(true);
        }

        [Test]
        public void EnableChanged_ShouldBeRaised_WhenEnablePropertyIsChanged()
        {
            bool raised = false;
            var registerMock = CreateRegisterMock();
            var objUT = new EightBitRegisterViewModel(registerMock);
            objUT.EnableChanged += (s, e) => raised = true;

            objUT.Enable = true;
            raised.Should().Be(true);

            raised = false;
            objUT.Enable = false;
            raised.Should().Be(true);
        }
    }
}
