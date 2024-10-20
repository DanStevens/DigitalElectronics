﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DotNetBitArray = System.Collections.BitArray;

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
    public class BitArray : ICollection, ICloneable, IReadOnlyList<Bit>, IReadOnlyList<bool>
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

        /// <summary>
        /// Gets or sets the value of the bit at a specific position in the BitArray.
        /// </summary>
        public Bit this[int index]
        {
            get => new (bitArray.Get(index));
            set => bitArray.Set(index, value?.Value ?? false);
        }

        bool IReadOnlyList<bool>.this[int index] => bitArray.Get(index);

        /// <summary>
        /// Gets the number of elements contained in the BitArray.
        /// </summary>
        public int Count => bitArray.Count;

        /// <summary>
        /// Gets a value indicating whether access to the BitArray is synchronized (thread safe).
        /// </summary>
        public bool IsSynchronized => bitArray.IsSynchronized;

        /// <summary>
        /// Gets an object that can be used to synchronize access to the BitArray.
        /// </summary>
        public object SyncRoot => bitArray.SyncRoot;

        /// <summary>
        /// Gets or sets the number of elements in the BitArray.
        /// </summary>
        public int Length => bitArray.Length;

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
        public void Get(int index) => bitArray.Get(index);

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
        /// Sets the bit at a specific position in the BitArray to the specified value.
        /// </summary>
        public void Set(int index, Bit value) => bitArray.Set(index, value?.Value ?? false);

        /// <summary>
        /// Sets all bits in the BitArray to the specified value.
        /// </summary>
        public void SetAll(bool value) => bitArray.SetAll(value);

        /// <summary>
        /// Sets all bits in the BitArray to the specified value.
        /// </summary>
        /// <param name="value"></param>
        public void SetAll(Bit value) => bitArray.SetAll(value?.Value ?? false);

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
            return string.Join(string.Empty, this.AsEnumerable<bool>().Select(b => b ? "1" : "0"));
        }

        internal string DebuggerDisplay =>
            ToString() + (Length <= sizeof(int)* 8 ? $" ({ToInt32()})" : string.Empty);

        /// <summary>
        /// Returns an enumerator that iterates through the BitArray.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return bitArray.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the BitArray as <see cref="Boolean"/> values.
        /// </summary>
        IEnumerator<bool> IEnumerable<bool>.GetEnumerator()
        {
            return new BitArrayAsBooleanEnumerator(bitArray.GetEnumerator());
        }

        IEnumerator<Bit> IEnumerable<Bit>.GetEnumerator()
        {
            return new BitArrayAsBitsEnumerator(bitArray.GetEnumerator());
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

        private class BitArrayAsBitsEnumerator : IEnumerator<Bit>
        {
            private readonly IEnumerator bitArrayEnumerator;

            public BitArrayAsBitsEnumerator(IEnumerator enumerator)
            {
                bitArrayEnumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
            }

            public bool MoveNext() => bitArrayEnumerator.MoveNext();

            public void Reset() => bitArrayEnumerator.Reset();

            public Bit Current => new ((bool)bitArrayEnumerator.Current);

            object IEnumerator.Current => bitArrayEnumerator.Current;

            public void Dispose()
            {
                // Nothing to dispose
            }
        }
    }
}
