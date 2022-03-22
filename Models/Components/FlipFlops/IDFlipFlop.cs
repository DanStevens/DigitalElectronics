namespace DigitalElectronics.Components.FlipFlops
{
    public interface IDFlipFlop : IOutputsQAndNQ
    {
        /// <summary>
        /// Simulates the receipt of a clock pulse
        /// </summary>
        void Clock();

        /// <summary>
        /// Sets value for 'Data' input
        /// </summary>
        void SetInputD(bool value);
    }
}
