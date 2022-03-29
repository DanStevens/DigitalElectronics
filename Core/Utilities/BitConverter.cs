using System;
using System.Collections;
using System.Linq;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.Utilities
{

    /// <summary>
    /// Converts base data types to a <see cref="BitArray"/>, and a `BitArray` to
    /// base data types.
    /// </summary>
    /// <remarks>The class can be configured to convert to and from binary forms using a
    /// specific endianness, or the system architecture's endianness.</remarks>
    /// <seealso cref="System.BitConverter.IsLittleEndian"/>
    // Dev note: This class has not been fully tested: Since most systems are little-endian, the endianness
    // conversion is probably skipped when ran on most systems by default.
    public class BitConverter
    {
        private readonly ByteConverter _byteConverter;

        public Endianness Endianness => _byteConverter.Endianness;

        /// <summary>
        /// Creates new instance of `BitConverter` configured to use the system's endianness
        /// </summary>
        /// <seealso cref="System.BitConverter.IsLittleEndian"/>
        public BitConverter()
            : this (Endianness.System)
        {
        }

        /// <summary>
        /// Creates a new instance of `BitConverter` configured to use the given endianness
        /// </summary>
        /// <param name="endianness">The endianness to use in conversions</param>
        public BitConverter(Endianness endianness)
        {
            _byteConverter = new ByteConverter(endianness);
        }

        /// <summary>
        /// Returns the specified 32-bit signed integer value as a <see cref="BitArray"/>.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>A `BitArray` with length 32, that represents the given integer with the endianness
        /// specified by <see cref="Endianness"/>.</returns>
        /// <seealso cref="ByteConverter.GetBytes(int)"/>
        public BitArray GetBits(int value)
        {
            return new BitArray(_byteConverter.GetBytes(value));
        }

        /// <summary>
        /// Returns the specified 16-bit signed integer value as a <see cref="BitArray"/>.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>A `BitArray` with length 16, that represents the given integer with the endianness
        /// specified by <see cref="Endianness"/>.</returns>
        /// <seealso cref="ByteConverter.GetBytes(short)"/>
        public BitArray GetBits(short value)
        {
            return new BitArray(_byteConverter.GetBytes(value));
        }

        /// <summary>
        /// Returns the specified 8-bit signed integer value as a <see cref="BitArray"/>.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>A `BitArray` with length 8, that represents the given integer with the endianness
        /// specified by <see cref="Endianness"/>.</returns>
        public BitArray GetBits(byte value)
        {
            return new BitArray(new byte[] { value });
        }

        /// <summary>
        /// Returns the specified 32-bit signed integer value as a <see cref="BitArray"/> padded or truncated
        /// to match the given <paramref name="length"/>.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="length">The number of bits in the resulting `BitArray`</param>
        /// <returns>Returns a BitArray with length <paramref name="length"/> that represents the given integer
        /// with the endianness of <see cref="Endianness"/>.</returns>
        /// <remarks>
        /// <paramref name="value"/> is converted to binary form, of which the first <paramref name="length"/> bits
        /// are used to set the BitArray. Therefore, the resulting BitArray will only correctly represent
        /// <paramref name="value"/> if <paramref name="length"/> is of sufficient number. If not, the actual
        /// resulting BitArray will be the equivalent to the full binary representation truncated
        /// to <paramref name="length"/> (removing high-order bits).
        /// </remarks>
        public BitArray GetBits(int value, int length)
        {
            return CreateBitArrayOfLengthAndPopulate(length, GetBits(value));
        }

        /// <summary>
        /// Returns the specified 16-bit signed integer value as a <see cref="BitArray"/> padded or truncated
        /// to match the given <paramref name="length"/>.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="length">The number of bits in the resulting `BitArray`</param>
        /// <returns>Returns a BitArray with length <paramref name="length"/> that represents the given integer
        /// with the endianness of <see cref="Endianness"/>.</returns>
        /// <remarks>
        /// <paramref name="value"/> is converted to binary form, of which the first <paramref name="length"/> bits
        /// are used to set the BitArray. Therefore, the resulting BitArray will only correctly represent
        /// <paramref name="value"/> if <paramref name="length"/> is of sufficient number. If not, the actual
        /// resulting BitArray will be the equivalent to the full binary representation truncated
        /// to <paramref name="length"/> (removing high-order bits).
        /// </remarks>
        public BitArray GetBits(short value, int length)
        {
            return CreateBitArrayOfLengthAndPopulate(length, GetBits(value));
        }

        /// <summary>
        /// Returns the specified 8-bit signed integer value as a <see cref="BitArray"/> padded or truncated
        /// to match the given <paramref name="length"/>.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="length">The number of bits in the resulting `BitArray`</param>
        /// <returns>Returns a BitArray with length <paramref name="length"/> that represents the given integer
        /// with the endianness of <see cref="Endianness"/>.</returns>
        /// <remarks>
        /// <paramref name="value"/> is converted to binary form, of which the first <paramref name="length"/> bits
        /// are used to set the BitArray. Therefore, the resulting BitArray will only correctly represent
        /// <paramref name="value"/> if <paramref name="length"/> is of sufficient number. If not, the actual
        /// resulting BitArray will be the equivalent to the full binary representation truncated
        /// to <paramref name="length"/> (removing high-order bits).
        /// </remarks>
        public BitArray GetBits(byte value, int length)
        {
            return CreateBitArrayOfLengthAndPopulate(length, GetBits(value));
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from the <see cref="BitArray"/>.
        /// </summary>
        /// <param name="value">A `BitArray` that includes the four bytes to convert.</param>
        /// <param name="startIndex">The starting position within `value`.</param>
        /// <returns>A 32-bit signed integer formed by four bytes beginning at `startIndex`.</returns>
        /// <exception cref="ArgumentException">`startIndex` is greater than or equal to the length
        /// of `value` minus 3, and is less than or equal to the length of `value` minus 1.</exception>
        /// <exception cref="ArgumentNullException">`value` is `null`</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than
        /// the length of value minus 1.</exception>
        public int ToInt32(BitArray[] value)
        {
            throw new NotImplementedException();
            //return _byteConverter.ToInt32(Normalize(value), 0);
        }

        // TODO: Add methods to mirror BitConverter methods

        private static BitArray CreateBitArrayOfLengthAndPopulate(int length, BitArray ba)
        {
            var result = new BitArray(length);
            for (int x = 0; x < length; x++) result.Set(x, ba[x]);
            return result;
        }
    }
}
