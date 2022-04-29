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
        public ParallelBus(int numberOfChannels, params IModule[] modules)
        {
            if (numberOfChannels <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfChannels), "Argument must be greater than 0");
            
            NumberOfChannels = numberOfChannels;
            _modules = modules.ToArray();
        }

        public int NumberOfChannels { get; }

        public BitArray? Output
        {
            get
            {
                try
                {
                    var singleModuleEnabledForOutput = _modules.SingleOrDefault(m => m.Output != null);

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
    }
}
