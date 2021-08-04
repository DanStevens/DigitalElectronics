using DigitalElectronics.Components.LogicGates;
using System.Diagnostics;

namespace DigitalElectronics.Components.FlipFlops
{
    /// <summary>
    /// Represents a SR NOR latch
    /// </summary>
    [DebuggerDisplay("SR Latch: R = {_nor1._inputA}; S = {_nor2._inputB}; Q = {OutputQ}")]
    public class SRLatch : ISRLatch
    {
        private NorGate _nor1;
        private NorGate _nor2;

        public SRLatch()
        {
            _nor1 = new NorGate();
            _nor2 = new NorGate();
            Sync();
        }

        /// <summary>
        /// Sets value for 'Reset' input
        /// </summary>
        public void SetInputR(bool value)
        {
            _nor1.SetInputA(value);
            Sync();
        }

        /// <summary>
        /// Sets value for 'Set' input
        /// </summary>
        public void SetInputS(bool value)
        {
            _nor2.SetInputB(value);
            Sync();
        }

        /// <summary>
        /// Gets state of Q output
        /// </summary>
        public bool OutputQ => _nor1.OutputQ;

        /// <summary>
        /// Gets state of Q̅ output, which is the opposite of Q output
        /// </summary>
        public bool OutputNQ => _nor2.OutputQ;

        /// <summary>
        /// Synchronises state between subcomponents
        /// </summary>
        private void Sync()
        {
            _nor2.SetInputA(_nor1.OutputQ);
            _nor1.SetInputB(_nor2.OutputQ);
        }

    }
}