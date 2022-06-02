using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.ViewModels.Modules.Tests
{
    public class FourDigit7SegmentDisplayViewModelTests
    {
        [Test]
        public void DoNothing()
        {
            var objUT = new FourDigit7SegmentDisplayViewModel();
            objUT.Should().NotBeNull();
        }
    }
}
