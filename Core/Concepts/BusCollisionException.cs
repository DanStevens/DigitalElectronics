using System;
using System.Runtime.Serialization;

namespace DigitalElectronics.Concepts
{
    [Serializable]
    public class BusCollisionException : Exception
    {
        public BusCollisionException() { }
        public BusCollisionException(string message) : base(message) { }
        public BusCollisionException(string message, Exception inner) : base(message, inner) { }

        protected BusCollisionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
