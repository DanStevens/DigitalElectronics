using System;
using System.Diagnostics;
using System.Linq;
using DigitalElectronics.Components.FlipFlops;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.Modules.Counters
{
    /// <summary>
    /// Models a multi-bit binary counter of N bits
    /// </summary>
    [DebuggerDisplay("Binary Counter: {Output}")]
    public class BinaryCounter
    {
        private readonly JKFlipFlop[] _jkFlipFlops;

        /// <summary>
        /// Constructs a multi-bit binary counter with the given number of bits
        /// </summary>
        /// <param name="numberOfBits">The size of the binary counter in bits</param>
        public BinaryCounter(int numberOfBits)
        {
            if (numberOfBits <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfBits), "Argument must be greater than 0");

            NumberOfBits = numberOfBits;
            
            _jkFlipFlops = new JKFlipFlop[NumberOfBits];
            for (int i = 0; i < NumberOfBits; i++)
                _jkFlipFlops[i] = CreateJKFlipFlop();

            JKFlipFlop CreateJKFlipFlop()
            {
                var jkFlipFlop = new JKFlipFlop();
                jkFlipFlop.SetInputJ(true);
                jkFlipFlop.SetInputK(true);
                return jkFlipFlop;
            }
        }
        
        /// <summary>
        /// The size of (number of bits in) the binary counter
        /// </summary>
        public int NumberOfBits { get; }

        /// <summary>
        /// The output of the binary counter
        /// </summary>
        public BitArray Output => new (_jkFlipFlops.Select(_ => _.OutputQ));

        /// <summary>
        /// Increments the binary counter by 1
        /// </summary>
        public void Inc()
        {
            for (int i = 0; i < NumberOfBits; i++)
            {
                _jkFlipFlops[i].Clock();
                if (_jkFlipFlops[i].OutputQ)
                    break;
            }
        }
    }
}
