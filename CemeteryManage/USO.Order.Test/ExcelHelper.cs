using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Web;

namespace USO.Order.Test
{
    public class ExcelHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static MemoryStream DataToExcel(DataTable data)
        {
            MemoryStream ms = new MemoryStream();
                using (data)
                {
                    IWorkbook workbook = new HSSFWorkbook();
                    ISheet sheet = workbook.CreateSheet();
                    IRow headerRow = sheet.CreateRow(0);

                    // 标题
                    foreach (DataColumn column in data.Columns)
                        headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);

                    // 绑定数据
                    int rowIndex = 1;

                    foreach (DataRow row in data.Rows)
                    {
                        IRow dataRow = sheet.CreateRow(rowIndex);

                        foreach (DataColumn column in data.Columns)
                        {
                            dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                        }

                        rowIndex++;
                    }

                    workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                }
            return ms;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="context"></param>
        /// <param name="fileName"></param>
        public static void MSToBrowser(MemoryStream ms, HttpContext context, string fileName)
        {
            if (context.Request.Browser.Browser == "IE")
                fileName = HttpUtility.UrlEncode(fileName);
            context.Response.AddHeader("Content-Disposition", "attachment;fileName=" + fileName);
            context.Response.BinaryWrite(ms.ToArray());
        }
    }
}
