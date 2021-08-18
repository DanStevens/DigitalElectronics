using System.Collections;
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
            var expectedOutput = new BitArray(new bool[] { y0, y1, y2, y3 });
            decoder.OutputY.Should().BeEquivalentTo(expectedOutput);
        }
    }
}
