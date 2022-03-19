using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalElectronics.Components.ALUs.Tests
{
    public class TestHalfAdder
    {
        [TestCase(false, false, false, false)]
        [TestCase(false, true, false, true)]
        [TestCase(true, false, false, true)]
        [TestCase(true, true, true, false)]
        public void TestLogic(bool inputA, bool inputB, bool expectedOutputC, bool expectedOutputS)
        {
            HalfAdder halfAdder = new HalfAdder();
            halfAdder.SetInputA(inputA);
            halfAdder.SetInputB(inputB);
            halfAdder.OutputC.Should().Be(expectedOutputC);
            halfAdder.OutputE.Should().Be(expectedOutputS);
        }
    }
}
