using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Domain
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 系统日志
        /// </summary>
        System = 1,
        /// <summary>
        /// 操作日志
        /// </summary>
        Control = 2,
        /// <summary>
        /// 错误日志
        /// </summary>
        Error = 3,
        /// <summary>
        /// 警告日志
        /// </summary>
        Alert = 4,
        /// <summary>
        /// 业务操作日志
        /// </summary>
        JobManage = 5

    }
    /// <summary>
    /// 日志类型扩展
    /// </summary>
    public static class LogTypeExtentions
    {
        public static string DescriptionFor(this LogType userStatus)
        {
            switch (userStatus)
            {
                default:
                case LogType.System:
                    return "系统日志";
                case LogType.Control:
                    return "操作日志";
                case LogType.Error:
                    return "错误日志";
                case LogType.Alert:
                    return "警告日志";
                case LogType.JobManage:
                    return "业务操作";
            }
        }
    }

    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// 无效
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// 有效
        /// </summary>
        Valid = 1

    }
    /// <summary>
    /// 用户状态扩展
    /// </summary>
    public static class UserStatusExtentions
    {
        public static string DescriptionFor(this UserStatus? userStatus)
        {
            switch (userStatus)
            {
                default:
                case UserStatus.Invalid:
                    return "无效";
                case UserStatus.Valid:
                    return "有效";

            }
        }
    }
}
