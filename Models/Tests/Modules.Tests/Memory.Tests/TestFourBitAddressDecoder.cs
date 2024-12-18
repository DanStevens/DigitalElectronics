using System;
using DigitalElectronics.Concepts;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace DigitalElectronics.Modules.Memory.Tests
{
    
    public class TestFourBitAddressDecoder
    {
        private const int NumberOfAddressBits = 4;
        private readonly int NumberOfOutputs = (int)Math.Pow(2, NumberOfAddressBits);

        private FourBitAddressDecoder _decoder;

        [SetUp]
        public void SetUp()
        {
            _decoder = new FourBitAddressDecoder();
        }

        [Test]
        public void NumberOfOutputs_ShouldBe16()
        {
            FourBitAddressDecoder.NumberOfOutputs.Should().Be(NumberOfOutputs);
        }

        [Test]
        public void OutputY_ShouldHaveLength16()
        {
            _decoder.OutputY.Length.Should().Be(NumberOfOutputs);
        }

        [Test]
        public void OutputY_ShouldBe0ByDefault()
        {
            _decoder.OutputY.ToInt32().Should().Be(0);
        }

        [Test]
        public void TestAllAddresses()
        {
            // For every possible address
            for (int a = 0; a < NumberOfOutputs; a++)
            {
                AssertAddress(a);
            }
        }

        private void AssertAddress(int a)
        {
            var address = new BitArray(a, NumberOfAddressBits);
            var expectedOutput = new BitArray((int)Math.Pow(2, a), NumberOfOutputs);
            _decoder.SetInputA(address);
            ClassicAssert.AreEqual(expectedOutput, _decoder.OutputY, $"a = {a}");
        }
    }
}
