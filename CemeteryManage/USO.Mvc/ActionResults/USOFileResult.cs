using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace USO.Mvc.ActionResults
{
    public class USOFileResult : FileResult
    {
        private byte[] fileContents;
        public USOFileResult(byte[] fileContents, string contentType, string fileName)
            : base(contentType)
        {
            this.fileContents = fileContents;
            this.FileDownloadName = HttpContext.Current.Request.Browser.Browser.ToUpper().IndexOf("FIREFOX") >= 0
                                        ? fileName
                                        : HttpContext.Current.Server.UrlEncode(fileName);
        }

        protected override void WriteFile(System.Web.HttpResponseBase response)
        {
            if (fileContents == null)
            {
                throw new ArgumentNullException("fileContents");
            }           
            response.OutputStream.Write(this.fileContents, 0, this.fileContents.Length);
        }
    }
}
