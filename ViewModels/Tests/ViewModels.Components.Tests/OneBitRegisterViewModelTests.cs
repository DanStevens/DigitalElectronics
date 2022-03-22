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
            var objUT = new OneBitRegisterDemoViewModel();
            objUT.Enable.Should().Be(false);
            objUT.Load.Should().Be(false);
            objUT.Data.Should().Be(false);
            objUT.ProbeQ.Should().Be(true);
            objUT.OutputQ.Should().Be(null);
        }

        [Test]
        public void Data_ShouldCallSetInputDMethodOnBitRegister_WhenSet()
        {
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterDemoViewModel(bitRegisterMock);
            
            // Set to true
            objUT.Data = true;
            bitRegisterMock.Received(1).SetInputD(true);

            // Set to false
            objUT.Data = false;
            bitRegisterMock.Received(1).SetInputD(false);
        }

        [Test]
        public void Load_ShouldCallSetInputLMethodOnBitRegister_WhenSet()
        {
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterDemoViewModel(bitRegisterMock);

            // Set to true
            objUT.Load = true;
            bitRegisterMock.Received(1).SetInputL(true);

            // Set to false
            objUT.Load = false;
            bitRegisterMock.Received(1).SetInputL(false);
        }

        [Test]
        public void Enable_ShouldRaisePropertyChangedEvent_WhenSet()
        {
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterDemoViewModel(bitRegisterMock);

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
            var objUT = new OneBitRegisterDemoViewModel(bitRegisterMock);
            objUT.Clock();
            bitRegisterMock.Received(1).Clock();
        }

        [Test]
        public void Clock_ThrowsInvalidOperationException_WhenLoadAndEnableAreTrue()
        {
            var objUT = new OneBitRegisterDemoViewModel();
            objUT.Enable = true;
            objUT.Load = true;
            var ex = Assert.Throws<InvalidOperationException>(() => objUT.Clock());
            ex.Message.Should().Be("Load and Enable should not both be set high at the same time");
        }

        [Test]
        public void PropertyChanged_ShouldBeRaisedChangedForDataProperty_WhenDataPropertyIsSet()
        {
            bool raised = false;
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterDemoViewModel(bitRegisterMock);
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
            var objUT = new OneBitRegisterDemoViewModel(bitRegisterMock);
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
            var objUT = new OneBitRegisterDemoViewModel(bitRegisterMock);
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
            var objUT = new OneBitRegisterDemoViewModel(bitRegisterMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.ProbeQ);
            objUT.Load = true;

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
        public void PropertyChanged_ShouldBeRaisedForProbeQ_UponClockCalled_WhenLoadIsFalse()
        {
            bool raised = false;
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterDemoViewModel(bitRegisterMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.ProbeQ);
            objUT.Load = false;

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
        public void PropertyChanged_ShouldBeRaisedForOutputQ_UponClockCalled_WhenEnableIsTrue()
        {
            bool raised = false;
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterDemoViewModel(bitRegisterMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.OutputQ);
            objUT.Enable = true;

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
        public void PropertyChanged_ShouldBeRaisedForOutputQ_UponClockCalled_WhenEnableIsFalse()
        {
            bool raised = false;
            var bitRegisterMock = Substitute.For<IRegisterBit>();
            var objUT = new OneBitRegisterDemoViewModel(bitRegisterMock);
            objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.OutputQ);
            objUT.Enable = false;

            objUT.Data.Should().Be(false);
            objUT.Clock();
            raised.Should().Be(false);

            raised = false;
            objUT.Data = true;
            raised.Should().Be(false);
            objUT.Clock();
            raised.Should().Be(false);
        }
    }
}
