using System.Collections.Generic;
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
        private readonly Dictionary<byte, string> segmentsToDigit = new()
        {
            { 0x00, "blank"}, { 0x3F, "'0'"}, { 0x06, "'1'"}, { 0x5B, "'2'"}, { 0x4F, "'3'"}, { 0x66, "'4'"},
            { 0x6D, "'5'"}, { 0x7D, "'6'"}, { 0x07, "'7'"}, { 0x7F, "'8'"}, { 0x6F, "'9'"},
        };

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

        // Using string for `input` parameter so that tests are listed in the correct order in
        // Resharper's Unit Test window
        [TestCase("000", 0x3F, 0x3F, 0x3F, 0x0)]
        [TestCase("001", 0x06, 0x3F, 0x3F, 0x0)]
        [TestCase("002", 0x5B, 0x3F, 0x3F, 0x0)]
        [TestCase("003", 0x4F, 0x3F, 0x3F, 0x0)]
        [TestCase("004", 0x66, 0x3F, 0x3F, 0x0)]
        [TestCase("005", 0x6D, 0x3F, 0x3F, 0x0)]
        [TestCase("006", 0x7D, 0x3F, 0x3F, 0x0)]
        [TestCase("007", 0x07, 0x3F, 0x3F, 0x0)]
        [TestCase("008", 0x7F, 0x3F, 0x3F, 0x0)]
        [TestCase("009", 0x6F, 0x3F, 0x3F, 0x0)]
        [TestCase("010", 0x3F, 0x06, 0x3F, 0x0)]
        [TestCase("011", 0x06, 0x06, 0x3F, 0x0)]
        [TestCase("012", 0x5B, 0x06, 0x3F, 0x0)]
        [TestCase("018", 0x7F, 0x06, 0x3F, 0x0)]
        [TestCase("019", 0x6F, 0x06, 0x3F, 0x0)]
        [TestCase("020", 0x3F, 0x5B, 0x3F, 0x0)]
        [TestCase("028", 0x7F, 0x5B, 0x3F, 0x0)]
        [TestCase("029", 0x6F, 0x5B, 0x3F, 0x0)]
        [TestCase("030", 0x3F, 0x4F, 0x3F, 0x0)]
        [TestCase("099", 0x6F, 0x6F, 0x3F, 0x0)]
        [TestCase("100", 0x3F, 0x3F, 0x06, 0x0)]
        [TestCase("199", 0x6F, 0x6F, 0x06, 0x0)]
        [TestCase("200", 0x3F, 0x3F, 0x5B, 0x0)]
        [TestCase("254", 0x66, 0x6D, 0x5B, 0x0)]
        [TestCase("255", 0x6D, 0x6D, 0x5B, 0x0)]
        public void Output_ShouldCycleBetweenDigits_WhenClockIsCalled(
            string input,
            byte position1sExpected,
            byte position10sExpected,
            byte position100sExpected,
            byte position1000sExpected)
        {
            var objUT = new ByteTo4DigitMultiplexedDisplayDecoder();
            objUT.SetInput(new BitArray(byte.Parse(input)));

            using (new AssertionScope())
            {
                AssertDigit(objUT.Output.ToByte(), position1sExpected, "1s");
                objUT.Clock();
                AssertDigit(objUT.Output.ToByte(), position10sExpected, "10s");
                objUT.Clock();
                AssertDigit(objUT.Output.ToByte(), position100sExpected, "100s");
                objUT.Clock();
                AssertDigit(objUT.Output.ToByte(), position1000sExpected, "1000s");
                objUT.Clock();
                AssertDigit(objUT.Output.ToByte(), position1sExpected, "1s");
                objUT.Clock();
                AssertDigit(objUT.Output.ToByte(), position10sExpected, "10s");
                objUT.Clock();
                AssertDigit(objUT.Output.ToByte(), position100sExpected, "100s");
                objUT.Clock();
                AssertDigit(objUT.Output.ToByte(), position1000sExpected, "1000s");
            }
        }

        [Test]
        public void DigitActivateStates_ShouldCycleActiveDigit_WhenClockIsCalled()
        {
            using (new AssertionScope())
            {
                var objUT = new ByteTo4DigitMultiplexedDisplayDecoder();
                objUT.DigitActivateStates.Should().HaveCount(4);
                objUT.DigitActivateStates.Should().ContainInOrder(true, false, false, false);
                objUT.Clock();
                objUT.DigitActivateStates.Should().ContainInOrder(false, true, false, false);
                objUT.Clock();
                objUT.DigitActivateStates.Should().ContainInOrder(false, false, true, false);
                objUT.Clock();
                objUT.DigitActivateStates.Should().ContainInOrder(false, false, false, true);
                objUT.Clock();
                objUT.DigitActivateStates.Should().ContainInOrder(true, false, false, false); 
            }
        }

        private void AssertDigit(byte digit, byte expected, string position)
        {
            digit.Should().Be(expected, $"digit {segmentsToDigit[expected]} should be in the {position} position");
        }
    }
}
