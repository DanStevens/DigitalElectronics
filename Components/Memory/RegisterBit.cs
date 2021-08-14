using DigitalElectronics.Components.FlipFlops;
using DigitalElectronics.Components.LogicGates;
using System.Diagnostics;

namespace DigitalElectronics.Components.Memory
{
    /// <summary>
    /// Represents a single-bit of a multi-bit register
    /// </summary>
    /// <remarks>
    /// A register 'latches' in whatever value is on 'Data' input at the point `Clock()` is
    /// invoked, but only if 'Load' input is high. If 'Load' is low, the currently latched
    /// value remains latched.
    /// </remarks>
    /// <seealso cref="NBitRegister"/>
    [DebuggerDisplay("Register: Q = {OutputQ}")]
    public class RegisterBit
    {
        private DFlipFlop _dFlipFlop;
        private Inverter _not;
        private AndGate _and1, _and2;
        private OrGate _or;
        private TriStateBuffer _triStateBuffer;

        public RegisterBit()
        {
            _dFlipFlop = new DFlipFlop();
            _not = new Inverter();
            _and1 = new AndGate();
            _and2 = new AndGate();
            _or = new OrGate();
            _triStateBuffer = new TriStateBuffer();
            Sync();
        }

        /// <summary>
        /// Sets for for 'Enabled' input
        /// </summary>
        /// <param name="value">Set to `true` to enable output and `false` to disable output</param>
        /// <remarks>
        /// The `Enabled` input determines whether the register outputs the currently latched value,
        /// or `null`, which represents the Z (high impedance) state.
        /// <seealso cref="NBitRegister.SetInputE"/>
        public void SetInputE(bool value)
        {
            _triStateBuffer.SetInputB(value);
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
        /// Gets state of C output, where `null` indicates Z (high impedance) state
        /// </summary>
        //public bool? OutputQ => _dFlipFlop.OutputQ;
        public bool? OutputQ => _triStateBuffer.OutputC;

        /// <summary>
        /// Simulates the receipt of a clock pulse
        /// </summary>
        public void Clock()
        {
            _dFlipFlop.Clock();
            Sync();
        }

        /// <summary>
        /// Returns the internal state of the register bit
        /// </summary>
        public bool ProbeState() => _triStateBuffer.ProbeInputA();

        private void Sync()
        {
            _and1.SetInputB(_not.OutputQ);
            _or.SetInputB(_and2.OutputQ);
            _and1.SetInputA(_dFlipFlop.OutputQ);
            _or.SetInputA(_and1.OutputQ);
            _dFlipFlop.SetInputD(_or.OutputQ);
            _triStateBuffer.SetInputA(_dFlipFlop.OutputQ);
        }

    }
}
