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
using DotNetBitVector32 = System.Collections.Specialized.BitVector32;
using FluentAssertions.Execution;

namespace DigitalElectronics.Concepts.Tests
{
    public class BitArrayTests
    {
        [Test]
        public void BitArray_ShouldBeImplicitlyCastableToDotNetBitArray()
        {
            DotNetBitVector32 dnBitArray;
            Assert.DoesNotThrow(() => dnBitArray = new BitArray(0));
        }

        [Test]
        public void BitArray_ShouldBeExplicitlyCastableToDotNetBitArray()
        {
            DotNetBitVector32 dnBitArray;
            Assert.DoesNotThrow(() => dnBitArray = (DotNetBitVector32)new BitArray(0));
        }

        [Test]
        public void BitArray_ShouldBeCreatableFromDotNetBitArray()
        {
            var dtBitArray = new DotNetBitVector32(0);
            _ = new BitArray(dtBitArray);
        }

        [Test]
        public void BitArray_ShouldBeCreatableFromArrayOfBits()
        {
            Bit[] bits = {new(false), new(true), new(false), new(true)};
            new BitArray(bits);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(33)]
        public void BitArray_ShouldThrowWhenLengthNotBetween0And32Inclusive(int length)
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new BitArray(0, length));
            ex.ParamName.Should().Be("length");
            ex.Message.Should().StartWith("Argument must be between 0 and 32 inclusive.");
        }

        [Test]
        public void BitArray_ShouldThrowWhenCreatedWithArrayWithMoreThan32Values()
        {
            var values = Enumerable.Repeat(false, 33).ToArray();
            var ex = Assert.Throws<ArgumentException>(() => new BitArray(values));
            ex.ParamName.Should().Be("values");
            ex.Message.Should().StartWith("Argument cannot contain more than 32 items.");
        }

        [Test]
        public void BitArray_ShouldThrowWhenCreatedWithEnumerableWithMoreThan32Values()
        {
            var enumerable = Enumerable.Repeat(false, 33);
            var ex = Assert.Throws<ArgumentException>(() => new BitArray(enumerable));
            ex.ParamName.Should().Be("values");
            ex.Message.Should().StartWith("Argument cannot contain more than 32 items.");
        }

        [Test]
        [TestCase(-1)]
        [TestCase(33)]
        public void Length_ShouldThrowWhenSetToValueNotBetween0And32Inclusive(int newLength)
        {
            var objUT = new BitArray(0, 32);
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => objUT.Length = newLength);
            ex.ParamName.Should().Be("value");
            ex.Message.Should().StartWith("Argument must be between 0 and 32 inclusive.");
        }

        [Test]
        public void GetIndexer_ShouldGetValueOfBitAtGivenIndex()
        {
            bool[] bits = {false, false, true, false, true, false, true, false};
            var objUT = new BitArray(bits);

            using (new AssertionScope())
            {
                for (int i = 0; i < bits.Length; i++)
                {
                    objUT[i].Should().Be(bits[i], $"(i => {i})");
                } 
            }
        }

        [Test]
        public void SetIndexer_ShouldSetValueOfBitAtIndex()
        {
            var objUT = new BitArray();
            objUT[2] = true;

            using (new AssertionScope())
            {
                for (int i = 0; i < 8; i++)
                {
                    objUT[i].Should().Be(i == 2, $"(i => {i})");
                } 
            }
        }

        [Test]
        public void AsList_OfBool_ShouldReturnReadOnlyListOfBools()
        {
            bool[] bits = { false, false, true, false, true, false, true, false };
            var objUT = new BitArray(bits);
            IReadOnlyList<bool> listOfBools = objUT.ToReadOnlyList<bool>();
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
            objUT.Length.Should().Be(8);
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
            for (int i = short.MinValue; i < short.MaxValue; i++)
            {
                var objUT = new BitArray(i);
                objUT.ToString(NumberFormat.SignedDecimal).Should().Be(i.ToString());
            }
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
        public void Length_ShouldCorrespondWithSizeOfIntUsedToCreateBitArray()
        {
            using (new AssertionScope())
            {
                new BitArray(0, length: 0).Length.Should().Be(0, "length arg is 0");
                new BitArray(0, length: 8).Length.Should().Be(8, "length arg is 8");
                new BitArray(0, length: 9).Length.Should().Be(9, "length arg is 9");
                new BitArray(0, length: 16).Length.Should().Be(16, "length arg is 16");

                new BitArray(true).Length.Should().Be(1, "created with single bool");
                new BitArray(true, false).Length.Should().Be(2, "created with 2 bools");
                new BitArray(new Bit()).Length.Should().Be(1, "created with single Bit object");
                new BitArray(new Bit(true), new Bit(false)).Length.Should().Be(2, "created with 2 Bit objects");

                new BitArray(new DotNetBitVector32()).Length.Should().Be(32, "created with .NET BitVector32");
                ////new BitArray(new [] { 0 }).Length.Should().Be(sizeof(int) * 8);
                ////new BitArray(new [] { 0, 0 }).Length.Should().Be(sizeof(int) * 8 * 2);
                new BitArray((byte)0).Length.Should().Be(sizeof(byte) * 8, "created with Byte value");

                var bitConverter = new BitConverter();
                bitConverter.GetBits((sbyte)0).Length.Should().Be(sizeof(sbyte) * 8);
                bitConverter.GetBits((ushort)0).Length.Should().Be(sizeof(ushort) * 8);
                bitConverter.GetBits((short)0).Length.Should().Be(sizeof(short) * 8);
                bitConverter.GetBits(0).Length.Should().Be(sizeof(int) * 8);
            }
        }

        [Test]
        [TestCase(0, 1, 0)]
        [TestCase(1, 1, 1)]
        [TestCase(2, 1, 0)]
        [TestCase(3, 1, 1)]
        [TestCase(14, 4, 14)]
        [TestCase(15, 4, 15)]
        [TestCase(16, 4, 0)]
        [TestCase(17, 4, 1)]
        [TestCase(254, 8, 254)]
        [TestCase(255, 8, 255)]
        [TestCase(256, 8, 0)]
        [TestCase(257, 8, 1)]
        [TestCase(int.MaxValue, 32, int.MaxValue)]
        [TestCase(int.MaxValue, 8, byte.MaxValue)]
        public void ToInt32_ShouldMaskResultAccordingToLength(int value, int length, int expected)
        {
            new BitArray(value, length).ToInt32().Should().Be(expected);
        }

        [Test]
        [TestCase(0, 1, 0)]
        [TestCase(1, 1, 1)]
        [TestCase(2, 1, 0)]
        [TestCase(3, 1, 1)]
        [TestCase(14, 4, 14)]
        [TestCase(15, 4, 15)]
        [TestCase(16, 4, 0)]
        [TestCase(17, 4, 1)]
        [TestCase(254, 8, 254)]
        [TestCase(255, 8, 255)]
        public void ToByte_ShouldMaskResultAccordingToLength(byte value, int length, byte expected)
        {
            new BitArray(value, length).ToByte().Should().Be(expected);
        }
    }
}
