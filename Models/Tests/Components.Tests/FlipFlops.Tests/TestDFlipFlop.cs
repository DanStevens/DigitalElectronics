using DigitalElectronics.Components.FlipFlops;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Components.FlipFlops.Tests
{
    public class TestDFlipFlop
    {
        private DFlipFlop _dFlipFlop;

        [SetUp]
        public void SetUp()
        {
            _dFlipFlop = new DFlipFlop();
        }

        [Test]
        public void TestInitialState()
        {
            _dFlipFlop.OutputQ.Should().Be(true);
            _dFlipFlop.OutputNQ.Should().Be(false);
        }

        [Test]
        public void Test()
        {
            _dFlipFlop.AssertOutputs(true, false);

            ReleaseD(); _dFlipFlop.AssertOutputs(true, false);
            Clock();    _dFlipFlop.AssertOutputs(false, true);
            PushD();    _dFlipFlop.AssertOutputs(false, true);
            ReleaseD(); _dFlipFlop.AssertOutputs(false, true);

            PushD();    _dFlipFlop.AssertOutputs(false, true);
            Clock();    _dFlipFlop.AssertOutputs(true, false);
            ReleaseD(); _dFlipFlop.AssertOutputs(true, false);

            PushD();    _dFlipFlop.AssertOutputs(true, false);
            Clock();    _dFlipFlop.AssertOutputs(true, false);
            ReleaseD(); _dFlipFlop.AssertOutputs(true, false);

            Clock();    _dFlipFlop.AssertOutputs(false, true);
            PushD();    _dFlipFlop.AssertOutputs(false, true);
            ReleaseD(); _dFlipFlop.AssertOutputs(false, true);
        }

        private void Clock() => _dFlipFlop.Clock();
        private void PushD() => _dFlipFlop.SetInputD(true);
        private void ReleaseD() => _dFlipFlop.SetInputD(false);
    }
}
