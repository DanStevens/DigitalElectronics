using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Components.LogicGates.Tests
{
    public class TestTripleInputAndGate
    {
        [TestCase(false, false, false, false)]
        [TestCase(true, false, false, false)]
        [TestCase(false, true, false, false)]
        [TestCase(true, true, false, false)]
        [TestCase(false, false, true, false)]
        [TestCase(true, false, true, false)]
        [TestCase(false, true, true, false)]
        [TestCase(true, true, true, true)]
        public void TestLogic(bool a, bool b, bool c, bool expectedQ)
        {
            var _3inAndGate = new TripleInputAndGate();
            _3inAndGate.SetInputA(a);
            _3inAndGate.SetInputB(b);
            _3inAndGate.SetInputC(c);
            _3inAndGate.OutputQ.Should().Be(expectedQ);
        }
    }
}
