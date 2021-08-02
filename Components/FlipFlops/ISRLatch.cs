namespace DigitalElectronics.Components.FlipFlops
{

    /// <summary>
    /// A component that has the same inputs and outputs as an SR Latch
    /// <seealso cref="SRLatch"/>
    /// </summary>
    public interface ISRLatch : IOutputsQAndNQ
    {
        /// <summary>
        /// Sets value for input 'Reset'
        /// </summary>
        void SetInputR(bool value);
        
        /// <summary>
        /// Sets value for input 'Set'
        /// </summary>
        void SetInputS(bool value);
    }
}