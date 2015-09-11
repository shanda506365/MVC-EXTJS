using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace USO.Domain.Extensions
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        private static readonly Regex EmailExpression = new Regex(@"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex WebUrlExpression = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex StripHTMLExpression = new Regex("<\\S[^><]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        //汉字字母验证
        private static readonly Regex CharacterExpression = new Regex("^([A-Za-z]|[\u4E00-\u9FA5])+$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        //手机固话验证
        private static readonly Regex PhoneNumberExpression = new Regex(@"(\(\d{3,4}\)|\d{3,4}-|\s)?\d{7,14}", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        //数字字母验证
        private static readonly Regex LetterExpression = new Regex(@"[A-Za-z0-9]+$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        //身份证验证
        private static readonly Regex IDCardExpression = new Regex(@"(^\d{15}$)|(^\d{17}([0-9]|X)$)", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        //数字汉字验证
        private static readonly Regex NumCharacterExpression = new Regex(@"^[0-9\u4e00-\u9fa5]+$", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        //汉字验证
        private static readonly Regex ChineseCharacterExpression = new Regex(@"[\u4e00-\u9fa5]*", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
         

        [DebuggerStepThrough]
        public static bool TextEquals(this string instance, string arg)
        {
            if (!string.Equals((instance ?? string.Empty).Trim(), (arg ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        [DebuggerStepThrough]
        public static string FormatWith(this string instance, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(instance))
                throw new ArgumentNullException("instance");

            return string.Format(CultureInfo.CurrentCulture, instance, args);
        }

        [DebuggerStepThrough]
        public static string Hash(this string instance)
        {
            if (string.IsNullOrWhiteSpace(instance))
                throw new ArgumentNullException("instance");

            using (var md5 = MD5.Create())
            {
                var data = Encoding.Unicode.GetBytes(instance);
                var hash = md5.ComputeHash(data);

                return Convert.ToBase64String(hash);
            }
        }

        [DebuggerStepThrough]
        public static T ToEnum<T>(this string instance, T defaultValue) where T : struct, IComparable, IFormattable
        {
            T convertedValue = defaultValue;

            if (!string.IsNullOrWhiteSpace(instance) && !Enum.TryParse(instance.Trim(), true, out convertedValue))
            {
                convertedValue = defaultValue;
            }

            return convertedValue;
        }

        [DebuggerStepThrough]
        public static bool IsIDCard(this string instance)
        {
            return !string.IsNullOrWhiteSpace(instance) && IDCardExpression.IsMatch(instance);
        }

        [DebuggerStepThrough]
        public static bool IsCharacter(this string instance)
        {
            return !string.IsNullOrWhiteSpace(instance) && CharacterExpression.IsMatch(instance);
        }

        [DebuggerStepThrough]
        public static bool IsChineseCharacter(this string instance)
        {
            return !string.IsNullOrWhiteSpace(instance) && ChineseCharacterExpression.IsMatch(instance);
        }

        [DebuggerStepThrough]
        public static bool IsPhoneNumber(this string instance)
        {
            return !string.IsNullOrWhiteSpace(instance) && PhoneNumberExpression.IsMatch(instance);
        }
        
        [DebuggerStepThrough]
        public static bool IsLetter(this string instance)
        {
            return !string.IsNullOrWhiteSpace(instance) && LetterExpression.IsMatch(instance);
        }

        [DebuggerStepThrough]
        public static bool NumCharacter(this string instance)
        {
            return !string.IsNullOrWhiteSpace(instance) && NumCharacterExpression.IsMatch(instance);
        }


        [DebuggerStepThrough]
        public static string StripHtml(this string instance)
        {
            if (string.IsNullOrWhiteSpace(instance))
                throw new ArgumentNullException("instance");

            return StripHTMLExpression.Replace(instance, string.Empty);
        }

        [DebuggerStepThrough]
        public static bool IsEmail(this string instance)
        {
            return !string.IsNullOrWhiteSpace(instance) && EmailExpression.IsMatch(instance);
        }

        [DebuggerStepThrough]
        public static bool IsWebUrl(this string instance)
        {
            return !string.IsNullOrWhiteSpace(instance) && WebUrlExpression.IsMatch(instance);
        }

        [DebuggerStepThrough]
        public static bool IsIPAddress(this string instance)
        {
            IPAddress ip;

            return !string.IsNullOrWhiteSpace(instance) && IPAddress.TryParse(instance, out ip);
        }

        public static string SetErrorCell(this string cellContent)
        {
            var errorContent = string.Format(@"<span class='t-error'>{0}</span>", cellContent);
            return errorContent;
        }
        public static string ConvertNullToEmpty(this string instance)
        {
            if (instance == null)
                return string.Empty;

            return instance;
        }

        public static string ClearPartNumber(this string partNumber)
        {
            if (string.IsNullOrEmpty(partNumber))
                return string.Empty;

            var sb = new StringBuilder();

            partNumber = partNumber.ToLower();
            foreach (var t in partNumber)
            {
                if (Char.IsLetterOrDigit(t))
                    sb.Append(t);
            }

            return sb.ToString();
        }

        public static int ToInt(this string integerString,int defaultValue)
        {
            int integer;
            if (int.TryParse(integerString, out integer))
            {
                return integer;
            }
            return defaultValue;
        }

        public static double ToDouble(this string doubleString, int defaultValue)
       {
           double doubleger;
           if (double.TryParse(doubleString, out doubleger))
           {
               return doubleger;
           }
           return defaultValue;
        }


        public static bool ToBoolean(this string str, bool defaultValue)
        {
            bool result;
            if (bool.TryParse(str, out result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// 默认使用','分给的Id串，如2,3,5,7,11,13
        /// </summary>
        /// <param name="listString"></param>
        /// <returns></returns>
        public static List<int> ToIdList(this string listString)
        {
            return listString.ToIdList(',');
        }
        
        public static List<int> ToIdList(this string listString, char splitter)
        {
            var list = new List<int>();
            if (string.IsNullOrEmpty(listString))
            {
                return list;
            }
            var listArray = listString.Split(splitter);
            foreach (var item in listArray)
            {
                var id = item.ToInt(0);
                if (id > 0)
                {
                    list.Add(id);
                }
            }
            return list;
        }

        public static string GetEnumDisplayName(this object enumItem)
        {
            var displayName = enumItem.ToString();
            var type = enumItem.GetType();
            var member = type.GetMember(displayName);
            if (member.Length==0)
            {
                return displayName;
            }
            var displayAttributes = member[0].GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
            if (displayAttributes != null && displayAttributes.Length > 0)
            {
                displayName = displayAttributes[0].GetName();
            }
            else
            {
                var displayNameAttributes = member[0].GetCustomAttributes(typeof(DisplayNameAttribute), false) as DisplayNameAttribute[];
                if (displayNameAttributes != null && displayNameAttributes.Length > 0)
                {
                    displayName = displayNameAttributes[0].DisplayName;
                }
            }

            return displayName;
        }
    }
}