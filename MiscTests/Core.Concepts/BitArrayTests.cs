using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using DigitalElectronics.Concepts;
using FluentAssertions;
using BitConverter = DigitalElectronics.Utilities.BitConverter;
using DotNetBitArray = System.Collections.BitArray;

namespace DigitalElectronics.Concepts.Tests
{
    public class BitArrayTests
    {
        [Test]
        public void BitArray_ShouldImplementListOfBoolean()
        {
            var objUT = new BitArray(length: 8);
            objUT.Should().BeAssignableTo<IList<bool>>();
        }

        [Test]
        public void BitArray_ShouldImplementList()
        {
            var objUT = new BitArray(length: 8);
            objUT.Should().BeAssignableTo<IList>();
        }

        [Test]
        public void BitArray_ShouldBeImplicitlyCastableToDotNetBitArray()
        {
            DotNetBitArray dnBitArray;
            Assert.DoesNotThrow(() => dnBitArray = new BitArray(0));
        }

        [Test]
        public void BitArray_ShouldBeExplicitlyCastableToDotNetBitArray()
        {
            DotNetBitArray dnBitArray;
            Assert.DoesNotThrow(() => dnBitArray = (DotNetBitArray)new BitArray(0));
        }

        [Test]
        public void CreatingBitArrayOfGivenLengthZero_ShouldBeConvertibleToDotNetBitArrayOfEqualLengthZero()
        {
            for (int i = 0; i < 8; i++)
            {
                var objUT = new BitArray(length: i);
                var dnBitArray = (DotNetBitArray)objUT;
                objUT.Length.Should().Be(dnBitArray.Length);
            }
        }

        [Test]
        public void CreatingBitArrayFromArrayOfBits_ShouldBeConvertibleToDotNetBitArrayOfSameBits()
        {
            bool[] bits = {true, false, true, false};
            var objUT = new BitArray(bits);
            var dnBitArray = (DotNetBitArray)objUT;
            dnBitArray.Should().BeEquivalentTo(bits);
        }

