using System.Collections;
using DigitalElectronics.Components.ALUs;

namespace DigitalElectronics.Modules.ALUs
{
    public class ArithmeticLogicUnit
    {
        private FullAdder[] _adders;

        public ArithmeticLogicUnit(int numberOfBits)
        {
            _adders = new FullAdder[numberOfBits];
            for (int x = 0; x < _adders.Length; x++) _adders[x] = new FullAdder();
            Sync();
        }

        public void SetInputA(BitArray data)
        {
            for (int x = 0; x < _adders.Length; x++) _adders[x].SetInputA(data[x]);
            Sync();
        }

        public void SetInputB(BitArray data)
        {
            for (int x = 0; x < _adders.Length; x++) _adders[x].SetInputB(data[x]);
            Sync();
        }

        /// <summary>
        /// Gets state of 'Sum' (∑) output
        /// </summary>
        public BitArray OutputE
        {
            get
            {
                var result = new BitArray(_adders.Length);
                for (int x = 0; x < _adders.Length; x++) result[x] = _adders[x].OutputE;
                return result;
            }
        }

        private void Sync()
        {
            // Carry the 1
            for (int x = 0; x < _adders.Length - 1; x++)
                _adders[x+1].SetInputC(_adders[x].OutputC);
        }
    }
}
