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
    public class MainItemListTreeController : Controller
    {

        public MainItemListTreeController(ISysLogService sysLogService)
        {
        }

        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadMainItemListTree()
        {
            var node = Request.Params["node"];
            var user = Session["loginuserInfo"] as UserDTO;
            var mainItemListTreeList = new List<ExReportListTreeDTO>();

            switch (node)
            {
                case "root":
                    //if (user.RoleDtos[0].FunctionsString.Contains("客户管理"))
                    //{
                    //    mainItemListTreeList.Add(new ExReportListTreeDTO
                    //    {
                    //        Id = 1,
                    //        Text = "客户管理",
                    //        Parent = new ExReportListTreeDTO
                    //            {
                    //                Id = 0,
                    //                Text = "陵园系统"
                    //            },
                    //        Expanded = true,
                    //        IconCls = "",
                    //        IsLeaf = true,
                    //        LinkSrc = "",
                    //        Cls = "treepanel-bigFontSize"
                    //    });
                    //}

                    //if (user.RoleDtos[0].FunctionsString.Contains("业务管理"))
                    //{
                        mainItemListTreeList.Add(new ExReportListTreeDTO
                        {
                            Id = 2,
                            Text = "业务管理",
                            Parent = new ExReportListTreeDTO
                            {
                                Id = 0,
                                Text = "陵园系统"
                            },
                            Expanded = true,
                            IconCls = "",
                            IsLeaf = false,
                            LinkSrc = "",
                            Cls = "treepanel-bigFontSize"
                        });
                    //}
                        mainItemListTreeList.Add(new ExReportListTreeDTO
                        {
                            Id = 3,
                            Text = "墓碑查询",
                            Parent = new ExReportListTreeDTO
                            {
                                Id = 0,
                                Text = "陵园系统"
                            },
                            Expanded = false,
                            IconCls = "",
                            IsLeaf = true,
                            LinkSrc = "",
                            Cls = "treepanel-bigFontSize"
                        });
                    break;
                case "2":
                    if (user.RoleDtos[0].FunctionsString.Contains("墓碑预订"))
                    {
                        mainItemListTreeList.Add(new ExReportListTreeDTO
                        {
                            Id = 201,
                            Text = "墓碑预订",
                            Parent = new ExReportListTreeDTO
                            {
                                Id = 2,
                                Text = "业务管理"
                            },
                            Expanded = true,
                            IconCls = "",
                            IsLeaf = true,
                            LinkSrc = "",
                            Cls = "treepanel-bigFontSize"
                        });
                    }
                    if (user.RoleDtos[0].FunctionsString.Contains("墓碑维护"))
                    {
                        mainItemListTreeList.Add(new ExReportListTreeDTO
                        {
                            Id = 202,
                            Text = "墓碑维护",
                            Parent = new ExReportListTreeDTO
                            {
                                Id = 2,
                                Text = "业务管理"
                            },
                            Expanded = true,
                            IconCls = "",
                            IsLeaf = true,
                            LinkSrc = "",
                            Cls = "treepanel-bigFontSize"
                        });
                    }
                    if (user.RoleDtos[0].FunctionsString.Contains("墓碑落葬"))
                    {
                        mainItemListTreeList.Add(new ExReportListTreeDTO
                        {
                            Id = 203,
                            Text = "墓碑落葬",
                            Parent = new ExReportListTreeDTO
                            {
                                Id = 2,
                                Text = "业务管理"
                            },
                            Expanded = true,
                            IconCls = "",
                            IsLeaf = true,
                            LinkSrc = "",
                            Cls = "treepanel-bigFontSize"
                        });
                    }
                    break;
            }



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