        [Test]
        public void CreatingBitArrayFromArrayOfByte_ShouldBeConvertibleToDotNetBitArrayOfSameBytes()
        {
            byte[] bytesIn = { 1, 2, 3, 4 };
            bool[] expectedBitsOut =
            {
                true, false, false, false, false, false, false, false, false, true, false, false, false, false,
                false, false, true, true, false, false, false, false, false, false, false, false, true, false,
                false, false, false, false
            };

            var objUT = new BitArray(bytesIn);
            var dnBitArray = (DotNetBitArray)objUT;
            dnBitArray.Should().BeEquivalentTo(expectedBitsOut);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void CreatingBitArrayOfGivenLengthWithDefaultValues_ShouldBeConvertibleToDotNotArrayOfSameValues(bool b)
        {
            for (int i = 0; i < 8; i++)
            {
                var objUT = new BitArray(i, b);
                var dnBitArray = (DotNetBitArray)objUT;
                dnBitArray.Should().BeEquivalentTo(Enumerable.Repeat(b, i));
            }
        }

        [Test]
        public void BitArray_ShouldBeCreatableFromDotNetBitArray()
        {
            var dtBitArray = new DotNetBitArray(0);
            _ = new BitArray(dtBitArray);
        }

        [Test]
        public void BitArray_ShouldBeCreatableFromAnotherBitArray()
        {
            var bitArray1 = new BitArray(0);
            var bitArray2 = new BitArray(bitArray1);
            bitArray1.Should().NotBeSameAs(bitArray2);
        }

        [Test]
        public void BitArray_ShouldBeCreatableFromArrayOfBits()
        {
            Bit[] bits = {new(false), new(true), new(false), new(true)};
            new BitArray(bits);
        }

        [Test]
        public void BitArray_ShouldBeCreatableFromLiteralArrayOfBools()
        {
            BitArray bitArray = new[] {false, true, false, true};
        }

        [Test]
        public void GetIndexer_ShouldDelegateToInternalDotNetBitArray()
        {
            bool[] bits = {false, false, true, false, true, false, true, false};
            var objUT = new BitArray(bits);

            for (int i = 0; i < bits.Length; i++)
            {
                objUT[i].Should().Be(bits[i]);
            }
        }

        [Test]
        public void SetIndexer_ShouldDelegateToInternalDotNetBitArray()
        {
            var dtBitArray = new DotNetBitArray(8);
            var objUT = new BitArray(dtBitArray);
            objUT[2] = true;

            for (int i = 0; i < dtBitArray.Length; i++)
            {
                dtBitArray[i].Should().Be(i == 2);
            }
        }

        [Test]
        public void AsList_OfBool_ShouldReturnReadOnlyListOfBools()
        {
            bool[] bits = { false, false, true, false, true, false, true, false };
            var objUT = new BitArray(bits);
            IReadOnlyList<bool> listOfBools = objUT.AsReadOnlyList<bool>();
            listOfBools.Should().Equal(bits);
        }

        [TestCase(0, "00000000")]
        [TestCase(1, "00000001")]
        [TestCase(2, "00000010")]
        [TestCase(3, "00000011")]
        [TestCase(128, "10000000")]
        [TestCase(254, "11111110")]
        [TestCase(255, "11111111")]
        public void ToString_WithArg_LsbBinary(int v, string expected)
        {
            var objUT = new BitArray((byte)v);
            objUT.ToString(NumberFormat.LsbBinary).Should().Be(expected);
        }

        [TestCase(0, "00000000")]
        [TestCase(1, "10000000")]
        [TestCase(2, "01000000")]
        [TestCase(3, "11000000")]
        [TestCase(128, "00000001")]
        [TestCase(254, "01111111")]
        [TestCase(255, "11111111")]
        public void ToString_WithArg_MsbBinary(int v, string expected)
        {
            var b = (byte)v;
            var objUT = new BitArray(b);
            objUT.ToString(NumberFormat.MsbBinary).Should().Be(expected);
        }

        [Test]
        public void ToString_WithArg_UnsignedDecimal()
        {
            var bitConverter = new BitConverter();
            for (int i = short.MinValue; i < short.MaxValue; i++)
            {
                var objUT = bitConverter.GetBits(i);
                objUT.ToString(NumberFormat.SignedDecimal).Should().Be(i.ToString());
            }
        }

        [Test]
        public void IndexOf_ShouldWorkCorrectly()
        {
            IList<bool> bitArray1 = new BitArray(false, true, true, false);
            bitArray1.IndexOf(false).Should().Be(0);
            bitArray1.IndexOf(true).Should().Be(1);

            IList<bool> bitArray2 = new BitArray((byte)0);
            bitArray2.IndexOf(false).Should().Be(0);
            bitArray2.IndexOf(true).Should().Be(-1);

            IList<bool> bitArray3 = new BitArray(byte.MaxValue);
            bitArray3.IndexOf(false).Should().Be(-1);
            bitArray3.IndexOf(true).Should().Be(0);
        }

        [Test]
        public void AsEnumerable_ReturnsListOfBools_WhenGivenTypeParameterOfBool()
        {
            bool[] bools = {false, true, false, true};
            var objUT = new BitArray(bools);
            objUT.AsEnumerable<bool>().Should().BeEquivalentTo(bools); 
        }

        [Test]
        public void AsEnumerable_ReturnsListOfBits_WhenGivenTypeParameterOfBit()
        {
            bool[] bools = { false, true, false, true };
            var expectedBits = bools.Select(b => new Bit(b));
            var objUT = new BitArray(bools);
            objUT.AsEnumerable<Bit>().Should().BeEquivalentTo(expectedBits);
        }

        [Test]
        public void Count_ShouldCorrespondWithSizeOfIntUsedToCreateBitArray()
        {
            new BitArray(length: 0).Count.Should().Be(0);
            new BitArray(length: 8).Count.Should().Be(8);
            new BitArray(length: 9).Count.Should().Be(9);
            new BitArray(length: 16).Count.Should().Be(16);

            new BitArray(true).Count.Should().Be(1);
            new BitArray(true, false).Count.Should().Be(2);
            new BitArray(new Bit()).Count.Should().Be(1);
            new BitArray(new Bit(true), new Bit(false)).Count.Should().Be(2);

            new BitArray(new DotNetBitArray(length: 32)).Count.Should().Be(32);
            new BitArray(new [] { 0 }).Count.Should().Be(sizeof(int) * 8);
            new BitArray(new [] { 0, 0 }).Count.Should().Be(sizeof(int) * 8 * 2);
            new BitArray((byte)0).Count.Should().Be(sizeof(byte) * 8);

            var bitConverter = new BitConverter();
            bitConverter.GetBits((sbyte)0).Count.Should().Be(sizeof(sbyte) * 8);
            bitConverter.GetBits((ushort)0).Count.Should().Be(sizeof(ushort) * 8);
            bitConverter.GetBits((short)0).Count.Should().Be(sizeof(short) * 8);
            bitConverter.GetBits(0).Count.Should().Be(sizeof(int) * 8);
        }

        [TestCase(-1, sizeof(int) * 8)]
        [TestCase(0, 1)] // Trim should never reduce length of BitArray below 1
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 2)]
        [TestCase(4, 3)]
        [TestCase(7, 3)]
        [TestCase(255, 8)]
        [TestCase(int.MinValue, sizeof(int) * 8)]
        [TestCase(int.MinValue + 1, sizeof(int) * 8)]
        [TestCase(int.MinValue + 1024, sizeof(int) * 8)]
        public void Trim_WhenGivenBitArrayOfLength32_TrimsToSmallestLengthPossible_WhenGivenNoArgs(int value, int expectedLength)
        {
            var bitConverter = new BitConverter();
            var bitArray = bitConverter.GetBits((int)value);
            bitArray.Length.Should().Be(sizeof(int) * 8);
            bitArray.Trim().Should().BeSameAs(bitArray);
            bitArray.Length.Should().Be(expectedLength);
            bitArray.Count.Should().Be(expectedLength);
            bitConverter.ToInt32(bitArray).Should().Be(value);
        }

