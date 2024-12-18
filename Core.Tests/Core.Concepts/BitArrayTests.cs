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
using NSubstitute;

namespace DigitalElectronics.Concepts.Tests
{
    public class BitArrayTests
    {
        #region Constructor tests

        [Test]
        public void BitArray_ShouldBeCreatableFromDotNetBitArray()
        {
            var dtBitArray = new DotNetBitVector32(0);
            _ = new BitArray(dtBitArray);
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
        public void BitArray_ShouldThrowWhenCreatedWithByteArrayWithMoreThan4Values()
        {
            var bytes = Enumerable.Repeat((byte)0, 5).ToArray();
            var ex = Assert.Throws<ArgumentException>(() => BitArray.FromBytes(bytes));
            ex.ParamName.Should().Be("bytes");
            ex.Message.Should().StartWith("Argument cannot contain more than 4 items.");
        }

        [Test]
        [TestCase(new bool[] { }, 0)]
        [TestCase(new bool[] { false }, 0)]
        [TestCase(new bool[] { true }, 1)]
        [TestCase(new bool[] { true, false }, 1)]
        [TestCase(new bool[] { true, true }, 3)]
        [TestCase(new bool[] { false, false, false, false }, 0)]
        [TestCase(new bool[] { true, false, false, false }, 1)]
        [TestCase(new bool[] { false, true, false, false }, 2)]
        [TestCase(new bool[] { false, false, true, false }, 4)]
        [TestCase(new bool[] { false, false, false, true }, 8)]
        [TestCase(new bool[] { false, true, true, true }, 14)]
        [TestCase(new bool[] { false, true, true, true, true, true, true, true }, 254)]
        public void BitArray_CreateFromBoolArray(bool[] bits, int expected)
        {
            using (new AssertionScope())
            {
                var bitArray = new BitArray(bits);
                bitArray.ToInt32().Should().Be(expected);
                bitArray.Length.Should().Be(bits.Length);
            }
        }

        [Test]
        [TestCase(new bool[] { }, 0)]
        [TestCase(new bool[] { false }, 0)]
        [TestCase(new bool[] { true }, 1)]
        [TestCase(new bool[] { true, false }, 1)]
        [TestCase(new bool[] { true, true }, 3)]
        [TestCase(new bool[] { false, false, false, false }, 0)]
        [TestCase(new bool[] { true, false, false, false }, 1)]
        [TestCase(new bool[] { false, true, false, false }, 2)]
        [TestCase(new bool[] { false, false, true, false }, 4)]
        [TestCase(new bool[] { false, false, false, true }, 8)]
        [TestCase(new bool[] { false, true, true, true }, 14)]
        [TestCase(new bool[] { false, true, true, true, true, true, true, true }, 254)]
        public void BitArray_CreateFromBoolEnumerable(bool[] bits, int expected)
        {
            using (new AssertionScope())
            {
                var bitArray = new BitArray(bits.AsEnumerable());
                bitArray.ToInt32().Should().Be(expected);
                bitArray.Length.Should().Be(bits.Length);
            }
        }

        [Test]
        public void BitArray_CreateFromBoolEnumerable_ShouldOnlyTakeFirst32Bits_WhenSequenceYields33Values()
        {
            using (new AssertionScope())
            {
                var components = new[] { true }
                    .Concat(Enumerable.Repeat(false, 31))
                    .Concat([true]);
                var bitArray = new BitArray(components);
                bitArray.ToInt32().Should().Be(1);
                bitArray.Length.Should().Be(32);
            }
        }

        [Test]
        [TestCase(new bool[] { }, 0, 0)]
        [TestCase(new bool[] { false }, 1, 0)]
        [TestCase(new bool[] { true }, 1, 1)]
        [TestCase(new bool[] { true, false }, 2, 1)]
        [TestCase(new bool[] { true, true }, 2, 3)]
        [TestCase(new bool[] { false, false, false, false }, 4, 0)]
        [TestCase(new bool[] { true, false, false, false }, 4, 1)]
        [TestCase(new bool[] { false, true, false, false }, 4, 2)]
        [TestCase(new bool[] { false, false, true, false }, 4, 4)]
        [TestCase(new bool[] { false, false, false, true }, 4, 8)]
        [TestCase(new bool[] { false, true, true, true }, 4, 14)]
        [TestCase(new bool[] { false, true, true, true }, 8, 14)]
        [TestCase(new bool[] { false, true, true, true, true, true, true, true }, 8, 254)]
        [TestCase(new bool[] { false, true, true, true, true, true, true, true }, 4, 14)]
        public void BitArray_CreateFromBoolEnumerableAndLength(bool[] bits, int length, int expected)
        {
            using (new AssertionScope())
            {
                var bitArray = new BitArray(bits.AsEnumerable(), length);
                bitArray.ToInt32().Should().Be(expected);
                bitArray.Length.Should().Be(length);
            }
        }

        [Test]
        [TestCase(-1)]
        [TestCase(33)]
        public void BitArray_CreateFromBoolEnumerableAndLength_ShouldThrow_WhenLengthArgIsNotBetween0And32(int length)
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new BitArray(Enumerable.Empty<bool>(), length));
            ex!.ParamName.Should().Be("length");
            ex.Message.Should().StartWith(BitArray.LengthOutOfRangeMessage);
        }

