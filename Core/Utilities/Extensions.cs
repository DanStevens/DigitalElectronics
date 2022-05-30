using System;
using System.Collections.Generic;
using System.Linq;
using DigitalElectronics.Concepts;
using DotNetBitArray = System.Collections.BitArray;

namespace DigitalElectronics.Utilities
{
    public static class Extensions
    {

        /// <summary>
        /// Returns an enumerable object of bools
        /// </summary>
        public static IEnumerable<bool> AsEnumerable(this DotNetBitArray ba)
        {
            foreach (bool item in ba ?? throw new ArgumentNullException(nameof(ba)))
                yield return item;
        }

        /// <summary>
        /// Returns the binary representation of the BitArray as a string
        /// </summary>
        /// <returns>A string of '1's or '0's representing each bit of the BitArray</returns>
        public static string ToString(this DotNetBitArray ba)
        {
            _ = ba ?? throw new ArgumentNullException(nameof(ba));
            return string.Join(string.Empty, AsEnumerable(ba).Select(b => b ? "1" : "0"));
        }

        public static byte ToByte(this DotNetBitArray dnba)
        {
            _ = dnba ?? throw new ArgumentNullException(nameof(dnba));

            if (dnba.Length > 8)
                throw new ArgumentException("Argument is too long to convert to byte without data loss.");

            byte[] bytes = new byte[1];
            dnba.CopyTo(bytes, 0);
            return bytes[0];
        }

        public static BitArray ToBitArray(this IEnumerable<Bit> bits)
        {
            return new BitArray(bits.ToArray());
        }

        public static BitArray ToBitArray(this IEnumerable<bool> bits)
        {
            return new BitArray(bits.ToArray());
        }
    }
}
