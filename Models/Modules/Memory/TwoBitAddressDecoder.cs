using DigitalElectronics.Components.LogicGates;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.Modules.Memory
{

    /// <summary>
    /// A 2-to-4 line address decoder
    /// </summary>
    public class TwoBitAddressDecoder
    {
        public const int NumberOfOutputs = 4;

        private Inverter _notA0 = new Inverter();
        private Inverter _notA1 = new Inverter();

        private AndGate[] _and = new AndGate[NumberOfOutputs];

        public TwoBitAddressDecoder()
        {
            for (var y = 0; y < NumberOfOutputs; y++)
                _and[y] = new AndGate();
        }

        /// <summary>
        /// Set the value of 'address line 1' input
        /// </summary>
        public void SetInputA0(bool value)
        {
            _notA0.SetInputA(value);
            _and[0].SetInputA(_notA0.OutputQ);
            _and[1].SetInputA(value);
            _and[2].SetInputB(_notA0.OutputQ);
            _and[3].SetInputA(value);
        }

        /// <summary>
        /// Set the value of 'address line 2' input
        /// </summary>
        public void SetInputA1(bool value)
        {
            _notA1.SetInputA(value);
            _and[0].SetInputB(_notA1.OutputQ);
            _and[1].SetInputB(_notA1.OutputQ);
            _and[2].SetInputA(value);
            _and[3].SetInputB(value);
        }

        public BitArray OutputY
        {
            get
            {
                var result = new BitArray(NumberOfOutputs);
                for (int e = 0; e < NumberOfOutputs; e++)
                    result[e] = _and[e].OutputQ;
                return result;
            }
        }
    }
}
