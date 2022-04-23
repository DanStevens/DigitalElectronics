using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Components.FlipFlops.Tests
{
    public class TestJKFlipFlop
    {
        [Test]
        public void InitialState()
        {
            var jkFlipFlop = new JKFlipFlop();
            jkFlipFlop.OutputQ.Should().Be(true);
            jkFlipFlop.OutputNQ.Should().Be(false);
        }

        [TestCase(false, false, true, false, false, true)]  // Hold, Hold
        [TestCase(false, false, true, false, true, false)]  // Hold, Reset
        [TestCase(false, false, true, true, false, true)]   // Hold, Set
        [TestCase(false, false, true, true, true, false)]   // Hold, Toggle
        [TestCase(false, true, false, false, false, false)] // Reset, Hold
        [TestCase(false, true, false, false, true, false)]  // Reset, Reset
        [TestCase(false, true, false, true, false, true)]   // Reset, Set
        [TestCase(false, true, false, true, true, true)]    // Reset, Toggle
        [TestCase(true, false, true, false, false, true)]   // Set, Hold
        [TestCase(true, false, true, false, true, false)]   // Set, Reset
        [TestCase(true, false, true, true, false, true)]    // Set, Set
        [TestCase(true, false, true, true, true, false)]    // Set, Toggle
        [TestCase(true, true, false, false, false, false)]  // Toggle, Hold
        [TestCase(true, true, false, false, true, false)]   // Toggle, Reset
        [TestCase(true, true, false, true, false, true)]    // Toggle, Set
        [TestCase(true, true, false, true, true, true)]     // Toggle, Toggle
        public void HoldState(bool j1, bool k1, bool q1, bool j2, bool k2, bool q2)
        {
            var jkFlipFlop = new JKFlipFlop();

            jkFlipFlop.SetInputJ(j1);
            jkFlipFlop.SetInputK(k1);
            jkFlipFlop.Clock();
            jkFlipFlop.OutputQ.Should().Be(q1);
            jkFlipFlop.OutputNQ.Should().Be(!q1);

            jkFlipFlop.SetInputJ(j2);
            jkFlipFlop.SetInputK(k2);
            jkFlipFlop.Clock();
            jkFlipFlop.OutputQ.Should().Be(q2);
            jkFlipFlop.OutputNQ.Should().Be(!q2);
        }
    }
}
