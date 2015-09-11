using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using USO.Domain;
using USO.Dto;
using USO.Dto.Order;
using USO.Domain.Customer;
using USO.Infrastructure;
using System.Collections.Generic;
using USO.Infrastructure.Order;
using USO.Infrastructure.Services;
using USO.Core;
using System.Transactions;
//using Moq;

namespace USO.Order.Test
{
    [TestClass]
    public class TestDivideSo
    {
        //private Mock<ISOService> _soService;

        public TestDivideSo()
        {
            //_soService = new Mock<ISOService>();
        }

        [TestMethod]
        public void TestDivideSO()
        {
            //销售组织（1 三台经营部 2梓潼经营部）
            int R3SalesChannelId = 1;
            int CustomerLegalId = 1;
            int DeliveryId = 1;
            int ReceiveAddressId = 1;
            string memo = "";
            
            List<StoreSOItemDTO> productList = new List<StoreSOItemDTO>();
            StoreSOItemDTO product = null;
            product = new StoreSOItemDTO();
            product.ProductId = 1;
            product.CustomerOrderNum = 2;
            product.CustomerOrderPrice = 300;
            product.RebateAmt = 10;
            product.CustomerLegalOrgId = 1;
            product.R3ProductGroupId = 1001;
            productList.Add(product);

            product = new StoreSOItemDTO();
            product.ProductId = 2;
            product.CustomerOrderNum = 1;
            product.CustomerOrderPrice = 40;
            product.RebateAmt = 5;
            product.CustomerLegalOrgId = 1;
            product.R3ProductGroupId = 1001;
            productList.Add(product);

            product = new StoreSOItemDTO();
            product.ProductId = 3;
            product.CustomerOrderNum = 1;
            product.CustomerOrderPrice = 40;
            product.RebateAmt = 3;
            product.CustomerLegalOrgId = 1;
            product.R3ProductGroupId = 1002;
            productList.Add(product);

            product = new StoreSOItemDTO();
            product.ProductId = 4;
            product.CustomerOrderNum = 2;
            product.CustomerOrderPrice = 500;
            product.RebateAmt = 70;
            product.CustomerLegalOrgId = 2;
            product.R3ProductGroupId = 1001;
            productList.Add(product);

            StoreSOMasterDTO storeSOMasterDTO = new StoreSOMasterDTO();

            StoreCreateSO(productList, storeSOMasterDTO);

        }

        #region 创建订单
        /// <summary>
        /// 创建订单（前台）
        /// </summary>
        /// <param name="storeSOItemDTO">订单产品信息</param>
        /// <param name="storeSOMasterDTO">订单信息</param>
        /// <returns></returns>
        private bool StoreCreateSO(IList<StoreSOItemDTO> storeSOItemDTO, StoreSOMasterDTO storeSOMasterDTO)
        {
            bool result = false;
            //拆单 
            List<SODTO> soList = DivideSO(storeSOItemDTO,storeSOMasterDTO);

            //验单 
            ServiceResult<SODTO> validResult = null;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (SODTO soItem in soList)
                    {
                       // validResult = _soService.Object.ValidateSo(soItem);
                        if (validResult == null)
                        {
                            //存单
                           // _soService.Object.CreateSo(soItem);
                        }
                    }
                    scope.Complete();
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            
            return result;
        }
        #endregion

