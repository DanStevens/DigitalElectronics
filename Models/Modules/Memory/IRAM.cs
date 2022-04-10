using DigitalElectronics.Concepts;

namespace DigitalElectronics.Modules.Memory
{
    public interface IRAM
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
        /// Sets value for 'Enabled' input
        /// </summary>
        /// <param name="value">Set to `true` to enable output and `false` to disable output</param>
        /// <remarks>
        /// The `Enabled` input determines whether the RAM module outputs the addressed memory location,
        /// or `null`, which represents the Z (high impedance) state.
        /// 
        /// When using a RAM module in a bus configuration, keep 'Enabled' input low except when
        /// performing a bus transfer.
        /// </remarks>
        void SetInputE(bool value);

        /// <summary>
        /// Simulates the receipt of a clock pulse
        /// </summary>
        /// <remarks>When <see cref="SixteenByteRAM.Clock"/> method is called, if the
        /// <see cref="SixteenByteRAM.SetInputL">'Load' input</see> is `true`, the data set via
        /// <see cref="SixteenByteRAM.SetInputD"/> is loaded into the memory location specified
        /// by the most recent call to <see cref="SixteenByteRAM.SetInputA"/>.</remarks>
        void Clock();

        /// <summary>
        /// The output of the RAM module
        /// </summary>
        /// <remarks>This property returns the data of one of the 16 memory locations,
        /// determined by the most recent call to <see cref="SetInputA(BitArray)"/>,
        /// unless <see cref="SetInputE(bool)">'Enable' input</see> has been set
        /// to `false`, in which case `null` is output.</remarks>
        BitArray Output { get; }
    }
}
