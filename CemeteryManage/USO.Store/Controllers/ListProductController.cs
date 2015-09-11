using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace USO.Web.Controllers
{
    public class ListProductController : Controller
    {


        public ActionResult ListProduct()
        {
            var testObj = new List<string>()
        {
            "aaa", "bbb", "ccc"
        };

            return PartialView("ListProduct", testObj);
        }
    }
}

