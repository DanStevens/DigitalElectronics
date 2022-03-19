using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Components.LogicGates.Tests
{
    public class TestAndGate
    {
        [TestCase(false, false, false)]
        [TestCase(false, true, false)]
        [TestCase(true, false, false)]
        [TestCase(true, true, true)]
        public void TestLogic(bool inputA, bool inputB, bool outputQExepected)
        {
            var and = new AndGate();
            and.SetInputA(inputA);
            and.SetInputB(inputB);
            and.OutputQ.Should().Be(outputQExepected);
        }
    }
}