
namespace USO.Mvc.Html
{
    using System;
    using System.Web.Mvc;
    using USO.Domain;

    public static class UrlHelperMatchExtensions
    {
      
        public static string UrlCleaner(this UrlHelper instance, string rawURL, string queryName, string queryValue)
        {
            // Configure the Url
            if (rawURL.IndexOf("?") != -1)
            {
                bool hasPart = false;
                string queryString = rawURL.Substring(rawURL.IndexOf("?") + 1);
                string[] parts = queryString.Split('&');
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].StartsWith(queryName))
                    {
                        parts[i] = queryName + "=" + queryValue;
                        hasPart = true;
                    }
                }
                if (hasPart == true)
                    rawURL = rawURL.Replace(queryString, string.Join("&", parts));
                else
                    rawURL = rawURL.Replace(queryString, string.Join("&", parts) + "&" + queryName + "=" + queryValue);
            }
            else
                rawURL = rawURL + "?" + queryName + "=" + queryValue;

            return rawURL;

        }
    }
}
