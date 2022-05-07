namespace DigitalElectronics.Concepts
{
    /// <summary>
    /// Represents a memory modules that has an shared input to 
    /// </summary>
    /// <remarks>'Indirectly addressable' means the module has a single input, which is
    /// used to set the address and/or data, via the <see cref="SetInputS(BitArray)"/>
    /// method. The <see cref="SetInputLA"/> and <see cref="DigitalElectronics.Modules.Memory.IRAM.SetInputLD(bool)"/>
    /// methods, which determine whether the data passed to the shared input is used
    /// as an address or data.</remarks>
    public interface ISharedAddrDataInput
    {
        /// <summary>
        /// Sets value of the 'Shared Data' inputs according to the given <see cref="BitArray"/>
        /// </summary>
        /// <param name="addressOrData">A <see cref="BitArray"/> containing the values can
        /// /// either be an address or data (or both)</param>
        void SetInputS(BitArray addressOrData);

        /// <summary>
        /// Sets the value of the 'Load Address' input
        /// </summary>
        /// <remarks>When set to `true`, when the module is <see cref="Clock">clocked</see>,
        /// the address is updated.</remarks>
        void SetInputLA(bool value);

        BitArray ProbeAddress();
    }
}
