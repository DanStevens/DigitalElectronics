using System.Diagnostics;

namespace DigitalElectronics.Components.LogicGates
{
    
    /// <summary>
    /// Represents a AND logic gate with 3 inputs
    /// </summary>
    [DebuggerDisplay("AND: A = {_and1._inputA}; B = {_and1._inputB}; C = {_and2._inputC}; Q = {OutputQ}")]
    public class TripleInputAndGate
    {
        private AndGate _and1, _and2;

        public TripleInputAndGate()
        {
            _and1 = new AndGate();
            _and2 = new AndGate();
        }

        public void SetInputA(bool a)
        {
            _and1.SetInputA(a);
            Sync();
        }

        public void SetInputB(bool b)
        {
            _and1.SetInputB(b);
            Sync();
        }

        public void SetInputC(bool c)
        {
            _and2.SetInputB(c);
            Sync();
        }

        public bool OutputQ => _and2.OutputQ;

        private void Sync()
        {
            _and2.SetInputA(_and1.OutputQ);
        }
    }
}
