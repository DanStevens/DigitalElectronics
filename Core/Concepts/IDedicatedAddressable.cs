namespace DigitalElectronics.Concepts
{
    /// <summary>
    /// Represents a component a dedicated address input
    /// </summary>
    /// <remarks>This interface represents a memory module that has a
    /// dedicated address input, with a number of lines (bits) equal to
    /// <see cref="DigitalElectronics.Modules.Memory.IRAM.AddressSize"/> bits,
    /// which are set via the <see cref="SetInputA"/> method.</remarks>
    public interface IDedicatedAddressable : IAddressable
    {
        /// <summary>
        /// Sets the 'Address' input according to the given <see cref="BitArray"/>
        /// </summary>
        /// <param name="address">A BitArray of max length of 4</param>
        /// <exception cref="System.ArgumentOutOfRangeException">when length of
        /// <paramref name="address"/> parameter exceeds <see cref="AddressSize"/></exception>
        /// <remarks>The address determines which location within the memory module
        /// that is written to or read from.</remarks>
        void SetInputA(BitArray address);
    }
}
