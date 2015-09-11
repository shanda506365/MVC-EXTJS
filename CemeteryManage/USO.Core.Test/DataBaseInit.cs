using System.Diagnostics;
using NUnit.Framework;
using System;
using System.Data.Entity;
using USO.Domain;
using USO.Infrastructure;
using System.Linq;
using System.Collections.Generic;


namespace USO.Core.Test
{
    public class DataBaseInit
    {
        [Test]
        public void D()
        {
            int? a = null;
            a = a ?? 0 + 1;

            Assert.AreEqual(1, a);

            a = a ?? 0 + 1;

            Assert.AreEqual(2, a);
        }


        [Test]
        public void ReCreateDataBase()
        {
            Console.WriteLine("Begin Creating database.....");
            Database.SetInitializer<USOEntities>(new DropCreateDatabaseAlways<USOEntities>());
            var usoEntities = new USOEntities();
            usoEntities.SaveChanges();
            Console.WriteLine("DataBase Created......");
        }

        [Test]
        public void TestInsertCustomer1()
        {
            var usoEntities = new USOEntities();
            //法人
            var customerLegal = new CustomerLegal
                {
                    LegalNumber = "0000001",
                    CustomerName = "邛崃市正大电器公司",
                    LegalPerson = "张三",
                    BisinessLicense = "00000000000000001",
                    Contact = "028-88888888"
                };
            usoEntities.CustomerLegals.Add(customerLegal);


            var org = usoEntities.Organizations.Find(8);
            //法人组织
            var customerLegalOrg = new CustomerLegalOrg
                {
                    CustomerLegal = customerLegal,
                    Organization = org,
                };
            usoEntities.CustomerLegalOrgs.Add(customerLegalOrg);

            //门店1
            var customerStore = new CustomerStore
                {
                    CustomerLegal = customerLegal,
                    StoreName = "邛崃东街经营部",
                    StoreAddress = "邛崃市东街101号",
                    StoreType = usoEntities.Dictionaries.Where(p => p.FieldName == "StoreType"&&p.FieldValue == "1").FirstOrDefault().Id
                };
            usoEntities.CustomerStores.Add(customerStore);


            //法人集团关系
            var r3ProductCompany1 =
                usoEntities.Organizations.FirstOrDefault(p => p.OrganizationType == OrganizationType.Head);
            var r3CustomerLegal = new R3CustomerLegal
                {
                    CustomerLegal = customerLegal,
                    Organization = r3ProductCompany1,
                    R3CustomerCode = "000001",
                };
            usoEntities.R3CustomerLegals.Add(r3CustomerLegal);

            //法人门店集团关系
           //var r3ProductCompany2 = usoEntities.R3ProductCompanys.Find(1);
           // var r3CustomerStore = new R3CustomerStore
           //     {
           //         CustomerStore = customerStore,
           //         R3ProductCompany = r3ProductCompany2,
           //         R3CustomerCode = "000001001"
           //     };
           // usoEntities.R3CustomerStores.Add(r3CustomerStore);

            //法人账号（管理员）
            var customerUserAdmin = new CustomerUser
                {
                    CustomerLegal = customerLegal,
                    UserName = "0001",
                    Password = "123",
                    CustomerRole = CustomerRole.Administrator,
                    CustomerUserStatus = CustomerUserStatus.Valid,
                };
            usoEntities.CustomerUsers.Add(customerUserAdmin);

            //法人账号（产品经理）
            var customerUserProductManager = new CustomerUser
            {
                CustomerLegal = customerLegal,
                UserName = "0002",
                Password = "123",
                CustomerRole = CustomerRole.ProductManager,
                CustomerUserStatus = CustomerUserStatus.Valid,
            };
            usoEntities.CustomerUsers.Add(customerUserProductManager);

            //法人账号(店员)
            var customerUserShopAssistant = new CustomerUser
            {
                CustomerLegal = customerLegal,
                UserName = "0003",
                Password = "123",
                CustomerRole = CustomerRole.ShopAssistant,
                CustomerUserStatus = CustomerUserStatus.Valid,
            };
            usoEntities.CustomerUsers.Add(customerUserShopAssistant);

            usoEntities.SaveChanges();
        }


