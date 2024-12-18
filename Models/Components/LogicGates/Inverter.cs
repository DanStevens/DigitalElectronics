using System;
using System.Diagnostics;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.Components.LogicGates
{
    
    /// <summary>
    /// Models an inverter or OR logic gate
    /// </summary>
    [DebuggerDisplay("NOT: A = {_inputA};  Q = {OutputQ}")]
    public class Inverter : IBooleanOutput
    {
        private bool _inputA;

        /// <summary>
        /// Sets value for A input
        /// </summary>
        public void SetInputA(bool value)
        {
            _inputA = value;
        }

        /// <summary>
        /// Gets state of Q output
        /// </summary>
        public bool OutputQ => !_inputA;

        bool IBooleanOutput.Output => OutputQ;
    }
}
