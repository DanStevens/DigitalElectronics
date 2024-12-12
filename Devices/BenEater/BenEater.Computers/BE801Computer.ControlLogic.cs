using System;
using DigitalElectronics.Modules.Counters;
using DigitalElectronics.Modules.Memory;
using DigitalElectronics.Concepts;
using static DigitalElectronics.BenEater.Computers.BE801Computer.ControlSignals;

namespace DigitalElectronics.BenEater.Computers
{

    public partial class BE801Computer
    {
        /// <summary>
        /// Control signal enumerations for <see cref="BE801Computer"/>, which when
        /// combine make a control word.
        /// </summary>
        [Flags]
        public enum ControlSignals : ushort
        {
            ///<summary>Halt</summary>
            Halt = 32768,
            ///<summary>Memory Register In</summary>
            MI = 16384,
            ///<summary>RAM In</summary>
            RI = 8192,
            ///<summary>RAM Out</summary>
            RO = 4096,
            ///<summary>Instruction Register Out</summary>
            IO = 2048,
            ///<summary>Instruction Register In</summary>
            II = 1024,
            ///<summary>A Register In</summary>
            AI = 512,
            ///<summary>A Register Out</summary>
            AO = 256,
            ///<summary>ALU Sum Out</summary>
            EO = 128,
            ///<summary>ALU Subtract</summary>
            SU = 64,
            ///<summary>B Register In</summary>
            BI = 32,
            ///<summary>OUT Register In</summary>
            OI = 16,
            ///<summary>Program Counter Enable</summary>
            CE = 8,
            ///<summary>Program Counter Out</summary>
            CO = 4,
            ///<summary>Jump</summary>
            J = 2,
            ///<summary>Unused</summary>
            Unused = 1,
            ///<summary>No signal</summary>
            _ = 0,
        }

        public enum Opcodes : byte
        {
            NOP,
            LDA,
            ADD,
            SUB,
            STA,
            LDI,
            JMP,
            OUT = 14,
            HLT = 15,
        }

        /// <summary>
        /// Microprograms for each of the instructions of the <see cref="BE801Computer"/>
        /// </summary>
        /// <remarks>
        /// The BE-801 computer supports an instruction cycle of up to 7 steps, including the fetch
        /// stage, which is 2 steps.
        /// </remarks>
        private static readonly ControlSignals[] _microprograms =
        {
            MI|CO,  RO|II|CE,  _,      _,      _,         _, _, _,   // 0000 - NOP
            MI|CO,  RO|II|CE,  IO|MI,  RO|AI,  _,         _, _, _,   // 0001 - LDA
            MI|CO,  RO|II|CE,  IO|MI,  RO|BI,  EO|AI,     _, _, _,   // 0010 - ADD
            MI|CO,  RO|II|CE,  IO|MI,  RO|BI,  EO|AI|SU,  _, _, _,   // 0011 - SUB
            MI|CO,  RO|II|CE,  IO|MI,  AO|RI,  _,         _, _, _,   // 0100 - STA
            MI|CO,  RO|II|CE,  IO|AI,  _,      _,         _, _, _,   // 0101 - LDI
            MI|CO,  RO|II|CE,  IO|J,   _,      _,         _, _, _,   // 0110 - JMP
            MI|CO,  RO|II|CE,  _,      _,      _,         _, _, _,   // 0111
            MI|CO,  RO|II|CE,  _,      _,      _,         _, _, _,   // 1000
            MI|CO,  RO|II|CE,  _,      _,      _,         _, _, _,   // 1001
            MI|CO,  RO|II|CE,  _,      _,      _,         _, _, _,   // 1010
            MI|CO,  RO|II|CE,  _,      _,      _,         _, _, _,   // 1011
            MI|CO,  RO|II|CE,  _,      _,      _,         _, _, _,   // 1100
            MI|CO,  RO|II|CE,  _,      _,      _,         _, _, _,   // 1101
            MI|CO,  RO|II|CE,  AO|OI,  _,      _,         _, _, _,   // 1110 - OUT
            MI|CO,  RO|II|CE,  Halt,   _,      _,         _, _, _,   // 1111 - HLT
        };

        private static readonly byte[] _microcode = new byte[_microprograms.Length * 2];

