using System;
using System.Configuration;
using System.Web.Mvc;

namespace MvcExtensions
{
    /// <summary>
    /// 解决MVC使用Duration的bug
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CacheOutputDurationAttribute : OutputCacheAttribute
    {
        /// <summary>
        /// 返回缓存时间
        /// </summary>
        public CacheOutputDurationAttribute()
        {
            try
            {
                Duration = int.Parse(ConfigurationManager.AppSettings["CacheOutputDuration"]) * 60;
            }catch
            {
                Duration = 1;
            }
        }
    }
}
