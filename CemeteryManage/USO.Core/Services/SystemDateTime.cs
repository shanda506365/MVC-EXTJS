
namespace USO.Core.Services
{
    using System;

    public class SystemDateTime : IDateTime
    {
        public DateTime UtcNow { get { return DateTime.UtcNow; } }
    }
}
