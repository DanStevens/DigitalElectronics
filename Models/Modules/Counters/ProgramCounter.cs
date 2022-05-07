﻿using System;
using System.Diagnostics;
using System.Linq;
using DigitalElectronics.Components.LogicGates;
using DigitalElectronics.Concepts;

#nullable enable

namespace DigitalElectronics.Modules.Counters
{

    /// <summary>
    /// Models a program counter
    /// </summary>
    [DebuggerDisplay("Program Counter: {this.ProbeState()}")]
    public class ProgramCounter : IProgramCounter
    {
        private readonly BinaryCounter _counter;
        private readonly TriStateBuffer[] _triStateBuffers;

        /// <summary>
        /// Constructs a program counter of the given size
        /// </summary>
        /// <param name="wordSize">The word size of the program counter in bits</param>
        /// <exception cref="ArgumentOutOfRangeException">if argument is less than 1</exception>
        public ProgramCounter(int wordSize)
        {
            if (wordSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(wordSize), "Argument must be greater than 0");

            WordSize = wordSize;
            _counter = new BinaryCounter(wordSize);

            _triStateBuffers = new TriStateBuffer[WordSize];
            for (int i = 0; i < WordSize; i++)
                _triStateBuffers[i] = new TriStateBuffer();

            Sync();
        }

        /// <summary>
        /// The word size of the program counter in bits
        /// </summary>
        public int WordSize { get; }

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
            for (int i = 0; i < WordSize; i++)
                _triStateBuffers[i].SetInputA(_counter.Output[i]);
        }
    }
}
