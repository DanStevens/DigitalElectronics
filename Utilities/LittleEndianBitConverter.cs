using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalElectronics.Utilities
{
    /// <summary>
    /// Converts base data types to an array of bytes, and an array of bytes to base data types.
    /// All conversions are ensured to be in little-endian regardless of the system architecture
    /// </summary>
    public static class LittleEndianBitConverter
    {
        public static byte[] GetBytes(int value)
        {
            return Normalize(BitConverter.GetBytes(value));
        }

        // TODO: Add methods to mirror BitConverter methods

        /// <summary>
        /// Normalises given byte array to little-endian form, depending on the current system
        /// architecture's endianness
        /// </summary>
        /// <param name="bytes">The array of bytes to normalise</param>
        /// <returns>The given array unmodified, if the current system is little-endian, otherwise
        /// the given array reversed</returns>
        private static byte[] Normalize(byte[] bytes)
        {
            return BitConverter.IsLittleEndian ? bytes : bytes.Reverse().ToArray();
        }
    }
}
