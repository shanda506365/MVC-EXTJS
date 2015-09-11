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
using USO.Infrastructure;
using USO.Infrastructure.Services;
using USO.Mvc.ActionResults;
using USO.Mvc.Utility;
using USO.Store.Security;
using USO.Store.ViewModels;

namespace USO.Store.Controllers
{
    public class TombstoneController : Controller
    {
        private readonly ITombstoneService _tombstoneService;
        private readonly ISysLogService _sysLogService;

        public TombstoneController(ITombstoneService tombstoneService, ISysLogService sysLogService)
        {
            _tombstoneService = tombstoneService;
            _sysLogService = sysLogService;
        }


        /// <summary>
        /// 加载墓碑表
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadTombstoneGrid()
        {
            var tombstones = LoadTombstones(false);


            var gsbModel = new GridStoreBaseModel<TombstoneDTO>
                {
                    success = true,
                    msg = "成功",
                    dataset = tombstones.Result.ToList(),
                    total = tombstones.Total
                };

            return Json(gsbModel);
        }
        /// <summary>
        /// 获取墓碑信息
        /// </summary>
        /// <param name="isOutExcel"></param>
        /// <returns></returns>
        private PagedResult<TombstoneDTO> LoadTombstones(bool isOutExcel)
        {
            var tombstoneQuery = new TombstoneQuery
                {
                    limit = isOutExcel ? 0 : int.Parse(Request.Params["limit"]),
                    page = isOutExcel ? 0 : int.Parse(Request.Params["page"]),
                    dir = Request.Params["dir"] == "ASC" ? ListSortDirection.Ascending : ListSortDirection.Descending,
                    sort = InitSortParam(Request.Params["sort"])
                };
            //过滤条件
            var filter = Request.Params["filter"];
            if (!string.IsNullOrEmpty(filter))
            {
                tombstoneQuery.filter = filter.ToObject<Tombstone>();
            }
            var lastPaymentDateQuery = Request.Params["LastPaymentDateQuery"];
            //补交时间
            if (!string.IsNullOrEmpty(lastPaymentDateQuery))
            {
                tombstoneQuery.LastPaymentDateQuery = lastPaymentDateQuery;
            }

            var expiryDateQuery = Request.Params["ExpiryDateQuery"];
            //到期时间
            if (!string.IsNullOrEmpty(expiryDateQuery))
            {
                tombstoneQuery.ExpiryDateQuery = expiryDateQuery;
            }

            var tombstones = _tombstoneService.Find(tombstoneQuery);
            return tombstones;
        }

        /// <summary>
        /// 新增墓碑
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult AddTombstone()
        {
            var tombstone = new TombstoneDTO
                {
                    Name = Request.Params["Name"],
                    AreaId = GlobalMethod.CoventToInt(Request.Params["AreaId"]),
                    RowId = GlobalMethod.CoventToInt(Request.Params["RowId"]),
                    ColumnId = GlobalMethod.CoventToInt(Request.Params["ColumnId"]),
                    ParentId = GlobalMethod.CoventToInt(Request.Params["ParentId"]),
                    Alias = Request.Params["Alias"],
                    Remark = Request.Params["Remark"],
                    CustomerId = GlobalMethod.CoventToInt(Request.Params["CustomerId"]),
                    CustomerName = Request.Params["CustomerName"],
                    StoneText = Request.Params["StoneText"],
                    ExpiryDate = GlobalMethod.CoventToDatetime(Request.Params["ExpiryDate"]),
                    BuyDate = GlobalMethod.CoventToDatetime(Request.Params["BuyDate"]),
                    LastPaymentDate = GlobalMethod.CoventToDatetime(Request.Params["LastPaymentDate"]),
                    BuryDate = GlobalMethod.CoventToDatetime(Request.Params["BuryDate"]),
                    Width = GlobalMethod.CoventToIntNotNull(Request.Params["Width"]),
                    Height = GlobalMethod.CoventToIntNotNull(Request.Params["Height"]),
                    //Acreage = Request.Params["Remark"],
                    SecurityLevelId = GlobalMethod.CoventToInt(Request.Params["SecurityLevelId"]),
                    Image = Request.Params["Image"],
                    ServiceLevelId = GlobalMethod.CoventToInt(Request.Params["ServiceLevelId"]),
                    TypeId = GlobalMethod.CoventToInt(Request.Params["TypeId"]),
                    PaymentStatusId = GlobalMethod.CoventToInt(Request.Params["PaymentStatusId"]),
                    SortNum = GlobalMethod.CoventToIntNotNull(Request.Params["SortNum"])
                };
            var result = _tombstoneService.Create(tombstone);
            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "新增墓碑"
                , result.success == true ? "新增墓碑:" + result.ResultOutDto.Id.ToString() : "新增墓碑失败:" + result.msg);
            return Json(result);
        }

