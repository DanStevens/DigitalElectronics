using System.Collections.Generic;
using System.Linq;
using DigitalElectronics.Components.LogicGates;
using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;

#nullable enable

namespace DigitalElectronics.Modules.Memory
{
    
    /// <summary>
    /// 16 byte Random Access Memory module with 8-bit word length and a 4-bit address
    /// </summary>
    public class SixteenByteRAM : IRAM
    {
        private const int WordSize = 8; // Bits
        private const int Capacity = 16;  // Words/Bytes

        private readonly FourBitAddressDecoder _addressDecoder;
        private readonly AndGate[] _andL;
        private readonly AndGate[] _andE;
        private readonly Register[] _8BitRegisters;

        /// <summary>
        /// Constructs a 16 byte RAM module with the address set to 0
        /// </summary>
        public SixteenByteRAM()
        {
            _addressDecoder = new FourBitAddressDecoder();

            _andL = new AndGate[Capacity];
            _andE = new AndGate[Capacity];
            _8BitRegisters = new Register[Capacity];

            for (int x = 0; x < Capacity; x++)
            { 
                _andL[x] = new AndGate();
                _andE[x] = new AndGate();
                _8BitRegisters[x] = new Register(WordSize);
            }

            SetInputA(new BitArray(4));
        }

        /// <summary>
        /// Sets the 4-bit value of 'Address' inputs  according to the given <see cref="BitArray"/>
        /// </summary>
        /// <param name="address">A BitArray of max length of 4</param>
        /// <exception cref="ArgumentOutOfangeException">when length of <paramref name="address"/> exceeds 4</exception>
        /// <remarks>The address determines which internal register to which data is written to or
        /// read from.</remarks>
        public void SetInputA(BitArray address)
        {
            if (address.Length > 4)
                throw new System.ArgumentOutOfRangeException(nameof(address), "Argument length cannot be greater than 4");
            
            _addressDecoder.SetInputA(address);
            Sync();
        }

        /// <summary>
        /// Sets value for the 'Load' input
        /// </summary>
        public void SetInputL(bool value)
        {
            for (int x = 0; x < Capacity; x++)
                _andL[x].SetInputB(value);
            Sync();
        }

        /// <summary>
        /// Sets the 'Data' inputs according to the given <see cref="BitArray"/>
        /// </summary>
        /// <param name="data">A <see cref="BitArray"/> containing values to set the current memory
        /// location to, starting with the low-order bit. If the BitArray contains less elements
        /// than the memory word length, the higher-order bits remain unchanged. If the BitArray contains
        /// more elements than the  than the memory word length, the excess elements are unused.</param>
        /// <remarks>This method writes the given data to the memory location specified by the most
        /// recent call to <see cref="SetInputA"/>.</remarks>
        public void SetInputD(BitArray data)
        {
            for (int x = 0; x < Capacity; x++)
                _8BitRegisters[x].SetInputD(data);
            Sync();
        }

        /// <summary>
        /// Sets value for 'Enabled' input
        /// </summary>
        /// <param name="value">Set to `true` to enable output and `false` to disable output</param>
        /// <remarks>
        /// The `Enabled` input determines whether the RAM module outputs the addressed memory location,
        /// or `null`, which represents the Z (high impedance) state.
        /// 
        /// When using a RAM module in a bus configuration, keep 'Enabled' input low except when
        /// performing a bus transfer.
        /// </remarks>
        public void SetInputE(bool value)
        {
            for (int x = 0; x < Capacity; x++)
                _andE[x].SetInputB(value);
            Sync();
        }

        /// <summary>
        /// Simulates the receipt of a clock pulse
        /// </summary>
        /// <remarks>When <see cref="Clock"/> method is called, if the
        /// <see cref="SetInputL(bool)">'Load' input</see> is `true`, the data set via
        /// <see cref="SetInputD(BitArray)"/> is loaded into the memory location specified
        /// by the most recent call to <see cref="SetInputA(BitArray)"/>.</remarks>
        public void Clock()
        {
            for (int x = 0; x < Capacity; x++)
                _8BitRegisters[x].Clock();
            Sync();
        }

        /// <summary>
        /// The output of the RAM module
        /// </summary>
        /// <remarks>This property returns the data of one of the 16 memory locations,
        /// determined by the most recent call to <see cref="SetInputA(BitArray)"/>,
        /// unless <see cref="SetInputE(bool)">'Enable' input</see> has been set
        /// to `false`, in which case `null` is output.</remarks>
        public BitArray? Output =>
            _8BitRegisters.FirstOrDefault(_ => _.Output != null)?.Output;

        public IList<BitArray> ProbeState()
        {
            return _8BitRegisters.Select(r => new BitArray(r.ProbeState())).ToArray();
        }

        public BitArray ProbeState(BitArray address)
        {
            return _8BitRegisters[address.ToByte()].ProbeState();
        }

        int IRAM.WordSize => WordSize;

        int IRAM.Capacity => Capacity;

        private void Sync()
        {
            for (int x = 0; x < Capacity; x++)
            {
                _andL[x].SetInputA(_addressDecoder.OutputY[x]);
                _andE[x].SetInputA(_addressDecoder.OutputY[x]);
                _8BitRegisters[x].SetInputL(_andL[x].OutputQ);
                _8BitRegisters[x].SetInputE(_andE[x].OutputQ);
            }
        }
    }
}
