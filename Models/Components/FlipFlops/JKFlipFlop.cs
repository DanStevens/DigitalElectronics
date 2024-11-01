using System.Diagnostics;
using DigitalElectronics.Components.LogicGates;

namespace DigitalElectronics.Components.FlipFlops
{

    /// <summary>
    /// Models a JK flip-flop
    /// </summary>
    [DebuggerDisplay("JK Flip-flop: Q = {OutputQ}; Q̅ = {OutputNQ}")]
    public class JKFlipFlop : IJKFlipFlop
    {
        private readonly SRLatch _srLatch;
        private readonly TripleInputAndGate _j3InAndGate;
        private readonly TripleInputAndGate _k3InAndGate;

        public JKFlipFlop()
        {
            _srLatch = new SRLatch();
            _j3InAndGate = new TripleInputAndGate();
            _k3InAndGate = new TripleInputAndGate();
            Sync();
        }

        /// <summary>
        /// Simulates the receipt of a clock pulse
        /// </summary>
        public void Clock()
        {
            _j3InAndGate.SetInputA(true);
            _k3InAndGate.SetInputA(true);
            Sync();
            _j3InAndGate.SetInputA(false);
            _k3InAndGate.SetInputA(false);
        }

        /// <summary>
        /// Sets the value for 'J' (set) input
        /// </summary>
        /// <param name="value"></param>
        public void SetInputJ(bool value)
        {
            _j3InAndGate.SetInputB(value);
        }

        /// <summary>
        /// Sets the value for 'K' (reset) input
        /// </summary>
        /// <param name="value"></param>
        public void SetInputK(bool value)
        {
            _k3InAndGate.SetInputB(value);
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
            _srLatch.SetInputS(_j3InAndGate.OutputQ);
            _srLatch.SetInputR(_k3InAndGate.OutputQ);
            _k3InAndGate.SetInputC(_srLatch.OutputQ);
            _j3InAndGate.SetInputC(_srLatch.OutputNQ);
        }

    }
}