        [Test]
        public void TestInsertCustomerStore1()
        {
            var usoEntities = new USOEntities();
            var customerLegal = usoEntities.CustomerLegals.Find(1);
            //门店1
            var customerStore = new CustomerStore
            {
                CustomerLegal = customerLegal,
                StoreName = "邛崃西街经营部",
                StoreAddress = "邛崃市西街999号",
                StoreType = usoEntities.Dictionaries.Where(p => p.FieldName == "StoreType" && p.FieldValue == "1").FirstOrDefault().Id
            };
            usoEntities.CustomerStores.Add(customerStore);


            //var r3ProductCompany = usoEntities.R3ProductCompanys.Find(1);


            ////法人门店集团关系
            //var r3CustomerStore = new R3CustomerStore
            //{
            //    CustomerStore = customerStore,
            //    R3ProductCompany = r3ProductCompany,
            //    R3CustomerCode = "000001002"
            //};
            //usoEntities.R3CustomerStores.Add(r3CustomerStore);

            //法人账号（管理员）
            var customerUserAdmin = new CustomerUser
            {
                CustomerLegal = customerLegal,
                UserName = "0001",
                Password = "123",
                CustomerRole = CustomerRole.Administrator,
                CustomerUserStatus = CustomerUserStatus.Valid,
            };
            usoEntities.CustomerUsers.Add(customerUserAdmin);

            //法人账号（产品经理）
            var customerUserProductManager = new CustomerUser
            {
                CustomerLegal = customerLegal,
                UserName = "0002",
                Password = "123",
                CustomerRole = CustomerRole.ProductManager,
                CustomerUserStatus = CustomerUserStatus.Valid,
            };
            usoEntities.CustomerUsers.Add(customerUserProductManager);

            //法人账号(店员)
            var customerUserShopAssistant = new CustomerUser
            {
                CustomerLegal = customerLegal,
                UserName = "0003",
                Password = "123",
                CustomerRole = CustomerRole.ShopAssistant,
                CustomerUserStatus = CustomerUserStatus.Valid,
            };
            usoEntities.CustomerUsers.Add(customerUserShopAssistant);
            usoEntities.SaveChanges();
        }

        [Test]
        public void TestInsertCustomer2()
        {
            var usoEntities = new USOEntities();
            //法人
            var customerLegal = new CustomerLegal
            {
                LegalNumber = "0000002",
                CustomerName = "邛崃市千禧电器公司",
                LegalPerson = "李四",
                BisinessLicense = "00000000000000002",
                Contact = "028-8777777"
            };
            usoEntities.CustomerLegals.Add(customerLegal);


            var org = usoEntities.Organizations.Find(8);
            //法人组织
            var customerLegalOrg = new CustomerLegalOrg
            {
                CustomerLegal = customerLegal,
                Organization = org,
            };
            usoEntities.CustomerLegalOrgs.Add(customerLegalOrg);

            //门店1
            var customerStore = new CustomerStore
            {
                CustomerLegal = customerLegal,
                StoreName = "邛崃建设路经营部",
                StoreAddress = "邛崃市建设路111号",
                StoreType = usoEntities.Dictionaries.Where(p => p.FieldName == "StoreType" && p.FieldValue == "1").FirstOrDefault().Id
            };
            usoEntities.CustomerStores.Add(customerStore);


            var r3ProductCompany =
                usoEntities.Organizations.FirstOrDefault(p => p.OrganizationType == OrganizationType.Head);
            //法人集团关系
            var r3CustomerLegal = new R3CustomerLegal
            {
                CustomerLegal = customerLegal,
                Organization = r3ProductCompany,
                R3CustomerCode = "000002",
            };
            usoEntities.R3CustomerLegals.Add(r3CustomerLegal);

            //法人门店集团关系
            //var r3CustomerStore = new R3CustomerStore
            //{
            //    CustomerStore = customerStore,
            //    R3ProductCompany = usoEntities.R3ProductCompanys.Find(1),
            //    R3CustomerCode = "000002001"
            //};
            //usoEntities.R3CustomerStores.Add(r3CustomerStore);

            //法人账号（管理员）
            var customerUserAdmin = new CustomerUser
            {
                CustomerLegal = customerLegal,
                UserName = "0001",
                Password = "123",
                CustomerRole = CustomerRole.Administrator,
                CustomerUserStatus = CustomerUserStatus.Valid,
            };
            usoEntities.CustomerUsers.Add(customerUserAdmin);

            //法人账号（产品经理）
            var customerUserProductManager = new CustomerUser
            {
                CustomerLegal = customerLegal,
                UserName = "0002",
                Password = "123",
                CustomerRole = CustomerRole.ProductManager,
                CustomerUserStatus = CustomerUserStatus.Valid,
            };
            usoEntities.CustomerUsers.Add(customerUserProductManager);

            //法人账号(店员)
            var customerUserShopAssistant = new CustomerUser
            {
                CustomerLegal = customerLegal,
                UserName = "0003",
                Password = "123",
                CustomerRole = CustomerRole.ShopAssistant,
                CustomerUserStatus = CustomerUserStatus.Valid,
            };
            usoEntities.CustomerUsers.Add(customerUserShopAssistant);

            usoEntities.SaveChanges();
        }

