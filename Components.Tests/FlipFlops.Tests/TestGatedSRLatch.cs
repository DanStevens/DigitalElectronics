using System;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Components.FlipFlops.Tests
{
    public class TestGatedSRLatch : TestSRLatchBase
    {
        private GatedSRLatch _gatedSRLatch;

        [SetUp]
        public void SetUp()
        {
            _gatedSRLatch = new GatedSRLatch();
            _srLatch = _gatedSRLatch;
        }

        [Test]
        public void TestInitialState()
        {
            _gatedSRLatch.OutputQ.Should().Be(true);
            _gatedSRLatch.OutputNQ.Should().Be(false);
        }

        [Test]
        public void Test1()
        {
            AssertOutputs(true, false);

            PushR();    AssertOutputs(true, false);
            ReleaseR(); AssertOutputs(true, false);

            PushS();    AssertOutputs(true, false);
            ReleaseS(); AssertOutputs(true, false);

            PushE();    AssertOutputs(true, false);

            PushR();    AssertOutputs(false, true);
            ReleaseR(); AssertOutputs(false, true);

            PushS();    AssertOutputs(true, false);
            ReleaseS(); AssertOutputs(true, false);

            PushR();    AssertOutputs(false, true);
            ReleaseR(); AssertOutputs(false, true);

            ReleaseE(); AssertOutputs(false, true);

            PushS();    AssertOutputs(false, true);
            ReleaseS(); AssertOutputs(false, true);
        }

        [Test]
        public void Test2()
        {
            AssertOutputs(true, false);

            PushE();    AssertOutputs(true, false);
            PushR();    AssertOutputs(false, true);
            ReleaseR(); AssertOutputs(false, true);
            ReleaseE(); AssertOutputs(false, true);

            PushS();    AssertOutputs(false, true);
            PushE();    AssertOutputs(true, false);
            ReleaseE(); AssertOutputs(true, false);
            ReleaseS(); AssertOutputs(true, false);

            PushR();    AssertOutputs(true, false);
            PushE();    AssertOutputs(false, true);
            ReleaseE(); AssertOutputs(false, true);
            ReleaseR(); AssertOutputs(false, true);

            PushS();    AssertOutputs(false, true);
            ReleaseS(); AssertOutputs(false, true);
        }

        private void PushE() => _gatedSRLatch.SetInputE(true);
        private void ReleaseE() => _gatedSRLatch.SetInputE(false);
    }
}
