using System;
using System.Collections.Specialized;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.Components;

/// <summary>
/// A component that has a boolean (1-bit) output
/// </summary>
public interface IBooleanOutput
{
    bool Output { get; }
}

/// <summary>
/// Extension methods for the <see cref="IBooleanOutput"/> interface
/// </summary>
public static class BooleanOutputExtensions
{
    /// <summary>
    /// Creates an instance of the <see cref="BitArray"/> to match the bit pattern given by array of
    /// <see cref="IBooleanOutput"/> components, with the first component representing the
    /// least-significant bit
    /// </summary>
    /// <param name="components">An array of components implementing <see cref="IBooleanOutput"/></param>
    /// <exception cref="ArgumentException">if <paramref name="components"/> contains more than 32 items</exception>
    /// <remarks>The <see cref="Length"/> property is set to length of <paramref name="components"/></remarks>
    //public static BitArray ToBitArray(this IBooleanOutput[] components) => ToBitArray(components.AsSpan());
    public static BitArray ToBitArray(this IBooleanOutput[] components)
    {
        if (components.Length > 32)
            throw new ArgumentException("Argument cannot contain more than 32 items.", nameof(components));

        var bitVector = new BitVector32();

        bitVector = new BitVector32();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i].Output)
            {
                bitVector[1 << i] = true;
            }
        }

        return new BitArray(bitVector, components.Length);
    }
}
