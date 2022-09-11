using System;
using System.Collections;
using DigitalElectronics.Modules;
using BitArray = DigitalElectronics.Concepts.BitArray;

#nullable enable

namespace DigitalElectronics.Components.Memory
{
    [Flags]
    public enum RegisterMode
    {
        None = 0,
        Read = 1,
        Write = 2,
        ReadWrite = 3
    }

    public interface IReadWriteRegister : IWritableRegister, IReadableRegister { }

    public interface IWritableRegister : IRegister, IInputModule
    {
        /// <summary>
        /// Sets value for 'Load' input
        /// </summary>
        /// <param name="value">Set to `true` to enable loading of a data into the register;
        /// otherwise loading is disabled</param>
        void SetInputL(bool value);

        /// <summary>
        /// Simulates the receipt of a clock pulse
        /// </summary>
        /// <remarks>When <see cref="Register.Clock"/> method is called, if the
        /// <see cref="Register.SetInputL">'Load' input</see> is `true`, the data set via
        /// <see cref="Register.SetInputD"/> is loaded into the registry.</remarks>
        void Clock();

        /// <summary>
        /// Resets the register, setting all bits to 1 and disabling output
        /// </summary>
        void Reset();
    }

    public interface IReadableRegister : IRegister, IOutputModule { }

    public interface IRegister : IModule
    {
        /// <summary>
        /// Indicates whether this register supports reading, writing or both
        /// </summary>
        RegisterMode Mode { get; }

        /// <summary>
        /// Returns the internal state of the register
        /// </summary>
        /// <remarks>Consumers can use this to get the register's output without have to set
        /// the 'enable' signal (<see cref="Register.SetInputE"/>) to `true`.</remarks>
        BitArray ProbeState();
    }
}
