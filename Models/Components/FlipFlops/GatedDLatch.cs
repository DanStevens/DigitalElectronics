using DigitalElectronics.Components.LogicGates;
using DigitalElectronics.Concepts;
using System.Diagnostics;

namespace DigitalElectronics.Components.FlipFlops
{
    [DebuggerDisplay("D Latch: Q = {OutputQ}; Q̅ = {OutputNQ}")]
    public class GatedDLatch : IGatedDLatch, IBooleanOutput
    {
        private readonly GatedSRLatch _gatedSRLatch;
        private readonly Inverter _inverter;

        public GatedDLatch()
        {
            _gatedSRLatch = new GatedSRLatch();
            _inverter = new Inverter();
            Sync();
        }

        /// <summary>
        /// Sets value for 'Data' input
        /// </summary>
        public void SetInputD(bool value)
        {
            _gatedSRLatch.SetInputS(value);
            _inverter.SetInputA(value);
            Sync();
        }

        private void Sync()
        {
            _gatedSRLatch.SetInputR(_inverter.OutputQ);
        }

        /// <summary>
        /// Sets value for 'Enable' input
        /// </summary>
        public void SetInputE(bool value)
        {
            _gatedSRLatch.SetInputE(value);
        }

        /// <summary>
        /// Gets state of Q output
        /// </summary>
        public bool OutputQ => _gatedSRLatch.OutputQ;

        /// <summary>
        /// Gets state of Q̅ output, which is the opposite of Q output
        /// </summary>
        public bool OutputNQ => _gatedSRLatch.OutputNQ;

        bool IBooleanOutput.Output => OutputQ;
    }
}
