using System;
using System.Runtime.Serialization;

namespace DigitalElectronics.Concepts
{
    [Serializable]
    public class BusContentionException : Exception
    {
        public const string DefaultMessage =
            "A bus contention has occurred: more than one device has attempted to place a value on " +
            "the same bus at the same time. Avoid enabling multiple devices connect to the same bus " +
            "at the same time.";

        public BusContentionException() : base(DefaultMessage) { }
        public BusContentionException(string message) : base(message) { }
        public BusContentionException(string message, Exception inner) : base(message, inner) { }

        protected BusContentionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
