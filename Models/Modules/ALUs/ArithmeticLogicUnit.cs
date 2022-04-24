using System;
using System.Diagnostics;
using System.Linq;
using DigitalElectronics.Components.ALUs;
using DigitalElectronics.Components.LogicGates;
using DigitalElectronics.Concepts;

#nullable enable

namespace DigitalElectronics.Modules.ALUs
{
    
    [DebuggerDisplay("ALU: {this.ProbeState()}")]
    public class ArithmeticLogicUnit : IArithmeticLogicUnit
    {
        private readonly FullAdder[] _adders;
        private readonly TriStateBuffer[] _3SBuffers;
        private readonly XorGate[] _xorGates;

        public ArithmeticLogicUnit(int sizeInBits)
        {
            if (sizeInBits <= 0)
                throw new ArgumentOutOfRangeException(nameof(sizeInBits), "Argument must be greater than 0");
            
            _adders = new FullAdder[sizeInBits];
            _3SBuffers = new TriStateBuffer[sizeInBits];
            _xorGates = new XorGate[sizeInBits];

            for (int x = 0; x < SizeInBits; x++)
            {
                _adders[x] = new FullAdder();
                _3SBuffers[x] = new TriStateBuffer();
                _xorGates[x] = new XorGate();
            }

            for (int x = 0; x < SizeInBits; x++) SyncBit(x);
        }

        public int SizeInBits => _adders.Length;

        /// <summary>
        /// Sets value for A input
        /// </summary>
        /// <param name="data">A BitArray representing the value for A input</param>
        public void SetInputA(BitArray data)
        {
            if (data == null) return;
            
            for (int x = 0; x < SizeInBits; x++)
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

            for (int x = 0; x < SizeInBits; x++)
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
            for (int x = 0; x < SizeInBits; x++)
            {
                _3SBuffers[x].SetInputB(value);
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
            
            for (int x = 0; x < SizeInBits; x++)
            {
                _xorGates[x].SetInputB(value);
                SyncBit(x);
            }
        }

        /// <summary>
        /// Gets state of 'Sum' (∑) output
        /// </summary>
        /// <return>The sum of 'A' input and 'B' input</return>
        public BitArray? OutputE
        {
            get
            {
                if (!_3SBuffers[0].OutputC.HasValue)
                    return null;
                
                var result = new BitArray(SizeInBits);
                for (int x = 0; x < SizeInBits; x++) result[x] = _3SBuffers[x].OutputC!.Value;
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
            if (x < SizeInBits - 1) _adders[x + 1].SetInputC(_adders[x].OutputC);
            _3SBuffers[x].SetInputA(_adders[x].OutputE);
        }
    }
}
