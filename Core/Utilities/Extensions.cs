using System;
using System.Collections.Generic;
using System.Linq;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.Utilities
{
    public static class Extensions
    {

        /// <summary>
        /// Returns an enumerable object of bools
        /// </summary>
        public static IEnumerable<bool> AsEnumerable(this BitArray ba)
        {
            foreach (bool item in ba ?? throw new ArgumentNullException(nameof(ba)))
                yield return item;
        }

        /// <summary>
        /// Returns the binary representation of the BitArray as a string
        /// </summary>
        /// <returns>A string of '1's or '0's representing each bit of the BitArray</returns>
        public static string ToString(this BitArray ba)
        {
            _ = ba ?? throw new ArgumentNullException(nameof(ba));
            return string.Join(string.Empty, AsEnumerable(ba).Select(b => b ? "1" : "0"));
        }

        public static byte ToByte(this BitArray ba)
        {
            _ = ba ?? throw new ArgumentNullException(nameof(ba));

            if (ba.Length > 8)
                throw new ArgumentException("Argument is too long to convert to byte without data loss.");

            byte[] bytes = new byte[1];
            ba.CopyTo(bytes, 0);
            return bytes[0];
        }
    }
}
