using System;
using System.Collections.Generic;
using System.Linq;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.Utilities
{
    public static class Extensions
    {
        public static BitArray ToBitArray(this IEnumerable<Bit> bits)
        {
            return new BitArray(bits.Select(b => b.Value));
        }

        public static BitArray ToBitArray(this IEnumerable<Bit> bits, int length)
        {
            return new BitArray(bits.Select(b => b.Value)) { Length = length };
        }
    }
}
