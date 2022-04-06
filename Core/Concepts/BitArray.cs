using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using DigitalElectronics.Utilities;
using DotNetBitArray = System.Collections.BitArray;

#if DEBUG
[assembly: InternalsVisibleTo("MiscTests")]
#endif

namespace DigitalElectronics.Concepts
{
    /// <summary>
    /// Manages a compact array of bit values, which are represented as Booleans, where `true` indicates
    /// that the bit is on (1) and `false` indicates the bit is off (0).
    /// </summary>
    /// <note>This class is a wrapper for the built-in .NET class <see cref="DotNetBitArray"/>, extending it
    /// in such a way as to add features like <see cref="IReadOnlyList{Boolean}"/> and
    /// <see cref="IEnumerable{Boolean}"/></note>
    /// <seealso cref="DotNetBitArray"/>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class BitArray : IList<bool>, IList, ICloneable, IReadOnlyList<bool>
    {
        /// <summary>
        /// Casts the <see cref="BitArray"/> to the built-in .NET class <see cref="DotNetBitArray"/>
        /// </summary>
        /// <param name="obj">The object to cast</param>
        public static implicit operator DotNetBitArray(BitArray obj) => obj.bitArray;

        public static implicit operator BitArray(bool[] bools) => new (bools);
        
        private readonly DotNetBitArray bitArray;

        // Duplicate the constructors for .NET BitArray
        public BitArray(DotNetBitArray bitArray) => this.bitArray = bitArray ?? throw new ArgumentNullException(nameof(bitArray));
        public BitArray(params bool[] values) => bitArray = new DotNetBitArray(values ?? throw new ArgumentNullException(nameof(values)));
        public BitArray(params byte[] bytes) => bitArray = new DotNetBitArray(bytes ?? throw new ArgumentNullException(nameof(bytes)));
        public BitArray(BitArray bits) => bitArray = new DotNetBitArray(bits.ToArray<bool>());
        public BitArray(int length) => bitArray = new DotNetBitArray(length);
        public BitArray(int[] values) => bitArray = new DotNetBitArray(values ?? throw new ArgumentNullException(nameof(values)));
        public BitArray(int length, bool defaultValue) => bitArray = new DotNetBitArray(length, defaultValue);
        public BitArray(Bit[] bits) : this(bits.Select(b => b?.Value ?? false).ToArray()) {}
        public BitArray(IEnumerable<bool> values) => bitArray = new DotNetBitArray(values.ToArray());

        /// <summary>
        /// Gets or sets the value of the bit at a specific position in the BitArray.
        /// </summary>
        public bool this[int index]
        {
            get => bitArray[index];
            set => bitArray[index] = value;
        }

        /// <summary>
        /// Gets the number of elements contained in the BitArray.
        /// </summary>
        public int Count => bitArray.Count;

        /// <summary>
        /// Gets or sets the number of elements in the BitArray.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The property is set to a value that is less than zero.</exception>
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
            get => bitArray.Length;
            set => bitArray.Length = value;
        }

        public bool IsSynchronized => bitArray.IsSynchronized;

        public object SyncRoot => bitArray.SyncRoot;

        public bool IsReadOnly => false;

        /// <summary>
        /// Performs the bitwise AND operation between the elements of the current BitArray object
        /// and the corresponding elements in the specified array. The current BitArray object
        /// will be modified to store the result of the bitwise AND operation.
        /// </summary>
        public BitArray And(BitArray value)
        {
            bitArray.And(value); // Mutates bitArray
            return this;
        }

        /// <summary>
        /// Performs the bitwise AND operation between the elements of the current BitArray object
        /// and the corresponding elements in the specified array. The current BitArray object
        /// will be modified to store the result of the bitwise AND operation.
        /// </summary>
        public BitArray And(DotNetBitArray value)
        {
            bitArray.And(value); // Mutates bitArray
            return this;
        }

        /// <summary>
        /// Creates a shallow copy of the BitArray.
        /// </summary>
        public object Clone() => new BitArray((DotNetBitArray)bitArray.Clone());

        /// <summary>
        /// Copies the entire BitArray to a compatible one-dimensional Array, starting at the specified index
        /// of the target array.
        /// </summary>
        public void CopyTo(Array array, int index) => bitArray.CopyTo(array, index);

