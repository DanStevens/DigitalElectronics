namespace DigitalElectronics.Utilities
{
    
    /// <summary>
    /// Represents a binary digit i.e. a 0 (false) or 1 (true)
    /// </summary>
    public class Bit : Box<bool>
    {
        public Bit(bool value) : base(value)
        { }

        public static implicit operator Bit(bool value)
        {
            return new Bit(value);
        }
        public static implicit operator bool(Bit bit)
        {
            return bit.Value;
        }
    }
}