        [Test]
        [TestCase(new bool[] { }, 0)]
        [TestCase(new bool[] { false }, 0)]
        [TestCase(new bool[] { true }, 1)]
        [TestCase(new bool[] { true, false }, 1)]
        [TestCase(new bool[] { true, true }, 3)]
        [TestCase(new bool[] { false, false, false, false }, 0)]
        [TestCase(new bool[] { true, false, false, false }, 1)]
        [TestCase(new bool[] { false, true, false, false }, 2)]
        [TestCase(new bool[] { false, false, true, false }, 4)]
        [TestCase(new bool[] { false, false, false, true }, 8)]
        [TestCase(new bool[] { false, true, true, true }, 14)]
        [TestCase(new bool[] { false, true, true, true, true, true, true, true }, 254)]
        public void BitArray_CreateFromSpanOfBool(bool[] bits, int expected)
        {
            using (new AssertionScope())
            {
                var bitArray = new BitArray(bits.AsSpan());
                bitArray.ToInt32().Should().Be(expected);
                bitArray.Length.Should().Be(bits.Length);
            }
        }

        [Test]
        [TestCase(new byte[] { }, 0)]
        [TestCase(new byte[] { 0 }, 0)]
        [TestCase(new byte[] { 255 }, 255)]
        [TestCase(new byte[] { 255, 0 }, 255)]
        [TestCase(new byte[] { 0, 1 }, 256)]
        [TestCase(new byte[] { 0x00, 0x00, 0x00, 0x00 }, 0x00000000)]
        [TestCase(new byte[] { 0xFF, 0x00, 0x00, 0x00 }, 0x000000FF)]
        [TestCase(new byte[] { 0x00, 0xFF, 0x00, 0x00 }, 0x0000FF00)]
        [TestCase(new byte[] { 0x00, 0x00, 0xFF, 0x00 }, 0x00FF0000)]
        [TestCase(new byte[] { 0x00, 0x00, 0x00, 0xFF }, (int)(~0xFF000000u + 1) * -1)]
        public void BitArray_CreateFromByteArray_LittleEndian(byte[] bytes, int expected)
        {
            using (new AssertionScope())
            {
                var bitArray = BitArray.FromBytes(bytes);
                bitArray.ToInt32().Should().Be(expected);
                bitArray.Length.Should().Be(bytes.Length * 8);
            }
        }

        [Test]
        [TestCase(new bool[] { }, 0)]
        [TestCase(new bool[] { false }, 0)]
        [TestCase(new bool[] { true }, 1)]
        [TestCase(new bool[] { true, false }, 1)]
        [TestCase(new bool[] { true, true }, 3)]
        [TestCase(new bool[] { false, false, false, false }, 0)]
        [TestCase(new bool[] { true, false, false, false }, 1)]
        [TestCase(new bool[] { false, true, false, false }, 2)]
        [TestCase(new bool[] { false, false, true, false }, 4)]
        [TestCase(new bool[] { false, false, false, true }, 8)]
        [TestCase(new bool[] { false, true, true, true }, 14)]
        [TestCase(new bool[] { false, true, true, true, true, true, true, true }, 254)]
        public void BitArray_CreateFromCollectionOfBooleanOutputComponents_ShouldReturnBitArray(bool[] bits, int expected)
        {
            using (new AssertionScope())
            {
                var collection = (ICollection<bool>)bits;
                var result = new BitArray(collection);
                result.ToInt32().Should().Be(expected);
                result.Length.Should().Be(collection.Count);
            }
        }

