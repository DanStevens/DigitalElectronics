using System;
using System.Diagnostics;
using DigitalElectronics.Concepts;

#nullable enable

namespace DigitalElectronics.Components.Memory
{
    /// <summary>
    /// Models a multi-bit register of N bits, where each bit has its own input
    /// </summary>
    [DebuggerDisplay("{this.Label,nq}: {this.ProbeState()}")]
    public class Register : IReadWriteRegister
    {
        private readonly RegisterBit[] _registers;

        /// <summary>
        /// Constructs a multi-bit register with the given size
        /// </summary>
        /// <param name="wordSize">The word size of the register in bits</param>
        public Register(int wordSize, RegisterMode mode = RegisterMode.ReadWrite)
        {
            if (wordSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(wordSize), "Argument must be greater than 0");

            Mode = mode;
            _registers = new RegisterBit[wordSize];
            for (int x = 0; x < wordSize; x++) _registers[x] = new RegisterBit();
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
            var upper = Math.Min(data.Length, _registers.Length);
            for (int x = 0; x < upper; x++) SetInputDx(x, data[x]);
        }

        /// <summary>
        /// Optional label to help identify the register
        /// </summary>
        public string Label { get; set; } = "Register";

        /// <summary>
        /// Value that indicates whether the register is read-only, write-only or read and write
        /// </summary>
        public RegisterMode Mode { get; }

        /// <summary>
        /// The number of bits in the register (N)
        /// </summary>
        public int WordSize => _registers.Length;

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
            if (IsReadable)
                for (int x = 0; x < WordSize; x++)
                    _registers[x].SetInputE(value);
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
            for (int x = 0; x < WordSize; x++) _registers[x].SetInputL(value);
        }

        /// <summary>
        /// Simulates the receipt of a clock pulse
        /// </summary>
        /// <remarks>When <see cref="Clock"/> method is called, if the
        /// <see cref="SetInputL(bool)">'Load' input</see> is `true`, the data set via
        /// <see cref="SetInputD(BitArray)"/> is loaded into the registry.</remarks>
        public void Clock()
        {
            for (int x = 0; x < WordSize; x++) _registers[x].Clock();
        }

        /// <summary>
        /// The tri-state output of the register
        /// </summary>
        /// <returns>If the output is enabled (see <see cref="SetInputE"/>,
        /// <see cref="BitArray"/> representing the current value; otherwise `null`,
        /// which represents the Z (high impedance) state</returns>
        public BitArray? Output
        {
            get
            {
                if (!IsReadable || !_registers[0].OutputQ.HasValue)
                    return null;

                var bitVector = new System.Collections.Specialized.BitVector32();
                for (int i = 0; i < _registers.Length; i++)
                    bitVector[1 << i] = _registers[i].OutputQ!.Value;
                return new BitArray(bitVector, WordSize);
            }
        }

        /// <summary>
        /// Returns the internal state of the register
        /// </summary>
        /// <remarks>Consumers can use this to get the register's state without have to set
        /// the 'enable' signal (<see cref="SetInputE(bool)"/>) to `true`.</remarks>
        public BitArray ProbeState()
        {
            var bitVector = new System.Collections.Specialized.BitVector32();
            for (int i = 0; i < _registers.Length; i++)
                bitVector[1 << i] = _registers[i].ProbeState();
            return new BitArray(bitVector, WordSize);
        }

        /// <summary>
        /// Whether or not the register is readable
        /// </summary>
        /// <return>`true` if the register can be read; otherwise `false`</return>
        /// A register is readable when the <see cref="RegisterMode.Read"/> flag is set.
        /// When a register is not readable, <see cref="Output"/> will always return
        /// `null` since setting the <see cref="SetInputE">enable signal></see> to
        /// `true` has no affect.
        public bool IsReadable => (Mode & RegisterMode.Read) == RegisterMode.Read;

        /// <summary>
        /// Resets the register, setting all bits to 1 and disabling output
        /// </summary>
        public void Reset()
        {
            foreach (var registerBit in _registers)
            {
                registerBit.SetInputL(true);
                registerBit.SetInputD(true);
                registerBit.Clock();
                registerBit.SetInputL(false);
                registerBit.SetInputE(false);
            }
        }
    }
}
