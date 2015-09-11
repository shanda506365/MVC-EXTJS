using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using USO.Domain;
using USO.Dto;
using USO.Infrastructure.Services;

namespace USO.Store.Security
{
    public static class GlobalMethod
    {
        public static string CoventToString(string str)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : str;
        }

        public static int?  CoventToInt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            return int.Parse(str);
        }
        public static int CoventToIntNotNull(string str)
        {
            if (string.IsNullOrEmpty(str)) return 0;
            return int.Parse(str);
        }

        public static float?  CoventToFloat(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            return float.Parse(str);
        }

        public static DateTime?  CoventToDatetime(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            if (str.Contains("T"))
            {
                return DateTime.Parse(str.Replace("T"," "));
            }
            return DateTime.Parse(str);
        }
        /// <summary>
        /// 简要 普通日志写入
        /// </summary>
        /// <param name="session"></param>
        /// <param name="sysLogService"></param>
        /// <param name="type"></param>
        /// <param name="success"></param>
        /// <param name="contorlName"></param>
        /// <param name="content"></param>
        public static void WriteLog(HttpSessionStateBase session, ISysLogService sysLogService,LogType type,
            bool success, string contorlName, string content)
        {
            //写入日志
            var curUser = session["loginuserInfo"] as UserDTO;
            if (curUser != null)
            {
                sysLogService.Create(new SysLogDTO
                {
                    Type = success==true?type:LogType.Error,
                    UserId = curUser.Id,
                    ControlName = contorlName,
                    Content = "用户(" + curUser.Id+")"+curUser.Name+"--"+content,
                    Date = DateTime.Now
                });
            }
        }
        /// <summary>
        /// 详细 业务的日志写入
        /// </summary>
        /// <param name="session"></param>
        /// <param name="sysLogService"></param>
        /// <param name="type"></param>
        /// <param name="success"></param>
        /// <param name="contorlName"></param>
        /// <param name="content"></param>
        /// <param name="dto"></param>
        public static void WriteLog(HttpSessionStateBase session, ISysLogService sysLogService, LogType type,
           bool success, string contorlName, string content,SysLogDTO dto)
        {
            //写入日志
            var curUser = session["loginuserInfo"] as UserDTO;
            if (curUser != null)
            {
                sysLogService.Create(new SysLogDTO
                {
                    Type = success == true ? type : LogType.Error,
                    UserId = curUser.Id,
                    ControlName = contorlName,
                    Content = "用户(" + curUser.Id + ")" + curUser.Name + "--" + content,
                    Date = DateTime.Now,
                    Applicanter = dto.Applicanter,
                    Telephone = dto.Telephone,
                    IDNumber = dto.IDNumber,
                    Money = dto.Money,
                    ControllTid = dto.ControllTid,
                    ControllIds = dto.ControllIds,
                    BuryMan = dto.BuryMan,
                    BuryDate = dto.BuryDate,
                    Remark = dto.Remark,
                    Remark2 = dto.Remark2
                });
            }
        }
    }
  
}