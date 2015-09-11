namespace USO.Core.Messaging.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using USO.Core.Logging;
    using USO.Core.Messaging.Events;
    using USO.Core.Messaging.Models;

    public class DefaultMessageManager : IMessageManager
    {
        private readonly IMessageEventHandler _messageEventHandler;
        private readonly IEnumerable<IMessagingChannel> _channels;

        private ILogger Logger { get; set; }

        public DefaultMessageManager(
            IDependencyResolver dependencyResolver,
            IMessageEventHandler messageEventHandler,
            ILoggerFactory loggerFactory)
        {
            _messageEventHandler = messageEventHandler;
            _channels = dependencyResolver.GetServices<IMessagingChannel>();
            Logger = loggerFactory.CreateLogger(GetType());
        }

        public void Send(string recipient, string type, string service, Dictionary<string, string> properties = null)
        {
            if (!HasChannels())
                return;

            Logger.Info("Sending message {0}", type);
            try
            {

                var context = new MessageContext
                {
                    Recipient = recipient,
                    Type = type,
                    Service = service
                };

                try
                {

                    if (properties != null)
                    {
                        foreach (var key in properties.Keys)
                            context.Properties.Add(key, properties[key]);
                    }

                    _messageEventHandler.Sending(context);

                    foreach (var channel in _channels)
                    {
                        channel.SendMessage(context);
                    }

                    _messageEventHandler.Sent(context);
                }
                finally
                {
                    context.MailMessage.Dispose();
                }

                Logger.Info("Message {0} sent", type);
            }
            catch (Exception e)
            {
                Logger.Error(e, "An error occured while sending the message {0}", type);
            }
        }

        public bool HasChannels()
        {
            return _channels.Any();
        }
        
    }
}
