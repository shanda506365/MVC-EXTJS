using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace USO.Core.Helper
{
    public class RegexHelper
    {
        #region 验证输入的字符串是否合法
        /// <summary>
        /// 验证输入的字符串是否合法，合法返回true,否则返回false。
        /// </summary>
        /// <param name="strInput">输入的字符串</param>
        /// <param name="strPattern">模式字符串</param>
        public static bool Validate(string strInput, string strPattern)
        {
            if (string.IsNullOrEmpty(strInput) || string.IsNullOrEmpty(strPattern)) return false;

            return Regex.IsMatch(strInput, strPattern);
        }
        #endregion

        #region 过滤正则表达式所获取的内容
        /// <summary>
        /// 过滤正则表达式所获取的内容
        /// </summary>
        /// <param name="strInput">输入的字符串</param>
        /// <param name="strPattern">模式字符串</param>
        /// <returns></returns>
        public static string Replace(string strInput, string strPattern)
        {
            if (string.IsNullOrEmpty(strInput) || string.IsNullOrEmpty(strPattern)) return strInput;

            Regex reg = new Regex(strPattern);
            foreach (Match match in reg.Matches(strInput))
            {
                strInput = strInput.Replace(match.Value, string.Empty);
            }
            return strInput;
        }
        #endregion

    }
}
