
namespace USO.Infrastructure
{
    using System;
    using USO.Core;

    public interface IWorkContextStateProvider : IDependency
    {
        /// <summary>
        /// 提供工作上下文CurrentOrganizations的属性值
        /// </summary>
        Func<T> Get<T>(string name);
    }
}
