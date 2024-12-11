using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using BitConverter = DigitalElectronics.Utilities.BitConverter;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Core.Tests")]

namespace DigitalElectronics.Concepts
{
    /// <summary>
    /// Manages a compact array of bit values, which are represented as Booleans, where `true` indicates
    /// that the bit is on (1) and `false` indicates the bit is off (0).
    /// </summary>
    /// <remarks>For memory efficiency, this class uses the <see cref="BitVector32"/> type as its internal
    /// storage for bits but combines this with a <see cref="Length"/> value. Methods such as
    /// <see cref="ToByte"/> and <see cref="ToInt32"/> automatically mask the value of the internal <see cref="BitVector32"/>
    /// according to the value of <see cref="Length"/>. This means the <see cref="BitArray"/> type
    /// can be used to represent binary data of any bit size, up to the maximum allowed by <see cref="bitVector"/>
    /// (that is 32 bits).</remarks>
    /// <seealso cref="BitVector32"/>
    [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct BitArray
    {
        internal const int BitVector32Length = 32;
        internal const string TooManyItemsMessageFormat = "Argument cannot contain more than {0} items.";
        internal static readonly string LengthOutOfRangeMessage = $"Argument must be between 0 and {BitVector32Length} inclusive.";
        
        private static readonly BitConverter bitConverter = new BitConverter();

        private BitVector32 bitVector;
        private int length;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitArray"/> with a value of 0 and length of 32
        /// </summary>
        public BitArray() : this(new BitVector32())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitArray"/> representing the given integer
        /// value and length
        /// </summary>
        /// <param name="value">The value to represent with the <see cref="BitArray"/></param>
        /// <param name="length">The length of the <see cref="BitArray"/> (see <see cref="Length"/>)</param>
        /// <exception cref="ArgumentOutOfRangeException">if <paramref name="length"/> is less 
        /// than zero or greater than 32</exception>
        public BitArray(int value, int length = BitVector32Length) : this (new BitVector32(value))
        {
            if (length < 0 || length > BitVector32Length)
                throw new ArgumentOutOfRangeException(nameof(length), LengthOutOfRangeMessage);

            Length = length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitArray"/> to be equivalent to the given
        /// <see cref="BitVector32"/>
        /// </summary>
        /// <param name="bitArray">The <see cref="BitVector32"/> to initialize this <see cref="BitArray"/></param>
        /// <param name="length">The length of the <see cref="BitArray"/> (see <see cref="Length"/>). Default is 32</param>
        public BitArray(BitVector32 bitArray, int length = BitVector32Length)
        {
            if (length < 0 || length > BitVector32Length)
                throw new ArgumentOutOfRangeException(nameof(length), LengthOutOfRangeMessage);

            bitVector = bitArray;
            Length = length;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="BitArray"/> to match the bit pattern given by array of
        /// <see cref="bool"/> values, starting with the least-significant bit
        /// </summary>
        /// <param name="values">An array of <see cref="bool"/> values in LSB-first order</param>
        /// <exception cref="ArgumentException">if <paramref name="values"/> contains more than 32 items</exception>
        /// <remarks>The <see cref="Length"/> property is set to length of <paramref name="values"/></remarks>
        public BitArray(params bool[] values) : this(values.AsSpan()) { }

        /// <summary>
        /// Initialize a new instance of the <see cref="BitArray"/> to match the bit pattern given a
        /// <see cref="Span{bool}"/>, starting with the least-significant bit
        /// </summary>
        /// <param name="values">A <see cref="Span{bool}"/> in LSB-first order</param>
        /// <exception cref="ArgumentException">if <paramref name="values"/> contains more than 32 items</exception>
        /// <remarks>The <see cref="Length"/> property is set to length of <paramref name="values"/></remarks>
        public BitArray(params Span<bool> values)
        {
            if (values.Length > BitVector32Length)
                throw new ArgumentException(string.Format(TooManyItemsMessageFormat, BitVector32Length), nameof(values));

            bitVector = new BitVector32();
            for (int i = 0; i < values.Length; i++)
                bitVector[1 << i] = values[i];
            Length = values.Length;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="BitArray"/> to match the bit pattern given by array of
        /// <see cref="bytes"/> values, in little-endian order
        /// </summary>
        /// <param name="bytes">An array of <see cref="byte"/> values in little-endian order</param>
        /// <exception cref="ArgumentException">if <paramref name="values"/> contains more than 32 items</exception>
        /// <remarks>The <see cref="Length"/> property is set to length of <paramref name="values"/></remarks>
        /// <exception cref="ArgumentException">if <paramref name="bytes"/> contains more than 32 items</exception>
        public BitArray(params byte[] bytes)
        {
            if (bytes.Length > 4)
                throw new ArgumentException(string.Format(TooManyItemsMessageFormat, 4), nameof(bytes));

            // Combine the bytes into a single 32-bit integer
            int combinedValue = 0;
            for (int i = 0; i < bytes.Length; i++)
                combinedValue |= bytes[i] << (8 * i);

            // Create the BitVector32 from the combined integer value
            bitVector = new BitVector32(combinedValue);
            Length = bytes.Length * 8;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="BitArray"/> to match the bit pattern of the sequence of
        /// <see cref="bool"/> values yielded by the given enumerable, starting with the least-significant bit
        /// </summary>
        /// <param name="values">An sequence returning <see cref="bool"/> values representing the bit pattern
        /// in LSB-first order</param>
        /// <remarks>The <see cref="Length"/> property is to equal the number of values yielded by<paramref name="values"/>
        /// up to a maximum of 32</remarks>
        public BitArray(IEnumerable<bool> values)
        {
            int i = 0;
            foreach (var v in values.Take(BitVector32Length))
                bitVector[1 << i++] = v;
            Length = i;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="BitArray"/> to match the bit pattern of the sequence of
        /// <see cref="bool"/> values yielded by the given enumerable, starting with the least-significant bit
        /// </summary>
        /// <param name="values">An sequence returning <see cref="bool"/> values representing the bit pattern
        /// in LSB-first order</param>
        /// <param name="length">The maximum number of items to take from <paramref name="values"/>.
        /// Also determines the value of the <see cref="Length"/> property</param>
        /// <exception cref="ArgumentOutOfRangeException">if <paramref name="length"/> is less than zero or greater than 32
        /// </exception>
        /// <remarks>The <see cref="Length"/> property is to equal the number of values yielded by<paramref name="values"/>
        /// up to a maximum of 32</remarks>
        public BitArray(IEnumerable<bool> values, int length)
        {
            if (length < 0 || length > BitVector32Length)
                throw new ArgumentOutOfRangeException(nameof(length), LengthOutOfRangeMessage);

            int i = 0;
            foreach (var v in values.Take(length))
                bitVector[1 << i++] = v;
            Length = length;
        }

        public BitArray(ICollection<bool> values) : this(values,
            values.Count <= BitVector32Length
                ? values.Count
                : throw new ArgumentException(string.Format(TooManyItemsMessageFormat, BitVector32Length), nameof(values)))
        { }

        /// <summary>
        /// Gets or sets the value of the bit at a specific position in the <see cref="BitArray"/>.
        /// </summary>
        /// <param name="index">The index of the bit to return, starting with least significant bit</param>
        public bool this[int index]
        {
            // BitVector32 uses mask to specify a bit but we want an index
            get => bitVector[1 << index];
            set => bitVector[1 << index] = value;
        }

        /// <summary>
        /// Gets or sets the length of the <see cref="BitArray"/> in bits
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The property is set to a value that is less
        /// than zero or greater than 32.</exception>
        /// <remarks>
        /// This property affects the values returned by <see cref="ToByte"/> and <see cref="ToInt32"/>
        /// methods. See these methods for more details.
        /// </remarks>
        public int Length
        {
            get => length;
            set
            {
                if (value < 0 || value > BitVector32Length)
                    throw new ArgumentOutOfRangeException(nameof(value), LengthOutOfRangeMessage);
                length = value;
            }
        }

        /// <summary>
        /// Returns a new <see cref="BitArray"/> containing the result of performing a bitwise AND
        /// operation between the bits of the current <see cref="BitArray"/> with the given
        /// <see cref="BitArray"/> instance.
        /// </summary>
        /// <param name="value">The <see cref="BitArray"/> to perform the bitwise operation with the
        /// current instance</param>
        public BitArray And(BitArray value)
        {
            return And(value.bitVector);
        }

        /// <summary>
        /// Returns a new <see cref="BitArray"/> containing the result of performing a bitwise AND
        /// operation between the bits of the current <see cref="BitArray"/> with the given
        /// <see cref="BitVector32"/> instance.
        /// </summary>
        /// <param name="value">The <see cref="BitVector32"/> to perform the bitwise operation with the
        /// current instance</param>
        public BitArray And(BitVector32 value)
        {
            return new BitArray(new BitVector32(bitVector.Data & value.Data));
        }

        /// <summary>
        /// Returns a new <see cref="BitArray"/> containing the result of performing a bitwise LEFT-SHIFT
        /// operation the given number of times against the current <see cref="BitArray"/>.
        /// </summary>
        /// <param name="count">The number of times to perform the bitwise operation with the
        /// current instance</param>
        public BitArray LeftShift(int count)
        {
            return new BitArray(new BitVector32(bitVector.Data << count));
        }

        /// <summary>
        /// Returns a new <see cref="BitArray"/> containing the result of performing a bitwise NOT
        /// operation against the current <see cref="BitArray"/>.
        /// </summary>
        public BitArray Not()
        {
            return new BitArray(new BitVector32(~bitVector.Data));
        }

        /// <summary>
        /// Returns a new <see cref="BitArray"/> containing the result of performing a bitwise OR
        /// operation between the bits of the current <see cref="BitArray"/> with the given
        /// <see cref="BitArray"/> instance.
        /// </summary>
        /// <param name="value">The <see cref="BitArray"/> to perform the bitwise operation with the
        /// current instance</param>
        public BitArray Or(BitArray value)
        {
            return Or(value.bitVector);
        }

        /// <summary>
        /// Returns a new <see cref="BitArray"/> containing the result of performing a bitwise OR
        /// operation between the bits of the current <see cref="BitArray"/> with the given
        /// <see cref="BitVector32"/> instance.
        /// </summary>
        /// <param name="value">The <see cref="BitVector32"/> to perform the bitwise operation with the
        /// current instance</param>
        public BitArray Or(BitVector32 value)
        {
            return new BitArray(new BitVector32(bitVector.Data | value.Data));
        }

        /// <summary>
        /// Returns a new <see cref="BitArray"/> containing the result of performing a bitwise RIGHT-SHIFT
        /// operation the given number of times against the current <see cref="BitArray"/>.
        /// </summary>
        /// <param name="count">The number of times to perform the bitwise operation with the
        /// current instance</param>
        public BitArray RightShift(int count)
        {
            return new BitArray(new BitVector32(bitVector.Data >> count));
        }

        /// <summary>
        /// Returns a new <see cref="BitArray"/> containing the result of performing a bitwise XOR
        /// operation between the bits of the current <see cref="BitArray"/> with the given
        /// <see cref="BitArray"/> instance.
        /// </summary>
        /// <param name="value">The <see cref="BitArray"/> to perform the bitwise operation with the
        /// current instance</param>
        public BitArray Xor(BitArray value)
        {
            return Xor(value.bitVector);
        }

        /// <summary>
        /// Returns a new <see cref="BitArray"/> containing the result of performing a bitwise XOR
        /// operation between the bits of the current <see cref="BitArray"/> with the given
        /// <see cref="BitVector32"/> instance.
        /// </summary>
        /// <param name="value">The <see cref="BitVector32"/> to perform the bitwise operation with the
        /// current instance</param>
        public BitArray Xor(BitVector32 value)
        {
            return new BitArray(new BitVector32(bitVector.Data ^ value.Data));
        }

        /// <summary>
        /// Returns the equivalent <see cref="byte"/> value for the bit pattern represented by
        /// the <see cref="BitArray"/>
        /// </summary>
        /// <remarks>This method simply calls <see cref="ToInt32"/> and cases it to a <see cref="byte"/></remarks>
        public byte ToByte()
        {
            return (byte)ToInt32();
        }

        /// <summary>
        /// Returns the equivalent <see cref="int"/> value for the bit pattern represented by
        /// the <see cref="BitArray"/>
        /// </summary>
        /// <remarks>The return value is influenced by the <see cref="Length"/> property, which is
        /// used to generate a mask that is selects the first <see cref="Length"/> number of bits
        /// starting with the least-significant bit.</remarks>
        public int ToInt32()
        {
            System.Diagnostics.Debug.Assert(Length >= 0 && Length <= BitVector32Length);

            if (Length == BitVector32Length)
                return bitVector.Data;

            var mask = (1 << Length) - 1;
            return bitVector.Data & mask;
        }

        /// <summary>
        /// Returns the binary representation of the <see cref="BitArray"/> as a string, starting with
        /// most significant bit
        /// </summary>
        /// <returns>A string of '1's or '0's representing each bit of the BitArray in MSB order</returns>
        public override string ToString()
        {
            return ToString(NumberFormat.MsbBinary);
        }

        /// <summary>
        /// Returns the string representation <see cref="BitArray"/> according to the given
        /// <see cref="NumberFormat"/>
        /// </summary>
        /// <param name="format">The <see cref="NumberFormat"/> to use</param>
        public string ToString(NumberFormat format)
        {
            return bitConverter.ToString(this, format);
        }


        /// <summary>
        /// Returns this <see cref="BitArray"/> as an enumerable of the given type
        /// </summary>
        /// <typeparam name="T">The type of the items of the enumerable, which can be <see cref="bool"/>
        /// or <see cref="Bit"/></typeparam>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> of bools or Bits</returns>
        /// <<exception cref="InvalidCastException">if <typeparamref name="T"/> is anything besides
        /// <see cref="bool"/> or <see cref="Bit"/></exception>
        public IEnumerable<T> AsEnumerable<T>()
        {
            ValidateType<T>();
            return Enumerate(this, Length);

            // Pass all data in variables to avoid capturing variables
            // Capturing `this` isn't allowed in structs (CS1673)
            IEnumerable<T> Enumerate(BitArray @this, int length)
            {
                for (int i = 0; i < length; i++)
                {
                    if (typeof(T) == typeof(Bit))
                        yield return (T)(object)new Bit(@this[i]);
                    else
                        yield return (T)(object)@this[i];
                }
            }
        }

        /// <summary>
        /// Returns this <see cref="BitArray"/> as an enumerable of <see cref="bool"/> values
        /// </summary>
        /// <returns>An enumerable that yields <see cref="bool"/> values representing the
        /// bit pattern of the <see cref="BitArray"/> starting with the least-significant bit</returns>
        public IEnumerable<bool> AsEnumerable() => AsEnumerable<bool>();

        /// <summary>
        /// Returns this <see cref="BitArray"/> as an array of the given type
        /// </summary>
        /// <typeparam name="T">The type of the items of the array, which can be <see cref="bool"/>
        /// or <see cref="Bit"/></typeparam>
        /// <returns>Returns an array of bools or Bits</returns>
        /// <<exception cref="InvalidCastException">if <typeparamref name="T"/> is anything besides
        /// <see cref="bool"/> or <see cref="Bit"/></exception>
        public T[] ToArray<T>()
        {
            ValidateType<T>();
            return (T[])(object)((typeof(T) == typeof(Bit)) ? Fill(new Bit[Length]) : Fill(new bool[Length]));
        }

        /// <summary>
        /// Returns this <see cref="BitArray"/> as an array of <see cref="bool"/> values
        /// </summary>
        /// <returns>An array of <see cref="bool"/> values representing the
        /// bit pattern of the <see cref="BitArray"/> starting with the least-significant bit</returns>
        public bool[] ToArray() => ToArray<bool>();

        private static void ValidateType<T>()
        {
            if (typeof(T) != typeof(bool) && typeof(T) != typeof(Bit))
                throw new NotSupportedException($"Only the following types are supported: System.Boolean, {typeof(Bit)}");
        }

        private IList<bool> Fill(IList<bool> list)
        {
            for (int i = 0; i < Length; i++)
                list[i] = this[i];
            return list;
        }

        private IList<Bit> Fill(IList<Bit> list)
        {
            for (int i = 0; i < Length; i++)
                list[i] = new Bit(this[i]);
            return list;
        }

#if DEBUG
        internal string DebuggerDisplay =>
            ToString() + (Length <= sizeof(int) * 8 ? $" ({ToInt32()})" : string.Empty);

        internal string AsLsbBinaryString => ToString(NumberFormat.LsbBinary);

        internal string AsSignedDecimal => ToString(NumberFormat.SignedDecimal);

        internal string AsHexadecimal => ToString(NumberFormat.UnsignedHexadecimal);
#endif
    }
}
