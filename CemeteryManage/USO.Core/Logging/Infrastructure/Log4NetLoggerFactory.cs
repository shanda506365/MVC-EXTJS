
namespace USO.Core.Logging.Infrastructure
{
    using System;

    /// <summary>
    /// An implementation of a logger factory that creates <see cref="Log4NetLogger"/>s.
    /// </summary>
    public class Log4NetLoggerFactory : LoggerFactoryBase
    {
        /// <summary>
        /// Creates a logger for the specified type.
        /// </summary>
        /// <param name="type">The type to create the logger for.</param>
        /// <returns>The newly-created logger.</returns>
        public override ILogger CreateLogger(Type type)
        {
            return new Log4NetLogger(type);
        }
    }
}
