//using System;
//using NUnit.Framework;
//using USO.Domain;
//using USO.Domain.Inventory;
//using USO.Infrastructure;
//using USO.Infrastructure.Mappers.Inventory;
//using USO.Infrastructure.Services;
//using USO.Infrastructure.Services.Inventory;
//using System.Data.Entity;
//using System.Linq;

//namespace USO.Test
//{
//    [TestFixture]
//    public class InventoryInterfaceTest
//    {
//        private IInventoryService _inventoryService;
//        private IInventoryOccupationOrderService _inventoryOccupationOrderService;

//        [SetUp]
//        public void Init()
//        {
//            //_inventoryService = new InventoryService(
//            //    new USOEntities(),
//            //    new OrganizationWarehouseLocationService(
//            //        new USOEntities(),
//            //        new OrganizationWarehouseLocationMapper()));

//            //_inventoryOccupationOrderService = new InventoryService(new USOEntities(),
//            //    new OrganizationWarehouseLocationService(
//            //        new USOEntities(),
//            //        new OrganizationWarehouseLocationMapper()));
//        }

//        [TearDown]
//        public void Clear()
//        {
//            _inventoryService = null;
//        }

//        [Test]
//        public void Test()
//        {
//            var quantity = _inventoryService.GetProductAvailableQuantity(8, 22, 28);

//            Assert.IsNotNull(quantity);
//            Assert.IsTrue(quantity > 0);

//            Console.WriteLine(quantity);

//        }

//        [Test]
//        public void DataInit()
//        {
//            var uso = new USOEntities();

//            string factoryName = "电视工厂";
//            string factoryCode = "9527";
//            var factory = uso.ProductionFactories.FirstOrDefault(p => p.Name == factoryName);
//            if (factory == null)
//            {
//                factory = new ProductionFactory()
//                    {
//                        Code = factoryCode,
//                        Name = factoryName,
//                        Status = 0
//                    };
//                uso.ProductionFactories.Add(factory);
//            }

//            string stockLocationName = "成都电视库位";
//            string stockLocationCode = "9527";
//            var stockLocation = uso.Warehouses.FirstOrDefault(l => l.Name == stockLocationName);
//            if (stockLocation == null)
//            {
//                stockLocation = new Warehouse()
//                    {
//                        Name = stockLocationName,
//                        // = stockLocationCode,
//                        //ProductionFactory = factory
//                    };
//                uso.Warehouses.Add(stockLocation);
//            }

//            var cdBranch = 7; //成都分公司
//            var qlDepartment = 8; //邛崃经营部


//            var orgStock =
//                uso.OrganizationStockInfos.FirstOrDefault(
//                    o => o.OrganizationId == qlDepartment
//                         && o.WarehouseId == stockLocation.Id
//                         && o.StockType == StockType.Department);

//            if (orgStock == null)
//            {
//                orgStock = new OrganizationStockInfo()
//                    {
//                        OrganizationId = qlDepartment,
//                        Warehouse = stockLocation,
//                        StockType = StockType.Department
//                    };
//                uso.OrganizationStockInfos.Add(orgStock);
//            }

//            var orgStockNoLimit =
//                uso.OrganizationStockInfos.FirstOrDefault(
//                    o => o.OrganizationId == null
//                         && o.WarehouseId == stockLocation.Id
//                         && o.StockType == StockType.NoLimit);
//            if (orgStockNoLimit == null)
//            {
//                orgStockNoLimit = new OrganizationStockInfo()
//                    {
//                        OrganizationId = null,
//                        Warehouse = stockLocation,
//                        StockType = StockType.NoLimit
//                    };
//                uso.OrganizationStockInfos.Add(orgStockNoLimit);
//            }

//            var orgStockBranch =
//               uso.OrganizationStockInfos.FirstOrDefault(
//                   o => o.OrganizationId == cdBranch
//                        && o.WarehouseId == stockLocation.Id
//                        && o.StockType == StockType.BrachCompany);
//            if (orgStockBranch == null)
//            {
//                orgStockBranch = new OrganizationStockInfo()
//                {
//                    OrganizationId = cdBranch,
//                    Warehouse = stockLocation,
//                    StockType = StockType.BrachCompany
//                };
//                uso.OrganizationStockInfos.Add(orgStockBranch);
//            }

//            for (int i = 1; i < 60; i++)
//            {
//                InitProductInventory(orgStock.Id, i, uso);
//                InitProductInventory(orgStockBranch.Id, i, uso);
//                InitProductInventory(orgStockNoLimit.Id, i, uso);
//            }

