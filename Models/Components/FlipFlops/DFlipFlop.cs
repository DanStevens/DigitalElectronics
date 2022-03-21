
using System.Diagnostics;

namespace DigitalElectronics.Components.FlipFlops
{
    
    /// <summary>
    /// Models a D flip-flop
    /// </summary>
    [DebuggerDisplay("D Flip-flop: Q = {OutputQ}; Q̅ = {OutputNQ}")]
    public class DFlipFlop : IOutputsQAndNQ
    {
        private GatedDLatch _gatedDLatch;

        public DFlipFlop()
        {
            _gatedDLatch = new GatedDLatch();
        }

        /// <summary>
        /// Simulates the receipt of a clock pulse
        /// </summary>
        public void Clock()
        {
            _gatedDLatch.SetInputE(true);
            _gatedDLatch.SetInputE(false);
        }

        /// <summary>
        /// Sets value for 'Data' input
        /// </summary>
        public void SetInputD(bool value)
        {
            _gatedDLatch.SetInputD(value);
        }

        /// <summary>
        /// Gets state of Q output
        /// </summary>
        public bool OutputQ => _gatedDLatch.OutputQ;

        /// <summary>
        /// Gets state of Q̅ output, which is the opposite of Q output
        /// </summary>
        public bool OutputNQ => _gatedDLatch.OutputNQ;

    }
}
