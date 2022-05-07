﻿using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.ALUs;
using DigitalElectronics.Modules.Comms;
using DigitalElectronics.Modules.Counters;
using DigitalElectronics.Modules.Memory;

namespace DigitalElectronics.Computers
{

    /// <summary>
    /// A bare-bones 8-bit manual control computer, meaning it doesn't have a control logic module.
    /// The operator must manually enable/disable module input/output and advance the clock.
    /// </summary>
    public class ManualControlComputer
    {
        private const int AddressSize = 4;
        private const int WordSize = 8;

        private readonly ProgramCounter _pc;
        private readonly SixteenByteIARAM _ram;
        private readonly Register _iRegister;
        private readonly ArithmeticLogicUnit _alu;
        private readonly Register _aRegister;
        private readonly Register _bRegister;
        private readonly Register _outRegister;
        private readonly ParallelBus _bus;

        /// <summary>Program counter</summary>
        public IProgramCounter PC => _pc;

        /// <summary>16 byte Random Access Memory</summary>
        public IRAM RAM => _ram;

        /// <summary>4-bit Memory Address Register</summary>
        public ISharedAddrDataInput MAR => _ram;

        /// <summary>8-bit A register</summary>
        public IReadWriteRegister ARegister => _aRegister;

        /// <summary>8-bit B register</summary>
        public IReadWriteRegister BRegister => _bRegister;

        /// <summary>Arithmetic Logic Unit</summary>
        public IArithmeticLogicUnit ALU => _alu;

        /// <summary>Instruction register</summary>
        public IReadWriteRegister IRegister => _iRegister;

        /// <summary>8-bit output register</summary>
        public IWritableRegister OutRegister => _outRegister;

        /// <summary>Main system bus</summary>
        public ParallelBus Bus => _bus;

        public ManualControlComputer()
        {
            _pc = new ProgramCounter(AddressSize);
            _ram = new SixteenByteIARAM();
            _iRegister = new Register(WordSize);
            _aRegister = new Register(WordSize);
            _bRegister = new Register(WordSize);
            _alu = new ArithmeticLogicUnit(WordSize);
            _outRegister = new Register(WordSize);

            _bus = new ParallelBus(WordSize,
                _pc, _ram, _iRegister, _aRegister, _bRegister, _alu, _outRegister);
        }

        public void Clock()
        {
            Bus.Transfer();
            PC.Clock();
            RAM.Clock();
            IRegister.Clock();
            ARegister.Clock();
            BRegister.Clock();
            OutRegister.Clock();

            ResetControlLines();
            SyncALU();
        }

        private void ResetControlLines()
        {
            _pc.SetInputE(false);
            _pc.SetInputCE(false);
            _ram.SetInputE(false);
            _ram.SetInputLA(false);
            _ram.SetInputLD(false);
            _iRegister.SetInputE(false);
            _iRegister.SetInputL(false);
            _aRegister.SetInputE(false);
            _aRegister.SetInputL(false);
            _bRegister.SetInputE(false);
            _bRegister.SetInputL(false);
            _alu.SetInputEO(false);
            _outRegister.SetInputL(false);
        }

        private void SyncALU()
        {
            _alu.SetInputA(_aRegister.ProbeState());
            _alu.SetInputB(_bRegister.ProbeState());
        }
    }
}
