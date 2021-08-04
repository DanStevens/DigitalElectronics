using System;
using System.Diagnostics;

namespace DigitalElectronics.Components.LogicGates
{
    
    /// <summary>
    /// Represents an inverter or OR logic gate
    /// </summary>
    [DebuggerDisplay("NOT: A = {_inputA};  Q = {OutputQ}")]
    public class Inverter
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
    }
}