using System.Collections;
using DigitalElectronics.Components.LogicGates;
using DigitalElectronics.Concepts;
using DigitalElectronics.Utilities;
using BitArray = DigitalElectronics.Concepts.BitArray;

namespace DigitalElectronics.Modules.Memory
{

    /// <summary>
    /// A 4-to-16 line address decoder
    /// </summary>
    /// <remarks>The 4-to-16 line address decoder converts a 4-bit address (4 lines)
    /// to 16-line output, of which only one line corresponding with the given address
    /// is set to `true`. Used in RAM modules to selectively enable the appropriate
    /// internal register.</remarks>
    public class FourBitAddressDecoder
    {
        public const int NumberOfAddressBits = 4;
        public const int NumberOfOutputs = 16;

        private Inverter[] _notA;
        private QuadInputAndGate[] _andY;

        private BitConverter _bitConverter = new(Endianness.Little);

        public FourBitAddressDecoder()
        {
            _notA = new Inverter[NumberOfAddressBits];
            for (int a = 0; a < NumberOfAddressBits; a++) _notA[a] = new Inverter();

            _andY = new QuadInputAndGate[NumberOfOutputs];
            for (int y = 0; y < NumberOfOutputs; y++) _andY[y] = new QuadInputAndGate();
        }

        /// <summary>
        /// Sets the 4-bit value of 'Address' inputs  according to the given <see cref="BitArray"/>
        /// </summary>
        /// <param name="address">A BitArray</param>
        /// <remarks>The address determines which internal register to which data is written to or
        /// read from.</remarks>
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

        /// <summary>
        /// The output of the address decoder
        /// </summary>
        /// <remarks>Returns a <see cref="BitArray"/> of length 16, with one of the bits
        /// set to true corresponding to the address set via
        /// <see cref="SetInputA(BitArray)"/>.</remarks>
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
