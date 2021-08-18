using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Modules.Memory.Tests
{
    public class TestTwoBitAddressDecoder
    {

        [TestCase(false, false, true, false, false, false)]
        [TestCase(true, false, false, true, false, false)]
        [TestCase(false, true, false, false, true, false)]
        [TestCase(true, true, false, false, false, true)]
        public void Test(bool a0, bool a1, bool y0, bool y1, bool y2, bool y3)
        {
            var decoder = new TwoBitAddressDecoder();
            decoder.SetInputA0(a0);
            decoder.SetInputA1(a1);
            decoder.OutputY0.Should().Be(y0);
            decoder.OutputY1.Should().Be(y1);
            decoder.OutputY2.Should().Be(y2);
            decoder.OutputY3.Should().Be(y3);
        }
    }
}
