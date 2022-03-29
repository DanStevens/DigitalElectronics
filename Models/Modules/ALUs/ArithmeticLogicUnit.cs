using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using DigitalElectronics.Components.ALUs;
using DigitalElectronics.Components.LogicGates;

namespace DigitalElectronics.Modules.ALUs
{
    
    [DebuggerDisplay("ALU: {this.ProbeState()}")]
    public class ArithmeticLogicUnit : IArithmeticLogicUnit
    {
        private readonly FullAdder[] _adders;
        private readonly TriStateBuffer[] _3Sbuffers;
        private readonly XorGate[] _xorGates;

        public ArithmeticLogicUnit(int numberOfBits)
        {
            if (numberOfBits <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfBits), "Argument must be greater than 0");
            
            _adders = new FullAdder[numberOfBits];
            _3Sbuffers = new TriStateBuffer[numberOfBits];
            _xorGates = new XorGate[numberOfBits];

            for (int x = 0; x < BitCount; x++)
            {
                _adders[x] = new FullAdder();
                _3Sbuffers[x] = new TriStateBuffer();
                _xorGates[x] = new XorGate();
            }

            for (int x = 0; x < BitCount; x++) SyncBit(x);
        }

        public int BitCount => _adders.Length;

        /// <summary>
        /// Sets value for A input
        /// </summary>
        /// <param name="data">A BitArray representing the value for A input</param>
        public void SetInputA(BitArray data)
        {
            if (data == null) return;
            
            for (int x = 0; x < BitCount; x++)
            {
                _adders[x].SetInputA(data[x]);
                SyncBit(x);
            }
        }

        /// <summary>
        /// Sets value for B input
        /// </summary>
        /// <param name="data">A BitArray representing the value for B input</param>
        public void SetInputB(BitArray data)
        {
            if (data == null) return;

            for (int x = 0; x < BitCount; x++)
            {
                _xorGates[x].SetInputA(data[x]);
                SyncBit(x);
            }
        }

        /// <summary>
        /// Sets the value for 'Sum output' signal
        /// </summary>
        /// <param name="value">Set to `true` to enable the 'Sum' output and `false`
        /// to disable the 'Sum' output</param>
        public void SetInputEO(bool value)
        {
            for (int x = 0; x < BitCount; x++)
            {
                _3Sbuffers[x].SetInputB(value);
                SyncBit(x);
            }
        }

        /// <summary>
        /// Sets the value for the 'Subtract' signal
        /// </summary>
        /// <param name="value">Set to `true` to set ALU to subtraction mode and `false` for
        /// addition mode</param>
        public void SetInputSu(bool value)
        {
            _adders[0].SetInputC(value);
            
            for (int x = 0; x < BitCount; x++)
            {
                _xorGates[x].SetInputB(value);
                SyncBit(x);
            }
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

        private void SyncBit(int x)
        {
            _adders[x].SetInputB(_xorGates[x].OutputQ);
            if (x < BitCount - 1) _adders[x + 1].SetInputC(_adders[x].OutputC);
            _3Sbuffers[x].SetInputA(_adders[x].OutputE);
        }
    }
}
