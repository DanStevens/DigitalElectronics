using DigitalElectronics.Concepts;

#nullable enable

namespace DigitalElectronics.Modules.Counters
{
    public interface IProgramCounter : IOutputModule, IInputModule
    {
        /// <summary>
        /// Returns the internal state of the program counter
        /// </summary>
        /// <remarks>Consumers can use this to get the program counter's state without have to set
        /// the 'enable' signal (<see cref="ProgramCounter.SetInputE"/>) to `true`.</remarks>
        BitArray ProbeState();

        /// <summary>
        /// Clocks (increments) the program counter by 1
        /// </summary>
        void Clock();

        /// <summary>
        /// Sets value for 'Count Enabled' input
        /// </summary>
        /// <param name="value"></param>
        void SetInputCE(bool value);
    }
}
