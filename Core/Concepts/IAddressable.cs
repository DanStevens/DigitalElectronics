#nullable enable

namespace DigitalElectronics.Concepts
{
    /// <summary>
    /// A component that accepts an address
    /// </summary>
    public interface IAddressable
    {
        /// <summary>
        /// The number of lines (bits) in the address input, or the word size of the addressable component
        /// </summary>
        /// <remarks>The `AddressSize` determines the address range, with the
        /// largest address being the square of the Address Size. For example, given
        /// an `AddressSize` of 4, the largest address is 15.</remarks>
        public int AddressSize { get; }

        /// <summary>
        /// Returns the currently latched addressed in the addressable component
        /// </summary>
        /// <returns></returns>
        BitArray ProbeAddress();
    }
}
