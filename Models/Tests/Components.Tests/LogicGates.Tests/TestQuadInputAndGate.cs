using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Components.LogicGates.Tests
{
    public class TestQuadInputAndGate
    {
        [TestCase(false, false, false, false, false)]
        [TestCase(true, false, false, false, false)]
        [TestCase(false, true, false, false, false)]
        [TestCase(true, true, false, false, false)]
        [TestCase(false, false, true, false, false)]
        [TestCase(true, false, true, false, false)]
        [TestCase(false, true, true, false, false)]
        [TestCase(true, true, true, false, false)]
        [TestCase(false, false, false, true, false)]
        [TestCase(true, false, false, true, false)]
        [TestCase(false, true, false, true, false)]
        [TestCase(true, true, false, true, false)]
        [TestCase(false, false, true, true, false)]
        [TestCase(true, false, true, true, false)]
        [TestCase(false, true, true, true, false)]
        [TestCase(true, true, true, true, true)]
        public void TestLogic(bool a, bool b, bool c, bool d, bool expectedQ)
        {
            var _4inAndGate = new QuadInputAndGate();
            _4inAndGate.SetInputA(a);
            _4inAndGate.SetInputB(b);
            _4inAndGate.SetInputC(c);
            _4inAndGate.SetInputD(d);
            _4inAndGate.OutputQ.Should().Be(expectedQ);
        }
    }
}
