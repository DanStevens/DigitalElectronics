using System.Diagnostics;

namespace DigitalElectronics.Components.LogicGates
{

    /// <summary>
    /// Represents a AND logic gate
    /// </summary>
    [DebuggerDisplay("AND: A = {_inputA}; B = {_inputB}; Q = {OutputQ}")]
    public class AndGate
    {
        private bool _inputA;
        private bool _inputB;

        public void SetInputA(bool value)
        {
            _inputA = value;
        }

        public void SetInputB(bool value)
        {
            _inputB = value;
        }

        /// <summary>
        /// Gets state of Q output
        /// </summary>
        public bool OutputQ => _inputA && _inputB;
    }
}