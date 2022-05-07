using System;
using System.Diagnostics;
using System.Linq;
using DigitalElectronics.Components.FlipFlops;
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
        // TODO: Replace these bool fields with component logic
        // Ideally we shouldn't be using a bool field here, but have some other components
        // (e.g. logic gates, latches, flip-flops) that implement this, but unfortunately
        // Ben Eater didn't cover this in his videos. He used a DM74LS163A chip as the PC
        // and skipped over creating a PC from a binary counter.
        private bool _countEnabledInputState;
        private bool _loadInputState;
        private BitArray _jumpAddress;

        private readonly BinaryCounter _counter;
        private readonly TriStateBuffer[] _triStateBuffers;

        /// <summary>
        /// Constructs a program counter of the given size
        /// </summary>
        /// <param name="addressSize">The word size of the program counter in bits</param>
        /// <exception cref="ArgumentOutOfRangeException">if argument is less than 1</exception>
        public ProgramCounter(int addressSize)
        {
            if (addressSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(addressSize), "Argument must be greater than 0");

            AddressSize = addressSize;
            _counter = new BinaryCounter(addressSize);

            _triStateBuffers = new TriStateBuffer[AddressSize];
            for (int i = 0; i < AddressSize; i++)
                _triStateBuffers[i] = new TriStateBuffer();

            _jumpAddress = new BitArray(length: AddressSize);

            Sync();
        }

        /// <summary>
        /// The word size of the program counter in bits
        /// </summary>
        public int AddressSize { get; }

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
        /// Sets value for 'Count Enabled' input
        /// </summary>
        /// <param name="value"></param>
        public void SetInputCE(bool value)
        {
            _countEnabledInputState = value;
        }

        /// <summary>
        /// Sets value for 'Load' input
        /// </summary>
        /// <param name="value">Set to `true` to enable the program counter to jump to the address
        /// given by the last call to <see cref="SetInputD(BitArray)"/></param>
        public void SetInputL(bool value)
        {
            // TODO: Get rid of bool field
            _loadInputState = value;
        }


        public void Clock()
        {
            if (_countEnabledInputState)
                Inc();
            if (_loadInputState)
                Jump(_jumpAddress);
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
        public void SetInputD(BitArray address)
        {
            _jumpAddress = address;
        }

        private void Jump(BitArray address)
        {
            _counter.Set(address);
            Sync();
        }

        /// <summary>
        /// Increments the program counter by 1
        /// </summary>
        private void Inc()
        {
            _counter.Clock();
            Sync();
        }

        private void Sync()
        {
            for (int i = 0; i < AddressSize; i++)
                _triStateBuffers[i].SetInputA(_counter.Output[i]);
        }

        int IModule.WordSize => AddressSize;
    }
}
