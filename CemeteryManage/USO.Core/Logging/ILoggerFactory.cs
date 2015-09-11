
namespace USO.Core.Logging
{
    using System;

    public interface ILoggerFactory
    {
        ILogger CreateLogger(Type type);
    }
}
