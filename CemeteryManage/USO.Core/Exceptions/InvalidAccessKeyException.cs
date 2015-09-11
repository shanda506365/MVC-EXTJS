
namespace USO.Core.Exceptions
{
    using System;

    [Serializable]
    public class InvalidAccessKeyException : Exception
    {
        public InvalidAccessKeyException(string key)
            : base(string.Format("The user api key '{0}' is not valid.", key))
        { }
    }
}
