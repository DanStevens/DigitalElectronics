namespace DigitalElectronics.Utilities
{
    /// <summary>
    /// The order of bits or bytes of a byte or word.
    /// </summary>
    public enum Endianness
    {
        /// <summary>The endianness according to the system</summary>
        System,
        
        /// <summary>Little-endian i.e. low-order bits/bytes are first</summary>
        Little,
            
        /// <summary>Big-endian i.e. high-order bits/bytes are first</summary>
        Big
    }
}
