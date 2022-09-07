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
    /// A bare-bones 8-bit manual control computer, meaning it doesn't have a control logic module.
    /// The operator must manually enable/disable module input/output and advance the clock.
    /// </summary>
    public class ManualControlComputer
    {
        /// <summary>
        /// Control signal enumerations for <see cref="ManualControlComputer"/>
        /// </summary>
        public enum ControlWord
        {
            ///<summary>Halt</summary>
            Halt,
            ///<summary>Memory Register In</summary>
            MI,
            ///<summary>RAM In</summary>
            RI,
            ///<summary>RAM Out</summary>
            RO,
            ///<summary>Instruction Register Out</summary>
            IO,
            ///<summary>Instruction Register In</summary>
            II,
            ///<summary>A Register In</summary>
            AI,
            ///<summary>A Register Out</summary>
            AO,
            ///<summary>ALU Sum Out</summary>
            EO,
            ///<summary>ALU Subtract</summary>
            SU,
            ///<summary>B Register In</summary>
            BI,
            ///<summary>B Register Out</summary>
            BO,
            /// <summary>OUT Register In</summary>
            OI,
            ///<summary>Program Counter Enable</summary>
            CE,
            ///<summary>Program Counter Out</summary>
            CO,
            ///<summary>Jump</summary>
            J
        }

        /// <summary>
        /// Maps control words to their corresponding micro operation
        /// </summary>
        private static readonly Dictionary<ControlWord, Action<ManualControlComputer>> _controlWordMap = new()
        {
            { ControlWord.Halt, c => throw new NotImplementedException() },
            { ControlWord.MI, c => c._ram.SetInputLA(true) },
            { ControlWord.RI, c => c._ram.SetInputLD(true) },
            { ControlWord.RO, c => c._ram.SetInputE(true) },
            { ControlWord.IO, c => c._instrRegister.SetInputE(true) },
            { ControlWord.II, c => c._instrRegister.SetInputL(true) },
            { ControlWord.AI, c => c._aRegister.SetInputL(true) },
            { ControlWord.AO, c => c._aRegister.SetInputE(true) },
            { ControlWord.EO, c => c._alu.SetInputEO(true) },
            { ControlWord.SU, c => c._alu.SetInputSu(true) },
            { ControlWord.BI, c => c._bRegister.SetInputL(true) },
            { ControlWord.BO, c => c._bRegister.SetInputE(true) },
            { ControlWord.OI, c => c._outRegister.SetInputL(true) },
            { ControlWord.CE, c => c._pc.SetInputCE(true) },
            { ControlWord.CO, c => c._pc.SetInputE(true) },
            { ControlWord.J, c => c._pc.SetInputL(true) },
        };

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

        public ManualControlComputer()
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

        // Sets the given control signal high
        public void SetControlSignal(ControlWord s)
        {
            _controlWordMap[s].Invoke(this);
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
                SetControlSignal(ControlWord.MI);
                _ram.SetInputS(new BitArray(i));
                Clock();

                SetControlSignal(ControlWord.RI);
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

        private void ResetControlLines()
        {
            _pc.SetInputE(false);
            _pc.SetInputCE(false);
            _ram.SetInputE(false);
            _ram.SetInputLA(false);
            _ram.SetInputLD(false);
            _instrRegister.SetInputE(false);
            _instrRegister.SetInputL(false);
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
