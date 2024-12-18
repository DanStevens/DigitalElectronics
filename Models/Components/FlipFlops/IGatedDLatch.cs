using DigitalElectronics.Concepts;

namespace DigitalElectronics.Components.FlipFlops
{
    public interface IGatedDLatch : IOutputsQAndNQ, IBooleanOutput
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
