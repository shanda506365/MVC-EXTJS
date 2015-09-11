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
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ISysLogService _sysLogService;
        private readonly IFunctionService _functionService;

        public RoleController(IRoleService roleService, ISysLogService sysLogService
            ,IFunctionService functionService)
        {
            _roleService = roleService;
            _sysLogService = sysLogService;
            _functionService = functionService;
        }

        /// <summary>
        /// 加载相应的功能
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadFunctions()
        {
            var functionQuery = new FunctionQuery()
            {
                limit = 0,
                page = 0,
                dir = ListSortDirection.Ascending,
                sort = "Id"
            };
            //过滤条件
            var filter = Request.Params["filter"];
            if (!string.IsNullOrEmpty(filter))
            {
                functionQuery.filter = filter.ToObject<Function>();
            }

            var functions = _functionService.Find(functionQuery);


            var gsbModel = new GridStoreBaseModel<FunctionDTO>
            {
                success = true,
                msg = "成功",
                dataset = functions.Result.ToList(),
                total = functions.Total
            };
            return Json(gsbModel);
        }


        /// <summary>
        /// 加载角色表
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadRoleGrid()
        {
            var roleQuery = new RoleQuery
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
                roleQuery.filter = filter.ToObject<Role>();
            }

            var roles = _roleService.Find(roleQuery);



            var gsbModel = new GridStoreBaseModel<RoleDTO>
            {
                success = true,
                msg = "成功",
                dataset = roles.Result.ToList(),
                total = roles.Total
            };
            return Json(gsbModel);
        }



        /// <summary>
        /// 新增角色
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult AddRole()
        {
            var funDtoList = GetFunDtoList();
            var role = new RoleDTO
            {
                Name = Request.Params["Name"],
                FunctionDtos = funDtoList
            };
            var result = _roleService.Create(role);
            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "新增角色"
                , result.success == true ? "新增角色:" + result.ResultOutDto.Id.ToString() : "新增角色失败:" + result.msg);
            return Json(result);
        }

        private List<FunctionDTO> GetFunDtoList()
        {
            string FunId = Request.Params["FunId"];
            if (String.IsNullOrEmpty(FunId))
            {
                return null;
            }
            var funIdList = FunId.Split(',');
            var funDtoList = new List<FunctionDTO>();
            for (var i = 0; i < funIdList.Length; i++)
            {
                var funDtos = _functionService.GetFunctionById(int.Parse(funIdList[i]));
                funDtoList.Add(funDtos);
            }
            return funDtoList;
        }


        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult Updaterole()
        {

            var id = int.Parse(Request.Params["Id"]);
            string msg = string.Empty;
            var funDtoList = GetFunDtoList();
            var role = new RoleDTO
            {
                Id = id,
                Name = Request.Params["Name"],
                FunctionDtos = funDtoList
            };

            var result = _roleService.Update(role);

            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "编辑角色", result.success == true ? result.ResultOutDto.Id.ToString() : result.msg);
            return Json(result);
        }


        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult Delrole()
        {
            var roleList = new List<RoleDTO>();
            var idList = Request.Params["idList"].Split(',');
            foreach (var id in idList)
            {
                roleList.Add(new RoleDTO
                {
                    Id = int.Parse(id)
                });
            }
            var result = _roleService.Delete(roleList);
            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success,
                "删除角色", result.success == true ? "删除角色:" + Request.Params["idList"] : "删除角色失败:" + result.msg);
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