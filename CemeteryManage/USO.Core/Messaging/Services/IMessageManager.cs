
namespace USO.Core.Messaging.Services
{
    using System.Collections.Generic;

    public interface IMessageManager : IDependency
    {
        /// <summary>
        /// Sends a message to a channel
        /// </summary>
        void Send(string recipient, string type, string service, Dictionary<string, string> properties = null);

        /// <summary>
        /// Wether at least one channel is active on the current site
        /// </summary>
        bool HasChannels();

    }
}
