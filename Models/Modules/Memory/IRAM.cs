using System.Collections.Generic;
using DigitalElectronics.Concepts;

#nullable enable

namespace DigitalElectronics.Modules.Memory
{
    
    /// <summary>
    /// Represents a Directly Addressable Random Access Memory module
    /// </summary>
    /// <remarks>'Directly addressable' means the module has a dedicated address input,
    /// with a number of lines (bits) equal to <see cref="IRAM.AddressSize"/> bits,
    /// which are set via the <see cref="SetInputA"/> method.</remarks>
    public interface IDARAM : IRAM
    {
        /// <summary>
        /// Sets the 'Address' input according to the given <see cref="BitArray"/>
        /// </summary>
        /// <param name="address">A BitArray of max length of 4</param>
        /// <exception cref="System.ArgumentOutOfRangeException">when length of
        /// <paramref name="address"/> parameter exceeds <see cref="IRAM.AddressSize"/></exception>
        /// <remarks>The address determines which location within the RAM module is written to or
        /// read from.</remarks>
        void SetInputA(BitArray address);
    }

    /// <summary>
    /// Represents an Indirectly Addressable Random Access Memory module
    /// </summary>
    /// <remarks>'Indirectly addressable' means the module has a single input, which is
    /// used to set the address and/or data, via the <see cref="SetInputS(BitArray)"/>
    /// method. The <see cref="SetInputLA"/> and <see cref="IRAM.SetInputLD"/>
    /// methods, which determine whether the data passed to the shared input is used
    /// as an address or data.</remarks>
    public interface IIARAM : IRAM
    {
        /// <summary>
        /// Sets value of the 'Shared Data' inputs according to the given <see cref="BitArray"/>
        /// </summary>
        /// <param name="addressOrData">A <see cref="BitArray"/> containing the values can
        /// /// either be an address or data (or both)</param>
        void SetInputS(BitArray addressOrData);

        /// <summary>
        /// Sets the value of the 'Load Address' input
        /// </summary>
        /// <remarks>When set to `true`, when the module is <see cref="Clock">clocked</see>,
        /// the address is updated.</remarks>
        void SetInputLA(bool value);
    }

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
        /// The number of bits in the address line, or the word size of the address
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
