using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalElectronics.Components.Memory;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace DigitalElectronics.ViewModels.Components.Tests
{
    public class OneBitRegisterViewModelTests
    {
        [Test]
        public void InitialState()
        {
            var objUT = new OneBitRegisterViewModel();
            objUT.Enable.Should().Be(false);
            objUT.Load.Should().Be(false);
            objUT.Data.Should().Be(false);
            objUT.ProbeQ.Should().Be(true);
            objUT.OutputQ.Should().Be(null);
        }

        [Test]
        public void Data_ShouldCallSetInputDMethodOnBitRegisterBit_WhenSet()
        {
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterViewModel(bitRegisterMock);
            
            // Set to true
            objUT.Data = true;
            bitRegisterMock.Received(1).SetInputD(true);

            // Set to false
            objUT.Data = false;
            bitRegisterMock.Received(1).SetInputD(false);
        }

        [Test]
        public void Load_ShouldCallSetInputLMethodOnBitRegisterBit_WhenSet()
        {
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterViewModel(bitRegisterMock);

            // Set to true
            objUT.Load = true;
            bitRegisterMock.Received(1).SetInputL(true);

            // Set to false
            objUT.Load = false;
            bitRegisterMock.Received(1).SetInputL(false);
        }

        [Test]
        public void Enable_ShouldCallSetInputEMethodOnRegisterBit_WhenSet()
        {
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterViewModel(bitRegisterMock);

            // Set to true
            objUT.Enable = true;
            bitRegisterMock.Received(1).SetInputE(true);

            // Set to false
            objUT.Enable = false;
            bitRegisterMock.Received(1).SetInputE(false);
        }

        [Test]
        public void Clock_ShouldCallClockOnBitRegister_WhenCalled()
        {
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterViewModel(bitRegisterMock);
            objUT.Clock();
            bitRegisterMock.Received(1).Clock();
        }

        [Test]
        public void Clock_ThrowsInvalidOperationException_WhenLoadAndEnableAreTrue()
        {
            var objUT = new OneBitRegisterViewModel {Enable = true, Load = true};
            var ex = Assert.Throws<InvalidOperationException>(() => objUT.Clock());
            ex.Message.Should().Be("Load and Enable should not both be set high at the same time");
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedChangedForDataProperty_WhenDataPropertyIsSet()
        {
            bool raised = false;
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterViewModel(bitRegisterMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Data);

            // Set to true
            objUT.Data = true;
            raised.Should().Be(true);
            raised = false;

            // Set to false
            objUT.Data = false;
            raised.Should().Be(true);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedChangedForLoadProperty_WhenLoadPropertyIsSet()
        {
            bool raised = false;
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterViewModel(bitRegisterMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Load);

            // Set to true
            objUT.Load = true;
            raised.Should().Be(true);
            raised = false;

            // Set to false
            objUT.Load = false;
            raised.Should().Be(true);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForEnableProperty_WhenEnablePropertyIsSet()
        {
            bool raised = false;
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterViewModel(bitRegisterMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Enable);

            // Set to true
            objUT.Enable = true;
            raised.Should().Be(true);
            raised = false;

            // Set to false
            objUT.Enable = false;
            raised.Should().Be(true);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForProbeQ_UponClockCalled_WhenLoadIsTrue()
        {
            bool raised = false;
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterViewModel(bitRegisterMock) {Load = true};
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.ProbeQ);

            objUT.Data.Should().Be(false);            
            objUT.Clock();
            raised.Should().Be(true);

            raised = false;
            objUT.Data = true;
            raised.Should().Be(false);
            objUT.Clock();
            raised.Should().Be(true);
        }

        [Test]
        public void PropertyChanged_ShouldNotBeRaisedForProbeQ_UponClockCalled_WhenLoadIsFalse()
        {
            bool raised = false;
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterViewModel(bitRegisterMock) {Load = false};
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.ProbeQ);

            objUT.Data.Should().Be(false);
            objUT.Clock();
            raised.Should().Be(false);

            raised = false;
            objUT.Data = true;
            raised.Should().Be(false);
            objUT.Clock();
            raised.Should().Be(false);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForOutputQ_UponEnableSetToTrue()
        {
            bool raised = false;
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterViewModel(bitRegisterMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.OutputQ);
            
            objUT.Enable = true;

            raised.Should().Be(true);
        }

        [Test]
        public void PropertyChanged_ShouldBeNotRaisedForOutputQ_UponDataChanged_WhenEnableIsFalse()
        {
            bool raised = false;
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterViewModel(bitRegisterMock) {Enable = false};
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.OutputQ);

            objUT.Data.Should().Be(false);
            raised.Should().Be(false);

            raised = false;
            objUT.Data = true;
            raised.Should().Be(false);
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedForOutputQ_UponDataChanged_WhenEnableIsTrue()
        {
            bool raised = false;
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterViewModel(bitRegisterMock) {Enable = true};
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.OutputQ);

            objUT.Data = true;
            raised.Should().Be(true);

            raised = false;
            objUT.Data = false;
            raised.Should().Be(true);
        }
    }
}
