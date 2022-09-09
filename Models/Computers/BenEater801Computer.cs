using System;
using System.Collections.Generic;
using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.ALUs;
using DigitalElectronics.Modules.Comms;
using DigitalElectronics.Modules.Counters;
using DigitalElectronics.Modules.Memory;

namespace DigitalElectronics.Computers
{

    /// <summary>
    /// A programmable 8-bit computer, based off Ben Eater's 8-bit breadboard computer.
    /// </summary>
    /// <seealso cref="https://eater.net/8bit">
    /// Ben Eater's 8-bit computer on eater.net</seealso>
    /// <seealso cref="https://youtube.com/playlist?list=PLowKtXNTBypGqImE405J2565dvjafglHU">
    /// Building an 8-bit breadboard computer! - YouTube Playlist</seealso>
    public partial class BenEater801Computer
    {
        private const int AddressSize = 4;
        private const int WordSize = 8;

        private readonly ProgramCounter _pc;
        private readonly SixteenByteIARAM _ram;
        private readonly Register _instrRegister;
        private readonly ArithmeticLogicUnit _alu;
        private readonly Register _aRegister;
        private readonly Register _bRegister;
        private readonly Register _outRegister;
        private readonly ParallelBus _bus;

        public BenEater801Computer()
        {
            _pc = new ProgramCounter(AddressSize);
            _ram = new SixteenByteIARAM();
            _instrRegister = new Register(WordSize);
            _aRegister = new Register(WordSize);
            _bRegister = new Register(WordSize);
            _alu = new ArithmeticLogicUnit(WordSize);
            _outRegister = new Register(WordSize);

            _bus = new ParallelBus(WordSize,
                _pc, _ram, _instrRegister, _aRegister, _bRegister, _alu, _outRegister);
        }

        public void Clock()
        {
            _bus.Transfer();
            _pc.Clock();
            _ram.Clock();
            _instrRegister.Clock();
            _aRegister.Clock();
            _bRegister.Clock();
            _outRegister.Clock();

            ResetControlLines();
            SyncALU();
        }

        /// <summary>
        /// Loads the given bytes into RAM, up to <see cref="IRAM.Capacity"/>
        /// </summary>
        /// <param name="image">An array of bytes to load</param>
        /// <exception cref="ArgumentNullException">when <paramref name="image"/> is null</exception>
        public void LoadRAM(byte[] image)
        {
            _ = image ?? throw new ArgumentNullException(nameof(image));

            var cap = Math.Min(image.Length, _ram.Capacity);
            for (byte i = 0; i < cap; i++)
            {
                SetControlSignal(ControlSignal.MI);
                _ram.SetInputS(new BitArray(i));
                Clock();

                SetControlSignal(ControlSignal.RI);
                _ram.SetInputS(new BitArray(image[i]));
                Clock();
            }
        }

        /// <summary>
        /// Returns internal state of Program Counter
        /// </summary>
        public BitArray ProbePC() => _pc.ProbeState();

        /// <summary>
        /// Returns internal state of Memory Address Register
        /// </summary>
        public BitArray ProbeMAR() => _ram.ProbeAddress();

        /// <summary>
        /// Returns internal state of RAM at the given address
        /// </summary>
        public BitArray ProbeRAM(BitArray address) => _ram.ProbeState(address);

        /// <summary>
        /// Returns internal state of Instruction Register
        /// </summary>
        public BitArray ProbeInstrRegister() => _instrRegister.ProbeState();

        /// <summary>
        /// Returns internal state of A Register
        /// </summary>
        public BitArray ProbeARegister() => _aRegister.ProbeState();

        /// <summary>
        /// Returns internal state of B Register
        /// </summary>
        public BitArray ProbeBRegister() => _bRegister.ProbeState();

        /// <summary>
        /// Returns internal state of OUT Register
        /// </summary>
        public BitArray ProbeOutRegister() => _outRegister.ProbeState();

        private void SyncALU()
        {
            _alu.SetInputA(_aRegister.ProbeState());
            _alu.SetInputB(_bRegister.ProbeState());
        }
    }
}
