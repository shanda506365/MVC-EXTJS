using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using USO.Dto.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using USO.Domain;
using System.IO;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Data;
using System.Web;

namespace USO.Order.Test
{
    [TestClass]
    public class InputOutputExcel
    {
        List<ProductDTO> products = new List<ProductDTO>();

        public InputOutputExcel()
        {
            ProductDTO product = new ProductDTO();
            product.Id = 1;
            product.R3Code = "CH1042886";
            product.BriefName = "手机A4S 长虹";
            product.SalesStatus = ProductSalesStatus.On;
            product.Status = ProductStatus.Init;
            product.ProductName = "手机A4S 长虹手机";
            product.R3ProductLineID = 1;
            product.R3ProductGroupID = 1;
            product.C3ID = 1;
            product.ManufacturerID = 1;
            product.IsSpecial = 0;

            products.Add(product);

            product = new ProductDTO();
            product.Id = 2;
            product.R3Code = "CH1042885";
            product.BriefName = "手机V6双核 长虹";
            product.SalesStatus = ProductSalesStatus.On;
            product.Status = ProductStatus.Init;
            product.ProductName = "手机V6双核 长虹智能手机";
            product.R3ProductLineID = 1;
            product.R3ProductGroupID = 1;
            product.C3ID = 1;
            product.ManufacturerID = 1;
            product.IsSpecial = 0;

            products.Add(product);

        }

        [TestMethod]
        public void Test()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ID").Caption = "系统编码";
            table.Columns.Add("R3Code").Caption = "R3物料代码";

            foreach (ProductDTO product in products)
            {
                table.Rows.Add(new object[]{product.Id,product.R3Code});
            }
            MemoryStream ms= ExcelHelper.DataToExcel(table);
            ExcelHelper.MSToBrowser(ms,HttpContext.Current,"test");
        }

       

        //private bool OutExcel(string title,List<ProductDTO> data,out string msg)
        //{
        //    bool result = false;
        //    string exMsg = string.Empty;

        //    Excel.Application app;
        //    Excel.Workbook book;
        //    Excel.Worksheet sheet;
        //    Excel.Range range;
        //    Excel.Range range2;

        //    app = new Excel.Application();
        //    book = app.Workbooks.Add();//添加新工作簿
        //    sheet = (Excel.Worksheet)book.Worksheets[1];

        //    int curIndex = 1;
        //    sheet.Cells[curIndex, 1] = title;
        //    curIndex++;

        //    //定义标题
        //    sheet.Cells[curIndex, 1] = "系统编码";
        //    sheet.Cells[curIndex, 2] = "R3物料代码";
        //    sheet.Cells[curIndex, 3] = "物料名称";
        //    sheet.Cells[curIndex, 4] = "销售状态";
        //    sheet.Cells[curIndex, 5] = "产品状态";
        //    sheet.Cells[curIndex, 6] = "产品显示名";
        //    sheet.Cells[curIndex, 7] = "产品线";
        //    sheet.Cells[curIndex, 8] = "产品组";
        //    sheet.Cells[curIndex, 9] = "大类";
        //    sheet.Cells[curIndex, 10] = "中类";
        //    sheet.Cells[curIndex, 11] = "小类";
        //    sheet.Cells[curIndex, 12] = "品牌";
        //    sheet.Cells[curIndex, 13] = "专属产品";
        //    curIndex++;

        //    //绑定数据
        //    foreach (ProductDTO product in data)
        //    {
        //        sheet.Cells[curIndex, 1] = product.Id;
        //        sheet.Cells[curIndex, 2] = product.R3Code;
        //        sheet.Cells[curIndex, 3] = product.BriefName;
        //        sheet.Cells[curIndex, 4] = product.SalesStatus;
        //        sheet.Cells[curIndex, 5] = product.ProductStatusString;
        //        sheet.Cells[curIndex, 6] = product.ProductName;
        //        sheet.Cells[curIndex, 7] = product.R3ProductLineID;
        //        sheet.Cells[curIndex, 8] = product.R3ProductGroupID;
        //        sheet.Cells[curIndex, 9] = product.Id;
        //        sheet.Cells[curIndex, 10] = product.Id;
        //        sheet.Cells[curIndex, 11] = product.C3ID;
        //        sheet.Cells[curIndex, 12] = product.ManufacturerID;
        //        sheet.Cells[curIndex, 13] = product.IsSpecial==1?"是":"否";
        //        curIndex++;
        //    }

        //    //格式定义
        //    range = (Excel.Range)sheet.get_Range("A1", "M1");
        //    range.Merge(0);
        //    range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;//纵向居中 
        //    range.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;//横向居中
        //    range.Font.Size = 18;
        //    range.Font.Name = "黑体";
        //    range.RowHeight = 24;


        //    range2 = sheet.get_Range("A2", "M" + Convert.ToString(data.Count + 2));
        //    range2.Borders.LineStyle = 1;
        //    range2.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;//居中
        //    range2.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
        //    range2.EntireColumn.AutoFit();//列宽自动  

        //    app.Application.DisplayAlerts = true;

        //    try
        //    {
        //        string path=@"E:\test.xls";
        //        sheet.SaveAs(path);
        //        app.Workbooks.Open(path);
        //        app.Visible = true;
        //        result=true;
        //    }
        //    catch (Exception ex)
        //    {
        //        result = false;
        //        exMsg = ex.Message;
        //    }
        //    finally
        //    {
        //        //app.Workbooks.Close();
        //        //app.Quit();
        //        //System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
        //        //System.Runtime.InteropServices.Marshal.ReleaseComObject(app);

        //    }

        //    msg = exMsg;
        //    return result;
        //}


    }
}
