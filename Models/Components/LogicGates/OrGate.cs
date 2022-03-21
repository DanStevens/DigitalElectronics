﻿using System.Diagnostics;

namespace DigitalElectronics.Components.LogicGates
{

    /// <summary>
    /// Represents a OR logic gate
    /// </summary>
    [DebuggerDisplay("AND: A = {_inputA}; B = {_inputB}; Q = {OutputQ}")]
    public class OrGate
    {
        private bool _inputA;
        private bool _inputB;

        /// <summary>
        /// Sets value for A input
        /// </summary>
        public void SetInputA(bool value)
        {
            _inputA = value;
        }

        /// <summary>
        /// Sets value for B input
        /// </summary
        public void SetInputB(bool value)
        {
            _inputB = value;
        }

        /// <summary>
        /// Gets state of Q output
        /// </summary>
        public bool OutputQ => _inputA || _inputB;
    }
}