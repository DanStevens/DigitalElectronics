﻿using System;
using System.Collections;
using DigitalElectronics.Utilities;
using FluentAssertions;
using NUnit.Framework;

namespace DigitalElectronics.Modules.ALUs.Tests
{
    public class TestArithmeticLogicUnit
    {
        // Number of bits or 'N'
        private const int N = 4;

        private ArithmeticLogicUnit _4bitAlu;

        [SetUp]
        public void SetUp()
        {
            _4bitAlu = new ArithmeticLogicUnit(N);
        }

        [Test]
        public void TestAddition()
        {
            for (int a = 0; a < 8; a++)
                for (int b = 0; b < 8; b++)
                {
                    _4bitAlu.SetInputA(CreateBitArrayFromInt(N, a));
                    _4bitAlu.SetInputB(CreateBitArrayFromInt(N, b));
                    _4bitAlu.OutputE.Should().BeEquivalentTo(CreateBitArrayFromInt(N, a + b), $"a = {a}; b = {b}");
                }
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
        private static BitArray CreateBitArrayFromInt(int length, int value)
        {
            var fullBitArray = new BitArray(LittleEndianBitConverter.GetBytes(value));
            var result = new BitArray(length);
            for (int x = 0; x < length; x++) result.Set(x, fullBitArray[x]);
            return result;
        }
    }
}
