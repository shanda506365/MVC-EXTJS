
namespace USO.Core.Messaging.Models
{
    using System.Collections.Generic;
    using System.Net.Mail;


    public class MessageContext
    {
        public MailMessage MailMessage { get; private set; }
        public string Type { get; set; }
        public string Service { get; set; }
        public string Recipient { get; set; }
        public Dictionary<string, string> Properties { get; private set; }
        public bool MessagePrepared { get; set; }

        public MessageContext()
        {
            Properties = new Dictionary<string, string>();
            MailMessage = new MailMessage();
        }
    }
}
