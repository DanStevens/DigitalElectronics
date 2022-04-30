using System.Collections;
using DigitalElectronics.Modules;
using BitArray = DigitalElectronics.Concepts.BitArray;

namespace DigitalElectronics.Components.Memory
{
    public interface IRegister : IInputModule, IOutputModule
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
        /// Returns the internal state of the register
        /// </summary>
        /// <remarks>Consumers can use this to get the register's output without have to set
        /// the 'enable' signal (<see cref="Register.SetInputE"/>) to `true`.</remarks>
        BitArray ProbeState();
    }
}
