
namespace USO.Infrastructure
{
    using System;
    using System.Runtime.Serialization;

    public class USOException : ApplicationException
    {
        public USOException(string message) : base(message) { }
        protected USOException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public USOException(string message, Exception innerException) : base(message, innerException) { }

    }
}