        /// <summary>
        /// Gets the value of the bit at a specific position in the BitArray.
        /// </summary>
        public bool Get(int index) => bitArray.Get(index);

        /// <summary>
        /// Shifts all the bit values of the current BitArray to the left on <paramref name="count"/> bits.
        /// </summary>
        public BitArray LeftShift(int count)
        {
            bitArray.LeftShift(count); // Mutates bitArray
            return this;
        }

        /// <summary>
        /// Inverts all the bit values in the current BitArray, so that elements set to `true` are changed to
        /// `false`, and elements set to false are changed to true.
        /// </summary>
        public BitArray Not()
        {
            bitArray.Not();  // Mutates bitArray
            return this;
        }

        /// <summary>
        /// Performs the bitwise OR operation between the elements of the current BitArray object and
        /// the corresponding elements in the specified array. The current BitArray object will be
        /// modified to store the result of the bitwise OR operation.
        /// </summary>
        public BitArray Or(BitArray value)
        {
            bitArray.Or(value);  // Mutates bitArray
            return this;
        }

        /// <summary>
        /// Performs the bitwise OR operation between the elements of the current BitArray object and
        /// the corresponding elements in the specified array. The current BitArray object will be
        /// modified to store the result of the bitwise OR operation.
        /// </summary>
        public BitArray Or(DotNetBitArray value)
        {
            bitArray.Or(value);  // Mutates bitArray
            return this;
        }

        public BitArray RightShift(int count)
        {
            bitArray.RightShift(count);  // Mutates bitArray
            return this;
        }

        /// <summary>
        /// Sets the bit at a specific position in the BitArray to the specified value.
        /// </summary>
        public void Set(int index, bool value) => bitArray.Set(index, value);

        /// <summary>
        /// Sets all bits in the BitArray to the specified value.
        /// </summary>
        public void SetAll(bool value) => bitArray.SetAll(value);

        /// <summary>
        /// Performs the bitwise exclusive OR operation between the elements of the current BitArray object against the
        /// corresponding elements in the specified array. The current BitArray object will be modified to store the
        /// result of the bitwise exclusive OR operation.
        /// </summary>
        public BitArray Xor(BitArray value)
        {
            bitArray.Xor(value);  // Mutates bitArray
            return this;
        }

        /// <summary>
        /// Performs the bitwise exclusive OR operation between the elements of the current BitArray object against the
        /// corresponding elements in the specified array. The current BitArray object will be modified to store the
        /// result of the bitwise exclusive OR operation.
        /// </summary>
        public BitArray Xor(DotNetBitArray value)
        {
            bitArray.Xor(value);  // Mutates bitArray
            return this;
        }

        public IReadOnlyList<T> AsReadOnlyList<T>()
        {
            return (IReadOnlyList<T>)this;
        }

        public byte ToByte()
        {
            if (Length > sizeof(byte) * 8)
                throw new ArgumentException($"{nameof(BitArray)} is too long to convert to Byte without data loss.");

            byte[] arr = new byte[1];
            CopyTo(arr, 0);
            return arr[0];
        }

        public int ToInt32()
        {
            if (Length > sizeof(int) * 8)
                throw new ArgumentException($"{nameof(BitArray)} is too long to convert to Int32 without data loss.");

            int[] arr = new int[1];
            CopyTo(arr, 0);
            return arr[0];
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
            switch (format)
            {
                case NumberFormat.LsbBinary:
                    return string.Join(string.Empty, this.AsEnumerable<bool>().Reverse().Select(b => b ? "1" : "0"));
                case NumberFormat.SignedDecimal:
                    return ToInt32().ToString();
                case NumberFormat.MsbBinary:
                default:
                    return string.Join(string.Empty, this.AsEnumerable<bool>().Select(b => b ? "1" : "0"));
            }
        }

