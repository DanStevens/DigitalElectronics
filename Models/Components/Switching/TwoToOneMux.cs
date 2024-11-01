using DigitalElectronics.Components.LogicGates;

#nullable enable

namespace DigitalElectronics.Components.Switching
{
    /// <summary>
    /// A 2-to-1 multiplexer (data selector)
    /// </summary>
    public class TwoToOneMux
    {
        private readonly AndGate _and0 = new();
        private readonly Inverter _not0 = new();
        private readonly AndGate _and1 = new();
        private readonly OrGate _or = new();

        /// <summary>
        /// Gets state of Z output
        /// </summary>
        public object OutputZ => _or.OutputQ;

        /// <summary>
        /// Sets value for A input
        /// </summary>
        public void SetInputA(bool value)
        {
            _and0.SetInputA(value);
            Sync();
        }

        /// <summary>
        /// Sets value for B input
        /// </summary>
        /// <param name="value"></param>
        public void SetInputB(bool value)
        {
            _and1.SetInputA(value);
            Sync();
        }

        /// <summary>
        /// Sets value for Select input
        /// </summary>
        public void SetInputSel(bool value)
        {
            _not0.SetInputA(value);
            _and0.SetInputB(_not0.OutputQ);
            _and1.SetInputB(value);
            Sync();
        }

        private void Sync()
        {
            _or.SetInputA(_and0.OutputQ);
            _or.SetInputB(_and1.OutputQ);
        }
    }
}
