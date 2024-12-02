using System;
using System.Linq;
using DigitalElectronics.Concepts;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Utilities.Tests
{
    public class BitConverterTests
    {
        [Test]
        public void GetBits_GivenByte_ShouldReturnBitArrayOfCountAndLength8()
        {
            var bitConverter = new BitConverter();
            var bitArray = bitConverter.GetBits((byte)0);
            bitArray.Count.Should().Be(8);
            bitArray.Length.Should().Be(8);
        }

        [Test]
        public void GetBits_GivenSByte_ShouldReturnBitArrayOfCountAndLength8()
        {
            var bitConverter = new BitConverter();
            var bitArray = bitConverter.GetBits((sbyte)0);
            bitArray.Count.Should().Be(8);
            bitArray.Length.Should().Be(8);
        }

        [Test]
        public void ToByte_ShouldConvertBitArrayToByte()
        {
            var bitConverter = new BitConverter();
            byte value = byte.MinValue;
            do
            {
                BitArray bitArray = bitConverter.GetBits(value);
                bitConverter.ToByte(bitArray).Should().Be(value);
            } while (value++ < byte.MaxValue);
        }

        [Test]
        public void ToSByte_ShouldConvertBitArrayToSByte()
        {
            var bitConverter = new BitConverter();
            sbyte value = sbyte.MinValue;
            do
            {
                BitArray bitArray = bitConverter.GetBits(value);
                bitConverter.ToSByte(bitArray).Should().Be(value);
            } while (value++ < sbyte.MaxValue);
        }

        [Test]
        public void ToUInt16_ShouldConvertBitArrayToUInt16()
        {
            var bitConverter = new BitConverter();
            ushort value = ushort.MinValue;
            do
            {
                BitArray bitArray = bitConverter.GetBits(value);
                bitConverter.ToUInt16(bitArray).Should().Be(value);
            } while (value++ < ushort.MaxValue);
        }

        [Test]
        public void ToInt16_ShouldConvertBitArrayToInt16()
        {
            var bitConverter = new BitConverter();
            short value = short.MinValue;
            do
            {
                BitArray bitArray = bitConverter.GetBits(value);
                bitConverter.ToInt16(bitArray).Should().Be(value);
            } while (value++ < short.MaxValue);
        }

        [Test]
        public void ToInt32_ShouldConvertBitArrayToInt32()
        {
            var bitConverter = new BitConverter();
            var random = new Random();
            
            // Select 1024 32-bit integers at random as it would take too long to test them all
            foreach (var _ in Enumerable.Range(0, 1024))
            {
                int value = random.Next(int.MinValue, int.MaxValue);
                BitArray bitArray = bitConverter.GetBits(value);
                bitConverter.ToInt32(bitArray).Should().Be(value);
            }
        }

        [Test]
        public void ToString_WhenGivenBitArrayContainingByteAndNumberFormatOfUnsignedDecimal_ShouldReturnValueAsString()
        {
            var bitConverter = new BitConverter();
            byte value = byte.MinValue;
            do
            {
                string valueAsString = Convert.ToString(value, 10);
                BitArray bitArray = bitConverter.GetBits(value);
                bitConverter.ToString(bitArray, NumberFormat.UnsignedDecimal).Should().Be(valueAsString);
            } while (value++ < byte.MaxValue);
        }

        [Test]
        public void ToString_WhenGivenBitArrayContainingSByteAndNumberFormatOfSignedDecimal_ShouldReturnValueAsString()
        {
            var bitConverter = new BitConverter();
            sbyte value = sbyte.MinValue;
            do
            {
                string valueAsString = Convert.ToString(value, 10);
                BitArray bitArray = bitConverter.GetBits(value);
                bitConverter.ToSByte(bitArray).Should().Be(value);
                bitConverter.ToString(bitArray, NumberFormat.SignedDecimal).Should().Be(valueAsString);
            } while (value++ < sbyte.MaxValue);
        }

        [Test]
        public void ToString_WhenGivenBitArrayContainingUInt16AndNumberFormatOfUnsignedDecimal_ShouldReturnValueAsString()
        {
            var bitConverter = new BitConverter();
            ushort value = ushort.MinValue;
            do
            {
                string valueAsString = Convert.ToString(value, 10);
                BitArray bitArray = bitConverter.GetBits(value);
                bitConverter.ToString(bitArray, NumberFormat.UnsignedDecimal).Should().Be(valueAsString);
            } while (value++ < ushort.MaxValue);
        }

        [Test]
        public void ToString_WhenGivenBitArrayContainingInt16AndNumberFormatOfSignedDecimal_ShouldReturnValueAsString()
        {
            var bitConverter = new BitConverter();
            short value = short.MinValue;
            do
            {
                string valueAsString = Convert.ToString(value, 10);
                BitArray bitArray = bitConverter.GetBits(value);
                bitConverter.ToString(bitArray, NumberFormat.SignedDecimal).Should().Be(valueAsString);
            } while (value++ < short.MaxValue);
        }

        [Test]
        public void ToString_WhenGivenBitArrayContainingByteAndNumberFormatOfUnsignedHexadecimal_ShouldReturnValueAsString()
        {
            var bitConverter = new BitConverter();
            byte value = byte.MinValue;
            do
            {
                string valueAsString = Convert.ToString(value, 16).ToUpperInvariant();
                BitArray bitArray = bitConverter.GetBits(value);
                bitConverter.ToString(bitArray, NumberFormat.UnsignedHexadecimal).Should().Be(valueAsString);
            } while (value++ < byte.MaxValue);
        }

        [Test]
        public void ToString_WhenGivenBitArrayContainingSByteAndNumberFormatOfHexadecimal_ShouldReturnValueAsString()
        {
            var bitConverter = new BitConverter();
            sbyte value = sbyte.MinValue;
            do
            {
                string valueAsString = Convert.ToString(value, 16).ToUpperInvariant();
                BitArray bitArray = bitConverter.GetBits(value);
                bitConverter.ToString(bitArray, NumberFormat.SignedHexadecimal).Should().Be(valueAsString);
            } while (value++ < sbyte.MaxValue);
        }

        [Test]
        public void ToString_WhenGivenBitArrayContainingUInt16AndNumberFormatOfUnsignedHexadecimal_ShouldReturnValueAsString()
        {
            var bitConverter = new BitConverter();
            ushort value = ushort.MinValue;
            do
            {
                string valueAsString = Convert.ToString(value, 16).ToUpperInvariant();
                BitArray bitArray = bitConverter.GetBits(value);
                bitConverter.ToString(bitArray, NumberFormat.UnsignedHexadecimal).Should().Be(valueAsString);
            } while (value++ < ushort.MaxValue);
        }

        [Test]
        public void ToString_WhenGivenBitArrayContainingInt16AndNumberFormatOfSignedHexadecimal_ShouldReturnValueAsString()
        {
            var bitConverter = new BitConverter();
            short value = short.MinValue;
            do
            {
                string valueAsString = Convert.ToString(value, 16).ToUpperInvariant();
                BitArray bitArray = bitConverter.GetBits(value);
                bitConverter.ToString(bitArray, NumberFormat.SignedHexadecimal).Should().Be(valueAsString);
            } while (value++ < short.MaxValue);
        }
    }
}
