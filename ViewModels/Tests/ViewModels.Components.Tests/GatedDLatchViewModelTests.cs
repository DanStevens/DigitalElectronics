using System.Runtime.Remoting;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.ViewModels.Components.Tests
{
    public class GatedDLatchViewModelTests
    {
        [Test]
        public void OutputQ_ShouldNotChange_WhenEnabledIsFalseAndDataIsChanged()
        {
            var objUT = new GatedDLatchViewModel();
            objUT.Enable.Should().Be(false);
            objUT.Data.Should().Be(false);
            objUT.OutputQ.Should().Be(true);

            objUT.Data = true;
            objUT.OutputQ.Should().Be(true);
            objUT.Data = false;
            objUT.OutputQ.Should().Be(true);
            objUT.Data = true;
            objUT.OutputQ.Should().Be(true);
        }

        [Test]
        public void OutputQ_ShouldFollowDataProperty_WhenEnabledIsTrue()
        {
            var objUT = new GatedDLatchViewModel { Enable = true };
            objUT.Data.Should().Be(false);
            objUT.OutputQ.Should().Be(false);

            objUT.Data = true;
            objUT.OutputQ.Should().Be(true);
            objUT.Data = false;
            objUT.OutputQ.Should().Be(false);
            objUT.Data = true;
            objUT.OutputQ.Should().Be(true);
        }
    }
}
