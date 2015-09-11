using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USO.Domain;
using USO.Dto;
using USO.Infrastructure.Services;
using USO.Mvc.ActionResults;
using USO.Store.Security;
using USO.Store.ViewModels;

namespace USO.Store.Controllers
{
    public class ExReportListTreeController : Controller
    {
        private readonly ISysLogService _sysLogService;

        public ExReportListTreeController()
        {
        }

        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadExReportListTree()
        {
            var mainItemListTreeList = new List<ExReportListTreeDTO>();
            mainItemListTreeList.Add(new ExReportListTreeDTO
                {
                    Id = 1,
                    Text = "报表统计1",
                    Parent = new ExReportListTreeDTO
                        {
                            Id=0,
                            Text="报表统计"
                        },
                    Expanded = true,
                    IconCls = "",
                    IsLeaf = true,
                    LinkSrc = ""
                });
            mainItemListTreeList.Add(new ExReportListTreeDTO
            {
                Id = 2,
                Text = "报表统计2",
                Parent = new ExReportListTreeDTO
                {
                    Id = 0,
                    Text = "报表统计"
                },
                Expanded = true,
                IconCls = "",
                IsLeaf = true,
                LinkSrc = ""
            });
            var gsbModel = new GridStoreBaseModel<ExReportListTreeDTO>
            {
                success = true,
                msg = "成功",
                dataset = mainItemListTreeList,
                total = mainItemListTreeList.Count
            };
          
            return Json(gsbModel);
        }
    }
}