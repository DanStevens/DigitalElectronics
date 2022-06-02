using DigitalElectronics.Concepts;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace DigitalElectronics.Modules.Output.Tests
{
    
    /// <summary>
    /// Tests for 4 digit signed/unsigned byte multiplexed display decoder
    /// </summary>
    public class ByteTo4DigitMultiplexedDisplayDecoderTests
    {
        [TestCase(0, 0x3F)]
        [TestCase(1, 0x06)]
        [TestCase(2, 0x5B)]
        [TestCase(9, 0x6F)]
        public void Output_ShouldBeSetForCC7SegmentDigit_WhenInputIsSetToValue(byte input, byte expectedOutput)
        {
            var objUT = new ByteTo4DigitMultiplexedDisplayDecoder();
            objUT.SetInput(new BitArray(input));
            objUT.Output.ToByte().Should().Be(expectedOutput);
        }

        [Test]
        public void Output_ShouldCycleBetweenDigits000_WhenInputIsZeroAndClockIsCalled()
        {
            var objUT = new ByteTo4DigitMultiplexedDisplayDecoder();
            objUT.SetInput(new BitArray((byte)0));

            using (new AssertionScope())
            {
                objUT.Output.ToByte().Should().Be(0x3F, "1s digit");
                objUT.Clock();
                objUT.Output.ToByte().Should().Be(0x3F, "10s digit");
                objUT.Clock();
                objUT.Output.ToByte().Should().Be(0x3F, "100s digit");
                objUT.Clock();
                objUT.Output.ToByte().Should().Be(0x0, "1000s digit");
                objUT.Clock();
                objUT.Output.ToByte().Should().Be(0x3F, "1s digit");
            }
        }

        [Test]
        public void Output_ShouldCycleBetweenDigits001_WhenInputIsOneAndClockIsCalled()
        {
            var objUT = new ByteTo4DigitMultiplexedDisplayDecoder();
            objUT.SetInput(new BitArray((byte)1));

            using (new AssertionScope())
            {
                objUT.Output.ToByte().Should().Be(0x06, "1s digit");
                objUT.Clock();
                objUT.Output.ToByte().Should().Be(0x3F, "10s digit");
                objUT.Clock();
                objUT.Output.ToByte().Should().Be(0x3F, "100s digit");
                objUT.Clock();
                objUT.Output.ToByte().Should().Be(0x0, "1000s digit");
                objUT.Clock();
                objUT.Output.ToByte().Should().Be(0x06, "1s digit");
            }
        }
    }
}
