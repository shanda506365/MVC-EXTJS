
namespace USO.Core.Services
{
    using System;


    public interface IHttpRuntime : IDependency
    {
        string AppDomainAppPath { get; }
    }
}
