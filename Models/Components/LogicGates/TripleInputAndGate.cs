using System.Diagnostics;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.Components.LogicGates
{
    
    /// <summary>
    /// Models a AND logic gate with 3 inputs
    /// </summary>
    [DebuggerDisplay("AND: A = {_andAB._inputA}; B = {_andAB._inputB}; C = {_andCQ._inputB}; Q = {OutputQ}")]
    public class TripleInputAndGate : IBooleanOutput
    {
        private AndGate _andAB, _andCQ;

        public TripleInputAndGate()
        {
            _andAB = new AndGate();
            _andCQ = new AndGate();
        }

        public void SetInputA(bool a)
        {
            _andAB.SetInputA(a);
            Sync();
        }

        public void SetInputB(bool b)
        {
            _andAB.SetInputB(b);
            Sync();
        }

        public void SetInputC(bool c)
        {
            _andCQ.SetInputB(c);
            Sync();
        }

        public bool OutputQ => _andCQ.OutputQ;

        bool IBooleanOutput.Output => OutputQ;

        private void Sync()
        {
            _andCQ.SetInputA(_andAB.OutputQ);
        }
    }
}
