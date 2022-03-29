using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;
using DigitalElectronics.Concepts;

[assembly: DebuggerDisplay("BitArray={DigitalElectronics.Utilities.Extensions.ToString(this)}", Target = typeof(BitArray))]

namespace DigitalElectronics.Utilities.Tests
{
    public class BitArrayComparerTests
    {
        [Test]
        public void Compare_GivenSameObject_ShouldReturnZero()
        {
            var bitArray = new BitArray(byte.MaxValue);
            var comparer = new BitArrayComparer();
            comparer.Compare(bitArray, bitArray);
        }

        [Test]
        public void Compare_NullComparedWithBitArray_ShouldReturnNeg1()
        {
            var comparer = new BitArrayComparer();
            comparer.Compare(new BitArray(0), null).Should().Be(1);
        }

        [Test]
        public void Compare_NullComparedWithBitArray_ShouldReturnPos1()
        {
            var comparer = new BitArrayComparer();
            comparer.Compare(null, new BitArray(0)).Should().Be(-1);
        }

        [Test]
        public void Compare_BitArrayComparedWithBitArray_ShouldReturnTBD()
        {
            var comparer = new BitArrayComparer();
            var bitConverter = new BitConverter();

            for (byte x = 0; x < 255; x++)
            {
                for (byte y = 0; y < 255; y++)
                {
                    var bitArrayX = bitConverter.GetBits(x);
                    var bitArrayY = bitConverter.GetBits(y);
                    var expected = x.CompareTo(y);

                    var result = comparer.Compare(bitArrayX, bitArrayY);
                    result.Should().Be(expected);
                }
            }
        }

        [Test]
        public void BitArrayToByte_ShouldConvertBitArrayBackToByte()
        {
            for (byte x = 0; x < 255; x++)
            {
                var ba = new BitConverter().GetBits(x);
                ba.ToByte().Should().Be(x);
            }
        }
    }
}
