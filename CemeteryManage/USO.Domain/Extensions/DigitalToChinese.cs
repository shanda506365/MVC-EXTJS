
namespace USO.Domain.Extensions
{
    using System;

    /// <summary>
    /// DigitalToChinese :实现将人民币的数字形式转换为中文格式。
    /// </summary>
    public static class DigitalToChinese
    {
        public static string ToChineseString(this decimal digital)
        {
            if (digital == 0.00m)
                return "零元整";

            var buf = string.Empty;                        /**//* 存放返回结果 */
            var strDecPart = string.Empty;                    /**//* 存放小数部分的处理结果 */
            var strIntPart = string.Empty;                    /**//* 存放整数部分的处理结果 */

            /**/
            /* 将数据分为整数和小数部分 */
            char[] cDelim = { '.' };
            string[] tmp = null;
            var strDigital = digital.ToString();
            if (strDigital[0] == '.')
            {                /**//* 处理数据首位为小数点的情况 */
                strDigital = "0" + strDigital;
            }
            tmp = strDigital.Split(cDelim, 2);

            /**/
            /* 整数部分的处理 */
            if (tmp[0].Length > 15)
            {
                throw new Exception("数值超出处理范围。");
            }
            var flag = true;        /**//* 标示整数部分是否为零 */
            if (digital >= 1.00m)
            {
                strIntPart = ConvertInt(tmp[0]);
                flag = false;
            }

            /**/
            /* 存在小数部分，则处理小数部分 */
            if (tmp.Length == 2)
                strDecPart = ConvertDecimal(tmp[1], flag);
            else
                strDecPart = "整";

            buf = strIntPart + strDecPart;
            return buf;
        }

        private static string ConvertInt(string intPart)
        {
            var buf = string.Empty;
            var length = intPart.Length;
            var curUnit = length;

            /**/
            /* 处理除个位以上的数据 */
            var tmpValue = string.Empty;                    /**//* 记录当前数值的中文形式 */
            var tmpUnit = string.Empty;                    /**//* 记录当前数值对应的中文单位 */
            int i;
            for (i = 0; i < length - 1; i++, curUnit--)
            {
                if (intPart[i] != '0')
                {
                    tmpValue = DigToCC(intPart[i]);
                    tmpUnit = GetUnit(curUnit - 1);
                }
                else
                {
                    /**/
                    /* 如果当前的单位是"万、亿"，则需要把它记录下来 */
                    if ((curUnit - 1) % 4 == 0)
                    {
                        tmpValue = string.Empty;
                        tmpUnit = GetUnit(curUnit - 1);
                    }
                    else
                    {
                        tmpUnit = string.Empty;
                        /**/
                        /* 如果当前位是零，则需要判断它的下一位是否为零，再确定是否记录'零' */
                        if (intPart[i + 1] != '0')
                        {
                            tmpValue = "零";
                        }
                        else
                        {
                            tmpValue = string.Empty;
                        }
                    }
                }
                buf += tmpValue + tmpUnit;
            }

            /**/
            /* 处理个位数据 */
            if (intPart[i] != '0')
                buf += DigToCC(intPart[i]);
            buf += "元";

            return buf;
        }


        /**/
        /// <summary>
        /// 小数部分的处理
        /// </summary>
        /// <param name="decPart">需要处理的小数部分</param>
        /// <param name="flag">标示整数部分是否为零</param>
        /// <returns></returns>
        private static string ConvertDecimal(string decPart, bool flag)
        {
            var buf = string.Empty;
            if ((decPart[0] == '0') && (decPart.Length > 1))
            {
                if (flag == false)
                {
                    buf += "零";
                }
                if (decPart[1] != '0')
                {
                    buf += DigToCC(decPart[1]) + "分";
                }
            }
            else
            {
                buf += DigToCC(decPart[0]) + "角";
                if ((decPart.Length > 1) && (decPart[1] != '0'))
                {
                    buf += DigToCC(decPart[1]) + "分";
                }
            }
            buf += "整";
            return buf;
        }


        /**/
        /// <summary>
        /// 获取人民币中文形式的对应位置的单位标志
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static string GetUnit(int n)
        {
            switch (n)
            {
                case 1:
                    return "拾";
                case 2:
                    return "佰";
                case 3:
                    return "仟";
                case 4:
                    return "万";
                case 5:
                    return "拾";
                case 6:
                    return "佰";
                case 7:
                    return "仟";
                case 8:
                    return "亿";
                case 9:
                    return "拾";
                case 10:
                    return "佰";
                case 11:
                    return "仟";
                case 12:
                    return "万";
                case 13:
                    return "拾";
                case 14:
                    return "佰";
                case 15:
                    return "仟";
                default:
                    return " ";
            }
        }



        /**/
        /// <summary>
        /// 数字转换为相应的中文字符 ( Digital To Chinese Char )
        /// </summary>
        /// <param name="c">以字符形式存储的数字</param>
        /// <returns></returns>
        private static string DigToCC(char c)
        {
            switch (c)
            {
                case '1':
                    return "壹";
                case '2':
                    return "贰";
                case '3':
                    return "叁";
                case '4':
                    return "肆";
                case '5':
                    return "伍";
                case '6':
                    return "陆";
                case '7':
                    return "柒";
                case '8':
                    return "捌";
                case '9':
                    return "玖";
                case '0':
                    return "零";
                default:
                    return "  ";
            }
        }
    }
}
