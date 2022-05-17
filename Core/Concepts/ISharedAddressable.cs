namespace DigitalElectronics.Concepts
{
    /// <summary>
    /// Represents a component with a shared address input
    /// </summary>
    /// <remarks>
    /// This interface represents a memory module that has a set of inputs lines (set via
    /// <see cref="SetInputS(BitArray)"/> method) that is used to specify an address, where the same input
    /// lines are used for other purposes, such as receiving data. The
    /// <see cref="SetInputLA(bool)">'Load Address'</see> input, when set, signals to the memory module
    /// that the given input should be interpret as an address
    /// </remarks>
    public interface ISharedAddressable : IAddressable
    {
        /// <summary>
        /// Sets value of the 'Shared' input lines according to the given <see cref="BitArray"/>
        /// </summary>
        /// <param name="addressOrData">A <see cref="BitArray"/> containing the values can
        /// /// either be an address or data (or both)</param>
        void SetInputS(BitArray addressOrData);

        /// <summary>
        /// Sets the value of the 'Load Address' input
        /// </summary>
        /// <remarks>When set to `true`, the module interprets the argument last given to
        /// <see cref="SetInputS(BitArray)"/> as an address; otherwise the it is interpreted as data.</remarks>
        void SetInputLA(bool value);
    }
}
