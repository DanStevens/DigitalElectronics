using System.Collections.Generic;
using DigitalElectronics.Concepts;
using DigitalElectronics.Utilities;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Modules.Memory.Tests
{
    public class TestTwoBitAddressDecoder
    {
        BitConverter _bitConverter = new BitConverter(Endianness.Little);

        [TestCase(false, false, 0b0001)]
        [TestCase(true, false, 0b0010)]
        [TestCase(false, true, 0b0100)]
        [TestCase(true, true, 0b1000)]
        public void Test(bool a0, bool a1, byte expectedY)
        {
            var decoder = new TwoBitAddressDecoder();
            decoder.SetInputA0(a0);
            decoder.SetInputA1(a1);
            var expectedOutput = _bitConverter.GetBits(expectedY, 4);
            decoder.OutputY.Should().BeEquivalentTo(expectedOutput.AsList<bool>());
        }
    }
}












// fluent assertions should beequivalentto class implements IEnumerable<X> and IEnumerable<Y> so cannot determine which on to use for asserting the equivalency of the collection
