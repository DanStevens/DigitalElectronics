namespace DigitalElectronics.Concepts
{
    /// <summary>
    /// Represents a memory module with a dedicated address input
    /// </summary>
    /// <remarks>A memory module that implements this interface has a
    /// dedicated address input, with a number of lines (bits) equal to
    /// <see cref="DigitalElectronics.Modules.Memory.IRAM.AddressSize"/> bits,
    /// which are set via the <see cref="SetInputA"/> method.</remarks>
    public interface IDedicatedAddrInput
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

        /// <summary>
        /// The number of lines (bits) in the address input, or the word size of the address
        /// </summary>
        /// <remarks>The `AddressSize` determines the address range, with the
        /// largest address being the square of the Address Size. For example, given
        /// an `AddressSize` of 4, the largest address is 15.</remarks>
        public int AddressSize { get; }
    }
}
