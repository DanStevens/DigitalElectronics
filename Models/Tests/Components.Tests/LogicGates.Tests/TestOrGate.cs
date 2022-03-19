using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Components.LogicGates.Tests
{
    public class TestOrGate
    {
        [TestCase(false, false, false)]
        [TestCase(false, true, true)]
        [TestCase(true, false, true)]
        [TestCase(true, true, true)]
        public void TestLogic(bool inputA, bool inputB, bool outputQExepected)
        {
            var and = new OrGate();
            and.SetInputA(inputA);
            and.SetInputB(inputB);
            and.OutputQ.Should().Be(outputQExepected);
        }
    }
}