using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NUnit.Framework;

namespace USO.Test
{
    public class RyanTest
    {
        [Test]
        public void UrlFormatTest()
        {
            var url = "http://localhost/";
            Console.WriteLine(UrlFormat(url));
            url = "http://localhost/abcd/efd";
            Console.WriteLine(UrlFormat(url));
            url = "http://localhost/abcd/efd?q=1&q=2";
            Console.WriteLine(UrlFormat(url));
            url = "http://localhost/abcd/efd?q=1&childtrace=true";
            Console.WriteLine(UrlFormat(url));
            url = "/abcd/efd?q=1&childtrace=true";
            Console.WriteLine(UrlFormat(url));
        }
        private string UrlFormat(string originalUrl)
        {
            var IsChildTraceParaName = "childtrace";
            var ParentTraceParaName = "parentTrace";
            var _isChildTrace = "sdsf?";
            var _parentTraceId = 22;
            
            var query = string.Empty;
            var path = originalUrl;
            if (originalUrl.Contains("?"))
            {
                var s = originalUrl.Split('?');
                path = s[0];
                query = s[1];
            }
            var queryString = HttpUtility.ParseQueryString(query);

            queryString.Add(ParentTraceParaName, _parentTraceId.ToString());
            if (queryString.AllKeys.Contains(IsChildTraceParaName))
            {
                queryString.Remove(IsChildTraceParaName);
            }
            queryString.Add(IsChildTraceParaName, _isChildTrace.ToString());
            if (queryString.AllKeys.Contains(ParentTraceParaName))
            {
                queryString.Remove(ParentTraceParaName);
            }
            queryString.Add(ParentTraceParaName, _parentTraceId.ToString());

            return string.Format("{0}?{1}", path, queryString);
        }
    }
}
