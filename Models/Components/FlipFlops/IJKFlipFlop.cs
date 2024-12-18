using DigitalElectronics.Concepts;

namespace DigitalElectronics.Components.FlipFlops
{
    public interface IJKFlipFlop : IOutputsQAndNQ, IBooleanOutput
    {
        /// <summary>
        /// Simulates the receipt of a clock pulse
        /// </summary>
        void Clock();

        /// <summary>
        /// Sets the value for 'J' (set) input
        /// </summary>
        /// <param name="value"></param>
        void SetInputJ(bool value);

        /// <summary>
        /// Sets the value for 'K' (reset) input
        /// </summary>
        /// <param name="value"></param>
        void SetInputK(bool value);
    }
}
