using DigitalElectronics.Concepts;

#nullable enable

namespace DigitalElectronics.Modules.Counters
{
    public interface IProgramCounter : IOutputModule
    {
        /// <summary>
        /// The size of the program counter in bits
        /// </summary>
        int SizeInBits { get; }

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
