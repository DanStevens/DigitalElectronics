using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Components.Memory.Tests
{
    public class TestRegister
    {
        Register _register;

        [SetUp]
        public void SetUp()
        {
            _register = new Register();
            AssertOutput(null);
            _register.SetInputE(true);
            AssertOutput(true);
        }

        [Test]
        public void AHighOutputDoesNotChange_WhenClocking_IfInputLIsLow()
        {
            ReleaseL(); AssertOutput(true);

            ReleaseD(); AssertOutput(true);
            Clock();    AssertOutput(true);
            PushD();    AssertOutput(true);
            Clock();    AssertOutput(true);

            ReleaseD(); AssertOutput(true);
            Clock();    AssertOutput(true);
            PushD();    AssertOutput(true);
            Clock();    AssertOutput(true);
        }

        [Test]
        public void ALowOutputDoesNotChange_WhenClocking_IfInputLIsLow()
        {
            // Get output to go low 
            PushL();    AssertOutput(true);
            Clock();    AssertOutput(false);
            ReleaseL(); AssertOutput(false);

            // Now test
            ReleaseD(); AssertOutput(false);
            Clock();    AssertOutput(false);
            PushD();    AssertOutput(false);
            Clock();    AssertOutput(false);

            // Repeat test
            ReleaseD(); AssertOutput(false);
            Clock();    AssertOutput(false);
            PushD();    AssertOutput(false);
            Clock();    AssertOutput(false);
        }

        [Test]
        public void OutputDoesChange_WhenClocking_IfInputLIsHigh()
        {
            PushL();    AssertOutput(true);
            Clock();    AssertOutput(false);
            Clock();    AssertOutput(false);

            PushD();    AssertOutput(false);
            Clock();    AssertOutput(true);
            ReleaseD(); AssertOutput(true);
            Clock();    AssertOutput(false);

            PushD();    AssertOutput(false);
            Clock();    AssertOutput(true);
            ReleaseD(); AssertOutput(true);
            Clock();    AssertOutput(false);
        }


        [Test]
        public void OutputRises_WhenClocking_IfInputDAndInputLAreHigh()
        {
            // Get output to go low 
            PushL();    AssertOutput(true);
            Clock();    AssertOutput(false);
            ReleaseL(); AssertOutput(false);

            PushD();    AssertOutput(false);
            PushL();    AssertOutput(false);
            Clock();    AssertOutput(true);
        }

        [Test]
        public void OutputFalls_WhenClocking_IfInputDIsLowAndInputLIsHigh()
        {
            ReleaseD(); AssertOutput(true);
            PushL();    AssertOutput(true);
            Clock();    AssertOutput(false);
        }

        [Test]
        public void CombinationTest()
        {
            AssertOutput(true);

            PushL();    AssertOutput(true);
            Clock();    AssertOutput(false);
            ReleaseL(); AssertOutput(false);

            PushL();    AssertOutput(false);
            PushD();    AssertOutput(false);
            Clock();    AssertOutput(true);
            ReleaseL(); AssertOutput(true);

            Clock();    AssertOutput(true);

            PushL();    AssertOutput(true);
            Clock();    AssertOutput(true);
            ReleaseD(); AssertOutput(true);
            Clock();    AssertOutput(false);
            ReleaseL(); AssertOutput(false);
            PushD();    AssertOutput(false);
            Clock();    AssertOutput(false);

            PushL();    AssertOutput(false);
            Clock();    AssertOutput(true);
        }

        [Test]
        public void OutputIsNullWhenInputEIsLow()
        {
            AssertOutput(true);
            ReleaseE(); AssertOutput(null);
            PushE();    AssertOutput(true);
            ReleaseE(); AssertOutput(null);

            PushL();    AssertOutput(null);
            ReleaseD(); AssertOutput(null);
            Clock();    AssertOutput(null);
            PushE();    AssertOutput(false);
        }

        [Test]
        public void ProbeState_ReturnsInternalState()
        {
            ReleaseE();
            AssertOutput(null);
            _register.ProbeState().Should().Be(true);

            PushL();    AssertOutput(null);
            ReleaseD(); AssertOutput(null);
            Clock();    AssertOutput(null);
            _register.ProbeState().Should().Be(false);
        }

        private void PushE() => _register.SetInputE(true);
        private void ReleaseE() => _register.SetInputE(false);
        private void PushL() => _register.SetInputL(true);
        private void ReleaseL() => _register.SetInputL(false);
        private void PushD() => _register.SetInputD(true);
        private void ReleaseD() => _register.SetInputD(false);
        private void Clock() => _register.Clock();
        private void AssertOutput(bool? v) => _register.OutputQ.Should().Be(v);
    }
}
