using System;
using System.Linq;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.Utilities
{

    /// <summary>
    /// Converts base data types to an array of bytes, and an array of bytes to base data types.
    /// </summary>
    /// <remarks>The class can be configured to convert to and from binary forms using a
    /// specific endianness, or the system architecture's endianness.</remarks>
    /// <seealso cref="System.BitConverter.IsLittleEndian"/>
    // Dev note: This class has not been fully tested: Since most systems are little-endian, the endianness
    // conversion is probably skipped when ran on most systems by default.
    public class ByteConverter
    {
        public Endianness Endianness { get; }

        /// <summary>
        /// Creates new instance of `ByteConverter` configured to use the system's endianness
        /// </summary>
        /// <seealso cref="System.BitConverter.IsLittleEndian"/>
        public ByteConverter()
            : this (Endianness.System)
        {
        }

        /// <summary>
        /// Creates a new instance of `ByteConverter` configured to use the given endianness
        /// </summary>
        /// <param name="endianness">The endianness to use in conversions</param>
        public ByteConverter(Endianness endianness)
        {
            Endianness = endianness;
        }

        /// <summary>
        /// Returns the specified 32-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes with length 4, that represent the given integer with the endianness
        /// specified by <see cref="Endianness"/>.</returns>
        public byte[] GetBytes(int value)
        {
            return Normalize(System.BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Returns the specified 16-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes with length 2, that represent the given integer with the endianness
        /// specified by <see cref="Endianness"/>.</returns>
        public byte[] GetBytes(short value)
        {
            return Normalize(System.BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes
        /// at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes that includes the four bytes to convert.</param>
        /// <param name="startIndex">The starting position within `value`.</param>
        /// <returns>A 32-bit signed integer formed by four bytes beginning at `startIndex`.</returns>
        /// <exception cref="ArgumentException">`startIndex` is greater than or equal to the length
        /// of `value` minus 3, and is less than or equal to the length of `value` minus 1.</exception>
        /// <exception cref="ArgumentNullException">`value` is `null`</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is less than zero or greater than
        /// the length of value minus 1.</exception>
        public int ToInt32(byte[] value, int startIndex)
        {
            return System.BitConverter.ToInt32(Normalize(value), startIndex);
        }

        /// <summary>
        /// Returns a 16-bit signed integer converted from two bytes
        /// at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes that includes the two bytes to convert.</param>
        /// <param name="startIndex">The starting position within `value`.</param>
        /// <returns>A 16-bit signed integer formed by four bytes beginning at `startIndex`.</returns>
        /// <exception cref="ArgumentException">`startIndex` equals the length of `value` minus 1.</exception>
        /// <exception cref="ArgumentNullException">`value` is `null`</exception>
        /// <exception cref="ArgumentOutOfRangeException">`startIndex` is less than zero or
        /// greater than the length of `value` minus 1.</exception>
        public int ToInt16(byte[] value, int startIndex)
        {
            return System.BitConverter.ToInt16(Normalize(value), startIndex);
        }

        // TODO: Add methods to mirror BitConverter methods

        /// <summary>
        /// Normalises given byte array endianness specified by <see cref="Endianness"/>,
        /// assuming the given byte array is the form of the system architecture's endianness.
        /// </summary>
        /// <param name="bytes">The array of bytes to normalise</param>
        /// <returns>The parameter is returned unmodified if <see cref="Endianness"/> is
        /// `System` or is the same as the system archiecture's endianness</returns>
        private byte[] Normalize(byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            
            bool reverse = Endianness != Endianness.System &&
                (System.BitConverter.IsLittleEndian && Endianness != Endianness.Little);

            return reverse ? bytes.Reverse().ToArray() : bytes;
        }
    }
}
