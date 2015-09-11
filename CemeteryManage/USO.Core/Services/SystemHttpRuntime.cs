
namespace USO.Core.Services
{
    using System.Web;


    public class SystemHttpRuntime : IHttpRuntime
    {
        public string AppDomainAppPath { get { return HttpRuntime.AppDomainAppPath; } }
    }
}
