﻿using System.Diagnostics;
using DigitalElectronics.Components.LogicGates;

namespace DigitalElectronics.Components.ALUs
{

    /// <summary>
    /// Models a half adder for summing a single pair of bits
    /// </summary>
    [DebuggerDisplay("A = {_xor._inputA}; B = {_xor._inputB}; ∑ = {OutputE}; C => {OutputC}")]
    public class HalfAdder
    {
        private readonly XorGate _xor;
        private readonly AndGate _and;

        public HalfAdder()
        {
            _xor = new XorGate();
            _and = new AndGate();
        }

        /// <summary>
        /// Sets value for A input
        /// </summary>
        public void SetInputA(bool value)
        {
            _xor.SetInputA(value);
            _and.SetInputA(value);
        }

        /// <summary>
        /// Sets value for B input
        /// </summary>
        public void SetInputB(bool value)
        {
            _xor.SetInputB(value);
            _and.SetInputB(value);
        }

        /// <summary>
        /// Gets state of 'Carry' output
        /// </summary>
        public bool OutputC => _and.OutputQ;

        /// <summary>
        /// Gets state of 'Sum' (∑) output
        /// </summary>
        public bool OutputE => _xor.OutputQ;

    }
}
