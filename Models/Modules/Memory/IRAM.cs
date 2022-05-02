using System.Collections.Generic;
using DigitalElectronics.Concepts;

#nullable enable

namespace DigitalElectronics.Modules.Memory
{
    /// <summary>
    /// Represents a Random Access Memory module
    /// </summary>
    public interface IRAM : IInputModule, IOutputModule
    {
        /// <summary>
        /// The capacity of the RAM in words
        /// </summary>
        public int Capacity { get; }

        /// <summary>
        /// The number of lines (bits) in the address input, or the word size of the address
        /// </summary>
        /// <remarks>The `AddressSize` determines the address range, with the
        /// largest address being the square of the Address Size. For example, given
        /// an `AddressSize` of 4, the largest address is 15.</remarks>
        public int AddressSize { get; }

        /// <summary>
        /// Sets value for the 'Load Data' input
        /// </summary>
        /// <remarks>When set to `true`, when the module is <see cref="Clock">clocked</see>,
        /// data is stored in the currently address memory location.</remarks>
        void SetInputLD(bool value);

        /// <summary>
        /// Simulates the receipt of a clock pulse
        /// </summary>
        /// <remarks>When <see cref="SixteenByteDARAM.Clock"/> method is called, if the
        /// <see cref="SixteenByteDARAM.SetInputLD">'Load' input</see> is `true`, the data set via
        /// <see cref="SixteenByteDARAM.SetInputD"/> is loaded into the memory location specified
        /// by the most recent call to <see cref="SixteenByteDARAM.SetInputA"/>.</remarks>
        void Clock();

        /// <summary>
        /// Returns the entire internal state of the RAM
        /// </summary>
        /// <returns>A list of <seealso cref="BitArray"/> objects representing the
        /// internal state of the memory.</returns>
        ///  <remarks>Consumers can use this to get the register's output without have to set
        /// the 'enable' signal (<see cref="Register.SetInputE"/>) to `true`.</remarks>
        IList<BitArray> ProbeState();

        /// <summary>
        /// Returns the internal state of the RAM at the given address
        /// </summary>
        /// <param name="address">The address to get</param>
        /// <returns>A <seealso cref="BitArray"/> objects representing the
        /// internal state of the memory at the given address.</returns>
        ///  <remarks>Consumers can use this to get the register's output without have to set
        /// the 'enable' signal (<see cref="Register.SetInputE"/>) to `true`.</remarks>
        BitArray ProbeState(BitArray address);
    }
}
