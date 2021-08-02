namespace DigitalElectronics.Components.FlipFlops
{

    /// <summary>
    /// A component that has two outputs: Q and Q̅ (NQ)
    /// </summary>
    public interface IOutputsQAndNQ
    {

        /// <summary>
        /// Gets state of Q output
        /// </summary>
        bool OutputNQ { get; }

        /// <summary>
        /// Gets state of Q̅ output, which is the opposite of Q output
        /// </summary>
        bool OutputQ { get; }
    }
}