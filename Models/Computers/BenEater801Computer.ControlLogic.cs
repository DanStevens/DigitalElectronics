using System.Collections.Generic;
using System;

namespace DigitalElectronics.Computers
{

    public partial class BenEater801Computer
    {
        /// <summary>
        /// Control signal enumerations for <see cref="BenEater801Computer"/>
        /// </summary>
        public enum ControlSignal
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
        private static readonly Dictionary<ControlSignal, Action<BenEater801Computer>> _controlWordMap = new()
        {
            { ControlSignal.Halt, c => throw new NotImplementedException() },
            { ControlSignal.MI, c => c._ram.SetInputLA(true) },
            { ControlSignal.RI, c => c._ram.SetInputLD(true) },
            { ControlSignal.RO, c => c._ram.SetInputE(true) },
            { ControlSignal.IO, c => c._instrRegister.SetInputE(true) },
            { ControlSignal.II, c => c._instrRegister.SetInputL(true) },
            { ControlSignal.AI, c => c._aRegister.SetInputL(true) },
            { ControlSignal.AO, c => c._aRegister.SetInputE(true) },
            { ControlSignal.EO, c => c._alu.SetInputEO(true) },
            { ControlSignal.SU, c => c._alu.SetInputSu(true) },
            { ControlSignal.BI, c => c._bRegister.SetInputL(true) },
            { ControlSignal.BO, c => c._bRegister.SetInputE(true) },
            { ControlSignal.OI, c => c._outRegister.SetInputL(true) },
            { ControlSignal.CE, c => c._pc.SetInputCE(true) },
            { ControlSignal.CO, c => c._pc.SetInputE(true) },
            { ControlSignal.J, c => c._pc.SetInputL(true) },
        };

        // Sets the given control signal high
        public void SetControlSignal(ControlSignal s)
        {
            _controlWordMap[s].Invoke(this);
        }
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
    }
}
