namespace DigitalElectronics.Concepts
{
    /// <summary>
    /// The numbering scheme for representing a number in binary form
    /// </summary>
    /// <seealso href="https://en.wikipedia.org/wiki/Bit_numbering#Most-_vs_least-significant_bit_first"/>
    public enum BitOrder
    {

        /// <summary>
        /// Bit order starts with most-significant bit (MSB)
        /// </summary>
        MsbFirst,

        /// <summary>
        /// Bit order starts with least-significant bit (LSB)
        /// </summary>
        LsbFirst
    }
}
