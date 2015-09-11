
namespace USO.Core.Exceptions
{
    using System;

    [Serializable]
    public class InvalidAppSettingException : Exception
    {
        public InvalidAppSettingException(string key, string invalidProperty)
            : base(string.Format("Property '{0}' is set to an invalid setting '{1}'.", key, invalidProperty))
        { }
    }
}
