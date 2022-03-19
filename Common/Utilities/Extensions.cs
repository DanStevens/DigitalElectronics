using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalElectronics.Utilities
{
    public static class Extensions
    {

        /// <summary>
        /// Returns an enumerable object of bools
        /// </summary>
        public static IEnumerable<bool> AsEnumerable(BitArray ba)
        {
            foreach (bool item in ba)
                yield return item;
        }

        /// <summary>
        /// Returns the binary representation of the BitArray as a string
        /// </summary>
        /// <returns>A string of '1's or '0's representing each bit of the BitArray</returns>
        public static string ToString(BitArray ba)
        {
            return string.Join(string.Empty, AsEnumerable(ba).Select(b => b ? "1" : "0"));
        }
    }
}
