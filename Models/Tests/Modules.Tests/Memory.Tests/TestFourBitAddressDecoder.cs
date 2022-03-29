using System;
using DigitalElectronics.Concepts;
using FluentAssertions;
using NUnit.Framework;
using BitConverter = DigitalElectronics.Utilities.BitConverter;

namespace DigitalElectronics.Modules.Memory.Tests
{
    
    public class TestFourBitAddressDecoder
    {
        private const int NumberOfAddressBits = 4;
        private readonly int NumberOfOutputs = (int)Math.Pow(2, NumberOfAddressBits);
        
        private BitConverter _bitConverter;
        private FourBitAddressDecoder _decoder;

        [SetUp]
        public void SetUp()
        {
            _bitConverter = new BitConverter(Endianness.Little);
            _decoder = new FourBitAddressDecoder();
        }

        [Test]
        public void NumberOfOutputs_ShouldBe16()
        {
            FourBitAddressDecoder.NumberOfOutputs.Should().Be(NumberOfOutputs);
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
            var address = _bitConverter.GetBits(a, NumberOfAddressBits);
            var expectedOutput = _bitConverter.GetBits((int)Math.Pow(2, a), NumberOfOutputs);
            _decoder.SetInputA(address);
            Assert.AreEqual(expectedOutput, _decoder.OutputY, $"a = {a}");
        }
    }
}
