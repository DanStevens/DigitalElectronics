using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Components.Switching.Tests
{
    public class TestTwoToOneMux
    {
        [Test]
        public void InitialState()
        {
            var twoToOneMux = new TwoToOneMux();
            twoToOneMux.OutputZ.Should().Be(false);
        }

        [TestCase(false, false, false, false)]
        [TestCase(false, false, true, false)]
        [TestCase(false, true, false, true)]
        [TestCase(false, true, true, true)]
        [TestCase(true, false, false, false)]
        [TestCase(true, false, true, true)]
        [TestCase(true, true, false, false)]
        [TestCase(true, true, true, true)]
        public void Test(bool inputS0, bool inputA, bool inputB, bool outputZ)
        {
            var twoToOneMux = new TwoToOneMux();
            twoToOneMux.SetInputSel(inputS0);
            twoToOneMux.SetInputA(inputA);
            twoToOneMux.SetInputB(inputB);
            twoToOneMux.OutputZ.Should().Be(outputZ);
        }
    }
}
