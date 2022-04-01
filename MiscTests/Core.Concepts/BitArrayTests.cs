using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using DigitalElectronics.Concepts;
using FluentAssertions;
using DotNetBitArray = System.Collections.BitArray;

namespace DigitalElectronics.Concepts.Tests
{
    public class BitArrayTests
    {
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
                var objUT = new BitArray(i);
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
        public void Indexer_ShouldBeOfTypeBit()
        {
            new BitArray(1)[0].Should().BeOfType<Bit>();
        }

        [Test]
        public void GetIndexer_ShouldDelegateToInternalDotNetBitArray()
        {
            bool[] bits = {false, false, true, false, true, false, true, false};
            var objUT = new BitArray(bits);

            for (int i = 0; i < bits.Length; i++)
            {
                objUT[i].Value.Should().Be(bits[i]);
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

        [Ignore("Due to equality support override between Box<T> and T, this isn't possible to verify")]
        public void GetIndexer_ShouldBypassCreatingBitObject_WhenCalledViaIReadOnlyListOfBool()
        {
            var objUT = new BitArray(false, true, false, false);
            objUT[1].Should().NotBeSameAs(true);

            var objUTAsList = (IReadOnlyList<bool>)objUT;
            objUTAsList[1].Should().Be(true);
        }

        [Test]
        public void AsList_OfBool_ShouldReturnReadOnlyListOfBools()
        {
            bool[] bits = { false, false, true, false, true, false, true, false };
            var objUT = new BitArray(bits);
            IReadOnlyList<bool> listOfBools = objUT.AsReadOnlyList<bool>();
            listOfBools.Should().Equal(bits);
        }

        [Test]
        public void AsList_OfBit_ShouldReturnReadOnlyListOfBits()
        {
            Bit[] bits = { false, false, true, false, true, false, true, false };
            var objUT = new BitArray(bits);
            IReadOnlyList<Bit> listOfBits = objUT.AsReadOnlyList<Bit>();
            listOfBits.Should().BeEquivalentTo(bits);
        }

    }
}
