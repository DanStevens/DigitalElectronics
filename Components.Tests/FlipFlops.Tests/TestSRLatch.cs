using NUnit.Framework;
using FluentAssertions;

namespace DigitalElectronics.Components.FlipFlops.Tests
{

    public class TestSRLatch : TestSRLatchBase
    {
        [SetUp]
        public void SetUp()
        {
            _srLatch = new SRLatch();
        }

        [Test]
        public void TestInitialState()
        {
            _srLatch.OutputQ.Should().Be(true);
            _srLatch.OutputNQ.Should().Be(false);
        }

        [TestCase(false, false, true, false)]
        [TestCase(false, true, true, false)]
        [TestCase(true, false, false, true)]
        public void TestInitialLogic(bool inputR, bool inputS, bool outputQExpected, bool outputNQExpected)
        {
            _srLatch.SetInputR(inputR);
            _srLatch.SetInputS(inputS);
            _srLatch.OutputQ.Should().Be(outputQExpected);
            _srLatch.OutputNQ.Should().Be(outputNQExpected);
        }

        [Test]
        public void Test()
        {
            PushR();    AssertOutputs(false, true);
            ReleaseR(); AssertOutputs(false, true);

            PushR();    AssertOutputs(false, true);
            ReleaseR(); AssertOutputs(false, true);

            PushS();    AssertOutputs(true, false);
            ReleaseS(); AssertOutputs(true, false);

            PushS();    AssertOutputs(true, false);
            ReleaseS(); AssertOutputs(true, false);

            PushR(); ReleaseR();
            AssertOutputs(false, true);

            PushS(); ReleaseS();
            AssertOutputs(true, false);
        }
    }
}