        [TestCase(0, 1)] // Trim should never reduce length of BitArray below 1
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 2)]
        [TestCase(4, 3)]
        [TestCase(7, 3)]
        [TestCase(255, 8)]
        [TestCase(ushort.MinValue, 1)]
        [TestCase(ushort.MinValue + 1, 1)]
        [TestCase(ushort.MinValue + 1024, 11)]
        public void Trim_WhenGivenBitArrayOfLength16_TrimsToSmallestLengthPossible_WhenGivenNoArgs(int value, int expectedLength)
        {
            var bitConverter = new BitConverter();
            var bitArray = bitConverter.GetBits((ushort)value);
            bitArray.Length.Should().Be(sizeof(ushort) * 8);
            bitArray.Trim().Should().BeSameAs(bitArray);
            bitArray.Length.Should().Be(expectedLength);
            bitArray.Count.Should().Be(expectedLength);
            bitConverter.ToUInt16(bitArray).Should().Be((ushort)value);
        }

        [TestCase(0, 1)] // Trim should never reduce length of BitArray below 1
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 2)]
        [TestCase(4, 3)]
        [TestCase(7, 3)]
        [TestCase(11, 4)]
        [TestCase(byte.MaxValue, 8)]
        public void Trim_WhenGivenBitArrayOfLength8_TrimsToSmallestLengthPossible_WhenGivenNoArgs(int value, int expectedLength)
        {
            var bitConverter = new BitConverter();
            var bitArray = bitConverter.GetBits((byte)value);
            bitArray.Length.Should().Be(sizeof(byte) * 8);
            bitArray.Trim().Should().BeSameAs(bitArray);
            bitArray.Length.Should().Be(expectedLength);
            bitArray.Count.Should().Be(expectedLength);
            bitConverter.ToByte(bitArray).Should().Be((byte)value);
        }

        [TestCase(-1, 1, sizeof(int) * 8)]
        [TestCase(0, 0, 1)] // Trim should never reduce length of BitArray below 1
        [TestCase(1, 1, 1)]
        [TestCase(2, 1, 2)]
        [TestCase(3, 1, 2)]
        [TestCase(4, 1, 3)]
        [TestCase(7, 1, 3)]
        [TestCase(11, 8, 8)]
        [TestCase(42, 8, 8)]
        [TestCase(645, 8, 10)]
        public void Trim_WhenGivenBitArrayOfLength32_TriesToTrimToGivenLength(int value, int targetLength, int expectedLength)
        {
            var bitConverter = new BitConverter();
            var bitArray = bitConverter.GetBits((int)value);
            bitArray.Length.Should().Be(sizeof(int) * 8);
            bitArray.Trim(targetLength).Should().BeSameAs(bitArray);
            bitArray.Length.Should().Be(expectedLength);
            bitArray.Count.Should().Be(expectedLength);
            bitConverter.ToInt32(bitArray).Should().Be(value);
        }

        [Test]
        public void Trim_ThrowsArgumentOutOfRangeException_WhenTargetLengthIsNegative()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new BitArray(length: 8).Trim(-1));
            ex.Message.Should().StartWithEquivalentOf("Argument must be non-negative");
            ex.ParamName.Should().Be("targetLength");
        }
    }
}
