using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using USO.Domain.Inventory;
using USO.Infrastructure;
using USO.Infrastructure.Security;
using USO.Infrastructure.Services;
using USO.Infrastructure.Services.Inventory;

namespace USO.Test
{
   [TestFixture]
   public class LeeliTest
    {
       readonly IDatabaseContext _databaseContext = new USOEntities();
       private  IInventoryService _inventoryService;

       [SetUp]
       public void Init()
       {
           IOrganizationStockInfoService orgStockInfoService = new OrganizationStockInfoService(_databaseContext, null);
           IOrganizationInventoryService organizationInventoryService =
               new OrganizationInventoryService(_databaseContext, null);
           IOrganizationWarehouseLocationService organizationWarehouseLocationService = new OrganizationWarehouseLocationService(_databaseContext,null);
           IAnnotationService annotationService = new AnnotationService(_databaseContext, null, null);
           IOccupationLogService occupationLogService = new OccupationLogService(_databaseContext);
           //_inventoryService = new InventoryService(_databaseContext, organizationWarehouseLocationService, orgStockInfoService, organizationInventoryService, null, annotationService, occupationLogService);
       }

       [TearDown]
       public void Clear()
       {
           _inventoryService = null;
       }

       [Test]
       public void Test_ScpDeliveryPlanService()
       {
           try
           {
               var test0 = _inventoryService.GetProductOnlineQuantity(11, 8393, 38531);
               var test1 = _inventoryService.GetProductOnlineQuantity(638, 8393, 38531);
               //var test2 = _inventoryService.RefreshInventory(38531, 8393, "CH5002563", false);
               //string sql =
               //    "SELECT top 1000  d.Id AS WarehouseId,e.R3Code as R3Code FROM uso.dbo.ProductInventory AS a WITH(NOLOCK)" +
               //    " INNER JOIN uso.dbo.R3StockLocation AS b WITH(NOLOCK) ON a.R3StockLocationId = b.Id " +
               //    " INNER JOIN uso.dbo.WarehouseR3StockLocation AS c WITH(NOLOCK) ON b.Id = c.R3StockLocationId" +
               //    " INNER JOIN uso.dbo.Warehouse AS d WITH(NOLOCK) ON c.WarehouseId = d.Id" +
               //    " INNER JOIN uso.dbo.ProductionFactory AS f WITH(NOLOCK) ON b.FactoryId = f.Id" +
               //    " INNER JOIN uso.dbo.Product AS e WITH(NOLOCK) ON a.ProductId = e.Id" +
               //    " ORDER BY NEWID()";
               //var res = _databaseContext
               // .SqlQuery<StockCodeFactoryCodeDto>(sql);

               //foreach (var dto in res)
               //{
               //    _inventoryService.Test(dto.R3Code, dto.WarehouseId.ToString());
               //}

               //_inventoryService.Test("ML9000143", "9461");



               //bool sss = _inventoryService.RefreshInventory(8269, "CH1001980", true);
               //_inventoryService.UpdateOrgInventoryInWarehouseQuantity1(1352, 74287, 9511, 10, OccupationOrderType.SalesOrder, 1449, 0);
           }
           catch (Exception ex)
           {
               throw ex;
           }
           //IScpDeliveryPlanService service = new ScpDeliveryPlanService(_databaseContext, null);
           //service.InventoryAssignFunction(); 
       }
    }

    public class StockCodeFactoryCodeDto
    {
        public int WarehouseId { get; set; }
        public string R3Code { get; set; }
    }
}
