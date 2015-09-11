
namespace USO.Core.Services
{
    using System;


    public class SystemGuid : IGuid
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}
