using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalElectronics.Components.ALUs.Tests
{
    public class TestFullAdder
    {
        [TestCase(false, false, false, false, false)]
        [TestCase(false, false, true, false, true)]
        [TestCase(false, true, false, false, true)]
        [TestCase(false, true, true, true, false)]
        [TestCase(true, false, false, false, true)]
        [TestCase(true, false, true, true, false)]
        [TestCase(true, true, true, true, true)]
        public void TestLogic(bool inputA, bool inputB, bool inputC, bool expectedOutputC, bool expectedOutputS)
        {
            FullAdder fullAdder = new FullAdder();
            fullAdder.SetInputA(inputA);
            fullAdder.SetInputB(inputB);
            fullAdder.SetInputC(inputC);
            fullAdder.OutputC.Should().Be(expectedOutputC, nameof(fullAdder.OutputC));
            fullAdder.OutputS.Should().Be(expectedOutputS, nameof(fullAdder.OutputS));
        }
    }
}
