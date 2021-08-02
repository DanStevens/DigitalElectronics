using DigitalElectronics.Components.LogicGates;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Components.Tests
{
    public class TestBuffer
    {
        [TestCase(false, false)]
        [TestCase(true, true)]
        public void TestLogic(bool inputA, bool outputQExpected)
        {
            Buffer buffer = new Buffer();
            buffer.SetInputA(inputA);
            buffer.OutputQ.Should().Be(outputQExpected);
        }
    }
}
