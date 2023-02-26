using System;
using System.Collections.Generic;
using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.ALUs;
using DigitalElectronics.Modules.Comms;
using DigitalElectronics.Modules.Counters;
using DigitalElectronics.Modules.Memory;

namespace DigitalElectronics.BenEater.Computers
{

    /// <summary>
    /// The BE-801, a programmable 8-bit computer, based off Ben Eater's 8-bit breadboard computer.
    /// </summary>
    /// <remarks>
    /// Specifications:
    ///   - 8-bit word length
    ///   - A and B 8-bit registers
    ///   - 4-bit address space
    ///   - 16 byte RAM
    ///   - 16-bit control word
    ///   
    /// The BE-801 is not turning complete as it lacks conditional jump instruction
    /// </remarks>
    /// <seealso cref="https://eater.net/8bit">
    /// Ben Eater's 8-bit computer on eater.net</seealso>
    /// <seealso cref="https://youtube.com/playlist?list=PLowKtXNTBypGqImE405J2565dvjafglHU">
    /// Building an 8-bit breadboard computer! - YouTube Playlist</seealso>
    public partial class BE801Computer
    {
        private const int AddressSize = 4;
        private const int WordSize = 8;

        static BE801Computer()
        {
            InitializeMicrocode();
        }

        private readonly ProgramCounter _pc;
        private readonly SixteenByteIARAM _ram;
        private readonly InstructionRegister _instrRegister;
        private readonly ArithmeticLogicUnit _alu;
        private readonly Register _aRegister;
        private readonly Register _bRegister;
        private readonly Register _outRegister;
        private readonly ParallelBus _bus;
        private readonly ROM _microcodeROM;


        public BE801Computer()
        {
            _pc = new ProgramCounter(AddressSize);
            _ram = new SixteenByteIARAM();
            _instrRegister = new InstructionRegister(WordSize) {  Label = "Instruction Register" };
            _aRegister = new Register(WordSize) { Label = "A Register" };
            _bRegister = new Register(WordSize) { Label = "B Register" };
            _alu = new ArithmeticLogicUnit(WordSize);
            _outRegister = new Register(WordSize) { Label = "Out Register" };

            _bus = new ParallelBus(WordSize,
                _pc, _ram, _instrRegister, _aRegister, _bRegister, _alu, _outRegister);

            _microcodeROM = new ROM(_microcode);
            _microcodeROM.SetInputE(true);
        }

        /// <summary>
        /// Resets registers to all 1s and program counter to zero. Leaves RAM untouched.
        /// </summary>
        public void Reset()
        {
            _instrRegister.Reset();
            _aRegister.Reset();
            _bRegister.Reset();
            _outRegister.Reset();
            _ram.ResetAddress();
            _pc.Reset();
            ResetControlLogic();
        }

        /// <summary>
        /// Performs a clock cycle
        /// </summary>
        /// <remarks>This method does nothing when computer is in 'halted' state
        /// (<see cref="HaltFlag"/> is `true`)</remarks>
        public void Clock()
        {
            if (HaltFlag)
                return;
            
            PerformControlLogic();

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
        /// Loads the given bytes into RAM, up to <see cref="IRAM.Capacity"/> and then calls
        /// <see cref="Reset"/>
        /// </summary>
        /// <param name="image">An array of bytes to load</param>
        /// <exception cref="ArgumentNullException">when <paramref name="image"/> is null</exception>
        /// <remarks>
        /// Loads the bytes from address 0 overwriting memory address up to length of
        /// <paramref name="image"/>.
        /// </remarks>
        public void LoadRAM(byte[] image)
        {
            _ = image ?? throw new ArgumentNullException(nameof(image));

            var cap = Math.Min(image.Length, _ram.Capacity);
            for (byte i = 0; i < cap; i++)
            {
                SetControlSignals(ControlSignals.MI);
                _ram.SetInputS(new BitArray(i));
                _ram.Clock();
                ResetControlLines();

                SetControlSignals(ControlSignals.RI);
                _ram.SetInputS(new BitArray(image[i]));
                _ram.Clock();
                ResetControlLines();
            }

            Reset();
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

        /// <summary>
        /// Returns the state of the Bus
        /// </summary>
        /// <returns></returns>
        public BitArray? ProbeBus() => _bus.Output;

        /// <summary>
        /// Returns the internal state of RAM
        /// </summary>
        /// <returns></returns>
        public IList<BitArray> ProbeRAM() => _ram.ProbeState();

        /// <summary>
        /// Returns the internal state of the ALU
        /// </summary>
        public BitArray ProbeALU() => _alu.ProbeState();

        private void SyncALU()
        {
            _alu.SetInputA(_aRegister.ProbeState());
            _alu.SetInputB(_bRegister.ProbeState());
        }

        internal ROM MicrocodeROM => _microcodeROM;
    }
}
