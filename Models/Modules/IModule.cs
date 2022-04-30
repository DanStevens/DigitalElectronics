using DigitalElectronics.Concepts;

#nullable enable

namespace DigitalElectronics.Modules
{
    /// <summary>
    /// Base interface for types of module
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// The word size for data contained in the module, in bits
        /// </summary>
        int WordSize { get; }
    }

    /// <summary>
    /// Represents a module that can write values to a bus.
    /// </summary>
    public interface IOutputModule : IModule
    {
        /// <summary>
        /// The tri-state output of the module
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
        /// The `Enabled` input determines whether the module is enabled for output. Disabled output
        /// represents the the Z (high impedance) state.
        /// </remarks>
        public void SetInputE(bool value);
    }

    /// <summary>
    /// Represents a module that can receive values from a bus
    /// </summary>
    public interface IInputModule : IModule
    {
        void SetInputD(BitArray data);
    }
}
