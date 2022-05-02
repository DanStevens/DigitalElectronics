using System.Collections.Generic;
using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.Modules.Memory
{
    /// <summary>
    /// 16 byte Indirectly Addressable Random Access Memory module with 8-bit word length and a
    /// 4-bit address register
    /// </summary>
    /// <remarks>
    /// 'Indirectly addressable' means the module has a single input, which is used to set the address
    /// and/or data, via the <see cref="SetInputS(BitArray)"/> method. The <see cref="SetInputLA"/> and
    /// <see cref="IRAM.SetInputLD"/> methods determine whether data passed to the shared input is
    /// used as address or data. Internally, this 16 byte IARAM module contains an 8-bit
    /// address register and a <see cref="SixteenByteDARAM">16 byte DARAM</see> module.
    /// </remarks>
    /// <seealso cref="IIARAM"/>
    public class SixteenByteIARAM : IIARAM
    {
        private readonly Register _addressRegister;
        private readonly SixteenByteDARAM _ram;

        public SixteenByteIARAM()
        {
            _addressRegister = new Register(AddressSize);
            _ram = new SixteenByteDARAM();
        }

        public int Capacity => ((IRAM)_ram).Capacity;

        public int AddressSize => 4;

        public void SetInputA(BitArray address)
        {
            if (address.Length > AddressSize)
                throw new System.ArgumentOutOfRangeException(nameof(address),
                    "Argument length cannot be greater than 4");
            _ram.SetInputA(address);
        }

        public void SetInputLD(bool value) => _ram.SetInputLD(value);

        public void Clock()
        {
            _addressRegister.Clock();
            _ram.Clock();
        }

        public IList<BitArray> ProbeState() => _ram.ProbeState();

        public BitArray ProbeState(BitArray address) => _ram.ProbeState(address);

        public int WordSize => _ram.WordSize;

        public BitArray? Output => _ram.Output;

        public void SetInputE(bool value) => _ram.SetInputE(value);

        /// <summary>
        /// Sets value of the 'Shared Data' inputs according to the given <see cref="BitArray"/>
        /// </summary>
        /// <param name="addressOrData">A <see cref="BitArray"/> containing the values can
        /// either be an address or data (or both)</param>
        /// <remarks>
        /// The RAM module has, in effect, an internal bus that joins the integrated
        /// 4-bit address register and the 16 Byte RAM module. The <see cref="SetInputS"/> method
        /// writes the given address or data to the bus. When the RAM module is
        /// <see cref="Clock">clocked</see>, the address or data value is transferred to which
        /// ever component is enabled for loading: <see cref="SetInputLA"/> for the address register
        /// and <see cref="SetInputLD"/> for the RAM module.</remarks>
        public void SetInputS(BitArray addressOrData)
        {
            _addressRegister.SetInputD(addressOrData);
            _ram.SetInputD(addressOrData);
        }

        /// <summary>
        /// Sets value of 'Load Address' input
        /// </summary>
        public void SetInputLA(bool value) => _addressRegister.SetInputL(value);

        /// <summary>
        /// Returns the internal state of the integrated address register
        /// </summary>
        /// <remarks>Since the integrated address register is write-only, this method
        /// can be called to see the current address.</remarks>
        public BitArray ProbeAddress() => _addressRegister.ProbeState();

        void IInputModule.SetInputD(BitArray data) => this.SetInputS(data);
    }
}
