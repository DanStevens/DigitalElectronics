#nullable enable

namespace DigitalElectronics.Concepts
{
    
    /// <summary>
    /// Represents a binary digit i.e. a 0 (false) or 1 (true)
    /// </summary>
    public class Bit : Box<bool>
    {
        public Bit() : this(false)
        {}

        public Bit(bool value) : base(value)
        { }

        public static implicit operator Bit(bool value)
        {
            return new Bit(value);
        }

        public int CompareTo(Bit? other)
        {
            if (other is null)
                return 1;

            return base.CompareTo(other.Value);
        }

        public bool Equals(Bit? other)
        {
            if (other is null)
                return false;

            return base.Equals(other.Value);
        }

        public override string ToString()
        {
            return $"Bit({Value})";
        }
    }
}
