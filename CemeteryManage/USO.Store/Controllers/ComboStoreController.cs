using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USO.Domain;
using USO.Domain.Extensions;
using USO.Dto;
using USO.Infrastructure.Services;
using USO.Infrastructure.Services.BaseNum;
using USO.Mvc.ActionResults;
using USO.Store.Security;
using USO.Store.ViewModels;

namespace USO.Store.Controllers
{
    public class ComboStoreController : Controller
    {
        private readonly IComboStoreService _comboStoreService;

        public ComboStoreController(IComboStoreService comboStoreService)
        {
            _comboStoreService = comboStoreService;
        }

        /// <summary>
        /// 加载所有角色
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadRoleStore()
        {
            var dtoList = _comboStoreService.GetAllRoles();
            return Json(dtoList);
        }

        /// <summary>
        /// 加载付款状态
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadDepartmentStore()
        {
            var dtoList = _comboStoreService.GetAllDepartments();
            return Json(dtoList);
        }

        /// <summary>
        /// 加载付款状态
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadPaymentStatusStore()
        {
            var paymentStatusList = _comboStoreService.GetAllPaymentStatus();
            return Json(paymentStatusList);
        }
        
         /// <summary>
        /// 加载墓碑类别
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadTombstoneTypeStore()
        {
            var tombstoneTypeList = _comboStoreService.GetAllTombstoneType();
            return Json(tombstoneTypeList);
        }
        /// <summary>
        /// 加载区域
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadAreaStore()
        {
            var areaList = _comboStoreService.GetAllArea();
            return Json(areaList);
        }
        /// <summary>
        /// 加载行
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadRowStore()
        {
            var rowList = _comboStoreService.GetAllRow();
            return Json(rowList);
        }
        /// <summary>
        /// 加载列
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadColumnStore()
        {
            var columnList = _comboStoreService.GetAllColumn();
            return Json(columnList);
        }
        /// <summary>
        /// 加载保密级别
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadSecurityLevelStore()
        {
            var securityLevelList = _comboStoreService.GetAllSecurityLevel();
            return Json(securityLevelList);
        }
        /// <summary>
        /// 加载服务级别
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadServiceLevelStore()
        {
            var serviceLevelList = _comboStoreService.GetAllServiceLevel();
            return Json(serviceLevelList);
        }


        /// <summary>
        /// 加载客户类型
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadCustomerTypeStore()
        {
            var customerTypeList = _comboStoreService.GetAllCustomerType();
            return Json(customerTypeList);
        }

        /// <summary>
        /// 加载国籍
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadNationalityStore()
        {
            var nationalityList = _comboStoreService.GetAllNationality();
            return Json(nationalityList);
        }

        /// <summary>
        /// 加载客户状态
        /// </summary>
        /// <returns></returns>
        [MyAuthorize]
        [HttpPost]
        public ActionResult LoadCustomerStatusStore()
        {
            var customerStatusList = _comboStoreService.GetAllCustomerStatus();
            return Json(customerStatusList);
        }
    }
}
