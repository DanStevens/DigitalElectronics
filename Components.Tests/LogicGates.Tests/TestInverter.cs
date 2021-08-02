using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Components.LogicGates.Tests
{

    public class TestInverter
    {
        [TestCase(false, true)]
        [TestCase(true, false)]
        public void TestLogic(bool inputA, bool outputQExpected)
        {
            Inverter inverter = new Inverter();
            inverter.SetInputA(inputA);
            inverter.OutputQ.Should().Be(outputQExpected);
        }
    }
}
