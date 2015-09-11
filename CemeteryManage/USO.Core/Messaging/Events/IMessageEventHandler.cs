using USO.Core.Events;
using USO.Core.Messaging.Models;


namespace USO.Core.Messaging.Events
{
    public interface IMessageEventHandler : IEventHandler
    {
        void Sending(MessageContext context);
        void Sent(MessageContext context);
    }
}