        /// <summary>
        /// Returns this <see cref="BitArray"/> as an enumerable of the given type
        /// </summary>
        /// <typeparam name="T">The type of the enumerable, which can be <see cref="bool"/> or <see cref="Bit"/></typeparam>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> of bools or Bits;
        /// returns null if <typeparamref name="T"/> is any other type</returns>
        public IEnumerable<T> AsEnumerable<T>()
        {
            if (typeof(T) == typeof(bool))
                return (IEnumerable<T>)bitArray.AsEnumerable();
            if (typeof(T) == typeof(Bit))
                return (IEnumerable<T>)this.Select(b => new Bit(b));
            return null;
        }

        #region IList<bool>, ICollection<bool> implementation

        int IList<bool>.IndexOf(bool item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i] == item) return i;
            }

            return -1;
        }

        void IList<bool>.Insert(int index, bool item) => BlockAttemptToAddRemoveOrInsertItems();
        void IList<bool>.RemoveAt(int index) => BlockAttemptToAddRemoveOrInsertItems();
        void ICollection<bool>.Add(bool item) => BlockAttemptToAddRemoveOrInsertItems();
        void ICollection<bool>.Clear() => Length = 0;
        bool ICollection<bool>.Contains(bool item) => throw new NotSupportedException();

        bool IList<bool>.this[int index]
        {
            get => this[index];
            set => this[index] = value;
        }

        void ICollection<bool>.CopyTo(bool[] array, int arrayIndex)
        {
            bitArray.CopyTo(array, arrayIndex);
        }

        bool ICollection<bool>.Remove(bool item)
        {
            BlockAttemptToAddRemoveOrInsertItems();
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new BitArrayAsBooleanEnumerator(bitArray.GetEnumerator());
        }


        #endregion

        #region IList, ICollection implementation

        int IList.Add(object value)
        {
            BlockAttemptToAddRemoveOrInsertItems();
            return default;
        }


        bool IList.Contains(object value)
        {
            BlockAttemptToAddRemoveOrInsertItems();
            return false;
        }

        int IList.IndexOf(object value) => value is bool b ? ((IList<bool>)this).IndexOf(b) : -1;
        void IList.Insert(int index, object value) => BlockAttemptToAddRemoveOrInsertItems();
        void IList.Remove(object value) => BlockAttemptToAddRemoveOrInsertItems();
        void IList.RemoveAt(int index) => BlockAttemptToAddRemoveOrInsertItems();
        void IList.Clear() => Length = 0;

        void ICollection.CopyTo(Array array, int index)
        {
            bitArray.CopyTo(array, index);
        }

        /// <summary>
        /// A <see cref="BitArray"/> is not fixed-size, but the same can only be changed by
        /// changing the <see cref="Length"/> property.
        /// </summary>
        bool IList.IsFixedSize => false;

        object IList.this[int index]
        {
            get => this[index];
            set => this[index] = (bool)value;
        }

        #endregion


        private static void BlockAttemptToAddRemoveOrInsertItems()
        {
            throw new NotSupportedException(
                "Adding, removing and inserting of bits is not supported, but the size can be changed " +
                "by changing the Length property");
        }

        #region IEnumerable<bool> members

        /// <summary>
        /// Returns an enumerator that iterates through the BitArray as <see cref="Boolean"/> values.
        /// </summary>
        IEnumerator<bool> IEnumerable<bool>.GetEnumerator()
        {
            return new BitArrayAsBooleanEnumerator(bitArray.GetEnumerator());
        }

        private class BitArrayAsBooleanEnumerator : IEnumerator<bool>
        {
            private readonly IEnumerator bitArrayEnumerator;

            public BitArrayAsBooleanEnumerator(IEnumerator enumerator)
            {
                bitArrayEnumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
            }

            public bool MoveNext() => bitArrayEnumerator.MoveNext();

            public void Reset() => bitArrayEnumerator.Reset();

            public bool Current => (bool)bitArrayEnumerator.Current;

            object IEnumerator.Current => bitArrayEnumerator.Current;

            public void Dispose()
            {
                // Nothing to dispose
            }
        }

        #endregion

#if DEBUG
        internal string DebuggerDisplay =>
            ToString() + (Count <= sizeof(int) * 8 ? $" ({ToInt32()})" : string.Empty);

        internal string AsLsbBinaryString => ToString(NumberFormat.LsbBinary);

        internal string AsSignedDecimal => ToString(NumberFormat.SignedDecimal);

#endif
    }
}
