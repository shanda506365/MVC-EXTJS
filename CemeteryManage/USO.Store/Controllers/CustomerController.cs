using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcExtensions;
using Newtonsoft.Json;
using USO.Domain;
using USO.Domain.Extensions;
using USO.Dto;
using USO.Infrastructure.Services;
using USO.Mvc.ActionResults;
using USO.Mvc.Utility;
using USO.Store.Security;
using USO.Store.ViewModels;

namespace USO.Store.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ISysLogService _sysLogService;

        public CustomerController(ICustomerService customerService,ISysLogService sysLogService)
        {
            _customerService = customerService;
            _sysLogService = sysLogService;
        }

        [MyAuthorize]
        public ActionResult Index()
        {
            return View("CustomerIndex");
        }

        /// <summary>
        /// 加载客户表
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadCustomerGrid()
        {
            var customers = LoadCustomers(false);

            var gsbModel = new GridStoreBaseModel<CustomerDTO>
                {
                    success = true,
                    msg = "成功",
                    dataset = customers.Result.ToList(),
                    total = customers.Total
                };
            return Json(gsbModel);
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        private PagedResult<CustomerDTO> LoadCustomers(bool isOutExcel)
        {
            var customerQuery = new CustomerQuery
                {
                    limit = isOutExcel?0:int.Parse(Request.Params["limit"]),
                    page = isOutExcel ? 0 :int.Parse(Request.Params["page"]),
                    dir = Request.Params["dir"] == "ASC" ? ListSortDirection.Ascending : ListSortDirection.Descending,
                    sort = InitSortParam(Request.Params["sort"])
                };
            //过滤条件
            var filter = Request.Params["filter"];
            if (!string.IsNullOrEmpty(filter))
            {
                customerQuery.filter = filter.ToObject<Customer>();
            }
            var buryDateQuery = Request.Params["BuryDateQuery"];
            //下葬日期
            if (!string.IsNullOrEmpty(buryDateQuery))
            {
                customerQuery.BuryDateQuery = buryDateQuery;
            }

            var customers = _customerService.Find(customerQuery);
            return customers;
        }

        /// <summary>
        /// 新增客户
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult AddCustomer()
        {
            var customer = new CustomerDTO
                {
                    FullName = Request.Params["FullName"],
                    LastName = Request.Params["LastName"],
                    FirstName = Request.Params["FirstName"],
                    MiddleName = Request.Params["MiddleName"],
                    Remark = Request.Params["Remark"],
                    Telephone = Request.Params["Telephone"],
                    Phone = Request.Params["Phone"],
                    OtherPhone = Request.Params["OtherPhone"],
                    Address = Request.Params["Address"],
                    CustomerTypeId = GlobalMethod.CoventToInt(Request.Params["CustomerTypeId"]),
                    LinkCustomerId = GlobalMethod.CoventToInt(Request.Params["LinkCustomerId"]),
                    BuryDate = GlobalMethod.CoventToDatetime(Request.Params["BuryDate"]),
                    DeathDate = GlobalMethod.CoventToDatetime(Request.Params["DeathDate"]),
                    CustomerStatusId = GlobalMethod.CoventToInt(Request.Params["CustomerStatusId"]),
                    NationalityId = GlobalMethod.CoventToInt(Request.Params["NationalityId"]),
                    IDNumber = Request.Params["IDNumber"]
                };
            var result = _customerService.Create(customer);
            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "新增客户"
                , result.success == true ? "新增客户:" + result.ResultOutDto.Id.ToString() : "新增客户失败:" + result.msg);
            return Json(result);
        }


        /// <summary>
        /// 编辑客户
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult UpdateCustomer()
        {
            var customer = new CustomerDTO
            {
                Id = int.Parse(Request.Params["Id"]),
                FullName = Request.Params["FullName"],
                LastName = Request.Params["LastName"],
                FirstName = Request.Params["FirstName"],
                MiddleName = Request.Params["MiddleName"],
                Remark = Request.Params["Remark"],
                Telephone = Request.Params["Telephone"],
                Phone = Request.Params["Phone"],
                OtherPhone = Request.Params["OtherPhone"],
                Address = Request.Params["Address"],
                CustomerTypeId = GlobalMethod.CoventToInt(Request.Params["CustomerTypeId"]),
                LinkCustomerId = GlobalMethod.CoventToInt(Request.Params["LinkCustomerId"]),
                BuryDate = GlobalMethod.CoventToDatetime(Request.Params["BuryDate"]),
                DeathDate = GlobalMethod.CoventToDatetime(Request.Params["DeathDate"]),
                CustomerStatusId = GlobalMethod.CoventToInt(Request.Params["CustomerStatusId"]),
                NationalityId = GlobalMethod.CoventToInt(Request.Params["NationalityId"]),
                IDNumber = Request.Params["IDNumber"]
            };
            var result = _customerService.Update(customer);
            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "编辑客户"
                , result.success == true ? "编辑客户:" + result.ResultOutDto.Id.ToString() : "编辑客户失败:" + result.msg);
            return Json(result);
        }


        /// <summary>
        /// 删除客户
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult DelCustomer()
        {
            var customerList = new List<CustomerDTO>();
            var idList = Request.Params["idList"].Split(',');
            foreach (var id in idList)
            {
                customerList.Add(new CustomerDTO
                    {
                        Id = int.Parse(id)
                    });
            }
            var result = _customerService.Delete(customerList);
            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "删除客户"
                , result.success == true ? "删除客户:" + Request.Params["idList"] : "删除客户失败:" + result.msg);
            return Json(result);
        }


        /// <summary>
        /// 导出客户信息
        /// </summary>
        /// <returns></returns>
        [FileDownload]
        public ActionResult OutPutExcelCustomer()
        {
            try
            {
                var customers = LoadCustomers(true);

                DataTable table = new DataTable();
                List<int> colWidths = new List<int>();
                table.Columns.Add("编号", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("全名", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("移动电话", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("电话", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("其他联系方式", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("地址", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("客户类型", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("联系人编号", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("联系人", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("下葬日期", typeof(string));
                colWidths.Add(5000);
                table.Columns.Add("死亡日期", typeof(string));
                colWidths.Add(5000);
                table.Columns.Add("客户状态", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("国籍", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("身份证号", typeof(string));
                colWidths.Add(10000);
                table.Columns.Add("备注", typeof(string));
                colWidths.Add(14000);

                foreach (var customer in customers.Result)
                {
                    DataRow newRow = table.NewRow();
                    newRow["编号"] = customer.Id;
                    newRow["全名"] = customer.FullNameString;
                    newRow["移动电话"] = customer.Telephone;
                    newRow["电话"] = customer.Phone;
                    newRow["其他联系方式"] = customer.OtherPhone;
                    newRow["地址"] = customer.Address;
                    newRow["客户类型"] = customer.CustomerTypeName;
                    newRow["联系人编号"] = customer.LinkCustomerId;
                    newRow["联系人"] = customer.LinkCustomerName;
                    newRow["下葬日期"] = customer.BuryDateString;
                    newRow["死亡日期"] = customer.DeathDateString;
                    newRow["客户状态"] = customer.CustomerStatusName;
                    newRow["国籍"] = customer.NationalityName;
                    newRow["身份证号"] = customer.IDNumber;
                    newRow["备注"] = customer.Remark;
                    table.Rows.Add(newRow);
                }
                //写入日志
                GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, true, "导出客户信息", "导出客户信息成功");
                return File(ImportXlsToDataTable.ExportXlsToDownload(table, colWidths).ToArray(), "application/ms-excel"
                    , "Customer"+DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            }
            catch (Exception ex)
            {
                //写入日志
                GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, false, "导出客户信息", "导出客户信息失败:"+ex.Message);
            }
            return new EmptyResult();
        }
        /// <summary>
        /// 上传客户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadCustomerExcel()
        {
           
            Session["CustomerPostFile"] = null;
            var table = new DataTable();
            var gsbModel = new GridStoreBaseModel<SheetInfoClass>
            {
                success = false,
                msg = "成功",
                dataset = new List<SheetInfoClass>(),//失败的DTO
                total = 0//成功数量
            };

            try
            {
                if (Request.Files == null || Request.Files.Count <= 0) return null;
                HttpPostedFileBase file = Request.Files[0];
                Session["CustomerPostFile"] = file;
                //table = ImportXlsToDataTable.ConvertToDataTable(file);
                gsbModel.dataset = ImportXlsToDataTable.GetExcelSheetStruct(file);
                gsbModel.success = true;
                //写入日志
                GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, true, "上传客户信息", "上传客户信息成功");
            }
            catch (Exception ex)
            {
                gsbModel.success = false;
                gsbModel.msg = ex.Message;
                //写入日志
                GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, false, "上传客户信息", "上传客户信息失败:"+ex.Message);
            }
            Response.ContentType = "text/html";
            return Content(gsbModel.ToJson());
        }

        /// <summary>
        /// 导入客户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InputCustomerStocksFromInput()
        {
            
            var table = new DataTable();
            var gsbModel = new GridStoreBaseModel<CustomerDTO>
            {
                success = false,
                msg = "成功",
                dataset = new List<CustomerDTO>(),//失败的DTO
                total = 0//成功数量
            };
            try
            {
                if (Session["CustomerPostFile"] == null || (Session["CustomerPostFile"] as HttpPostedFileBase) ==null)
                {
                    gsbModel.success = false;
                    gsbModel.msg = "无任何导入信息";
                }
                //if (Request.Files == null || Request.Files.Count <= 0) return null;
                //HttpPostedFileBase file = Request.Files[0];
                table = ImportXlsToDataTable.ConvertToDataTable(Session["CustomerPostFile"] as HttpPostedFileBase);
                var index = 0;
                var customeDtoList = new List<CustomerDTO>();
                foreach (var rows in table.Rows)
                {
                    var row = (DataRow)rows;
                    var id = index++;
                  
                    var fullName = row["全名"].ToString().Trim();
                    var iDNumber = row["身份证号"].ToString().Trim();

                    var customer = new CustomerDTO()
                    {
                        FullName = fullName,
                        IDNumber = iDNumber
                    };
                    customeDtoList.Add(customer);
                }

                var result = _customerService.Create(customeDtoList);
                gsbModel.success = true;
                gsbModel.dataset = result.Result.ToList();
                gsbModel.total = result.Total;
                //写入日志
                GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, true, "导入客户信息", "导入客户信息");

            }
            catch (Exception ex)
            {
                gsbModel.success = false;
                gsbModel.msg = ex.Message;
                //写入日志
                GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, false, "导入客户信息", "导入客户信息失败:" + ex.Message);
            }
            return Json(gsbModel);
           
        }

        /// <summary>
        /// 初始化排序字段
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string InitSortParam(string str)
        {
            var sortStr = str;
            if (str == "CustomerTypeName")
            {
                return "CustomerTypeId";
            }
            else if (str == "CustomerStatusName")
            {
                return "CustomerStatusId";
            }
            else if (str == "FullName")
            {
                return "LastName";
            }
            return sortStr;
        }

    }
}
