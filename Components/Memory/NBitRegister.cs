namespace DigitalElectronics.Components.Memory
{
    /// <summary>
    /// Represents a multi-bit register of N bits, where each bit has its own input
    /// </summary>
    public class NBitRegister
    {

        private Register[] _registers;

        /// <summary>
        /// Constructs a multi-bit register with the given number of bits
        /// </summary>
        /// <param name="numberOfBits"></param>
        public NBitRegister(int numberOfBits)
        {
            _registers = new Register[numberOfBits];
            for (int x = 0; x < numberOfBits; x++) _registers[x] = new Register();
        }

        /// <summary>
        /// The number of bits in the register (N)
        /// </summary>
        public int BitCount => _registers.Length;

        /// <summary>
        /// Sets for for 'Enabled' input
        /// </summary>
        /// <param name="value">Set to `true` to enable output and `false` to disable output</param>
        /// <remarks>
        /// The `Enabled` input determines whether the register outputs the currently latched value,
        /// or `null`, which represents the Z (high impedance) state.
        /// 
        /// When using a register in a bus configuration, keep 'Enabled' input low expect when
        /// performing a bus transfer.
        public void SetInputE(bool value)
        {
            for (int x = 0; x < BitCount; x++) _registers[x].SetInputE(value);
        }

        /// <summary>
        /// Sets value for the input 'Data' for the bit at the given position
        /// </summary>
        /// <param name="x">The bit position (zero-based index)</param>
        public void SetInputDx(int x, bool value)
        {
            _registers[x].SetInputD(value);
        }

        /// <summary>
        /// Sets value for 'Load' input
        /// </summary>
        /// <param name="value"></param>
        public void SetInputL(bool value)
        {
            for (int x = 0; x < BitCount; x++) _registers[x].SetInputL(value);
        }
        
        /// <summary>
        /// Gets state of Q output for the given bit
        /// </summary>
        /// <param name="x">The bit position (zero-based index)</param>
        /// <returns></returns>
        public bool? GetOutputQx(int x)
        {
            return _registers[x].OutputQ;
        }

        /// <summary>
        /// Simulates the receipt of a clock pulse
        /// </summary>
        public void Clock()
        {
            for (int x = 0; x < BitCount; x++) _registers[x].Clock();
        }
    }
}
