using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Xml;
using System;
using NPOI.SS.Formula.Functions;
using NPOI.HPSF;
using System.Threading;
using NPOI.HSSF.Util;

namespace USO.Mvc.Utility
{
    public class SheetInfoClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Excel导入和导出
    /// </summary>
    public static class ImportXlsToDataTable
    {
        /// <summary>
        /// Excel导入到DataTable中
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable(string fileName)
        {
            ISheet sheet;
            var sheetType = "XSSFWorkbook";
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                //读取2007
                if (fileName.Contains(".xlsx"))
                {
                    var xssfworkbook = new XSSFWorkbook(file);
                    sheet = xssfworkbook.GetSheetAt(0);
                    sheetType = "XSSFWorkbook";
                }
                else
                {
                    var hssfworkbook = new HSSFWorkbook(file);
                    sheet = hssfworkbook.GetSheetAt(0);
                    sheetType = "HSSFWorkbook";
                }
            }
            DataTable table = new DataTable();
            var headerRow = sheet.GetRow(0);
            var cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
            var rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

            //handling header.
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                var column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
            {
                var row = sheet.GetRow(i);
                var dataRow = table.NewRow();

                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = GetCellValue(row.GetCell(j), sheetType);
                    }
                }
                table.Rows.Add(dataRow);
            }
            if (table.Columns.Contains("R3物料代码"))
            {
                table.DefaultView.RowFilter = " R3物料代码 ''";
            }
            return table;
        }

        public static DataTable ConvertToDataTables(HttpPostedFileBase httpPostedFileBase)
        {
            ISheet sheet;
            string fileName = httpPostedFileBase.FileName;
            using (Stream file = httpPostedFileBase.InputStream)
            {
                //读取2007
                if (fileName.Contains(".xlsx"))
                {
                    var xssfworkbook = new XSSFWorkbook(file);
                    sheet = xssfworkbook.GetSheetAt(0);
                }
                else
                {
                    var hssfworkbook = new HSSFWorkbook(file);
                    sheet = hssfworkbook.GetSheetAt(0);
                }
            }
            DataTable table = new DataTable();
            var headerRow = sheet.GetRow(0);
            var cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
            var rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

            //handling header.
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                var column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
            {
                var row = sheet.GetRow(i);
                var dataRow = table.NewRow();

                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = GetCellValue(row.GetCell(j));
                    }
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }


        public static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.BLANK:
                    return string.Empty;
                case CellType.BOOLEAN:
                    return cell.BooleanCellValue.ToString();
                case CellType.ERROR:
                    return cell.ErrorCellValue.ToString();
                case CellType.NUMERIC:
                case CellType.Unknown:
                default:
                    return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.STRING:
                    return cell.StringCellValue;
                case CellType.FORMULA:
                    try
                    {
                        var e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }


        public static List<SheetInfoClass> GetExcelSheetStruct(HttpPostedFileBase httpPostedFileBase)
        {
            ISheet sheet;
            string fileName = httpPostedFileBase.FileName;
            var sheetInfoClassList = new List<SheetInfoClass>();
            using (Stream file = httpPostedFileBase.InputStream)
            {
                //读取2007
                if (fileName.Contains(".xlsx"))
                {
                    var xssfworkbook = new XSSFWorkbook(file);
                    int total = xssfworkbook.NumberOfSheets;
                    for (int i = 0; i < total; i++)
                    {
                        var sheetInfoClass = new SheetInfoClass
                            {
                                Id=i,
                                Name = xssfworkbook.GetSheetName(i)
                            };
                        sheetInfoClassList.Add(sheetInfoClass);
                    }
                }
                else
                {
                    var hssfworkbook = new HSSFWorkbook(file);
                    int total = hssfworkbook.NumberOfSheets;
                      for (int i = 0; i < total; i++)
                    {
                        var sheetInfoClass = new SheetInfoClass
                            {
                                Id=i,
                                Name = hssfworkbook.GetSheetName(i)
                            };
                        sheetInfoClassList.Add(sheetInfoClass);
                    }
                }
            }
            return sheetInfoClassList;
        }





        public static DataTable ConvertToDataTable(HttpPostedFileBase httpPostedFileBase)
        {
            ISheet sheet;
            var sheetType = "XSSFWorkbook";
            string fileName = httpPostedFileBase.FileName;
            using (Stream file = httpPostedFileBase.InputStream)
            {
                //读取2007
                if (fileName.Contains(".xlsx"))
                {
                    var xssfworkbook = new XSSFWorkbook(file);
                    sheet = xssfworkbook.GetSheetAt(0);
                    sheetType = "XSSFWorkbook";
                }
                else
                {
                    var hssfworkbook = new HSSFWorkbook(file);
                    sheet = hssfworkbook.GetSheetAt(0);
                    sheetType = "HSSFWorkbook";
                }
            }
            DataTable table = new DataTable();
            var headerRow = sheet.GetRow(0);
            var cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
            var rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

            //handling header.
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                var column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
            {
                var row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (table.Columns[j].ColumnName == "R3物料代码")
                            {
                                if (GetCellValue(row.GetCell(j), sheetType) != string.Empty)
                                {
                                    dataRow[j] = GetCellValue(row.GetCell(j), sheetType);
                                    dataRow[j] = dataRow[j].ToString().Trim();
                                    continue;
                                }
                            }

                            if (table.Columns[j].ColumnName == "生效时间" || table.Columns[j].ColumnName == "截止时间")
                            { 
                                if (!string.IsNullOrWhiteSpace(row.GetCell(j).ToString()))
                                {
                                    if(row.GetCell(j).CellType == CellType.STRING)
                                    {
                                        dataRow[j] = row.GetCell(j);
                                    }
                                    else
                                    {
                                        dataRow[j] = row.GetCell(j).DateCellValue;
                                    }
                                    continue;
                                }
                            }

                            dataRow[j] = GetCellValue(row.GetCell(j), sheetType);
                        }
                    }
                }
                if (table.Columns.Contains("R3物料代码"))
                {
                    if (!string.IsNullOrWhiteSpace(dataRow["R3物料代码"].ToString()))
                        table.Rows.Add(dataRow);
                }
                else
                {
                    table.Rows.Add(dataRow);
                }
            }

            return table;
        }

        /// <summary>
        /// 批量导入物料代码，没有列名
        /// </summary>
        /// <param name="httpPostedFileBase"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTableNoHeader(HttpPostedFileBase httpPostedFileBase)
        {
            ISheet sheet;
            var sheetType = "XSSFWorkbook";
            string fileName = httpPostedFileBase.FileName;
            using (Stream file = httpPostedFileBase.InputStream)
            {
                //读取2007
                if (fileName.Contains(".xlsx"))
                {
                    var xssfworkbook = new XSSFWorkbook(file);
                    sheet = xssfworkbook.GetSheetAt(0);
                    sheetType = "XSSFWorkbook";
                }
                else
                {
                    var hssfworkbook = new HSSFWorkbook(file);
                    sheet = hssfworkbook.GetSheetAt(0);
                    sheetType = "HSSFWorkbook";
                }
            }
            DataTable table = new DataTable();
            var headerRow = sheet.GetRow(0);
            var cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
            var rowCount = sheet.PhysicalNumberOfRows;//LastRowNum = PhysicalNumberOfRows - 1

            //handling header.
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                var column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            for (int i = (sheet.FirstRowNum); i < rowCount; i++)
            {
                var row = sheet.GetRow(i);
                var dataRow = table.NewRow();

                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = GetCellValue(row.GetCell(j), sheetType);
                    }
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static MemoryStream ExportXlsToDownload(DataTable dt)
        {
            var hssfworkbook = new HSSFWorkbook();
            //将数据写入Excel文件  
            MemoryStream file = GenerateData(hssfworkbook, dt);
            hssfworkbook.Write(file);
            return file;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static MemoryStream ExportXlsToDownload(DataTable dt,List<int> colWidths)
        {
            var hssfworkbook = new HSSFWorkbook();
            //将数据写入Excel文件  
            MemoryStream file = GenerateData(hssfworkbook, dt, colWidths);
            hssfworkbook.Write(file);
            return file;
        }

        /// <summary>
        /// 得到数据
        /// </summary>
        /// <param name="hssfworkbook"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static MemoryStream GenerateData(HSSFWorkbook hssfworkbook, DataTable dt)
        {
            try
            {
                var workbook = new HSSFWorkbook();
                var ds = new DataSet();
                if (dt.Rows.Count > 65535)
                {
                    var rowsNum = 65535;
                    //获取所需创建的表数量
                    int tableNum = (int)Math.Ceiling(Convert.ToDouble(dt.Rows.Count) / Convert.ToDouble(rowsNum));
                    //获取数据余数
                    // int remainder = dt.Rows.Count % rowsNum;

                    DataTable[] tableSlice = new DataTable[tableNum];
                    for (int c = 0; c < tableNum; c++)
                    {
                        tableSlice[c] = new DataTable();
                        foreach (DataColumn dc in dt.Columns)
                            tableSlice[c].Columns.Add(dc.ColumnName, dc.DataType);
                    }
                    for (int i = 0; i < tableNum; i++)
                    {
                        if (i == (tableNum - 1))
                        {
                            for (int j = i * rowsNum; j < dt.Rows.Count; j++)
                                tableSlice[i].ImportRow(dt.Rows[j]);
                        }
                        else
                        {
                            for (int j = i * rowsNum; j < ((i + 1) * rowsNum); j++)
                                tableSlice[i].ImportRow(dt.Rows[j]);
                        }

                    }

                    //for (int i = 0; i < tableNum; i++)
                    //    // if the current table is not the last one
                    //    if (i != tableNum - 1)
                    //        for (int j = i * rowsNum; j < ((i + 1) * rowsNum); j++)
                    //            tableSlice[i].ImportRow(dt.Rows[j]);
                    //    else
                    //        for (int k = i * rowsNum; k < ((i + 1) * rowsNum + remainder); k++)
                    //            tableSlice[i].ImportRow(dt.Rows[k]);
                    foreach (DataTable dd in tableSlice)
                        ds.Tables.Add(dd);
                }
                else
                {
                    ds.Tables.Add(dt);
                }

                var ms = new MemoryStream();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    var sheet = hssfworkbook.CreateSheet("Sheet" + (i + 1));
                    var headerRow = sheet.CreateRow(0);
                    var table = ds.Tables[i];
                    // handling header.
                    foreach (DataColumn column in table.Columns)
                    {
                        var cell = headerRow.CreateCell(column.Ordinal);
                        cell.SetCellValue(column.Caption);
                        var style = hssfworkbook.CreateCellStyle();
                        style.BorderBottom = BorderStyle.THIN;
                        style.BottomBorderColor = HSSFColor.BLACK.index;
                        style.BorderLeft = BorderStyle.THIN;
                        style.LeftBorderColor = HSSFColor.BLACK.index;
                        style.BorderRight = BorderStyle.THIN;
                        style.RightBorderColor = HSSFColor.BLACK.index;
                        style.BorderTop = BorderStyle.THIN;
                        style.TopBorderColor = HSSFColor.BLACK.index;
                        style.Alignment = HorizontalAlignment.CENTER;
                        cell.CellStyle = style;
                        sheet.SetColumnWidth(column.Ordinal, 4000);
                    }

                    // handling value.
                    int rowIndex = 1;
                    foreach (DataRow row in table.Rows)
                    {
                        var dataRow = sheet.CreateRow(rowIndex);
                        foreach (DataColumn column in table.Columns)
                        {
                          
                            var cell = dataRow.CreateCell(column.Ordinal);
                            cell.SetCellValue(new HSSFRichTextString(row[column].ToString()));
                        }
                        rowIndex++;
                    }
                    workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                }
                return ms;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        /// <summary>
        /// 得到数据
        /// </summary>
        /// <param name="hssfworkbook"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static MemoryStream GenerateData(HSSFWorkbook hssfworkbook, DataTable dt,List<int> colWidths)
        {
            try
            {
                var workbook = new HSSFWorkbook();
               
                var ds = new DataSet();
                if (dt.Rows.Count > 65535)
                {
                    var rowsNum = 65535;
                    //获取所需创建的表数量
                    int tableNum = (int)Math.Ceiling(Convert.ToDouble(dt.Rows.Count) / Convert.ToDouble(rowsNum));
                    //获取数据余数
                    // int remainder = dt.Rows.Count % rowsNum;

                    DataTable[] tableSlice = new DataTable[tableNum];
                    for (int c = 0; c < tableNum; c++)
                    {
                        tableSlice[c] = new DataTable();
                        foreach (DataColumn dc in dt.Columns)
                            tableSlice[c].Columns.Add(dc.ColumnName, dc.DataType);
                    }
                    for (int i = 0; i < tableNum; i++)
                    {
                        if (i == (tableNum - 1))
                        {
                            for (int j = i * rowsNum; j < dt.Rows.Count; j++)
                                tableSlice[i].ImportRow(dt.Rows[j]);
                        }
                        else
                        {
                            for (int j = i * rowsNum; j < ((i + 1) * rowsNum); j++)
                                tableSlice[i].ImportRow(dt.Rows[j]);
                        }

                    }

                    //for (int i = 0; i < tableNum; i++)
                    //    // if the current table is not the last one
                    //    if (i != tableNum - 1)
                    //        for (int j = i * rowsNum; j < ((i + 1) * rowsNum); j++)
                    //            tableSlice[i].ImportRow(dt.Rows[j]);
                    //    else
                    //        for (int k = i * rowsNum; k < ((i + 1) * rowsNum + remainder); k++)
                    //            tableSlice[i].ImportRow(dt.Rows[k]);
                    foreach (DataTable dd in tableSlice)
                        ds.Tables.Add(dd);
                }
                else
                {
                    ds.Tables.Add(dt);
                }

                var ms = new MemoryStream();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    var sheet = hssfworkbook.CreateSheet("Sheet" + (i + 1));
                    var headerRow = sheet.CreateRow(0);
                    var table = ds.Tables[i];
                    // handling header.
                    foreach (DataColumn column in table.Columns)
                    {
                        var cell = headerRow.CreateCell(column.Ordinal);
                        cell.SetCellValue(column.Caption);
                        var style = hssfworkbook.CreateCellStyle();
                        style.BorderBottom = BorderStyle.THIN;
                        style.BottomBorderColor = HSSFColor.BLACK.index;
                        style.BorderLeft = BorderStyle.THIN;
                        style.LeftBorderColor = HSSFColor.BLACK.index;
                        style.BorderRight = BorderStyle.THIN;
                        style.RightBorderColor = HSSFColor.BLACK.index;
                        style.BorderTop = BorderStyle.THIN;
                        style.TopBorderColor = HSSFColor.BLACK.index;
                        style.Alignment = HorizontalAlignment.CENTER;
                        style.VerticalAlignment = VerticalAlignment.CENTER;
                        cell.CellStyle = style;
                        sheet.SetColumnWidth(column.Ordinal, 4000);
                    }

                    // handling value.
                    int rowIndex = 1;
                    var style1 = hssfworkbook.CreateCellStyle();
                    foreach (DataRow row in table.Rows)
                    {
                        var dataRow = sheet.CreateRow(rowIndex);
                        foreach (DataColumn column in table.Columns)
                        {

                            var cell = dataRow.CreateCell(column.Ordinal);

                            style1.VerticalAlignment = VerticalAlignment.CENTER;
                           
                            if (row[column].ToString().Contains("\r\n"))
                            {
                                style1.WrapText = true;
                                if (column.Ordinal < colWidths.Count)
                                {
                                    sheet.SetColumnWidth(column.Ordinal, colWidths[column.Ordinal]);
                                }
                                else
                                {
                                    sheet.SetColumnWidth(column.Ordinal, 14000);
                                }
                            }
                            else
                            {
                                if (column.Ordinal < colWidths.Count)
                                {
                                    sheet.SetColumnWidth(column.Ordinal, colWidths[column.Ordinal]);
                                }
                            }

                            cell.SetCellValue(new HSSFRichTextString(row[column].ToString()));
                            cell.CellStyle = style1;
                        }
                        rowIndex++;
                    }
                    workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                }
                return ms;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="dt">找到的数据</param>
        /// <param name="table">没有找到的r3code</param>
        /// <returns></returns>
        public static MemoryStream ExportXlsToDownloadByTemplate(DataTable dt, DataTable table)
        {
            var hssfworkbook = new HSSFWorkbook();
            //将数据写入Excel文件  
            MemoryStream file = GenerateDataByTemplate(hssfworkbook, dt, table);
            hssfworkbook.Write(file);
            return file;
        }

        /// <summary>
        /// 得到数据
        /// </summary>
        /// <param name="hssfworkbook"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static MemoryStream GenerateDataByTemplate(HSSFWorkbook hssfworkbook, DataTable dt, DataTable dataTable)
        {
            try
            {
                var workbook = new HSSFWorkbook();
                var ds = new DataSet();
                if (dt.Rows.Count > 65535)
                {
                    var rowsNum = 65535;
                    //获取所需创建的表数量
                    int tableNum = (int)Math.Ceiling(Convert.ToDouble(dt.Rows.Count) / Convert.ToDouble(rowsNum));
                    //获取数据余数
                    // int remainder = dt.Rows.Count % rowsNum;

                    DataTable[] tableSlice = new DataTable[tableNum];
                    for (int c = 0; c < tableNum; c++)
                    {
                        tableSlice[c] = new DataTable();
                        foreach (DataColumn dc in dt.Columns)
                            tableSlice[c].Columns.Add(dc.ColumnName, dc.DataType);
                    }
                    for (int i = 0; i < tableNum; i++)
                    {
                        if (i == (tableNum - 1))
                        {
                            for (int j = i * rowsNum; j < dt.Rows.Count; j++)
                                tableSlice[i].ImportRow(dt.Rows[j]);
                        }
                        else
                        {
                            for (int j = i * rowsNum; j < ((i + 1) * rowsNum); j++)
                                tableSlice[i].ImportRow(dt.Rows[j]);
                        }

                    }

                    //for (int i = 0; i < tableNum; i++)
                    //    // if the current table is not the last one
                    //    if (i != tableNum - 1)
                    //        for (int j = i * rowsNum; j < ((i + 1) * rowsNum); j++)
                    //            tableSlice[i].ImportRow(dt.Rows[j]);
                    //    else
                    //        for (int k = i * rowsNum; k < ((i + 1) * rowsNum + remainder); k++)
                    //            tableSlice[i].ImportRow(dt.Rows[k]);
                    foreach (DataTable dd in tableSlice)
                        ds.Tables.Add(dd);
                }
                else
                {
                    ds.Tables.Add(dt);
                }
                ds.Tables.Add(dataTable);
                var ms = new MemoryStream();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    var sheetName = "Sheet" + (i + 1);
                    if (i == ds.Tables.Count - 1)
                        sheetName = "营销平台不存在的物料";
                    var sheet = hssfworkbook.CreateSheet(sheetName);
                    var headerRow = sheet.CreateRow(0);
                    var table = ds.Tables[i];
                    // handling header.
                    foreach (DataColumn column in table.Columns)
                    {
                        var cell = headerRow.CreateCell(column.Ordinal);
                        cell.SetCellValue(column.Caption);
                        var style = hssfworkbook.CreateCellStyle();
                        style.BorderBottom = BorderStyle.THIN;
                        style.BottomBorderColor = HSSFColor.BLACK.index;
                        style.BorderLeft = BorderStyle.THIN;
                        style.LeftBorderColor = HSSFColor.BLACK.index;
                        style.BorderRight = BorderStyle.THIN;
                        style.RightBorderColor = HSSFColor.BLACK.index;
                        style.BorderTop = BorderStyle.THIN;
                        style.TopBorderColor = HSSFColor.BLACK.index;
                        style.Alignment = HorizontalAlignment.CENTER;
                        cell.CellStyle = style;
                        sheet.SetColumnWidth(column.Ordinal, 4000);
                    }

                    // handling value.
                    int rowIndex = 1;
                    foreach (DataRow row in table.Rows)
                    {
                        var dataRow = sheet.CreateRow(rowIndex);
                        foreach (DataColumn column in table.Columns)
                        {
                            var cell = dataRow.CreateCell(column.Ordinal);
                            cell.SetCellValue(row[column].ToString());
                        }
                        rowIndex++;
                    }
                    workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                }
                return ms;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }



        /// <summary>
        /// 根据Excel列类型获取列的值
        /// </summary>
        /// <param name="cell">Excel列</param>
        /// <returns></returns>
        public static string GetCellValue(ICell cell, string sheetType)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.BLANK:
                    return string.Empty;
                case CellType.BOOLEAN:
                    return cell.BooleanCellValue.ToString();
                case CellType.ERROR:
                    return cell.ErrorCellValue.ToString();
                case CellType.NUMERIC:
                case CellType.Unknown:
                default:
                    return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.STRING:
                    return cell.StringCellValue;
                case CellType.FORMULA:
                    try
                    {
                        if (sheetType == "HSSFWorkbook")
                        {
                            var e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                            e.EvaluateInCell(cell);
                            return cell.ToString();
                        }
                        else
                        {
                            var e = new XSSFFormulaEvaluator(cell.Sheet.Workbook);
                            e.EvaluateInCell(cell);
                            return cell.ToString();
                        }
                    }
                    catch
                    {
                        return cell.ToString();
                    }
            }
        }


        public static MemoryStream EntityToExcel<T>(IList<T> list, params string[] propertyName)
        {
            //创建流对象
            using (MemoryStream ms = new MemoryStream())
            {
                //将参数写入到一个临时集合中
                List<string> propertyNameList = new List<string>();
                if (propertyName != null)
                    propertyNameList.AddRange(propertyName);
                //床NOPI的相关对象
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                IRow headerRow = sheet.CreateRow(0);

                if (list.Count > 0)
                {
                    //通过反射得到对象的属性集合
                    PropertyInfo[] propertys = list[0].GetType().GetProperties();
                    //遍历属性集合生成excel的表头标题
                    for (int i = 0; i < propertys.Length; i++)
                    {
                        //判断此属性是否是用户定义属性
                        if (propertyNameList.Count == 0)
                        {
                            headerRow.CreateCell(i).SetCellValue(propertys[i].Name);
                        }
                        else
                        {
                            if (propertyNameList.Contains(propertys[i].Name))
                                headerRow.CreateCell(i).SetCellValue(propertys[i].Name);
                        }
                    }


                    int rowIndex = 1;
                    //遍历集合生成excel的行集数据
                    for (int i = 0; i < list.Count; i++)
                    {
                        IRow dataRow = sheet.CreateRow(rowIndex);
                        for (int j = 0; j < propertys.Length; j++)
                        {
                            if (propertyNameList.Count == 0)
                            {
                                object obj = propertys[j].GetValue(list[i], null);
                                dataRow.CreateCell(j).SetCellValue(obj.ToString());
                            }
                            else
                            {
                                if (propertyNameList.Contains(propertys[j].Name))
                                {
                                    object obj = propertys[j].GetValue(list[i], null);
                                    dataRow.CreateCell(j).SetCellValue(obj.ToString());
                                }
                            }
                        }
                        rowIndex++;
                    }
                }
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }

    }
}
