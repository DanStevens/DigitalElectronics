namespace DigitalElectronics.Components.FlipFlops
{
    public interface IGatedDLatch : IOutputsQAndNQ
    {
        /// <summary>
        /// Sets value for 'Data' input
        /// </summary>
        void SetInputD(bool value);

        /// <summary>
        /// Sets value for 'Enable' input
        /// </summary>
        void SetInputE(bool value);
    }
}
