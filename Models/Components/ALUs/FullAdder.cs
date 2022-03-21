using System.Diagnostics;
using DigitalElectronics.Components.LogicGates;

namespace DigitalElectronics.Components.ALUs
{

    /// <summary>
    /// Models a full adder for summing a single pair of bits
    /// </summary>
    [DebuggerDisplay("A = {_halfAdder1._xor._inputA}; B = {_halfAdder1._xor._inputB}; ∑ = {OutputE}; C => {OutputC}")]
    public class FullAdder
    {
        private readonly HalfAdder _halfAdder1, _halfAdder2;
        private readonly OrGate _or;

        public FullAdder()
        {
            _halfAdder1 = new HalfAdder();
            _halfAdder2 = new HalfAdder();
            _or = new OrGate();
        }

        /// <summary>
        /// Sets value for A input
        /// </summary>
        public void SetInputA(bool value)
        {
            _halfAdder1.SetInputA(value);
            Sync();
        }

        /// <summary>
        /// Sets value for B input
        /// </summary>
        public void SetInputB(bool value)
        {
            _halfAdder1.SetInputB(value);
            Sync();

        }

        /// <summary>
        /// Sets value for 'Carry' input
        /// </summary>
        public void SetInputC(bool value)
        {
            _halfAdder2.SetInputB(value);
            Sync();
        }

        private void Sync()
        {
            _halfAdder2.SetInputA(_halfAdder1.OutputE);
            _or.SetInputA(_halfAdder1.OutputC);
            _or.SetInputB(_halfAdder2.OutputC);
        }

        /// <summary>
        /// Gets state of 'Carry' output
        /// </summary>
        public bool OutputC => _or.OutputQ;

        /// <summary>
        /// Gets state of 'Sum' (∑) output
        /// </summary>
        public bool OutputE => _halfAdder2.OutputE;

    }
}
