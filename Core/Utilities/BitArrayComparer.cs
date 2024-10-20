﻿using System;
using System.Collections.Generic;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.Utilities
{
    public class BitArrayComparer : IComparer<BitArray>
    {
        public int Compare(BitArray x, BitArray y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (ReferenceEquals(null, y))
            {
                return 1;
            }

            if (ReferenceEquals(null, x))
            {
                return -1;
            }

            return x.ToByte().CompareTo(y.ToByte());
        }
    }
}
