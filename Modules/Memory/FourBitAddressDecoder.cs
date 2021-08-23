using System.Collections;
using DigitalElectronics.Components.LogicGates;

namespace DigitalElectronics.Modules.Memory
{

    /// <summary>
    /// A 4-to-16 line address decoder
    /// </summary>
    public class FourBitAddressDecoder
    {
        public const int NumberOfAddressBits = 4;
        public const int NumberOfOutputs = 16;

        private Inverter[] _notA;
        private QuadInputAndGate[] _andY;

        private Utilities.BitConverter _bitConverter = new(Utilities.Endianness.Little);

        public FourBitAddressDecoder()
        {
            _notA = new Inverter[NumberOfAddressBits];
            for (int a = 0; a < NumberOfAddressBits; a++) _notA[a] = new Inverter();

            _andY = new QuadInputAndGate[NumberOfOutputs];
            for (int y = 0; y < NumberOfOutputs; y++) _andY[y] = new QuadInputAndGate();
        }

        public void SetInputA(BitArray address)
        {
            for (int a = 0; a < NumberOfAddressBits; a++)
                _notA[a].SetInputA(address[a]);

            for (int y = 0; y < NumberOfOutputs; y++)
                SetY(y);

            void SetY(int y)
            {
                var mask = _bitConverter.GetBits(y, NumberOfAddressBits);
                
                _andY[y].SetInputA(_GetA(0, mask[0]));
                _andY[y].SetInputB(_GetA(1, mask[1]));
                _andY[y].SetInputC(_GetA(2, mask[2]));
                _andY[y].SetInputD(_GetA(3, mask[3]));
            }

            bool _GetA(int a, bool ordered)
            {
                return ordered ? address[a] : _notA[a].OutputQ;
            }
        }

        public BitArray OutputY
        {
            get
            {
                var result = new BitArray(NumberOfOutputs);

                for (int y = 0; y < NumberOfOutputs; y++)
                    result[y] = _andY[y].OutputQ;

                return result;
            }
        }
    }
}
