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

        /// <summary>
        /// Optional label to help identify the module
        /// </summary>
        string Label { get; set; }
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
        /// <summary>
        /// Sets the 'Data' inputs according to the given <see cref="BitArray"/>
        /// </summary>
        /// <param name="data">A <see cref="BitArray"/> containing values to write to the module to,
        /// starting with the low-order bit. If the BitArray contains less elements than the
        /// <see cref="IModule.WordSize"/> of the module, the higher-order bits remain unchanged.
        /// If the BitArray contains more elements than the word size, the excess elements are
        /// unused.</param>
        void SetInputD(BitArray data);
    }
}
