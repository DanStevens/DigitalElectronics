namespace DigitalElectronics.Components.Memory
{
    public interface IRegisterBit
    {
        /// <summary>
        /// Sets for for 'Enabled' input
        /// </summary>
        /// <param name="value">Set to `true` to enable output and `false` to disable output</param>
        /// <remarks>
        /// The `Enabled` input determines whether the register outputs the currently latched value,
        /// or `null`, which represents the Z (high impedance) state.
        /// <seealso cref="NBitRegister.SetInputE"/>
        void SetInputE(bool value);

        /// <summary>
        /// Sets value for 'Load' input
        /// </summary>
        void SetInputL(bool value);

        /// <summary>
        /// Sets value for 'Data' input
        /// </summary>
        void SetInputD(bool value);

        /// <summary>
        /// Gets state of C output, where `null` indicates Z (high impedance) state
        /// </summary>
//public bool? OutputQ => _dFlipFlop.OutputQ;
        bool? OutputQ { get; }

        /// <summary>
        /// Simulates the receipt of a clock pulse
        /// </summary>
        void Clock();

        /// <summary>
        /// Returns the internal state of the register bit
        /// </summary>
        bool ProbeState();
    }
}
