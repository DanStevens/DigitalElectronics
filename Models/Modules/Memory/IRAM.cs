using System.Collections.Generic;
using DigitalElectronics.Concepts;

#nullable enable

namespace DigitalElectronics.Modules.Memory
{
    public interface IRAM : IOutputModule
    {
        /// <summary>
        /// The word size in bits, typically 8 bits.
        /// </summary>
        public int WordSize { get; }

        /// <summary>
        /// The capacity of the RAM in words
        /// </summary>
        public int Capacity { get; }

        /// <summary>
        /// Sets the 4-bit value of 'Address' inputs  according to the given <see cref="BitArray"/>
        /// </summary>
        /// <param name="address">A BitArray of max length of 4</param>
        /// <exception cref="ArgumentOutOfangeException">when length of <paramref name="address"/> exceeds 4</exception>
        /// <remarks>The address determines which internal register to which data is written to or
        /// read from.</remarks>
        void SetInputA(BitArray address);

        /// <summary>
        /// Sets value for the 'Load' input
        /// </summary>
        void SetInputL(bool value);

        /// <summary>
        /// Sets the 'Data' inputs according to the given <see cref="BitArray"/>
        /// </summary>
        /// <param name="data">A <see cref="BitArray"/> containing values to set the current memory
        /// location to, starting with the low-order bit. If the BitArray contains less elements
        /// than the memory word length, the higher-order bits remain unchanged. If the BitArray contains
        /// more elements than the  than the memory word length, the excess elements are unused.</param>
        /// <remarks>This method writes the given data to the memory location specified by the most
        /// recent call to <see cref="SixteenByteRAM.SetInputA"/>.</remarks>
        void SetInputD(BitArray data);

        /// <summary>
        /// Simulates the receipt of a clock pulse
        /// </summary>
        /// <remarks>When <see cref="SixteenByteRAM.Clock"/> method is called, if the
        /// <see cref="SixteenByteRAM.SetInputL">'Load' input</see> is `true`, the data set via
        /// <see cref="SixteenByteRAM.SetInputD"/> is loaded into the memory location specified
        /// by the most recent call to <see cref="SixteenByteRAM.SetInputA"/>.</remarks>
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
