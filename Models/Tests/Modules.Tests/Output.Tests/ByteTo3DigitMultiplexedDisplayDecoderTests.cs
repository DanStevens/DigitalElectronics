using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Output;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Modules.Tests.Output.Tests
{
    
    /// <summary>
    /// Tests for 3 digit signed/unsigned byte multiplexed display decoder
    /// </summary>
    public class ByteTo3DigitMultiplexedDisplayDecoderTests
    {
        [TestCase(0, 0x3F)]
        [TestCase(1, 0x06)]
        [TestCase(2, 0x5B)]
        [TestCase(9, 0x6F)]
        public void Output_ShouldBeSetForCC7SegmentDigit_WhenInputIsSetToValue(byte input, byte expectedOutput)
        {
            var objUT = new ByteTo3DigitMultiplexedDisplayDecoder();
            objUT.SetInput(new BitArray(input));
            objUT.Output.ToByte().Should().Be(expectedOutput);
        }
    }
}