        private void PriceInit(int productId, int customerId, int companyId, int departmentId)
        {
            var context = new USOEntities();
            int PRODUCT_ID = productId;
            //法人id为40的测试数据
            //const int ORGANIZATION_ID_BRANCHCOMPANY = 16;//成都分公司
            //const int ORGANIZATION_ID_DEPARTMENT = 18;//邛崃经营部
            //const int CUSTOMER_ID = 40;
            //const decimal COST_PRICE = 100;
            //const decimal ACCT_PRICE = 110;
            //const decimal REDLINE_PRICE_BRACHCOMPANY = 120;
            //const decimal REDLINE_PRICE_DEPARTMENT = 130;
            //const decimal CUSTOMER_PRICE = 140;

            //法人id为41的测试数据
            int ORGANIZATION_ID_BRANCHCOMPANY = companyId;//成都分公司
            int ORGANIZATION_ID_DEPARTMENT = departmentId;//邛崃经营部
            int CUSTOMER_ID = customerId;
            const decimal COST_PRICE = 100;
            const decimal ACCT_PRICE = 110;
            const decimal REDLINE_PRICE_BRACHCOMPANY = 120;
            const decimal REDLINE_PRICE_DEPARTMENT = 130;
            const decimal CUSTOMER_PRICE = 140;


            var product = context.Product.FirstOrDefault(p => p.Id == PRODUCT_ID);
            Assert.IsNotNull(product);

            var organizationBranchCompay =
                context.Organizations.FirstOrDefault(o => o.Id == ORGANIZATION_ID_BRANCHCOMPANY);
            Assert.IsNotNull(organizationBranchCompay);

            var organizationDepartment =
                context.Organizations.FirstOrDefault(o => o.Id == ORGANIZATION_ID_DEPARTMENT);
            Assert.IsNotNull(organizationDepartment);

            var costPrice = context.CostPrices.FirstOrDefault(c => c.ProductId == PRODUCT_ID);
            if (costPrice != null)
            {
                context.CostPrices.Attach(costPrice);
                costPrice.StandardCostCurrentMonth = COST_PRICE;

            }
            else
            {
                costPrice = new CostPrice()
                {
                    ProductId = PRODUCT_ID,
                    StandardCostCurrentMonth = COST_PRICE,
                    BusinessTime = DateTime.UtcNow
                };
                context.CostPrices.Add(costPrice);
            }

            var redLinePriceBranchCompany = context.RedLinePrices.FirstOrDefault(r => r.ProductId == PRODUCT_ID && r.OrganizationId == ORGANIZATION_ID_BRANCHCOMPANY);
            var redLinePriceDepartment = context.RedLinePrices.FirstOrDefault(r => r.ProductId == PRODUCT_ID && r.OrganizationId == ORGANIZATION_ID_DEPARTMENT);

            if (redLinePriceBranchCompany != null)
            {
                context.RedLinePrices.Attach(redLinePriceBranchCompany);
                redLinePriceBranchCompany.Price = REDLINE_PRICE_BRACHCOMPANY;
                redLinePriceBranchCompany.InternalAccountPrice = ACCT_PRICE;

            }
            else
            {
                redLinePriceBranchCompany = new RedLinePrice()
                {
                    ProductId = PRODUCT_ID,
                    OrganizationId = ORGANIZATION_ID_BRANCHCOMPANY,
                    InternalAccountPrice = ACCT_PRICE,
                    Price = REDLINE_PRICE_BRACHCOMPANY,
                    BusinessTime = DateTime.UtcNow
                };
                context.RedLinePrices.Add(redLinePriceBranchCompany);
            }

            if (redLinePriceDepartment != null)
            {
                context.RedLinePrices.Attach(redLinePriceDepartment);
                redLinePriceDepartment.Price = REDLINE_PRICE_DEPARTMENT;
                redLinePriceDepartment.InternalAccountPrice = ACCT_PRICE;


            }
            else
            {
                redLinePriceDepartment = new RedLinePrice()
                {
                    ProductId = PRODUCT_ID,
                    OrganizationId = ORGANIZATION_ID_DEPARTMENT,
                    InternalAccountPrice = ACCT_PRICE,
                    Price = REDLINE_PRICE_DEPARTMENT,
                    BusinessTime = DateTime.UtcNow
                };
                context.RedLinePrices.Add(redLinePriceDepartment);
            }

            var customerPrice = context.CustomerPricePolicies.FirstOrDefault(c => c.ProductId == PRODUCT_ID && c.OrganizationId == ORGANIZATION_ID_DEPARTMENT && c.CustomerType == CustomerType.CustomerId && c.TypeValue == customerId);
            if (customerPrice != null)
            {
                context.CustomerPricePolicies.Attach(customerPrice);
                customerPrice.Price = CUSTOMER_PRICE;

            }
            else
            {
                customerPrice = new CustomerPricePolicy()
                {
                    ProductId = PRODUCT_ID,
                    OrganizationId = ORGANIZATION_ID_DEPARTMENT,
                    CustomerType = CustomerType.CustomerId,
                    TypeValue = CUSTOMER_ID,
                    Price = CUSTOMER_PRICE,
                    EffectiveOn = DateTime.UtcNow
                };
                context.CustomerPricePolicies.Add(customerPrice);
            }


            context.SaveChanges();
        }

        [Test]
        //初始化产品各层级价格，初始化产品id为25-50，经营部id为8，分公司id为12的各级价格
        public void PriceDataInit()
        {
            for (int i = 57; i <= 58; i++)
            {
                PriceInit(i, 40, 16, 18);
                PriceInit(i, 41, 17, 20);
            }
        }

        [Test]
        public void PriceDateInit1()
        {
            PriceInit(57, 40, 19, 16);
        }
    }
}
