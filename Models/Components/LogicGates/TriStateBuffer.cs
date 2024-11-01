using System.Diagnostics;

namespace DigitalElectronics.Components.LogicGates
{
    
    /// <summary>
    /// Models a tri-state buffer
    /// </summary>
    [DebuggerDisplay("3S Buffer: A = {_inputA}; B = {_inputB}; C = {OutputC}")]
    public class TriStateBuffer
    {
        private bool _inputA;
        private bool _inputB;

        /// <summary>
        /// Sets value for A (data) input
        /// </summary>
        public void SetInputA(bool value)
        {
            _inputA = value;
        }

        /// <summary>
        /// Sets value for B (enable) input
        /// </summary>
        public void SetInputB(bool value)
        {
            _inputB = value;
        }

        /// <summary>
        /// Gets state of C output, where `null` indicates Z (high impedance) state
        /// </summary>
        public bool? OutputC => _inputB ? _inputA : null;

        /// <summary>
        /// Returns the current state of A input
        /// </summary>
        public bool ProbeInputA() => _inputA;
    }
}
