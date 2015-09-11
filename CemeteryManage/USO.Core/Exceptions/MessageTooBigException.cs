
namespace USO.Core.Exceptions
{
    using System;

    public class MessageTooBigException : Exception
    {
        public MessageTooBigException()
            : base("The message exceeded the maximum size allowed by the system.")
        {
        }
    }
}
