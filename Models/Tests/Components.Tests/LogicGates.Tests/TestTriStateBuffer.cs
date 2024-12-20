﻿using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Components.LogicGates.Tests
{
    public class TestTriStateBuffer
    {
        [TestCase(false, false, null)]
        [TestCase(true, false, null)]
        [TestCase(false, true, false)]
        [TestCase(true, true, true)]
        public void TestLogic(bool inputA, bool inputB, bool? expectedOutputC)
        {
            TriStateBuffer triStateBuffer = new TriStateBuffer();
            triStateBuffer.SetInputA(inputA);
            triStateBuffer.SetInputB(inputB);
            triStateBuffer.OutputC.Should().Be(expectedOutputC);
        }

        [Test]
        public void ProbeInputA_ReturnsCurrentStateOfInputA()
        {
            TriStateBuffer triStateBuffer = new TriStateBuffer();
            triStateBuffer.OutputC.Should().Be(null);
            triStateBuffer.ProbeInputA().Should().Be(false);
            triStateBuffer.SetInputA(true);
            triStateBuffer.SetInputB(false);
            triStateBuffer.OutputC.Should().Be(null);
            triStateBuffer.ProbeInputA().Should().Be(true);
        }
    }
}
