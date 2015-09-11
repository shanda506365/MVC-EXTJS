
namespace USO.Core.Exceptions
{
    using System;

    [Serializable]
    public class MissingConnectionStringSettingsException : MissingAppSettingException
    {
        public MissingConnectionStringSettingsException(string key)
            : base(string.Format("connection string setting '{0}'", key))
        { }
    }
}
