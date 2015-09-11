using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USO.Domain;
using USO.Domain.Extensions;
using USO.Dto;
using USO.Infrastructure;
using USO.Infrastructure.Services;
using USO.Infrastructure.Services.BaseNum;
using USO.Mvc.ActionResults;
using USO.Store.Security;
using USO.Store.ViewModels;

namespace USO.Store.Controllers
{
    public class OtherController :Controller
    {
        /// <summary>
        /// 加载心跳
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult Heartbeat()
        {
            var result = new JsonResult
                {
                    Data = new
                        {
                            ResultOutDto = "null",
                            success = true,
                            msg = "",
                            code = "",
                            Redirect = ""
                        }
                };
            return Json(result);
        }
    }
}