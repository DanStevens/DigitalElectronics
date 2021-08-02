using System;
using System.Diagnostics;

namespace DigitalElectronics.Components.LogicGates
{
    /// <summary>
    /// Represents a digital buffer
    /// </summary>
    [DebuggerDisplay("Buffer: A = {_inputA}; Q = {OutputQ}")]
    public class Buffer
    {
        private bool _inputA;

        public void SetInputA(bool v)
        {
            _inputA = v;
        }

        /// <summary>
        /// Gets state of Q output
        /// </summary>
        public bool OutputQ => _inputA;

    }
}