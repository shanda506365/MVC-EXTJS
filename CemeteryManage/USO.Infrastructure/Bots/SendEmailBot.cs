namespace USO.Infrastructure.Bots
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Net.Mail;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using MvcExtensions;
    using USO.Core.Messaging.Models;
    using USO.Core.Messaging.Services;
    using USO.Core.Tasks;


    public class SendEmailBot : IBot
    {
        private readonly IDisposable subscription;
      

        public void Dispose()
        {
            subscription.Dispose();
        }
    }
}
