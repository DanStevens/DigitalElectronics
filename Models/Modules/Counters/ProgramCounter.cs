using System;
using System.Linq;
using DigitalElectronics.Components.LogicGates;
using DigitalElectronics.Concepts;

#nullable enable

namespace DigitalElectronics.Modules.Counters
{

    /// <summary>
    /// Models a program counter
    /// </summary>
    public class ProgramCounter : IProgramCounter
    {
        private readonly BinaryCounter _counter;
        private readonly TriStateBuffer[] _triStateBuffers;

        /// <summary>
        /// Constructs a program counter of the given size
        /// </summary>
        /// <param name="sizeInBits">The size of the program counter in bits</param>
        /// <exception cref="ArgumentOutOfRangeException">if argument is less than 1</exception>
        public ProgramCounter(int sizeInBits)
        {
            if (sizeInBits <= 0)
                throw new ArgumentOutOfRangeException(nameof(sizeInBits), "Argument must be greater than 0");

            SizeInBits = sizeInBits;
            _counter = new BinaryCounter(sizeInBits);

            _triStateBuffers = new TriStateBuffer[SizeInBits];
            for (int i = 0; i < SizeInBits; i++)
                _triStateBuffers[i] = new TriStateBuffer();

            Sync();
        }

        /// <summary>
        /// The size of the program counter in bits
        /// </summary>
        public int SizeInBits { get; }

        /// <summary>
        /// The tri-state output of the register
        /// </summary>
        /// <returns>If the output is enabled (see <see cref="SetInputE"/>,
        /// <see cref="BitArray"/> representing the current value; otherwise `null`,
        /// which represents the Z (high impedance) state</returns>
        public BitArray? Output => _triStateBuffers[0].OutputC.HasValue ?
            new BitArray(_triStateBuffers.Select(_ => _.OutputC!.Value)) : null;

        /// <summary>
        /// Sets value for 'Enabled' input
        /// </summary>
        /// <param name="value">Set to `true` to enable output and `false` to disable output</param>
        /// <remarks>
        /// The `Enabled` input determines whether the register outputs the currently latched value,
        /// or `null`, which represents the Z (high impedance) state.
        /// 
        /// When using a register in a bus configuration, keep 'Enabled' input low except when
        /// performing a bus transfer.
        /// </remarks>
        public void SetInputE(bool value)
        {
            foreach (var buffer in _triStateBuffers)
                buffer.SetInputB(value);
        }

        /// <summary>
        /// Returns the internal state of the program counter
        /// </summary>
        /// <remarks>Consumers can use this to get the program counter's state without have to set
        /// the 'enable' signal (<see cref="SetInputE(bool)"/>) to `true`.</remarks>
        public BitArray ProbeState() => _counter.Output;

        /// <summary>
        /// Sets the program counter to the given value
        /// </summary>
        /// <param name="address">The value (or address) to jump to</param>
        public void Jump(BitArray address)
        {
            _counter.Set(address);
            Sync();
        }

        /// <summary>
        /// Increments the program counter by 1
        /// </summary>
        public void Inc()
        {
            _counter.Inc();
            Sync();
        }

        private void Sync()
        {
            for (int i = 0; i < SizeInBits; i++)
                _triStateBuffers[i].SetInputA(_counter.Output[i]);
        }
    }
}
