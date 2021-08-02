using FluentAssertions;

namespace DigitalElectronics.Components.FlipFlops.Tests
{
    public class TestSRLatchBase
    {
        protected ISRLatch _srLatch;

        protected void PushR() => _srLatch.SetInputR(true);
        protected void PushS() => _srLatch.SetInputS(true);
        protected void ReleaseR() => _srLatch.SetInputR(false);
        protected void ReleaseS() => _srLatch.SetInputS(false);

        protected void AssertOutputs(bool outputQExpected, bool outputNQExpected)
        {
            _srLatch.AssertOutputs(outputQExpected, outputNQExpected);
        }
    }
}