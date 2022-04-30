using System;
using System.Linq;
using DigitalElectronics.Concepts;

#nullable enable

namespace DigitalElectronics.Modules.Comms
{
    /// <summary>
    /// Models a parallel bus
    /// </summary>
    public class ParallelBus
    {
        private readonly IModule[] _modules;

        /// <summary>
        /// Creates a new parallel bus
        /// </summary>
        /// <param name="numberOfChannels">The number of channels (bits)</param>
        /// <param name="modules">Modules to connect to the bus</param>
        public ParallelBus(int numberOfChannels, params IModule[] modules)
        {
            if (numberOfChannels <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfChannels), "Argument must be greater than 0");
            
            NumberOfChannels = numberOfChannels;
            _modules = modules.ToArray();
        }

        /// <summary>
        /// The number of channels (or bits) the bus has
        /// </summary>
        public int NumberOfChannels { get; }

        /// <summary>
        /// The tri-state output of the parallel bus
        /// </summary>
        /// <returns>If one and only one attached module is enable for output, the output of that
        /// module; otherwise null, which represents the Z (high impedance) state.</returns>
        /// <exception cref="BusContentionException">if more than one module is enable for output.</exception>
        public BitArray? Output
        {
            get
            {
                try
                {
                    var singleModuleEnabledForOutput =
                        _modules.OfType<IOutputModule>().SingleOrDefault(m => m.Output != null);

                    if (singleModuleEnabledForOutput?.Output == null)
                        return null;

                    return new BitArray(singleModuleEnabledForOutput.Output) { Length = NumberOfChannels };
                }
                catch (InvalidOperationException)
                {
                    throw new BusContentionException();
                }
            }
        }

        /// <summary>
        /// Writes the given data to the bus
        /// </summary>
        /// <param name="data">The data to written</param>
        /// <exception cref="ArgumentNullException">if the <paramref name="data"/> arg is `null`</exception>
        /// <remarks>This method invokes <see cref="IInputModule.SetInputD"/> method on all input
        /// modules attached to the bus.</remarks>
        public void Write(BitArray data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));

            foreach (var inputModule in _modules.OfType<IInputModule>())
                inputModule.SetInputD(data);
        }
    }
}
