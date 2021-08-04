using DigitalElectronics.Components.LogicGates;
using System;
using System.Diagnostics;

namespace DigitalElectronics.Components.FlipFlops
{
    /// <summary>
    /// Represents a Gated SR NOR latch
    /// </summary>
    [DebuggerDisplay("SR Latch: E = {_and1._inputA}, R = {_and1._inputA}; S = {_and2._inputB}; Q = {OutputQ}")]
    public class GatedSRLatch : ISRLatch, IOutputsQAndNQ
    {
        private SRLatch _srLatch;
        private AndGate _and1;
        private AndGate _and2;

        public GatedSRLatch()
        {
            _srLatch = new SRLatch();
            _and1 = new AndGate();
            _and2 = new AndGate();
            Sync();
        }

        /// <summary>
        /// Sets value for 'Reset' input
        /// </summary>
        public void SetInputR(bool value)
        {
            _and1.SetInputA(value);
            Sync();
        }

        /// <summary>
        /// Sets value for 'Set' input
        /// </summary>
        public void SetInputS(bool value)
        {
            _and2.SetInputB(value);
            Sync();
        }

        /// <summary>
        /// Sets value for 'Enable' input
        /// </summary>
        /// <param name="value"></param>
        public void SetInputE(bool value)
        {
            _and1.SetInputB(value);
            _and2.SetInputA(value);
            Sync();
        }

        /// <summary>
        /// Gets state of Q output
        /// </summary>
        public bool OutputQ => _srLatch.OutputQ;

        /// <summary>
        /// Gets state of Q̅ output, which is the opposite of Q output
        /// </summary>
        public bool OutputNQ => _srLatch.OutputNQ;

        private void Sync()
        {
            _srLatch.SetInputR(_and1.OutputQ);
            _srLatch.SetInputS(_and2.OutputQ);
        }

    }
}