        #endregion

        #region Length property tests

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
        [TestCase(-1)]
        [TestCase(33)]
        public void Length_ShouldThrowWhenSetToValueNotBetween0And32Inclusive(int newLength)
        {
            var objUT = new BitArray(0, 32);
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => objUT.Length = newLength);
            ex.ParamName.Should().Be("value");
            ex.Message.Should().StartWith("Argument must be between 0 and 32 inclusive.");
        }

        #endregion

        #region this[] indexer tests

        [Test]
        public void GetIndexer_ShouldGetValueOfBitAtGivenIndex()
        {
            bool[] bits = { false, false, true, false, true, false, true, false };
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
        public void ToArray_OfBool_ShouldReturnReadOnlyListOfBools()
        {
            bool[] bits = { false, false, true, false, true, false, true, false };
            var objUT = new BitArray(bits);
            var listOfBools = objUT.ToArray<bool>();
            listOfBools.Should().Equal(bits);
        }

        #endregion

        #region ToString method tests

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
            var objUT = new BitArray((byte)v);
            objUT.ToString(NumberFormat.MsbBinary).Should().Be(expected);
        }

        [Test]
        public void ToString_WithArg_UnsignedDecimal()
        {
            var bitConverter = new BitConverter();
            for (int i = short.MinValue; i < short.MaxValue; i++)
            {
                var objUT = new BitArray(i);
                objUT.ToString(NumberFormat.SignedDecimal).Should().Be(i.ToString());
            }
        }

        #endregion

        #region AsEnumerable method tests

        [Test]
        public void AsEnumerable_ShouldReturnSequenceOfBools_WhenGivenTypeParameterOfBool()
        {
            bool[] bools = { false, true, false, true };
            var objUT = new BitArray(bools);
            objUT.AsEnumerable<bool>().Should().BeEquivalentTo(bools);
        }

        [Test]
        public void AsEnumerable_ShouldReturnSequenceOfBits_WhenGivenTypeParameterOfBit()
        {
            bool[] bools = { false, true, false, true };
            var expectedBits = bools.Select(b => new Bit(b));
            var objUT = new BitArray(bools);
            objUT.AsEnumerable<Bit>().Should().BeEquivalentTo(expectedBits);
        }

        [Test]
        public void AsEnumerable_ShouldReturnSequence_WhenGivenNoTypeParameter()
        {
            bool[] bools = { false, true, false, true };
            var objUT = new BitArray(bools);
            objUT.AsEnumerable().Should().BeEquivalentTo(bools);
        }

        [Test]
        public void AsEnumerable_ShouldThrow_WhenGivenTypeParameterNotBoolOrInt()
        {
            bool[] bools = { false, true, false, true };
            var objUT = new BitArray(bools);
            var ex = Assert.Throws<NotSupportedException>(() => objUT.AsEnumerable<int>());
            ex!.Message.Should().Be($"Only the following types are supported: System.Boolean, {typeof(Bit)}");
        }

        #endregion

        #region ToArray method tests

        [Test]
        public void ToArray_ShouldReturnArrayOfBools_WhenGivenTypeParameterOfBool()
        {
            bool[] bools = { false, true, false, true };
            var objUT = new BitArray(bools);
            objUT.ToArray<bool>().Should().BeEquivalentTo(bools);
        }

        [Test]
        public void ToArray_ShouldReturnArrayOfBits_WhenGivenTypeParameterOfBit()
        {
            bool[] bools = { false, true, false, true };
            var expectedBits = bools.Select(b => new Bit(b));
            var objUT = new BitArray(bools);
            objUT.ToArray<Bit>().Should().BeEquivalentTo(expectedBits);
        }

        [Test]
        public void ToArray_ShouldReturnArrayOfBools_WhenGivenNoTypeParameter()
        {
            bool[] bools = { false, true, false, true };
            var objUT = new BitArray(bools);
            objUT.ToArray().Should().BeEquivalentTo(bools);
        }

        [Test]
        public void ToArray_ShouldThrow_WhenGivenTypeParameterNotBoolOrInt()
        {
            bool[] bools = { false, true, false, true };
            var objUT = new BitArray(bools);
            var ex = Assert.Throws<NotSupportedException>(() => objUT.ToArray<int>());
            ex!.Message.Should().Be($"Only the following types are supported: System.Boolean, {typeof(Bit)}");
        }

        #endregion

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

        #region Static factory method tests

        [Test]
        [TestCase(new bool[] { }, 0)]
        [TestCase(new bool[] { false }, 0)]
        [TestCase(new bool[] { true }, 1)]
        [TestCase(new bool[] { true, false }, 1)]
        [TestCase(new bool[] { true, true }, 3)]
        [TestCase(new bool[] { false, false, false, false }, 0)]
        [TestCase(new bool[] { true, false, false, false }, 1)]
        [TestCase(new bool[] { false, true, false, false }, 2)]
        [TestCase(new bool[] { false, false, true, false }, 4)]
        [TestCase(new bool[] { false, false, false, true }, 8)]
        [TestCase(new bool[] { false, true, true, true }, 14)]
        [TestCase(new bool[] { false, true, true, true, true, true, true, true }, 254)]
        public void FromList_GivenArrayOfBooleanOutputComponents_ShouldReturnBitArray(bool[] componentOutputs, int expected)
        {
            using (new AssertionScope())
            {
                var components = componentOutputs.Select(CreateMockComponent).ToArray();
                var result = BitArray.FromList(components);
                result.ToInt32().Should().Be(expected);
                result.Length.Should().Be(componentOutputs.Length);
            }
        }

        [Test]
        public void FromList_GivenArrayOfBooleanOutputComponents_ShouldThrowWhenArgHasMoreThanThan32Items()
        {
            var components = Enumerable.Repeat(false, 33).Select(CreateMockComponent).ToArray();
            var ex = Assert.Throws<ArgumentException>(() => BitArray.FromList(components));
            ex!.ParamName.Should().Be("components");
            ex.Message.Should().StartWith("Argument cannot contain more than 32 items.");
        }

        [Test]
        [TestCase(new bool[] { }, 0)]
        [TestCase(new bool[] { false }, 0)]
        [TestCase(new bool[] { true }, 1)]
        [TestCase(new bool[] { true, false }, 1)]
        [TestCase(new bool[] { true, true }, 3)]
        [TestCase(new bool[] { false, false, false, false }, 0)]
        [TestCase(new bool[] { true, false, false, false }, 1)]
        [TestCase(new bool[] { false, true, false, false }, 2)]
        [TestCase(new bool[] { false, false, true, false }, 4)]
        [TestCase(new bool[] { false, false, false, true }, 8)]
        [TestCase(new bool[] { false, true, true, true }, 14)]
        [TestCase(new bool[] { false, true, true, true, true, true, true, true }, 254)]
        public void FromComponents_GivenSequenceOfBooleanOutputComponents_ShouldReturnBitArray(bool[] componentOutputs, int expected)
        {
            using (new AssertionScope())
            {
                var components = componentOutputs.Select(CreateMockComponent);
                var result = BitArray.FromComponents(components);
                result.ToInt32().Should().Be(expected);
                result.Length.Should().Be(componentOutputs.Length);
            }
        }

        [Test]
        public void FromComponents_GivenSequenceOfBooleanOutputComponents_ShouldOnlyTakeFirst32Bits_WhenSequenceYields33Values()
        {
            using (new AssertionScope())
            {
                var components = new[] { true }
                    .Concat(Enumerable.Repeat(false, 31))
                    .Concat([true])
                    .Select(CreateMockComponent);
                var result = BitArray.FromComponents(components);
                result.ToInt32().Should().Be(1);
                result.Length.Should().Be(32);
            }
        }

        [Test]
        [TestCase(new bool[] { }, 0, 0)]
        [TestCase(new bool[] { false }, 1, 0)]
        [TestCase(new bool[] { true }, 1, 1)]
        [TestCase(new bool[] { true, false }, 2, 1)]
        [TestCase(new bool[] { true, true }, 2, 3)]
        [TestCase(new bool[] { false, false, false, false }, 4, 0)]
        [TestCase(new bool[] { true, false, false, false }, 4, 1)]
        [TestCase(new bool[] { false, true, false, false }, 4, 2)]
        [TestCase(new bool[] { false, false, true, false }, 4, 4)]
        [TestCase(new bool[] { false, false, false, true }, 4, 8)]
        [TestCase(new bool[] { false, true, true, true }, 4, 14)]
        [TestCase(new bool[] { false, true, true, true }, 8, 14)]
        [TestCase(new bool[] { false, true, true, true, true, true, true, true }, 8, 254)]
        [TestCase(new bool[] { false, true, true, true, true, true, true, true }, 4, 14)]
        public void FromComponents_GivenSequenceOfBooleanOutputComponentsAndLength_ShouldReturnBitArray(bool[] componentOutputs, int length, int expected)
        {
            using (new AssertionScope())
            {
                var components = componentOutputs.Select(CreateMockComponent);
                var result = BitArray.FromComponents(components, length);
                result.ToInt32().Should().Be(expected);
                result.Length.Should().Be(length);
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(4)]
        [TestCase(8)]
        [TestCase(16)]
        [TestCase(32)]
        public void FromComponents_GivenSequenceOfBooleanOutputComponentsAndLength_ShouldOnlyTakeFirstLengthBits(int length)
        {
            using (new AssertionScope())
            {
                var components = new[] { true }
                    .Concat(Enumerable.Repeat(false, length - 1))
                    .Concat([true])
                    .Select(CreateMockComponent);
                var result = BitArray.FromComponents(components, length);
                result.ToInt32().Should().Be(1);
                result.Length.Should().Be(length);
            }
        }

        [Test]
        [TestCase(-1)]
        [TestCase(33)]
        public void FromComponents_GivenSequenceOfBooleanOutputComponentsAndLength_ShouldThrowWhenLengthNotBetween0And32(int length)
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => BitArray.FromComponents(Enumerable.Empty<IBooleanOutput>(), length));
            ex!.ParamName.Should().Be("length");
            ex.Message.Should().StartWith(BitArray.LengthOutOfRangeMessage);
        }

        [Test]
        [TestCase(new bool[] { }, 0)]
        [TestCase(new bool[] { false }, 0)]
        [TestCase(new bool[] { true }, 1)]
        [TestCase(new bool[] { true, false }, 1)]
        [TestCase(new bool[] { true, true }, 3)]
        [TestCase(new bool[] { false, false, false, false }, 0)]
        [TestCase(new bool[] { true, false, false, false }, 1)]
        [TestCase(new bool[] { false, true, false, false }, 2)]
        [TestCase(new bool[] { false, false, true, false }, 4)]
        [TestCase(new bool[] { false, false, false, true }, 8)]
        [TestCase(new bool[] { false, true, true, true }, 14)]
        [TestCase(new bool[] { false, true, true, true, true, true, true, true }, 254)]
        public void FromComponents_GivenCollectionOfBooleanOutputComponents_ShouldReturnBitArray(bool[] componentOutputs, int expected)
        {
            using (new AssertionScope())
            {
                var components = (ICollection<IBooleanOutput>)componentOutputs.Select(CreateMockComponent).ToArray();
                var result = BitArray.FromComponents(components);
                result.ToInt32().Should().Be(expected);
                result.Length.Should().Be(components.Count);
            }
        }

        [Test]
        public void FromList_GivenListOfBooleanOutputComponents_ShouldThrowWhenArgHasMoreThanThan32Items()
        {
            var components = Enumerable.Repeat(false, 33).Select(CreateMockComponent).ToList();
            var ex = Assert.Throws<ArgumentException>(() => BitArray.FromList(components));
            ex!.ParamName.Should().Be("components");
            ex.Message.Should().StartWith("Argument cannot contain more than 32 items.");
        }

        private static IBooleanOutput CreateMockComponent(bool output)
        {
            var mock = Substitute.For<IBooleanOutput>();
            mock.Output.Returns(output);
            return mock;
        }

        #endregion
    }
}
