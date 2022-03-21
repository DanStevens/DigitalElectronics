using DigitalElectronics.Components.FlipFlops;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Components.FlipFlops.Tests
{
    public class TestGatedDLatch
    {
        private GatedDLatch _gatedDLatch;

        [SetUp]
        public void SetUp()
        {
            _gatedDLatch = new GatedDLatch();
        }

        [Test]
        public void TestInitialState()
        {
            _gatedDLatch.OutputQ.Should().Be(true);
            _gatedDLatch.OutputNQ.Should().Be(false);
        }

        [Test]
        public void Test()
        {
            _gatedDLatch.AssertOutputs(true, false);

            PushE(); _gatedDLatch.AssertOutputs(false, true);
            PushD(); _gatedDLatch.AssertOutputs(true, false);
            ReleaseD(); _gatedDLatch.AssertOutputs(false, true);
            ReleaseE(); _gatedDLatch.AssertOutputs(false, true);

            PushE(); _gatedDLatch.AssertOutputs(false, true);
            ReleaseE(); _gatedDLatch.AssertOutputs(false, true);
            PushE(); _gatedDLatch.AssertOutputs(false, true);
            PushD(); _gatedDLatch.AssertOutputs(true, false);
            ReleaseE(); _gatedDLatch.AssertOutputs(true, false);
            ReleaseD(); _gatedDLatch.AssertOutputs(true, false);

            PushE(); _gatedDLatch.AssertOutputs(false, true);
            ReleaseE(); _gatedDLatch.AssertOutputs(false, true);
            PushD(); _gatedDLatch.AssertOutputs(false, true);
            ReleaseD(); _gatedDLatch.AssertOutputs(false, true);
        }

        private void PushD() => _gatedDLatch.SetInputD(true);
        private void ReleaseD() => _gatedDLatch.SetInputD(false);
        private void PushE() => _gatedDLatch.SetInputE(true);
        private void ReleaseE() => _gatedDLatch.SetInputE(false);
    }
}
