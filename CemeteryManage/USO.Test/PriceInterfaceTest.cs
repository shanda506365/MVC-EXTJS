using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using USO.Dto;
using USO.Infrastructure.Services;

namespace USO.Test
{
    [TestFixture]
    public class PriceInterfaceTest
    {
        private IPriceService _priceService;
        private const int ExistsProductId = 1;
        private const int NotExistsProductId = -999;
        private const int ExistsCusomerId = 1;
        private const int NotExistsCustomerId = -999;

        [SetUp]
        public void Init()
        {
            //_priceService = new PriceService();
        }

        [TearDown]
        public void Clear()
        {
            _priceService = null;
        }

        [Test]
        public void Test_GetPrices_With_Exists_ProductId()
        {
            //var actual = _priceService.GetPrices(ExistsProductId, ExistsCusomerId, 0);

            //Assert.IsNotNull(actual);
            //Assert.IsNull(actual.CustomerPrice);
        }
    }
}
