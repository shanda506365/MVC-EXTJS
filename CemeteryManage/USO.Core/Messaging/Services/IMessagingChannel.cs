
namespace USO.Core.Messaging.Services
{
    using System.Collections.Generic;
    using USO.Core.Messaging.Models;

    public interface IMessagingChannel : IDependency
    {
        /// <summary>
        /// Actually sends the message though this channel
        /// </summary>
        void SendMessage(MessageContext message);

    }
}
