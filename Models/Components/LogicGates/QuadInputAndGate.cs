using System.Diagnostics;

namespace DigitalElectronics.Components.LogicGates
{
    
    /// <summary>
    /// Models a AND logic gate with 4 inputs
    /// </summary>
    [DebuggerDisplay("AND: A = {_andAB._inputA}; B = {_andAB._inputB}; C = {_andCD._inputA};  D = {_andCD._inputB}; Q = {OutputQ}")]
    public class QuadInputAndGate
    {
        private readonly AndGate _andAB, _andCD;
        private readonly AndGate _andQ;

        public QuadInputAndGate()
        {
            _andAB = new AndGate();
            _andCD = new AndGate();
            _andQ = new AndGate();
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
            _andCD.SetInputA(c);
            Sync();
        }

        public void SetInputD(bool d)
        {
            _andCD.SetInputB(d);
            Sync();
        }

        public bool OutputQ => _andQ.OutputQ;

        private void Sync()
        {
            _andQ.SetInputA(_andAB.OutputQ);
            _andQ.SetInputB(_andCD.OutputQ);
        }
    }
}
