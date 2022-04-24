using DigitalElectronics.Concepts;

#nullable enable

namespace DigitalElectronics.Modules.Counters
{
    public interface IProgramCounter
    {
        /// <summary>
        /// The size of the program counter in bits
        /// </summary>
        int SizeInBits { get; }

        /// <summary>
        /// The tri-state output of the register
        /// </summary>
        /// <returns>If the output is enabled (see <see cref="SetInputE"/>,
        /// <see cref="BitArray"/> representing the current value; otherwise `null`,
        /// which represents the Z (high impedance) state</returns>
        BitArray? Output { get; }

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
        void SetInputE(bool value);

        /// <summary>
        /// Returns the internal state of the program counter
        /// </summary>
        /// <remarks>Consumers can use this to get the program counter's state without have to set
        /// the 'enable' signal (<see cref="ProgramCounter.SetInputE"/>) to `true`.</remarks>
        BitArray ProbeState();

        /// <summary>
        /// Sets the program counter to the given value
        /// </summary>
        /// <param name="address">The value (or address) to jump to</param>
        void Jump(BitArray address);

        /// <summary>
        /// Increments the program counter by 1
        /// </summary>
        void Inc();
    }
}
