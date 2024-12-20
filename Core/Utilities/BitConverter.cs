﻿using System;
using System.Linq;
using System.Runtime.InteropServices;
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

        #region Constructors

        /// <summary>
        /// Creates new instance of `BitConverter` configured to use the system's endianness
        /// </summary>
        /// <seealso cref="System.BitConverter.IsLittleEndian"/>
        public BitConverter()
            : this(Endianness.System)
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

        #endregion
        
        #region GetBits method overloads

        /// <summary>
        /// Returns the specified 8-bit unsigned integer value as a <see cref="BitArray"/>.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>A `BitArray` with length 8, that represents the given integer.</returns>
        public BitArray GetBits(byte value)
        {
            return new BitArray(value);
        }

        /// <summary>
        /// Returns the specified 8-bit signed integer value as a <see cref="BitArray"/>.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>A `BitArray` with length 8, that represents the given integer>.</returns>
        public BitArray GetBits(sbyte value)
        {
            var bitArray = new BitArray((byte)value);
            bitArray.Length = sizeof(sbyte) * 8;
            return bitArray;
        }

        /// <summary>
        /// Returns the specified 16-bit unsigned integer value as a <see cref="BitArray"/>.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>A `BitArray` with length 16, that represents the given integer>.</returns>
        public BitArray GetBits(ushort value)
        {
            var bitArray = new BitArray(new int[] {value});
            bitArray.Length = sizeof(ushort) * 8;
            return bitArray;
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

        #endregion

        #region Integer conversion methods

        /// <summary>
        /// Returns a 32-bit signed integer converted from the <see cref="BitArray"/>.
        /// </summary>
        /// <param name="value">A `BitArray` that includes the four bytes to convert.</param>
        /// <exception cref="ArgumentNullException">`value` is `null`</exception>
        public int ToInt32(BitArray value)
        {
            return (int)BitArrayToInteger(value, sizeof(int) * 8);
        }

        /// <summary>
        /// Returns a 16-bit signed integer converted from the <see cref="BitArray"/>.
        /// </summary>
        /// <param name="value">A `BitArray` that includes the four bytes to convert.</param>
        /// <exception cref="ArgumentNullException">`value` is `null`</exception>
        public short ToInt16(BitArray value)
        {
            return (short)BitArrayToInteger(value, sizeof(short) * 8);
        }

        /// <summary>
        /// Returns an 8-bit signed integer converted from the <see cref="BitArray"/>.
        /// </summary>
        /// <param name="value">A `BitArray` that includes the four bytes to convert.</param>
        /// <exception cref="ArgumentNullException">`value` is `null`</exception>
        public sbyte ToSByte(BitArray value)
        {
            return (sbyte)BitArrayToInteger(value, sizeof(sbyte) * 8);
        }

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from the <see cref="BitArray"/>.
        /// </summary>
        /// <param name="value">A `BitArray` that includes the four bytes to convert.</param>
        /// <exception cref="ArgumentNullException">`value` is `null`</exception>
        public ushort ToUInt16(BitArray value)
        {
            return (ushort)BitArrayToInteger(value, sizeof(ushort) * 8);
        }

        /// <summary>
        /// Returns an 8-bit unsigned integer converted from the <see cref="BitArray"/>.
        /// </summary>
        /// <param name="value">A `BitArray` that includes the four bytes to convert.</param>
        /// <exception cref="ArgumentNullException">`value` is `null`</exception>
        public byte ToByte(BitArray value)
        {
            return (byte)BitArrayToInteger(value, sizeof(byte) * 8);
        }

        #endregion

        public string ToString(BitArray value, NumberFormat format)
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));

            var binaryString = MaybeToBinary();
            if (binaryString != null)
            {
                return binaryString;
            }

            bool isSigned = format is NumberFormat.SignedHexadecimal or NumberFormat.SignedDecimal;
            int toBase = GetBaseForFormat(format);
            return AdjustToFormat(ToStringRaw());

            string ToStringRaw()
            {
                if (value.Length <= 8)
                    return isSigned ? Convert.ToString(ToSByte(value), toBase) : Convert.ToString(ToByte(value), toBase);
                if (value.Length <= 16)
                    return isSigned ? Convert.ToString(ToInt16(value), toBase) : Convert.ToString(ToUInt16(value), toBase);
                if (value.Length <= 32)
                    return isSigned
                        ? Convert.ToString(ToInt32(value), toBase) :
                        //Convert.ToString(ToUInt32(value), toBase);
                        throw new NotImplementedException();
                //if (length <= 64)
                //    return isSigned ?
                //        Convert.ToString(ToUInt64(value), toBase) :
                //        Convert.ToString(ToUInt32(value), toBase);
                throw new NotImplementedException();
            }

            string AdjustToFormat(string s)
            {
                if (format is NumberFormat.SignedHexadecimal or NumberFormat.UnsignedHexadecimal)
                {
                    return s.ToUpperInvariant();
                }

                return s;
            }

            string MaybeToBinary()
            {
                if (format is NumberFormat.MsbBinary)
                {
                    return string.Join(string.Empty, value.AsEnumerable<bool>().Select(b => b ? "1" : "0"));
                }

                if (format is NumberFormat.LsbBinary)
                {
                    return string.Join(string.Empty, value.AsEnumerable<bool>().Reverse().Select(b => b ? "1" : "0"));
                }

                return null;
            }
        }

        private static int GetBaseForFormat(NumberFormat format)
        {
            return format switch
            {
                NumberFormat.SignedDecimal => 10,
                NumberFormat.UnsignedDecimal => 10,
                NumberFormat.SignedHexadecimal => 16,
                NumberFormat.UnsignedHexadecimal => 16,
                _ => throw new ArgumentException($"Cannot determine base for format {format}", nameof(format))
            };
        }

        private static Type InferIntTypeFromLength(int length, bool isSigned)
        {
            if (length <= 8)
                return isSigned ? typeof(sbyte) : typeof(byte);
            if (length <= 16)
                return isSigned ? typeof(short) : typeof(ushort);
            if (length <= 32)
                return isSigned ? typeof(int) : typeof(uint);
            if (length <= 64)
                return isSigned ? typeof(long) : typeof(ulong);
            throw new NotImplementedException();
        }

        private static BitArray CreateBitArrayOfLengthAndPopulate(int length, BitArray ba)
        {
            var result = new BitArray(length);
            for (int x = 0; x < length; x++) result.Set(x, ba[x]);
            return result;
        }

        private static ulong BitArrayToInteger(BitArray value, int length)
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));

            ulong result = 0;
            for (int i = 0; i < Math.Min(value.Count, length); i++)
            {
                if (value[i])
                {
                    result |= 1UL << i;
                }
            }

            return result;
        }
    }
}
