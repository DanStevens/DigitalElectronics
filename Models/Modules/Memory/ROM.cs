using System;
using System.Linq;
using System.Collections.Generic;
using DigitalElectronics.Concepts;
using BitConverter = DigitalElectronics.Utilities.BitConverter;
using System.Diagnostics;

namespace DigitalElectronics.Modules.Memory
{

    /// <summary>
    /// Represents a Read-Only Memory module
    /// </summary>
    /// <remarks>The contents of the ROM are specified at creation time as a sequence of bytes of any
    /// arbitrary length. Thereafter the contents can be output (read) but not modified.</remarks>
    [DebuggerDisplay("{this.Label,nq}: Addr={this.ProbeAddress()}")]
    public class ROM : IOutputModule, IDedicatedAddressable
    {
        private readonly BitConverter _bitConverter = new();
        private readonly byte[] _data;
        private int _address;
        private bool _enabled;

        /// <summary>
        /// Creates a Read-Only Memory module with the given contents
        /// </summary>
        /// <param name="data">A sequence of bytes corresponding to each addressable byte in the ROM.
        /// Can be of any size greater than 0.</param>
        /// <exception cref="ArgumentNullException">when <paramref name="data"/>is `null`</exception>
        /// <exception cref="ArgumentException">when length of <paramref name="data"/> is 0</exception>
        public ROM(IEnumerable<byte> data)
        {
            _data = data?.ToArray() ?? throw new ArgumentNullException(nameof(data));

            if (_data.Length == 0)
                throw new ArgumentException("Argument must contain at least one byte", nameof(data));
        }

        public string Label { get; set; } = "ROM";

        public int WordSize => 8;

        public int Capacity => _data.Length;

        public int MaxAddress => Capacity - 1;

        public BitArray? Output => _enabled ? _bitConverter.GetBits(_data[_address]) : null;

        public void SetInputE(bool value)
        {
            _enabled = value;
        }

        public BitArray ProbeAddress()
        {
            // TODO: Trim bits to number sufficient for MaxAddress
            return _bitConverter.GetBits(_address);
        }

        public void SetInputA(BitArray address)
        {
            var i = address.ToInt32();

            if (i < 0 || i > MaxAddress)
                throw new ArgumentOutOfRangeException(nameof(address),
                    "Address must be within range defined by AddressRange property");

            _address = i;
        }

        public void SetInputA(int lineIndex, bool value)
        {
            if (value)
                _address |= 1 << lineIndex;
            else
                _address &= ~(1 << lineIndex);
        }

        public IList<BitArray> ProbeState() => _data.Select(b => new BitArray(b)).ToArray();
    }
}
