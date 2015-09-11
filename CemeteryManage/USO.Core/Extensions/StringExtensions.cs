
using System.Diagnostics;
using MvcExtensions;

namespace USO.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Text;

    public static class StringExtensions
    {
        public static string NullifyEmpty(this string stringToCheck)
        {
            return string.IsNullOrWhiteSpace(stringToCheck) ? null : stringToCheck;
        }

        public static string NullToEmpty(this string instance)
        {
            if (instance == null)
                return string.Empty;

            return instance;
        }

        public static string AppendWithForwardSlash(this string front, string back)
        {
            if (front == null)
            {
                throw new ArgumentNullException("front");
            }
            if (back == null)
            {
                throw new ArgumentNullException("back");
            }
            return front.EndsWith("/") || back.StartsWith("/")
                ? front + back
                : string.Join("/", front, back);
        }

        public static string ToHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }

        public static byte[] ToByteArray(this string hex)
        {
            return Enumerable.Range(0, hex.Length).
                Where(x => 0 == x % 2).
                Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).
                ToArray();
        }

        public static bool IsSlugValid(this string slug)
        {
            return String.IsNullOrWhiteSpace(slug) || Regex.IsMatch(slug, @"^[^:?#\[\]@!$&'()*+,;=\s\string.Empty\<\>]+$") && !(slug.StartsWith(".") || slug.EndsWith("."));
        }

        public static string AsSlug(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var disallowed = new Regex(@"[/:?#\[\]@!$&'()*+,;=\s\string.Empty\<\>]+");

            var slug = disallowed.Replace(text, "-").Trim('-');

            if (slug.Length > 1000)
                slug = slug.Substring(0, 1000);

            // dots are not allowed at the begin and the end of routes
            slug = RemoveDiacritics(slug.Trim('.').ToLower());

            return slug;
        }

        public static string RemoveDiacritics(this string slug)
        {
            string stFormD = slug.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public static string SafeTrim(this string instant)
        {
            if (instant == null)
                return null;

            return instant.Trim();
        }

        /// <summary>
        /// 过滤掉特殊字符
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string RemoveSpecials(this string target)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(target))
            {
                target.Each(a =>
                {
                    if (Char.IsLetterOrDigit(a))//过滤掉特殊字符
                    {
                        sb.Append(a);
                    }
                });
            }
            return sb.ToString();
        }
    }
   

    class Hash998
    {
        private long _hash;

        public short Value { get { return (short)((_hash < 0 ? -_hash : _hash) % 998); } }

        public void AddString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;
            _hash += GetHashCode(value.ToCharArray());
        }

        const int INITIAL_HASH = 0;
        const int MULTIPLIER = 31;
        public static int GetHashCode(char[] array)
        {
            if (array == null)
            {
                return 0;
            }
            int hash = INITIAL_HASH;
            int arraySize = array.Length;
            for (int i = 0; i < arraySize; i++)
            {
                hash = MULTIPLIER * hash + array[i];
            }
            return hash;

        }
    }
}