        /// <summary>
        /// Maps control signals to their corresponding micro operation
        /// </summary>
        private static readonly Dictionary<ControlSignals, Action<BE801Computer>> _controlSignalMap = new()
        {
            { Halt, c => c.HaltFlag = true },
            { MI, c => c._ram.SetInputLA(true) },
            { RI, c => c._ram.SetInputLD(true) },
            { RO, c => c._ram.SetInputE(true) },
            { IO, c => c._instrRegister.SetInputE(true) },
            { II, c => c._instrRegister.SetInputL(true) },
            { AI, c => c._aRegister.SetInputL(true) },
            { AO, c => c._aRegister.SetInputE(true) },
            { EO, c => c._alu.SetInputEO(true) },
            { SU, c => c._alu.SetInputSu(true) },
            { BI, c => c._bRegister.SetInputL(true) },
            { OI, c => c._outRegister.SetInputL(true) },
            { CE, c => c._pc.SetInputCE(true) },
            { CO, c => c._pc.SetInputE(true) },
            { J, c => c._pc.SetInputL(true) },
            { Unused, c => {} },
            { _, c => {} },
        };

        static void InitializeMicrocode()
        {
            for (int i = 0; i < _microprograms.Length; i++)
            {
                var splitControlWord = BitConverter.GetBytes((ushort)_microprograms[i]);
                System.Diagnostics.Debug.Assert(splitControlWord.Length == 2);
                _microcode[i] = splitControlWord[0];
            }

            for (int i = 0; i < _microprograms.Length; i++)
            {
                var splitControlWord = BitConverter.GetBytes((ushort)_microprograms[i]);
                System.Diagnostics.Debug.Assert(splitControlWord.Length == 2);
                _microcode[_microprograms.Length + i] = splitControlWord[1];
            }
        }

        private readonly BinaryCounter _stepCounter = new BinaryCounter(4);
        private readonly FourBitAddressDecoder _stepLimiter = new FourBitAddressDecoder();

        /// <summary>
        /// When `true`, the computer operates in manual control mode, meaning an external operator
        /// must set the <see cref="SetControlSignal(ControlSignals)">control signals</see> manually.
        /// When `false`, the computer operates based off instructions programmed in RAM, starting at
        /// memory address 0.
        /// </summary>
        public bool ManualControlMode { get; set; }

        /// <summary>
        /// When `true` computer is in 'halted' state, meaning that calling <see cref="Clock"/> does
        /// nothing.
        /// </summary>
        /// <remarks>The `HLT` instructions causes this flag to be set to `true`</remarks>
        public bool HaltFlag { get; set; }

        /// <summary>
        /// Sets the given control signal high
        /// </summary>
        /// <param name="controlSignal">The control signal</param>
        public void SetControlSignal(ControlSignals controlSignal)
        {
            System.Diagnostics.Debug.WriteLine("Set control signal {0}", controlSignal);
            _controlSignalMap[controlSignal].Invoke(this);
        }

        /// <summary>
        /// Sets the line associated with each control signal in the given control word to high
        /// </summary>
        /// <param name="controlWord">The control word</param>
        public void SetControlSignals(ControlSignals controlWord)
        {
            for (int i = 0; i < 16; i++)
            {
                var controlSignal = (ControlSignals)(1 << i);
                if (controlWord.HasFlag(controlSignal))
                    SetControlSignal(controlSignal);
            }
        }

        /// <summary>
        /// Returns the state of the internal microinstruction step counter
        /// </summary>
        /// <returns></returns>
        public BitArray ProbeMicroinstrStepCounter() => _stepCounter.Output;

        private void PerformControlLogic()
        {
            if (ManualControlMode) return;

            _stepCounter.Clock();
            _stepLimiter.SetInputA(_stepCounter.Output);

            // Reset step counter to 0 as soon as it hits 6
            if (_stepLimiter.OutputY[6]) _stepCounter.Set(new BitArray(0, length: 4));

            var instr = _instrRegister.ProbeState();
            var step = _stepCounter.Output;

            var addr = new BitArray(step[0], step[1], step[2], instr[4], instr[5], instr[6], instr[7], false);
            _microcodeROM.SetInputA(addr);
            var controlWordLowByte = _microcodeROM.Output!.Value.ToByte();

            for (int i = 0; i < 8; i++)
            {
                var lowControlSignal = (byte)(1 << i);
                if ((controlWordLowByte & lowControlSignal) != 0)
                    SetControlSignal((ControlSignals)lowControlSignal);
            }

            addr[7] = true;
            _microcodeROM.SetInputA(addr);
            var controlWordHighByte = _microcodeROM.Output!.Value.ToByte() << 8;

            for (int i = 8; i < 16; i++)
            {
                var highControlSignal = (ushort)(1 << i);
                if ((controlWordHighByte & highControlSignal) != 0)
                    SetControlSignal((ControlSignals)highControlSignal);
            }
        }

        private void ResetControlLogic()
        {
            _stepCounter.Reset();
            ResetControlLines();
            HaltFlag = false;
        }

        private void ResetControlLines()
        {
            _pc.SetInputE(false);
            _pc.SetInputCE(false);
            _pc.SetInputL(false);
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
            _alu.SetInputSu(false);
            _outRegister.SetInputL(false);
            _outRegister.SetInputE(false);
        }
    }
}