        /// <summary>
        /// 批量新增墓碑
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult AddTombstoneRowList()
        {
            var id = int.Parse(Request.Params["Id"]);
            var rowId = int.Parse(Request.Params["RowId"]);
            var typeId = int.Parse(Request.Params["TypeId"]);
            int count = int.Parse(Request.Params["count"]);

            var result = _tombstoneService.CreateList(id,rowId,typeId,count);
            var idsStr = "";
            if (result.ResultOutDtos.Any())
            {
                foreach (var dto in result.ResultOutDtos)
                {
                    idsStr += dto.Id.ToString()+",";
                }
            }
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "批量新增墓碑"
                , result.success == true ? "批量新增墓碑:" + idsStr : "批量新增墓碑失败:" + result.msg);

            return Json(result);
        }

        /// <summary>
        /// 编辑墓碑
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult UpdateTombstone()
        {
            var tombstone = new TombstoneDTO
            {
                Id = GlobalMethod.CoventToIntNotNull(Request.Params["Id"]),
                Name = Request.Params["Name"],
                AreaId = GlobalMethod.CoventToInt(Request.Params["AreaId"]),
                RowId = GlobalMethod.CoventToInt(Request.Params["RowId"]),
                ColumnId = GlobalMethod.CoventToInt(Request.Params["ColumnId"]),
                ParentId = GlobalMethod.CoventToInt(Request.Params["ParentId"]),
                Alias = Request.Params["Alias"],
                Remark = Request.Params["Remark"],
                CustomerId = GlobalMethod.CoventToInt(Request.Params["CustomerId"]),
                CustomerName = Request.Params["CustomerName"],
                StoneText = Request.Params["StoneText"],
                ExpiryDate = GlobalMethod.CoventToDatetime(Request.Params["ExpiryDate"]),
                BuyDate = GlobalMethod.CoventToDatetime(Request.Params["BuyDate"]),
                LastPaymentDate = GlobalMethod.CoventToDatetime(Request.Params["LastPaymentDate"]),
                BuryDate = GlobalMethod.CoventToDatetime(Request.Params["BuryDate"]),
                Width = GlobalMethod.CoventToIntNotNull(Request.Params["Width"]),
                Height = GlobalMethod.CoventToIntNotNull(Request.Params["Height"]),
                //Acreage = Request.Params["Remark"],
                SecurityLevelId = GlobalMethod.CoventToInt(Request.Params["SecurityLevelId"]),
                Image = Request.Params["Image"],
                ServiceLevelId = GlobalMethod.CoventToInt(Request.Params["ServiceLevelId"]),
                TypeId = GlobalMethod.CoventToInt(Request.Params["TypeId"]),
                PaymentStatusId = GlobalMethod.CoventToInt(Request.Params["PaymentStatusId"]),
                SortNum = GlobalMethod.CoventToIntNotNull(Request.Params["SortNum"])
            };
            var result = _tombstoneService.Update(tombstone);
            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "编辑墓碑"
                , result.success == true ? "编辑墓碑:" + result.ResultOutDto.Id.ToString() : "编辑墓碑失败:" + result.msg);
            return Json(result);
        }


        /// <summary>
        /// 删除墓碑
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult DelTombstone()
        {
            var tombstoneList = new List<TombstoneDTO>();
            var idList = Request.Params["idList"].Split(',');
            foreach (var id in idList)
            {
                tombstoneList.Add(new TombstoneDTO
                    {
                        Id = int.Parse(id)
                    });
            }
            var result = _tombstoneService.Delete(tombstoneList);
            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "删除墓碑"
                , result.success == true ? "删除墓碑:" + Request.Params["idList"] : "删除墓碑失败:" + result.msg);
            return Json(result);
        }

        /// <summary>
        /// 墓碑排序
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult SortTombstonePng()
        {
            var tombstoneIdList = Request.Params["tombstoneIdList"];
            var idList = tombstoneIdList.Split(',');

            var result = _tombstoneService.SorTombstone(idList);
            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "墓碑排序"
                , result.success == true ? "墓碑排序:" + Request.Params["tombstoneIdList"] : "墓碑排序失败:" + result.msg);
            return Json(result);
        }

        /// <summary>
        /// 导出墓碑信息
        /// </summary>
        /// <returns></returns>
        [FileDownload]
        public ActionResult OutPutExcelTombstone()
        {
            try
            {
                var tombstones = LoadTombstones(true);

                DataTable table = new DataTable();
                List<int> colWidths = new List<int>();
                #region 添加列
                table.Columns.Add("编号", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("全名", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("别名", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("所属区域", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("所属行", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("所属列", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("付款状态", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("到期日期", typeof(string));
                colWidths.Add(5000);
                table.Columns.Add("购买日期", typeof(string));
                colWidths.Add(5000);
                table.Columns.Add("备注", typeof(string));
                colWidths.Add(14000);
                table.Columns.Add("所属客户", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("碑文", typeof(string));
                colWidths.Add(14000);
                table.Columns.Add("墓碑宽度", typeof(string));
                colWidths.Add(4000);
                table.Columns.Add("墓碑高度", typeof(string));
                colWidths.Add(4000);

                #endregion
                foreach (var tombstone in tombstones.Result)
                {
                    DataRow newRow = table.NewRow();
                    newRow["编号"] = tombstone.Id;
                    newRow["全名"] = tombstone.Name;
                    newRow["别名"] = tombstone.Alias;
                    newRow["所属区域"] = tombstone.AreaEntity.Name;
                    newRow["所属行"] = tombstone.RowEntity.Name;
                    newRow["所属列"] = tombstone.ColumnEntity.Name;
                    newRow["备注"] = tombstone.Remark;
                    newRow["所属客户"] = tombstone.CustomerName;
                    newRow["碑文"] = tombstone.StoneText;
                    newRow["到期日期"] = tombstone.ExpiryDateString;
                    newRow["购买日期"] = tombstone.BuyDateString;
                    newRow["墓碑宽度"] = tombstone.Width;
                    newRow["墓碑高度"] = tombstone.Height;
                    newRow["付款状态"] = tombstone.PaymentStatusEntity.Name;
                    table.Rows.Add(newRow);
                }
                //写入日志
                GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, true, "导出墓碑信息", "导出墓碑信息成功");
                return File(ImportXlsToDataTable.ExportXlsToDownload(table, colWidths).ToArray(), "application/ms-excel"
                    , "Tombstone" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            }
            catch (Exception ex)
            {
                //写入日志
                GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, false, "导出墓碑信息", "导出墓碑信息失败:" + ex.Message);
            }
            return new EmptyResult();
        }


        /// <summary>
        /// 墓碑落葬
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult BuryPeopleTombstone()
        {
            var tombstoneId = int.Parse(Request.Params["tombstoneId"]);
            var customerIds = Request.Params["customerIds"].Split(',');
            var customerList = new List<int>();
            for (var i = 0; i < customerIds.Length; i++)
            {
                customerList.Add(int.Parse(customerIds[i]));
            }
            var result = _tombstoneService.BuryPeopleTombstone(tombstoneId, customerList);
            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "墓碑落葬"
                , result.success == true ? "墓碑落葬:" + Request.Params["tombstoneId"] + "客户(" + Request.Params["customerIds"] + ")" : "墓碑落葬失败:" + result.msg);
            return Json(result);
        }
        /// <summary>
        /// 墓碑解除落葬
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult UnBuryPeopleTombstone()
        {
            var tombstoneId = int.Parse(Request.Params["tombstoneId"]);
            var customerIds = Request.Params["customerIds"].Split(',');
            var customerList = new List<int>();
            for (var i = 0; i < customerIds.Length; i++)
            {
                customerList.Add(int.Parse(customerIds[i]));
            }
            var result = _tombstoneService.UnburyPeopleTombstone(tombstoneId, customerList);
            //写入日志
            GlobalMethod.WriteLog(Session, _sysLogService, LogType.Control, result.success, "墓碑解除落葬"
                , result.success == true ? "墓碑解除落葬:" + Request.Params["tombstoneId"] + "客户(" + Request.Params["customerIds"] + ")" : "墓碑解除落葬失败:" + result.msg);
            return Json(result);
        }

        private string InitSortParam(string str)
        {
            var sortStr = str;
            if (str == "AreaEntity")
            {
                return "AreaId";
            }
            else if (str == "SecurityLevelName")
            {
                return "SecurityLevelId";
            }
            else if (str == "ServiceLevelName")
            {
                return "SecurityLevelId";
            }
            else if (str == "PaymentStatusEntity")
            {
                return "PaymentStatusId";
            }
            return sortStr;
        }

    }
}
