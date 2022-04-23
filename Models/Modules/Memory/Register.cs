using System;
using System.Diagnostics;
using System.Linq;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.Components.Memory
{
    /// <summary>
    /// Models a multi-bit register of N bits, where each bit has its own input
    /// </summary>
    [DebuggerDisplay("Register: {this.ProbeState()}")]
    public class Register : IRegister
    {

        private readonly RegisterBit[] _registers;

        /// <summary>
        /// Constructs a multi-bit register with the given number of bits
        /// </summary>
        /// <param name="numberOfBits"></param>
        public Register(int numberOfBits)
        {
            if (numberOfBits <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfBits), "Argument must be greater than 0");

            _registers = new RegisterBit[numberOfBits];
            for (int x = 0; x < numberOfBits; x++) _registers[x] = new RegisterBit();
        }

        /// <summary>
        /// Sets the 'Data' inputs according to the given <see cref="BitArray"/>
        /// </summary>
        /// <param name="data">A <see cref="BitArray"/> containing values to set the register to,
        /// starting with the low-order bit. If the BitArray contains less elements than the number
        /// of bits in the register, the higher-order bits remain unchanged. If the BitArray contains
        /// more elements than the number of bits in the register, the excess elements are unused.</param>
        public void SetInputD(BitArray data)
        {
            if (data == null) return;
            
            var upper = Math.Min(data.Length, _registers.Length);
            for (int x = 0; x < upper; x++) SetInputDx(x, data[x]);
        }

        /// <summary>
        /// The number of bits in the register (N)
        /// </summary>
        public int BitCount => _registers.Length;

        /// <summary>
        /// Sets value for 'Enabled' input
        /// </summary>
        /// <param name="value">Set to `true` to enable output and `false` to disable output</param>
        /// <remarks>
        /// The `Enabled` input determines whether the register outputs the currently latched value,
        /// or `null`, which represents the Z (high impedance) state.
        /// 
        /// When using a register in a bus configuration, keep 'Enabled' input low except when
        /// performing a bus transfer.
        /// </remarks>
        public void SetInputE(bool value)
        {
            for (int x = 0; x < BitCount; x++) _registers[x].SetInputE(value);
        }

        /// <summary>
        /// Sets value for the input 'Data' for the bit at the given position
        /// </summary>
        /// <param name="x">The bit position (zero-based index)</param>
        private void SetInputDx(int x, bool value)
        {
            _registers[x].SetInputD(value);
        }

        /// <summary>
        /// Sets value for 'Load' input
        /// </summary>
        /// <param name="value">Set to `true` to enable loading of a data into the register;
        /// otherwise loading is disabled</param>
        public void SetInputL(bool value)
        {
            for (int x = 0; x < BitCount; x++) _registers[x].SetInputL(value);
        }

        /// <summary>
        /// Simulates the receipt of a clock pulse
        /// </summary>
        /// <remarks>When <see cref="Clock"/> method is called, if the
        /// <see cref="SetInputL(bool)">'Load' input</see> is `true`, the data set via
        /// <see cref="SetInputD(BitArray)"/> is loaded into the registry.</remarks>
        public void Clock()
        {
            for (int x = 0; x < BitCount; x++) _registers[x].Clock();
        }

        /// <summary>
        /// The output of the register
        /// </summary>
        public BitArray Output
        {
            get
            {
                return !_registers[0].OutputQ.HasValue ?
                    null :
                    new BitArray(_registers.Select(_ => _.OutputQ.Value));
            }
        }

        /// <summary>
        /// Returns the internal state of the register
        /// </summary>
        /// <remarks>Consumers can use this to get the register's output without have to set
        /// the 'enable' signal (<see cref="SetInputE(bool)"/>) to `true`.</remarks>
        public BitArray ProbeState()
        {
            return new BitArray(_registers.Select(_ => _.ProbeState()));
        }
    }
}
