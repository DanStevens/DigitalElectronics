using System;
using System.Collections;
using System.Diagnostics;
using DigitalElectronics.Utilities;
using FluentAssertions;
using NUnit.Framework;

[assembly: DebuggerDisplay("BitArray={DigitalElectronics.Utilities.Extensions.ToString(this)}", Target = typeof(BitArray))]

namespace DigitalElectronics.Modules.ALUs.Tests
{
    public class TestArithmeticLogicUnit
    {
        // Number of bits or 'N'
        private const int N = 4;

        private ByteConverter _byteConverter;
        private ArithmeticLogicUnit _4bitAlu;

        [SetUp]
        public void SetUp()
        {
            _byteConverter = new ByteConverter(Endianness.Little);
            _4bitAlu = new ArithmeticLogicUnit(N);
        }

        [Test]
        public void Constructor_GivenZeroNumberOfBits_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ArithmeticLogicUnit(0));
        }

        [Test]
        public void BitCount_ShouldBeN()
        {
            _4bitAlu.BitCount.Should().Be(N);
        }

        [Test]
        public void OutputIsNull_WhenInputEOIsLow()
        {
            _4bitAlu.SetInputEO(false);
            _4bitAlu.OutputE.Should().BeNull();
            _4bitAlu.SetInputEO(true);
            _4bitAlu.OutputE.Should().BeEquivalentTo(new BitArray(N));
        }

        [Test]
        public void ProbeState_ReturnsInternalState()
        {
            _4bitAlu.ProbeState().Should().BeEquivalentTo(new BitArray(N));
            _4bitAlu.SetInputA(CreateBitArrayFromInt(N, 3));
            _4bitAlu.SetInputB(CreateBitArrayFromInt(N, 5));
            _4bitAlu.ProbeState().Should().BeEquivalentTo(CreateBitArrayFromInt(N, 8));
        }

        [Test]
        public void TestAdditionFull()
        {
            _4bitAlu.SetInputEO(true);
            _4bitAlu.SetInputSu(false);

            for (int a = -8; a < 8; a++)
                for (int b = -8; b < 8; b++)
                    AssertSumOfAAndB(a, b, a + b);
        }

        [Test]
        public void TestSubtractionFull()
        {
            _4bitAlu.SetInputEO(true);
            _4bitAlu.SetInputSu(true);

            for (int a = -8; a < 8; a++)
                for (int b = -8; b < 8; b++)
                    AssertSumOfAAndB(a, b, a - b);
        }

        private void AssertSumOfAAndB(int a, int b, int expectedSum)
        {
            var dataA = CreateBitArrayFromInt(N, a);
            _4bitAlu.SetInputA(dataA);
            var dataB = CreateBitArrayFromInt(N, b);
            _4bitAlu.SetInputB(dataB);
            var expectation = CreateBitArrayFromInt(N, expectedSum);
            _4bitAlu.OutputE.Should().BeEquivalentTo(expectation, $"a = {a}; b = {b}");
        }

        /// <summary>
        /// Creates a <see cref="BitArray"/> of the given length with the bits sets to the little-endian binary
        /// representation of the given integer value.
        /// </summary>
        /// <param name="length">The number of bits in the new BitArray</param>
        /// <param name="value">The integer value</param>
        /// <returns>Returns a BitArray contain the little-endian binary representation of
        /// <paramref name="value"/>.</returns>
        /// <remarks>
        /// <paramref name="value"/> is converted to binary form, of which the first <paramref name="length"/> bits
        /// are used to set the BitArray. Therefore, the resulting BitArray will only correctly represent
        /// <paramref name="value"/> if <paramref name="length"/> is of sufficient number. If not, he actual
        /// resulting BitArray will be the equivalent to the full binary representation truncated
        /// to <paramref name="length"/> (removing high-order bits).
        /// </remarks>
        private BitArray CreateBitArrayFromInt(int length, int value)
        {
            var fullBitArray = new BitArray(_byteConverter.GetBytes(value));
            var result = new BitArray(length);
            for (int x = 0; x < length; x++) result.Set(x, fullBitArray[x]);
            return result;
        }
    }
}
