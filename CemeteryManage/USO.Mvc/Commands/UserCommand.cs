
namespace USO.Mvc.Commands
{
    using System.Web.Mvc;

    [Bind(Exclude = "UserName, IPAddress, Browser, Referrer")]
    public class UserCommand
    {
        public string UserName { get; set; }

        public string IPAddress { get; set; }

        public string Browser { get; set; }

        public string Referrer { get; set; }
    }
}