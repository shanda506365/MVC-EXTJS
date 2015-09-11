using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Core.Helper
{
    public static class DateHelper
    {
        /// <summary>
        /// eg:2013年05月02日 14:06
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToLongTime(DateTime date)
        {
            DateTime returnDate;
            try
            {
                //returnDate = Convert.ToDateTime(date.GetDateTimeFormats('f')[0]);
                returnDate = date;
            }
            catch { returnDate = date; }

            return date;
        }
    }
}
