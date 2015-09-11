using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Core.Helper
{
    public class ConstantHelper
    {
        /// <summary>
        /// WebService用户名
        /// </summary>
        public static string WSUserName = ConfigurationManager.AppSettings["R3OrderInterfaceName"];

        /// <summary>
        /// WebService密码
        /// </summary>
        public static string WSPassword = ConfigurationManager.AppSettings["R3OrderInterfacePwd"];

        public static string GetAppSettingValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
