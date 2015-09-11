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
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISysLogService _sysLogService;

        public UserController(IUserService userService, ISysLogService sysLogService)
        {
            _userService = userService;
            _sysLogService = sysLogService;
        }


        /// <summary>
        /// 加载用户表
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadUserGrid()
        {
            var userQuery = new UserQuery
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
                userQuery.filter = filter.ToObject<User>();
            }
            var createDateQuery = Request.Params["CreateDateQuery"];
            //创建时间
            if (!string.IsNullOrEmpty(createDateQuery))
            {
                userQuery.CreateDateQuery = createDateQuery;
            }

            var users = _userService.Find(userQuery);



            var gsbModel = new GridStoreBaseModel<UserDTO>
            {
                success = true,
                msg = "成功",
                dataset = users.Result.ToList(),
                total = users.Total
            };
            return Json(gsbModel);
        }



        /// <summary>
        /// 新增用户
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult AddUser()
        {
            var roleIdItemId = Request.Params["roleIdItemId"];
            var role = new RoleDTO
                {
                    Id = int.Parse(roleIdItemId)
                };
            var user = new UserDTO
            {
                Name = Request.Params["Name"],
                DepartmentId = GlobalMethod.CoventToInt(Request.Params["DepartmentId"]),
                LoginName = Request.Params["LoginName"],
                Code = Request.Params["Code"],
                Remark = Request.Params["Remark"],
                Password = Request.Params["newpassHd"],
                Position = Request.Params["Position"],
                CreateDate = DateTime.Now,
                Status = (UserStatus)GlobalMethod.CoventToIntNotNull(Request.Params["Status"]),
                RoleDtos = new List<RoleDTO>{ role }
            };
            var result = _userService.Create(user);
            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "新增用户"
                , result.success == true ? "新增用户:"+ result.ResultOutDto.Id.ToString() : "新增用户失败:" + result.msg);
            return Json(result);
        }


        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult UpdateUser()
        {
            var roleIdItemId = Request.Params["roleIdItemId"];
            var role = new RoleDTO
            {
                Id = int.Parse(roleIdItemId)
            };
            //如果有密码改动首先判断旧密码
            var modifiPass = Request.Params["modifiPass"];
            var id = int.Parse(Request.Params["Id"]);
            string msg = string.Empty;

            var user = new UserDTO
            {
                Id = id,
                Name = Request.Params["Name"],
                DepartmentId = GlobalMethod.CoventToInt(Request.Params["DepartmentId"]),
                LoginName = Request.Params["LoginName"],
                Code = Request.Params["Code"],
                Remark = Request.Params["Remark"],
                Position = Request.Params["Position"],
                Status = (UserStatus)GlobalMethod.CoventToIntNotNull(Request.Params["Status"]),
                RoleDtos = new List<RoleDTO>{ role }
            };
            //如果要修改密码
            if (modifiPass != null)
            {
                //oldPassword
                var oldPassword = Request.Params["oldpassHd"];
                if (!_userService.CheckUserPassword(id, oldPassword, out msg))
                {
                    var result1 = new DataControlResult<UserDTO>();
                    result1.code = MyErrorCode.ResDBError;
                    result1.msg = msg;
                    result1.success = false;
                    //写入日志
                    GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result1.success, "编辑用户"
                        , result1.success == true ? "编辑用户:" + result1.ResultOutDto.Id.ToString() : "编辑用户失败:" + result1.msg);
                    return Json(result1);
                }
                //检验通过
                user.Password = Request.Params["newpassHd"];
            }

            var result = _userService.Update(user);

            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "编辑用户", result.success == true ? result.ResultOutDto.Id.ToString() : result.msg);
            return Json(result);
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult DelUser()
        {
            var userList = new List<UserDTO>();
            var idList = Request.Params["idList"].Split(',');
            foreach (var id in idList)
            {
                userList.Add(new UserDTO
                {
                    Id = int.Parse(id)
                });
            }
            var result = _userService.Delete(userList);
            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success,
                "删除用户", result.success == true ? "删除用户:" + Request.Params["idList"] : "删除用户失败:" + result.msg);
            return Json(result);
        }


        //
        // GET: /User/
        public ActionResult UserLogin()
        {
            return View("UserLogin");
        }
        /// <summary>
        /// 检验密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckUserPassword()
        {
            var userName = Request.Params["myusername"];
            var password = Request.Params["password"];
            var result = _userService.CheckUserLogin(new UserDTO
                {
                    LoginName = userName,
                    Password = password
                });
            if (result.success)
            {
                //创建sessiontoken
                Guid guid = Guid.NewGuid();
                result.msg = guid.ToString();
                Session["sessiontoken"] = guid.ToString();
                Session["loginuserInfo"] = result.ResultOutDto;
            }
            else
            {
                result.msg = "";
                Session["sessiontoken"] = null;
                Session["loginuserInfo"] = null;
            }
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "用户登录",
                result.success == true ? "登录成功" : "登录失败:" + result.msg);
            return Json(result);
        }

        /// <summary>
        /// 获取当前用户的角色权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCurrUserInfo()
        {
            var user = Session["loginuserInfo"] as UserDTO;
            var result = _userService.GetUserById(user.Id);
            Session["loginuserInfo"] = result.ResultOutDto;
            result.success = true;
            return Json(result);
        }
        /// <summary>
        /// 用户退出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserLoginOut()
        {
            var result = new DataControlResult<UserDTO>();

            result.ResultOutDto = null;
            result.code = MyErrorCode.ResOK;
            result.msg = string.Empty;
            result.success = true;
            Session["sessiontoken"] = null;
            Session["loginuserInfo"] = null;

            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "用户退出", result.msg);

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
            if (str == "DepartmentEntity")
            {
                return "DepartmentId";
            }
            else if (str == "StatusString")
            {
                return "Status";
            }
            return sortStr;
        }
    }
}
