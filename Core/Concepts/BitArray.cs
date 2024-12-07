using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using BitConverter = DigitalElectronics.Utilities.BitConverter;

#if DEBUG
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Core.Tests")]
#endif

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
        private const string LengthOutOfRangeMessage = "Argument must be between 0 and 32 inclusive.";
        private const string TooManyItemsMessage = "Argument cannot contain more than 32 items.";

        private static readonly BitConverter bitConverter = new BitConverter();

        /// <summary>
        /// Casts the <see cref="BitArray"/> to the built-in .NET class <see cref="BitVector32"/>
        /// </summary>
        /// <param name="obj">The object to cast</param>
        public static implicit operator BitVector32(BitArray obj) => obj.bitVector;

        ////public static implicit operator BitArray(bool[] bools) => new (bools);
        
        private BitVector32 bitVector;
        private int length;

        public BitArray() : this(new BitVector32())
        { }

        public BitArray(int value, int length = 32) : this (new BitVector32(value))
        {
            if (length < 0 || length > 32)
                throw new ArgumentOutOfRangeException(nameof(length), LengthOutOfRangeMessage);

            Length = length;
        }

        public BitArray(BitVector32 bitArray)
        {
            bitVector = bitArray;
            Length = 32;
        }

        public BitArray(params bool[] values)
        {
            if (values.Length > 32)
                throw new ArgumentException(TooManyItemsMessage, nameof(values));

            bitVector = new BitVector32();
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i])
                {
                    bitVector[1 << i] = true;
                }
            }

            Length = values.Length;
        }


        public BitArray(params byte[] bytes)
        {
            if (bytes.Length > 4)
                throw new ArgumentException("Byte array must be 4 bytes or fewer.");

            // Combine the bytes into a single 32-bit integer
            int combinedValue = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                combinedValue |= bytes[i] << (8 * i);
            }

            // Create the BitVector32 from the combined integer value
            bitVector = new BitVector32(combinedValue);
            Length = bytes.Length * 8;
        }

        ////public BitArray(int length) => bitVector = new DotNetBitVector32(length);
        ////public BitArray(int[] values) => bitVector = new DotNetBitVector32(values ?? throw new ArgumentNullException(nameof(values)));
        //public BitArray(int length, bool defaultValue) => bitVector = new DotNetBitVector32(length, defaultValue);
        public BitArray(Bit[] bits) : this(bits.AsEnumerable()) { }
        
        public BitArray(IEnumerable<bool> values)
        {
            int i = 0;

            foreach (var v in values)
            {
                if (v)
                    bitVector[1 << i] = true;
                i++;

                if (i > 32)
                    throw new ArgumentException(TooManyItemsMessage, nameof(values));
            }

            Length = i;
        }

        public BitArray(IEnumerable<Bit> bits) : this(bits.Select(b => b?.Value ?? false)) { }

        /// <summary>
        /// Gets or sets the value of the bit at a specific position in the BitArray.
        /// </summary>
        /// <param name="index">The index of the bit to return, starting with least significant bit</param>
        public bool this[int index]
        {
            // BitVector32 uses mask to specify a bit but we want an index
            get => bitVector[1 << index];
            set => bitVector[1 << index] = value;
        }

        /// <summary>
        /// Gets or sets the number of elements in the BitArray.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The property is set to a value that is less than zero or greater than 32.</exception>
        /// <remarks>
        /// Length and Count return the same value. Length can be set to a specific value,
        /// but Count is read-only.
        /// 
        /// If Length is set to a value that is less than Count, the BitArray is truncated
        /// and the elements after the index value -1 are deleted.
        /// 
        /// If Length is set to a value that is greater than Count, the new elements are set
        /// to false.
        /// 
        /// Retrieving the value of this property is an O(1) operation. Setting this
        /// property is an O(n) operation.
        /// </remarks>
        public int Length
        {
            get => length;
            set
            {
                if (value < 0 || value > 32)
                    throw new ArgumentOutOfRangeException(nameof(value), LengthOutOfRangeMessage);
                length = value;
            }
        }

        /// <summary>
        /// Performs the bitwise AND operation between the elements of the current <see cref="BitArray"/>
        /// and the corresponding elements in the specified array. The current <see cref="BitArray"/>
        /// will be modified to store the result of the bitwise AND operation.
        /// </summary>
        public BitArray And(BitArray value)
        {
            return And(value.bitVector);
        }

        /// <summary>
        /// Performs the bitwise AND operation between the elements of the current <see cref="BitArray"/>
        /// and the corresponding elements in the specified array. The current <see cref="BitArray"/>
        /// will be modified to store the result of the bitwise AND operation.
        /// </summary>
        public BitArray And(BitVector32 value)
        {
            return new BitArray(new BitVector32(bitVector.Data & value.Data));
        }

        /// <summary>
        /// Gets the value of the bit at a specific position in the BitArray.
        /// </summary>
        public bool Get(int index) => bitVector[index];

        /// <summary>
        /// Shifts all the bit values of the current BitArray to the left on <paramref name="count"/> bits.
        /// </summary>
        public BitArray LeftShift(int count)
        {
            return new BitArray(new BitVector32(bitVector.Data << count));
        }

        /// <summary>
        /// Inverts all the bit values in the current BitArray, so that elements set to `true` are changed to
        /// `false`, and elements set to false are changed to true.
        /// </summary>
        public BitArray Not()
        {
            return new BitArray(new BitVector32(~bitVector.Data));
        }

        /// <summary>
        /// Performs the bitwise OR operation between the elements of the current <see cref="BitArray"/> and
        /// the corresponding elements in the specified array. The current <see cref="BitArray"/> will be
        /// modified to store the result of the bitwise OR operation.
        /// </summary>
        public BitArray Or(BitArray value)
        {
            return Or(value.bitVector);
        }

        /// <summary>
        /// Performs the bitwise OR operation between the elements of the current <see cref="BitArray"/> and
        /// the corresponding elements in the specified array. The current <see cref="BitArray"/> will be
        /// modified to store the result of the bitwise OR operation.
        /// </summary>
        public BitArray Or(BitVector32 value)
        {
            return new BitArray(new BitVector32(bitVector.Data | value.Data));
        }

        public BitArray RightShift(int count)
        {
            return new BitArray(new BitVector32(bitVector.Data >> count));
        }

        /// <summary>
        /// Sets the bit at a specific position in the BitArray to the specified value.
        /// </summary>
        public void Set(int index, bool value) => bitVector[index] = value;

        /// <summary>
        /// Sets all bits in the BitArray to the specified value.
        /// </summary>
        ////public void SetAll(bool value) => bitVector.SetAll(value);

        /// <summary>
        /// Performs the bitwise exclusive OR operation between the elements of the current <see cref="BitArray"/> against the
        /// corresponding elements in the specified array. The current <see cref="BitArray"/> will be modified to store the
        /// result of the bitwise exclusive OR operation.
        /// </summary>
        public BitArray Xor(BitArray value)
        {
            return Xor(value.bitVector);
        }

        /// <summary>
        /// Performs the bitwise exclusive OR operation between the elements of the current <see cref="BitArray"/> against the
        /// corresponding elements in the specified array. The current <see cref="BitArray"/> will be modified to store the
        /// result of the bitwise exclusive OR operation.
        /// </summary>
        public BitArray Xor(BitVector32 value)
        {
            return new BitArray(new BitVector32(bitVector.Data ^ value.Data));
        }

        public byte ToByte()
        {
            if (Length > sizeof(byte) * 8)
                throw new ArgumentException($"{nameof(BitArray)} is too long to convert to Byte without data loss.");

            return (byte)ToInt32();
        }

        public int ToInt32()
        {
            System.Diagnostics.Debug.Assert(Length > 0 && Length <= 32);

            if (Length == 32)
                return bitVector.Data;

            var mask = (1 << Length) - 1;
            return bitVector.Data & mask;
        }

        /// <summary>
        /// Returns the binary representation of the BitArray as a string
        /// </summary>
        /// <returns>A string of '1's or '0's representing each bit of the BitArray</returns>
        public override string ToString()
        {
            return ToString(NumberFormat.MsbBinary);
        }

        public string ToString(NumberFormat format)
        {
            return bitConverter.ToString(this, format);
        }

        /////// <summary>
        /////// Removes 'leading' zeros from the <see cref="BitArray"/>. The current <see cref="BitArray"/>
        /////// will be modified to store the result of the Trim operation.
        /////// </summary>
        /////// <returns>A <see cref="BitArray"/> representing the same value but with the leading
        /////// zeros removed. A BitArray representing zero is always trimmed to <see cref="Length"/>
        /////// of 1, never 0.</returns>
        /////// <remarks>From a positional notation perspective, the <see cref="Trim"/> method removes
        /////// any 'leading' zero digits from binary number represented by the <see cref="BitArray"/>.
        /////// For example, a BitArray of <see cref="Length"/> 8 representing the decimal 11
        /////// (which would be `00001011` in binary representation) will be 'trimmed' to a Length
        /////// of 4 (i.e. `1011`).
        ///////
        /////// From a technical perspective, since the BitArray orders bits from most-significant to
        /////// least significant, the method actually trims trailing bits. So with our previous
        /////// example, the decimal 11 would be represented internally with an array of 8 bools
        /////// <c>{true, true, false, true, false, false, false, false}</c>. The method would
        /////// mutate the BitArray to the array <c>{true, true, false, true}</c>
        ///////
        /////// The length of the BitArray is modified by assigning a new length to the
        /////// <see cref="Length"/> property</remarks>
        ////public void Trim()
        ////{
        ////    Trim(1);
        ////}

        /// <summary>
        /// Like <see cref="Trim()"/> method but only reduces the length of the <see cref="BitArray"/>
        /// at most down to the given <paramref name="targetLength"/>.
        /// </summary>
        /// <param name="targetLength">The desired length for the BitArray</param>
        /// <returns>A <see cref="BitArray"/> of either <see cref="Length"/>
        /// <paramref name="targetLength"/>targetLength</returns> or the longest length that can represent
        /// the same value. For example, calling <c>Trim(8)</c> on a BitArray representing decimal 11
        /// of length 32 will reduce the <see cref="Length"/> to 8, while the same call on a BitArray
        /// representing decimal 645 will be reduce the Length to 10, since this many bits is needed
        /// to fully represent the decimal value 645.
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="targetLength"/> is less than one</exception>
        ////public void Trim(int targetLength)
        ////{
        ////    if (targetLength < 0)
        ////        throw new ArgumentOutOfRangeException(nameof(targetLength), "Argument must be non-negative");

        ////    var leadingZerosCount = AsEnumerable<bool>().Reverse().TakeWhile(b => !b).Count();
        ////    var newLength = Math.Max(targetLength, Length - leadingZerosCount);
        ////    Length = Math.Max(1, newLength);
        ////}

        /// <summary>
        /// Returns this <see cref="BitArray"/> as an enumerable of the given type
        /// </summary>
        /// <typeparam name="T">The type of the enumerable, which can be <see cref="bool"/> or <see cref="Bit"/></typeparam>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> of bools or Bits;
        /// returns null if <typeparamref name="T"/> is any other type</returns>
        public IEnumerable<T> AsEnumerable<T>()
        {
            for (int i = 0; i < Length; i++)
            {
                if (typeof(T) == typeof(Bit))
                {
                    yield return (T)(object)new Bit(this[i]);
                }
                else
                {
                    yield return (T)(object)this[i];
                }
            }
        }

        public IEnumerable<bool> AsEnumerable() => AsEnumerable<bool>();

        /// <summary>
        /// Returns this <see cref="BitArray"/> as an list of the given type
        /// </summary>
        /// <typeparam name="T">The type of the list, which can be <see cref="bool"/> or <see cref="Bit"/></typeparam>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> of bools or Bits;
        /// returns null if <typeparamref name="T"/> is any other type</returns>
        public IList<T> ToList<T>() => AsEnumerable<T>().ToList();
        
        public IList<bool> ToList() => AsEnumerable().ToList();

        public IReadOnlyList<T> ToReadOnlyList<T>() => (IReadOnlyList<T>)ToList<T>();

        public IReadOnlyList<bool> ToReadOnlyList() => (IReadOnlyList<bool>)ToList();

        public T[] ToArray<T>() => AsEnumerable<T>().ToArray();

        public bool[] ToArray() => AsEnumerable<bool>().ToArray();

#if DEBUG
        internal string DebuggerDisplay =>
            ToString() + (Length <= sizeof(int) * 8 ? $" ({ToInt32()})" : string.Empty);

        internal string AsLsbBinaryString => ToString(NumberFormat.LsbBinary);

        internal string AsSignedDecimal => ToString(NumberFormat.SignedDecimal);

        internal string AsHexadecimal => ToString(NumberFormat.UnsignedHexadecimal);

#endif
    }
}