//            uso.SaveChanges();
//        }

//        private void InitProductInventory(int orgStockInfoId, int productId, USOEntities entities)
//        {
//            var productInventory = entities.OrganizationInventories.FirstOrDefault(i => i.OrganizationStockInfoID == orgStockInfoId && i.ProductId == productId);
//            if (productInventory == null)
//            {
//                entities.OrganizationInventories.Add(new OrganizationInventory()
//                    {
//                        OrganizationStockInfoID = orgStockInfoId,
//                        ProductId = productId,
//                        AvailableQuantity = 190,
//                        OccupationQuantity = 10,
//                        OnlineQuantity = 190,
//                        OwnQuantity = 200,
//                    });
//            }
//        }

//        [Test]
//        public void Test_AddProductOccupationQuantity()
//        {
//            _inventoryService.AddProductOccupationQuantity(83, OccupationOrderType.SalesOrder, 1);
//        }

//        [Test]
//        public void Test_SubtractProductOccupationQuantity()
//        {
//            _inventoryService.SubtractOccupationQuantity(83, OccupationOrderType.SalesOrder, 1);
//        }

//        [Test]
//        public void Test_FinishOccupationQuantity()
//        {
//            _inventoryService.AdjustInventoryAfterCreatedR3SalesOrder(83, OccupationOrderType.SalesOrder, 1);
//        }

//        [Test]
//        public void CreateInventoryOccupationOrderTest()
//        {
//            var entity = new InventoryOccupationOrder()
//                {
//                    Canceled = false,
//                    CanceledOn = DateTime.UtcNow,
//                    Finished = false,
//                    FinishedOn = DateTime.UtcNow,
//                    OrderId = 1,
//                    OrderType = OccupationOrderType.SalesOrder
//                };

//            _inventoryOccupationOrderService.CreateInventoryOccupationOrder(entity);

//        }

//        [Test]
//        public void GetOrdersTest()
//        {
//            var orders = _inventoryOccupationOrderService.GetOrders(new int[] { 1 });

//            Assert.IsNotNull(orders);
//            Assert.IsTrue(orders.Count > 0);
//        }

//        [Test]
//        public void CancelOrderTest()
//        {
//            _inventoryOccupationOrderService.CancelOrder(1);
//        }

//        [Test]
//        public void FinishOrder()
//        {
//            _inventoryOccupationOrderService.FinishOrder(1);
//        }

//        [Test]
//        public void Test_AdjustInventory()
//        {
//            IventoryAdjustmentUtil.AdjustInventory(5004, 57, 1, 1, 1);
//            IventoryAdjustmentUtil.AdjustInventory(5004, 57, -1, -1, -1);
//        }

//        [Test]
//        public void TestGroupSum()
//        {

//        }

//        [Test]
//        public void Test_GenerateOrganizationStockInfo()
//        {
//            //取得OrganizationWarehouseLocation表保存的OrganizationId和R3StockLocationId
//            //假定分别为1,1，这里的Organizationid为分公司Id
//            //1.判断OrganizationStockInfo表是否有StockType=0,R3StockLocaitonId=1,OrganizationID为null的数据
//            //如果不存在，插入数据
//            //2.判断判断OrganizationStockInfo表是否有StockType=1,R3StockLocaitonId=1,OrganizationID=1的数据
//            //如果不存在，插入数据
//            //3.根据上面的分公司OrganizationId取得所有下属经营部的Id
//            //遍历经营部Id,
//            //每次循环判断OrgnizationStockInfo表中是否有StockType=2,R3StockLocationId=1,OrganizationId=经营部Id的数据
//            //如果不存在，插入数据

//            Console.WriteLine(new Random().Next(100));

//        }

//        [Test]
//        public void Test_Delivery()
//        {

//        }

//        [Test]
//        public void Test_GetInventory()
//        {
//            var quantity = _inventoryService.GetProductAvailableQuantity(8, 1, 121);

//            Assert.IsNotNull(quantity);
//            Assert.IsTrue(quantity > 0);

//            Console.WriteLine(quantity); 
//        }


//        [Test]
//        public void ReCreateDataBase()
//        {
//            Console.WriteLine("Begin Creating database.....");
//            Database.SetInitializer<USOEntities>(new DropCreateDatabaseAlways<USOEntities>());
//            var usoEntities = new USOEntities();
//            usoEntities.SaveChanges();
//            Console.WriteLine("DataBase Created......");
//        }

//    }
//}
