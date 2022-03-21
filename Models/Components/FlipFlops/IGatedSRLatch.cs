namespace DigitalElectronics.Components.FlipFlops
{
    public interface IGatedSRLatch : ISRLatch
    {
        /// <summary>
        /// Sets value for 'Enable' input
        /// </summary>
        /// <param name="value"></param>
        void SetInputE(bool value);
    }
}
