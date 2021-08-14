using System;
using System.Collections;
using System.Linq;
using DigitalElectronics.Components.ALUs;
using DigitalElectronics.Components.LogicGates;

namespace DigitalElectronics.Modules.ALUs
{
    public class ArithmeticLogicUnit
    {
        private FullAdder[] _adders;
        private TriStateBuffer[] _3Sbuffers;

        public ArithmeticLogicUnit(int numberOfBits)
        {
            if (numberOfBits <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfBits), "Argument must be greater than 0");
            
            _adders = new FullAdder[numberOfBits];
            _3Sbuffers = new TriStateBuffer[numberOfBits];
            for (int x = 0; x < _adders.Length; x++)
            {
                _adders[x] = new FullAdder();
                _3Sbuffers[x] = new TriStateBuffer();
            }
            Sync();
        }

        public int BitCount => _adders.Length;

        /// <summary>
        /// Sets value for A input
        /// </summary>
        /// <param name="data">A BitArray representing the value for A input</param>
        public void SetInputA(BitArray data)
        {
            for (int x = 0; x < _adders.Length; x++) _adders[x].SetInputA(data[x]);
            Sync();
        }

        /// <summary>
        /// Sets value for B input
        /// </summary>
        /// <param name="data">A BitArray representing the value for B input</param>
        public void SetInputB(BitArray data)
        {
            for (int x = 0; x < _adders.Length; x++) _adders[x].SetInputB(data[x]);
            Sync();
        }

        /// <summary>
        /// Sets the value for 'Sum output' signal
        /// </summary>
        /// <param name="value">Set to `true` to enable the 'Sum' output and `false`
        /// to disable the 'Sum' output</param>
        public void SetInputEO(bool value)
        {
            for (int x = 0; x < BitCount; x++) _3Sbuffers[x].SetInputB(value);
            Sync();
        }

        /// <summary>
        /// Gets state of 'Sum' (∑) output
        /// </summary>
        /// <return>The sum of 'A' input and 'B' input</return>
        public BitArray OutputE
        {
            get
            {
                if (!_3Sbuffers[0].OutputC.HasValue)
                    return null;
                
                var result = new BitArray(BitCount);
                for (int x = 0; x < BitCount; x++) result[x] = _3Sbuffers[x].OutputC.Value;
                return result;
            }
        }

        /// <summary>
        /// Returns the internal state of the ALU
        /// </summary>
        /// <remarks>Consumers can use this to get the ALU's sum output without have to set
        /// the 'Sum Output' signal (<see cref="SetInputEO(bool)"/>) to `true`.</remarks>
        public BitArray ProbeState()
        {
            return new BitArray(_adders.Select(_ => _.OutputE).ToArray());
        }

        private void Sync()
        {
            CarryTheOne();
            SyncTriStateBuffersWithAdders();

            void CarryTheOne()
            {
                for (int x = 0; x < _adders.Length - 1; x++)
                    _adders[x + 1].SetInputC(_adders[x].OutputC);
            }

            void SyncTriStateBuffersWithAdders()
            {
                for (int x = 0; x < BitCount; x++)
                    _3Sbuffers[x].SetInputA(_adders[x].OutputE);
            }
        }
    }
}