        #region 拆单
        /// <summary>
        /// 拆分订单
        /// </summary>
        /// <param name="storeSOItemDTO">订单产品信息</param>
        /// <param name="storeSOMasterDTO">订单信息</param>
        /// <returns></returns>
        private List<SODTO> DivideSO(IList<StoreSOItemDTO> storeSOItemDTO,StoreSOMasterDTO storeSOMasterDTO)
        {
            //私有字段
            List<StoreSOItemDTO> productByOrgIdList=new List<StoreSOItemDTO>();
            List<SODTO> soList = new List<SODTO>();
            List<SOItemDTO> soItemList = null;
            SODTO so = null;
            SOItemDTO soItem = null;
            decimal soAmt = 0;//订单总金额
            decimal productsAmt = 0;//产品总金额

            //获得组织列表
            var orgIdList = (from d in storeSOItemDTO select d.CustomerLegalOrgId).Distinct().ToList();
            //遍历每个组织
            foreach (var orgId in orgIdList)
            {

                //获取该组织下的产品
                productByOrgIdList = (from d in storeSOItemDTO where d.CustomerLegalOrgId == orgId select d).ToList();
                //获得产品组列表
                var productGroupIdList = (from d in productByOrgIdList select d.R3ProductGroupId).Distinct().ToList();
                
                if (productByOrgIdList.Count() >= 1)
                {
                    //遍历每个产品组
                    foreach (var groupId in productGroupIdList)
                    {
                        //获得属于该组的产品
                        var orderProduct = (from d in productByOrgIdList where d.R3ProductGroupId == groupId select d).ToList();
                        //产品列表
                        soItemList = new List<SOItemDTO>();
                        soAmt = 0;//订单总金额
                        productsAmt = 0;//产品总金额
                        foreach (StoreSOItemDTO product in orderProduct)
                        {
                            //给订单从表字段赋值
                            soItem = new SOItemDTO();
                            
                            soItem.ProductId = product.ProductId;
                            //soItem.ProductName = product.ProductName;
                            soItem.CustomerOrderNum = product.CustomerOrderNum;
                            soItem.CustomerOrderPrice = product.CustomerOrderPrice;
                            soItem.RebateNum = product.RebateNum;
                            soItem.RebateAmt = product.RebateAmt;
                            soItemList.Add(soItem);
                            productsAmt += soItem.CustomerOrderNum * soItem.CustomerOrderPrice;
                            soAmt += soItem.CustomerOrderNum * (soItem.CustomerOrderPrice - product.RebateAmt);
                        }
                        //给订单主表字段赋值
                        so = new SODTO();
                        so.OrderType = storeSOMasterDTO.OrderType;
                        so.CustomerLegalId = storeSOMasterDTO.CustomerLegalId;
                        so.DeliveryId = storeSOMasterDTO.DeliveryId;
                        so.ReceiveAddressId = storeSOMasterDTO.ReceiveAddressId;
                        so.OrderFrom = storeSOMasterDTO.OrderFrom;
                        so.SOAmt = soAmt;
                        so.ProductsAmt = productsAmt;
                        so.TotalRebate = productsAmt-soAmt;
                        so.Memo = storeSOMasterDTO.Memo;
                        so.Items = soItemList;
                        so.R3ProductGroupId = groupId;
                        so.CustomerLegalId = orgId;

                        //将订单添加到订单列表
                        soList.Add(so);
                    }
                } 
            }

            return soList;
        }
        #endregion

    }

    #region 实体

    /// <summary>
    /// 销售组织订单
    /// </summary>
    public class SOByOrgDTO 
    {
        /// <summary>
        /// 销售组织
        /// </summary>
        public int OrgId { get; set; }
        /// <summary>
        /// 该销售组织下的订单
        /// </summary>
        public List<SOByGroupDTO> SOByGroupList { get; set; }
    }

    /// <summary>
    /// 产品组订单
    /// </summary>
    public class SOByGroupDTO
    {
        /// <summary>
        /// 产品组
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 该产品组下的订单
        /// </summary>
        public List<SODTO> SOList { get; set; }
    }

    /// <summary>
    /// StoreSOMasterDTO
    /// </summary>
    public class StoreSOMasterDTO
    {
        public StoreSOMasterDTO() { }

        /// <summary>
        /// 是否人工审核
        /// </summary>
        public int UserAduit { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public int R3StockLocationId { get; set; }

        /// <summary>
        /// 售达方(客户id)
        /// </summary>
        public int CustomerLegalId { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        public int DeliveryId { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public int ReceiveAddressId { get; set; }

        /// <summary>
        /// 客户备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public OrderFrom OrderFrom { get; set; }
    }

    /// <summary>
    /// OrderProductDTO
    /// </summary>
    public class StoreSOItemDTO
    {

        public StoreSOItemDTO() { }

        /// <summary>
        /// 产品编号
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 产品数量
        /// </summary>
        public int CustomerOrderNum { get; set; }
        /// <summary>
        /// 产品单价
        /// </summary>
        public decimal CustomerOrderPrice { get; set; }
        /// <summary>
        /// 销售组织
        /// </summary>
        public int CustomerLegalOrgId { get; set; }
        /// <summary>
        /// 产品组
        /// </summary>
        public int R3ProductGroupId { get; set; }

        /// <summary>
        /// 返利编号
        /// </summary>
        public int RebateNum { get; set; }

        /// <summary>
        /// 返利金额
        /// </summary>
        public decimal RebateAmt { get; set; }
        
    }
    #endregion

    
}
