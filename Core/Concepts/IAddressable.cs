#nullable enable

using System;

namespace DigitalElectronics.Concepts
{
    /// <summary>
    /// A component that accepts an address
    /// </summary>
    public interface IAddressable
    {
        /// <summary>
        /// The maximum address accepted by the addressable
        /// </summary>
        int MaxAddress { get; }

        /// <summary>
        /// Returns the currently latched addressed in the addressable component
        /// </summary>
        /// <returns></returns>
        BitArray ProbeAddress();
    }
}
