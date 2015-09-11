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
    public class JobManageController : Controller
    {
        private readonly ITombstoneService _tombstoneService;
        private readonly ISysLogService _sysLogService;

        public JobManageController(ITombstoneService tombstoneService, ISysLogService sysLogService)
        {
            _tombstoneService = tombstoneService;
            _sysLogService = sysLogService;
        }


        /// <summary>
        /// 预订墓碑
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult OrderTombstone()
        {
            //墓碑Id
            var id = int.Parse(Request.Params["Id"]);
            DateTime? lastPaymentDate = null;
            if (Request.Params["LastPaymentDate"] != null)
            {
                lastPaymentDate = DateTime.Parse(Request.Params["LastPaymentDate"]);
            }

            //申请人
            var applicanter = Request.Params["Applicanter"];
            var telephone = Request.Params["Telephone"];
            var iDNumber = Request.Params["IDNumber"];
            var remark = Request.Params["Remark"];
            decimal? money = null;
            if (Request.Params["Money"] != null && Request.Params["Money"] != "")
            {
                money = decimal.Parse(Request.Params["Money"]);
            }

            var isFullMoney = Request.Params["IsFullMoney"];
            var manageLimit = Request.Params["ManageLimit"];
            var supperManage = Request.Params["SupperManage"];

            var dto = new SysLogDTO
                {
                    Applicanter = applicanter,
                    Telephone = telephone,
                    IDNumber = iDNumber,
                    Money = money,
                    ControllTid = id,
                    Remark = remark
                };

            DataControlResult<TombstoneDTO> result;
            if (isFullMoney != null)
            {
                result = _tombstoneService.SaleTombstone(id, DateTime.Now);
                //result = _tombstoneService.SaleTombstone(id, DateTime.Now, int.Parse(manageLimit), supperManage != null ? true : false);
                GlobalMethod.WriteLog(Session, _sysLogService, LogType.JobManage, result.success, "预订墓碑(全款)"
                   , result.success == true ? "预订墓碑:" + id : "预订墓碑失败:" + result.msg, dto);
            }
            else
            {
                result = _tombstoneService.OrderTombstone(id, (DateTime)lastPaymentDate);
                GlobalMethod.WriteLog(Session, _sysLogService, LogType.JobManage, result.success, "预订墓碑"
               , result.success == true ? "预订墓碑:" + id : "预订墓碑失败:" + result.msg, dto);
            }



            return Json(result);
        }


        /// <summary>
        /// 查询已订墓碑相关业务信息
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult GetOrderJobInfoTombstone()
        {
            //墓碑Id
            var id = int.Parse(Request.Params["Id"]);
            var controllName = Request.Params["controllName"];
            var sysLogs = _sysLogService.GetJobInfoTombstone(id, controllName);
            var result = new DataControlResult<SysLogDTO>();
            if (sysLogs.Result.Any())
            {
                result.ResultOutDto = sysLogs.Result.FirstOrDefault();
                result.success = true;
                result.msg = "";
            }
            else
            {
                result.ResultOutDto = null;
                result.success = false;
                result.msg = "无此墓碑的操作信息记录";
            }
            return Json(result);
        }





        /// <summary>
        /// 落葬墓碑
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult BuryTombstone()
        {
            //墓碑Id
            var id = int.Parse(Request.Params["Id"]);

            //var again = Request.Params["again"];//再次落葬

            DateTime? buryDate = null;
            if (Request.Params["BuryDate"] != null)
            {
                buryDate = DateTime.Parse(Request.Params["BuryDate"]);
            }
            //是否首次落葬
            var firstBury = Request.Params["FirstBury"] != null;

            //申请人 落葬人
            var applicanter = Request.Params["Applicanter"];
            var buryMan = Request.Params["BuryMan"];
            var telephone = Request.Params["Telephone"];
            var iDNumber = Request.Params["IDNumber"];
            var manageLimit = Request.Params["ManageLimit"];
            var supperManage = Request.Params["SupperManage"];
            var remark = Request.Params["Remark"];
            var remark2 = Request.Params["Remark2"];
            var dto = new SysLogDTO
            {
                Applicanter = applicanter,
                Telephone = telephone,
                IDNumber = iDNumber,
                ControllTid = id,
                BuryMan = buryMan,
                BuryDate = buryDate,
                Remark = remark,
                Remark2 = remark2
            };

            var result = _tombstoneService.BuryTombstone(id, DateTime.Now, int.Parse(manageLimit), supperManage != null ? true : false);
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.JobManage, result.success, "落葬墓碑"
               , result.success == true ? "落葬墓碑:" + id : "落葬墓碑失败:" + result.msg, dto);
            if (firstBury && result.success)//调用更新到期日期
            {
                var tombDto = new TombstoneDTO
                    {
                        Id = id,
                        ExpiryDate = ((DateTime)buryDate).AddYears(int.Parse(manageLimit))
                    };
                _tombstoneService.UpdateExpiryDate(tombDto);
            }

            return Json(result);
        }

        /// <summary>
        /// 落葬墓碑(编辑日志行项目)
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult UpdateBuryMan()
        {
            //墓碑Id
            var id = int.Parse(Request.Params["Id"]);
            //记录id 
            var logId = int.Parse(Request.Params["logId"]);

            //var again = Request.Params["again"];//再次落葬

            DateTime? buryDate = null;
            if (Request.Params["BuryDate"] != null)
            {
                buryDate = DateTime.Parse(Request.Params["BuryDate"]);
            }

            //申请人 落葬人
            var applicanter = Request.Params["Applicanter"];
            var buryMan = Request.Params["BuryMan"];
            var telephone = Request.Params["Telephone"];
            var iDNumber = Request.Params["IDNumber"];
            var manageLimit = Request.Params["ManageLimit"];
            var supperManage = Request.Params["SupperManage"];
            var remark = Request.Params["Remark"];

            var dto = new SysLogDTO
            {
                Id = logId,
                Applicanter = applicanter,
                Telephone = telephone,
                IDNumber = iDNumber,
                BuryMan = buryMan,
                BuryDate = buryDate,
                Remark = remark
            };
            var result = _tombstoneService.BuryTombstone(id, buryDate ?? DateTime.Now, int.Parse(manageLimit), supperManage != null ? true : false);
            if (!result.success)
            {
                return Json(result);
            }
            var result1 = _sysLogService.Update(dto);

            //GlobalMethod.WriteLog(Session, _sysLogService, LogType.JobManage, result.success, "落葬墓碑"
            //   , result.success == true ? "落葬墓碑:" + id : "落葬墓碑失败:" + result.msg, dto);


            return Json(result1);
        }
        /// <summary>
        /// 查询 墓碑相关业务信息 日志
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult GetTombstoneJobInfoLog()
        {
            //墓碑Id
            var id = int.Parse(Request.Params["Id"]);
            var controllName = Request.Params["controllName"];
            var sysLogs = _sysLogService.GetJobInfoTombstone(id, controllName);
            var result = new DataControlResult<SysLogDTO>();
            if (sysLogs.Result.Any())
            {
                result.ResultOutDtos = sysLogs.Result.ToList();
                result.success = true;
                result.msg = "";
            }
            else
            {
                result.ResultOutDtos = new List<SysLogDTO>();
                result.success = false;
                result.msg = "无此墓碑的操作信息记录";
            }
            return Json(result);
        }

        //EditApplicanter
        /// <summary>
        /// 落葬后的修改申请人
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult EditApplicanter()
        {
            //墓碑Id
            var id = int.Parse(Request.Params["Id"]);

            //申请人 落葬人
            var applicanter = Request.Params["Applicanter"];
            var telephone = Request.Params["Telephone"];
            var iDNumber = Request.Params["IDNumber"];
            var remark = Request.Params["Remark"];
            var dto = new SysLogDTO
            {
                Applicanter = applicanter,
                Telephone = telephone,
                IDNumber = iDNumber,
                ControllTid = id,
                Remark = remark
            };
            var result = new DataControlResult<SysLogDTO>();
            try
            {
                GlobalMethod.WriteLog(Session, _sysLogService, LogType.JobManage, true, "修改申请人"
              , "修改申请人墓碑编号:" + id, dto);
                result.ResultOutDtos = new List<SysLogDTO>();
                result.success = true;
                result.msg = "";
            }
            catch (Exception ex)
            {
                result.ResultOutDtos = new List<SysLogDTO>();
                result.success = false;
                result.msg = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// 续交管理费
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult RenewManageLimit()
        {
            //墓碑Id
            var id = int.Parse(Request.Params["Id"]);

            //续交年限
            var manageLimit = int.Parse(Request.Params["ManageLimit"]);
            var remark = Request.Params["remark"];

            var result = new DataControlResult<SysLogDTO>();
            try
            {
                var tomb = new TombstoneDTO
                    {
                        Id = id,
                        ManageLimit = manageLimit
                    };
                var tombResult = _tombstoneService.RenewManageLimit(tomb);
                var dto = new SysLogDTO
                {
                    ControllTid = id,
                    Remark = remark
                };
                GlobalMethod.WriteLog(Session, _sysLogService, LogType.JobManage, true, "续交管理费"
             , "续交管理费墓碑编号:" + id + ";续交年限：" + manageLimit + ";本次续交后到期日期：" + tombResult.ResultOutDto.ExpiryDateShortString, dto);

                var maxManageDate = tombResult.ResultOutDto.ExpiryDateShortString;
                var starDate = ((DateTime)tombResult.ResultOutDto.ExpiryDate).AddYears(-(int)tombResult.ResultOutDto.ManageLimit).ToString("yyyy-MM-dd");
                var msg = starDate + "至" + maxManageDate;
                result.ResultOutDtos = new List<SysLogDTO>();
                result.success = true;
                result.msg = msg + ";" + tombResult.ResultOutDto.ManageLimit.ToString();
            }
            catch (Exception ex)
            {
                result.ResultOutDtos = new List<SysLogDTO>();
                result.success = false;
                result.msg = ex.Message;
            }

            return Json(result);
        }


        /// <summary>
        /// 查询 墓碑相关续交历史记录
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult GetTombstoneRenewManangeLog()
        {
            //墓碑Id
            var id = int.Parse(Request.Params["Id"]);
            var sysLogs = _sysLogService.GetRenewManageTombstone(id);
            //var result = new DataControlResult<SysLogDTO>();
            //if (sysLogs.Result.Any())
            //{
            //    result.ResultOutDtos = sysLogs.Result.ToList();
            //    result.success = true;
            //    result.msg = "";
            //}
            //else
            //{
            //    result.ResultOutDtos = new List<SysLogDTO>();
            //    result.success = false;
            //    result.msg = "无此墓碑的操作信息记录";
            //}
            //return Json(result);
            var gsbModel = new GridStoreBaseModel<SysLogDTO>
            {
                success = true,
                msg = "成功",
                dataset = sysLogs.Result.ToList(),
                total = sysLogs.Total
            };
            return Json(gsbModel);
            
        }


    }
}
