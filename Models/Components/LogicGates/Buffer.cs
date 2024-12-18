using System;
using System.Diagnostics;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.Components.LogicGates
{
    /// <summary>
    /// Models a digital buffer
    /// </summary>
    [DebuggerDisplay("Buffer: A = {_inputA}; Q = {OutputQ}")]
    public class Buffer : IBooleanOutput
    {
        private bool _inputA;

        /// <summary>
        /// Sets value for A input
        /// </summary>
        public void SetInputA(bool v)
        {
            _inputA = v;
        }

        /// <summary>
        /// Gets state of Q output
        /// </summary>
        public bool OutputQ => _inputA;

        bool IBooleanOutput.Output => OutputQ;
    }
}
