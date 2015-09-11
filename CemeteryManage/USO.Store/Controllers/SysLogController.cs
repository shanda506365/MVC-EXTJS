using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcExtensions;
using Newtonsoft.Json;
using USO.Domain;
using USO.Domain.Extensions;
using USO.Dto;
using USO.Infrastructure;
using USO.Infrastructure.Services;
using USO.Mvc.ActionResults;
using USO.Mvc.Utility;
using USO.Store.Security;
using USO.Store.ViewModels;

namespace USO.Store.Controllers
{
    public class SysLogController : Controller
    {
        private readonly ISysLogService _sysLogService;
        public SysLogController(ISysLogService sysLogService)
        {
            _sysLogService = sysLogService;
        }

        /// <summary>
        /// 加载日志表
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadSysLogGrid()
        {
            var query = new SysLogQuery
                {
                    limit = int.Parse(Request.Params["limit"]),
                    page = int.Parse(Request.Params["page"]),
                    dir = Request.Params["dir"] == "ASC" ? ListSortDirection.Ascending : ListSortDirection.Descending,
                    sort = InitSortParam(Request.Params["sort"])
                };
            //过滤条件
            var filter = Request.Params["filter"];
            if (!string.IsNullOrEmpty(filter))
            {
                query.filter = filter.ToObject<SysLog>();
            }


            var syslogs = _sysLogService.Find(query);



            var model = new GridStoreBaseModel<SysLogDTO>
                {
                    success = true,
                    msg = "成功",
                    dataset = syslogs.Result.ToList(),
                    total = syslogs.Total
                };
            return Json(model);
        }

        /// <summary>
        /// 新增日志
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult AddSysLog()
        {
            var dto = new SysLogDTO
                {
                    UserId = GlobalMethod.CoventToIntNotNull(Request.Params["UserId"])
                };
            var result = _sysLogService.Create(dto);
            return Json(result);
        }

       

         /// <summary>
        /// 初始化排序字段
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string InitSortParam(string str)
        {
            var sortStr = str;
            if (str == "CustomerTypeName")
            {
                return "CustomerTypeId";
            }
            return sortStr;
        }
    }
}
