
namespace USO.Core.Exceptions
{
    using System;

    [Serializable]
    public class ObjectDoesNotExistException : Exception
    {
        public ObjectDoesNotExistException()
        { }

        public ObjectDoesNotExistException(string message)
            : base(message)
        { }
    }

    [Serializable]
    public class ObjectDoesNotExistException<T> : ObjectDoesNotExistException
    {
        public ObjectDoesNotExistException()
            : base(string.Format("Could not find instance of {0} in persistence.", typeof(T).Name))
        { }

        public ObjectDoesNotExistException(string message)
            : base(message)
        { }
    }
}
