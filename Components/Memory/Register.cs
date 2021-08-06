using DigitalElectronics.Components.FlipFlops;
using DigitalElectronics.Components.LogicGates;
using System.Diagnostics;

namespace DigitalElectronics.Components.Memory
{
    /// <summary>
    /// Represents a single-bit register
    /// </summary>
    /// <remarks>
    /// A register 'latches' in whatever value is on 'Data' input at the point `Clock()` is
    /// invoked, but only if 'Load' input is high. If 'Load' is low, the currently latched
    /// value remains latched.
    /// </remarks>
    [DebuggerDisplay("Register: Q = {OutputQ}")]
    public class Register
    {
        private DFlipFlop _dFlipFlop;
        private Inverter _not;
        private AndGate _and1, _and2;
        private OrGate _or;

        public Register()
        {
            _dFlipFlop = new DFlipFlop();
            _not = new Inverter();
            _and1 = new AndGate();
            _and2 = new AndGate();
            _or = new OrGate();
            Sync();
        }

        /// <summary>
        /// Sets value for 'Load' input
        /// </summary>
        public void SetInputL(bool value)
        {
            _not.SetInputA(value);
            _and2.SetInputA(value);
            Sync();
        }

        /// <summary>
        /// Sets value for 'Data' input
        /// </summary>
        public void SetInputD(bool value)
        {
            _and2.SetInputB(value);
            Sync();
        }
        /// <summary>
        /// Gets state of Q output
        /// </summary>
        public bool OutputQ => _dFlipFlop.OutputQ;

        /// <summary>
        /// Simulates the receipt of a clock pulse
        /// </summary>
        public void Clock()
        {
            _dFlipFlop.Clock();
        }

        private void Sync()
        {
            _and1.SetInputB(_not.OutputQ);
            _or.SetInputB(_and2.OutputQ);
            _and1.SetInputA(_dFlipFlop.OutputQ);
            _or.SetInputA(_and1.OutputQ);
            _dFlipFlop.SetInputD(_or.OutputQ);
        }

    }
}
