using NUnit.Framework;
using USO.Infrastructure.Services;

namespace USO.Test
{
    [TestFixture]
    public class CustomerInterfaceTest
    {
        private ICustomerInfoService _customerInfoService;
        private const int ExistsCustomerId = 1;
        private const int NotExistsCustomerId = -999;
        private const int ExistsCustomerLegalId = 1;
        private const int NotExistsCustomerLegalId = -999;

        [SetUp]
        public void Init()
        {
            //_customerInfoService = new CustomerInfoService();
        }

        [TearDown]
        public void Clear()
        {
            _customerInfoService = null;
        }

        [Test]
        public void Test_GetSalesChannel_With_Exists_CustomerId()
        {
            var actual = _customerInfoService.GetSalesChannelByCustomerLegalId(ExistsCustomerId);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
            Assert.IsTrue(actual[0].ChannelName == "连锁");
        }

        [Test]
        public void Test_GetSalesChannel_With_NotExists_CustomerId()
        {
            var actual = _customerInfoService.GetSalesChannelByCustomerLegalId(NotExistsCustomerId);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count <= 0);
        }

        [Test]
        public void Test_GetCustomerInfo_With_No_Condition()
        {
            var actual = _customerInfoService.GetCustomerLegalInfo(null, null, null);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
        }

        [Test]
        public void Test_GetCustomerInfo_With_Exists_CustomerLegalId()
        {
            var actual = _customerInfoService.GetCustomerLegalInfo(ExistsCustomerLegalId, null, null);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
        }

        [Test]
        public void Test_GetCustomerInfo_With_NotExists_CustomerLegalId()
        {
            var actual = _customerInfoService.GetCustomerLegalInfo(NotExistsCustomerLegalId, null, null);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count <= 0);
        }

        [Test]
        public void Test_GetCustomerOrganizationsByCustomerLegalId_With_Exists_CustomerId()
        {
            var actual = _customerInfoService.GetCustomerOrganizationsByCustomerLegalId(40);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
        }

        [Test]
        public void Test_GetCustomerOrganizationsByCustomerLegalId_With_NotExists_CustomerId()
        {
            var actual = _customerInfoService.GetCustomerOrganizationsByCustomerLegalId(NotExistsCustomerLegalId);

            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.Count > 0);
        }

        [Test]
        public void Test_GetProductCompayByCustomerLegalId_With_Exists_CustomerLegaId()
        {
            var actual = _customerInfoService.GetCustomerOrganizationsByCustomerLegalId(ExistsCustomerId);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
        }

        [Test]
        public void Test_GetProductCompayByCustomerLegalId_With_NotExists_CustomerLegaId()
        {
            var actual = _customerInfoService.GetCustomerOrganizationsByCustomerLegalId(NotExistsCustomerId);

            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.Count > 0);
        }
    }
}
