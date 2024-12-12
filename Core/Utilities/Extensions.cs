using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.Utilities
{
    public static class Extensions
    {
        public static BitArray ToBitArray(this IList<Bit> values)
        {
            if (values.Count > BitArray.BitVector32Length)
                throw new ArgumentException(string.Format(BitArray.TooManyItemsMessageFormat, BitArray.BitVector32Length), nameof(values));

            var bitVector = new BitVector32();
            for (int i = 0; i < values.Count; i++)
                bitVector[1 << i] = values[i]?.Value ?? false;
            return new BitArray(bitVector, values.Count);
        }

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
