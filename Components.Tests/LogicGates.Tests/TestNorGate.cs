using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Components.LogicGates.Tests
{
    public class TestNorGate
    {
        [TestCase(false, false, true)]
        [TestCase(false, true, false)]
        [TestCase(true, false, false)]
        [TestCase(true, true, false)]
        public void TestLogic(bool inputA, bool inputB, bool outputQExepected)
        {
            var nor = new NorGate();
            nor.SetInputA(inputA);
            nor.SetInputB(inputB);
            nor.OutputQ.Should().Be(outputQExepected);
        }
    }
}