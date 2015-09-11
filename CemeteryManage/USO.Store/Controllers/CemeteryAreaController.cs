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
    public class CemeteryAreaController : Controller
    {
        private readonly ICemeteryAreasService _cemeteryAreasService;
        private readonly ISysLogService _sysLogService;


        public CemeteryAreaController(ICemeteryAreasService cemeteryAreasService, ISysLogService sysLogService)
        {
            _cemeteryAreasService = cemeteryAreasService;
            _sysLogService = sysLogService;
        }

        /// <summary>
        /// 加载墓碑区域表
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadCemeteryAreaGrid()
        {
            var cemeteryAreasQuery = new CemeteryAreasQuery
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
                cemeteryAreasQuery.filter = filter.ToObject<CemeteryAreas>();
            }

            var cemeteryAreass = _cemeteryAreasService.Find(cemeteryAreasQuery);



            var gsbModel = new GridStoreBaseModel<CemeteryAreasDTO>
            {
                success = true,
                msg = "成功",
                dataset = cemeteryAreass.Result.ToList(),
                total = cemeteryAreass.Total
            };
            return Json(gsbModel);
        }



        /// <summary>
        /// 新增墓碑区域
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult AddCemeteryArea()
        {
            var cemeteryAreas = new CemeteryAreasDTO
            {
                Id = GlobalMethod.CoventToIntNotNull(Request.Params["Id"]),
                Name = Request.Params["Name"],
                Alias = Request.Params["Alias"],
                Remark = Request.Params["Remark"],
                RowSort = Request.Params["RowSort"]
            };
            var result = _cemeteryAreasService.Create(cemeteryAreas);
            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "新增墓碑区域"
                , result.success == true ? "新增墓碑区域:" + result.ResultOutDto.Id.ToString() : "新增墓碑区域失败:" + result.msg);
            return Json(result);
        }


        /// <summary>
        /// 编辑墓碑区域
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult UpdateCemeteryArea()
        {
            var cemeteryAreas = new CemeteryAreasDTO
            {
                Id = GlobalMethod.CoventToIntNotNull(Request.Params["Id"]),
                Name = Request.Params["Name"],
                Alias = Request.Params["Alias"],
                Remark = Request.Params["Remark"],
                RowSort = Request.Params["RowSort"]
            };

            var result = _cemeteryAreasService.Update(cemeteryAreas);

            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "编辑墓碑区域", result.success == true ? result.ResultOutDto.Id.ToString() : result.msg);
            return Json(result);
        }


        /// <summary>
        /// 删除墓碑区域
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult DelCemeteryArea()
        {
            var cemeteryAreasList = new List<CemeteryAreasDTO>();
            var idList = Request.Params["idList"].Split(',');
            foreach (var id in idList)
            {
                cemeteryAreasList.Add(new CemeteryAreasDTO
                {
                    Id = int.Parse(id)
                });
            }
            var result = _cemeteryAreasService.Delete(cemeteryAreasList);
            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success,
                "删除墓碑区域", result.success == true ? "删除墓碑区域:" + Request.Params["idList"] : "删除墓碑区域失败:" + result.msg);
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
          
            return sortStr;
        }
    }
}