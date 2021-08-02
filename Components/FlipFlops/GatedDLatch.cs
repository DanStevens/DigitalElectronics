using DigitalElectronics.Components.LogicGates;

namespace DigitalElectronics.Components.FlipFlops
{
    public class GatedDLatch : IOutputsQAndNQ
    {
        private GatedSRLatch _gatedSRLatch;
        private Inverter _inverter;

        public GatedDLatch()
        {
            _gatedSRLatch = new GatedSRLatch();
            _inverter = new Inverter();
        }

        /// <summary>
        /// Sets value for input 'Data'
        /// </summary>
        public void SetInputD(bool value)
        {
            _gatedSRLatch.SetInputS(value);
            _inverter.SetInputA(value);
            _gatedSRLatch.SetInputR(_inverter.OutputQ);
        }

        /// <summary>
        /// Sets value for input 'Enable'
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

    }
}
