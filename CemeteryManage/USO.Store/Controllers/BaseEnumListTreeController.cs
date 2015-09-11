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
    public class BaseEnumListTreeController : Controller
    {

        public BaseEnumListTreeController()
        {
        }

        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadBaseEnumListTree()
        {
            var node = Request.Params["node"];
            var user = Session["loginuserInfo"] as UserDTO;
            var mainItemListTreeList = new List<ExReportListTreeDTO>();
            switch (node)
            {
                case "root":
                    if (user.RoleDtos[0].FunctionsString.Contains("用户管理"))
                    {
                        mainItemListTreeList.Add(new ExReportListTreeDTO
                            {
                                Id = 1,
                                Text = "用户管理",
                                Parent = new ExReportListTreeDTO
                                    {
                                        Id = 0,
                                        Text = "基础数据管理"
                                    },
                                Expanded = true,
                                IconCls = "",
                                IsLeaf = true,
                                LinkSrc = "",
                                Cls = "treepanel-bigFontSize"
                            });
                    }
                    if (user.RoleDtos[0].FunctionsString.Contains("角色管理"))
                    {
                        mainItemListTreeList.Add(new ExReportListTreeDTO
                            {
                                Id = 2,
                                Text = "角色管理",
                                Parent = new ExReportListTreeDTO
                                    {
                                        Id = 0,
                                        Text = "基础数据管理"
                                    },
                                Expanded = true,
                                IconCls = "",
                                IsLeaf = true,
                                LinkSrc = "",
                                Cls = "treepanel-bigFontSize"
                            });
                    }
                    if (user.RoleDtos[0].FunctionsString.Contains("墓区设置"))
                    {
                        mainItemListTreeList.Add(new ExReportListTreeDTO
                            {
                                Id = 30,
                                Text = "墓区设置",
                                Parent = new ExReportListTreeDTO
                                    {
                                        Id = 0,
                                        Text = "基础数据管理"
                                    },
                                Expanded = true,
                                IconCls = "",
                                IsLeaf = false,
                                LinkSrc = "",
                                Children = new List<ExReportListTreeDTO>(),
                                Cls = "treepanel-bigFontSize"
                            });
                    }
                    if (user.RoleDtos[0].FunctionsString.Contains("日志管理"))
                    {
                        mainItemListTreeList.Add(new ExReportListTreeDTO
                            {
                                Id = 4,
                                Text = "日志管理",
                                Parent = new ExReportListTreeDTO
                                    {
                                        Id = 0,
                                        Text = "基础数据管理"
                                    },
                                Expanded = true,
                                IconCls = "",
                                IsLeaf = true,
                                LinkSrc = "",
                                Cls = "treepanel-bigFontSize"
                            });
                    }
                    break;
                case "30":
                    if (user.RoleDtos[0].FunctionsString.Contains("区域管理"))
                    {
                        mainItemListTreeList.Add(new ExReportListTreeDTO
                            {
                                Id = 3001,
                                Text = "区域管理",
                                Parent = new ExReportListTreeDTO
                                    {
                                        Id = 30,
                                        Text = "墓区设置"
                                    },
                                Expanded = false,
                                IconCls = "",
                                IsLeaf = true,
                                LinkSrc = "",
                                Cls = "treepanel-bigFontSize"
                            });
                    }
                    if (user.RoleDtos[0].FunctionsString.Contains("墓碑管理"))
                    {
                        mainItemListTreeList.Add(new ExReportListTreeDTO
                        {
                            Id = 3002,
                            Text = "墓碑管理",
                            Parent = new ExReportListTreeDTO
                            {
                                Id = 30,
                                Text = "墓区设置"
                            },
                            Expanded = false,
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