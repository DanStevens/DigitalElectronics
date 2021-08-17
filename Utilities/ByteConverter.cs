using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalElectronics.Utilities
{

    /// <summary>
    /// The order of bits or bytes of a byte or word.
    /// </summary>
    public enum Endianness
    {
        /// <summary>The endianness according to the system</summary>
        System,
        
        /// <summary>Little-endian i.e. low-order bits/bytes are first</summary>
        Little,
            
        /// <summary>Big-endian i.e. high-order bits/bytes are first</summary>
        Big
    }

    /// <summary>
    /// Converts base data types to an array of bytes, and an array of bytes to base data types.
    /// </summary>
    /// <remarks>The class can be configured to convert to and from binary forms using a
    /// specific endianness, or the system architecture's endianness.</remarks>
    /// <seealso cref="System.BitConverter.IsLittleEndian"/>
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
        /// <returns>An array of bytes with length 4, that represent the given integer.</returns>
        public byte[] GetBytes(int value)
        {
            return Normalize(BitConverter.GetBytes(value));
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
            return BitConverter.ToInt32(Normalize(value), startIndex);
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
            
            bool reverse = Endianness == Endianness.System ||
                (BitConverter.IsLittleEndian && Endianness != Endianness.Little);

            return reverse ? bytes.Reverse().ToArray() : bytes;
        }
    }
}
