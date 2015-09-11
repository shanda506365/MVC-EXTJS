using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Dto;
using USO.Domain;
using USO.Infrastructure;


namespace USO.Test
{
    public class GavinTest
    {
        //private USOEntities uso;
        //public Organization 多媒体驻外分公司对象;
        //public Organization 成都分公司对象;
        //public Organization 内江分公司对象;
        //public Organization 成都经营部对象;
        //public Organization 邛崃经营部对象;
        //public Organization 内江经营部对象;

        //public CustomerLegal _资阳市飞龙电器有限责任公司;
        //public CustomerLegal _四川泸州万福商贸有限公司;
        //public CustomerLegal _泸州市纳溪区大渡龙氏电器经营部;
        ///// <summary>
        ///// 初始化数据库多媒体驻外分公司，成都分公司，内江分公司，成都经营，内江经营部，邛崃经营部
        ///// </summary>
        //public void Init()
        //{  
        //    uso=new USOEntities();
        //    多媒体驻外分公司对象 = AddOrganization("多媒体驻外分公司(业务集成)", 0, OrganizationType.Head);           
        //    成都分公司对象 = AddOrganization("成都分公司(业务集成)", 多媒体驻外分公司对象.Id, OrganizationType.Branch);            
        //    内江分公司对象 = AddOrganization("内江分公司(业务集成)", 多媒体驻外分公司对象.Id, OrganizationType.Branch);           
        //    成都经营部对象 = AddOrganization("成都经营部(业务集成)", 成都分公司对象.Id, OrganizationType.Department);           
        //    邛崃经营部对象 = AddOrganization("邛崃经营部(业务集成)", 成都分公司对象.Id, OrganizationType.Department);           
        //    内江经营部对象 = AddOrganization("内江经营部(业务集成)", 内江分公司对象.Id, OrganizationType.Department);

        //    _泸州市纳溪区大渡龙氏电器经营部 = AddCustomerLegal("bz001","泸州市纳溪区大渡龙氏电器经营部");
        //    _四川泸州万福商贸有限公司 = AddCustomerLegal("bz002", "四川泸州万福商贸有限公司");
        //    _资阳市飞龙电器有限责任公司 = AddCustomerLegal("bz003", "资阳市飞龙电器有限责任公司");

        //    AddCustomerLegalOrg(_泸州市纳溪区大渡龙氏电器经营部.Id, 内江经营部对象.Id);
        //    AddCustomerLegalOrg(_四川泸州万福商贸有限公司.Id, 内江经营部对象.Id);
        //    AddCustomerLegalOrg(_资阳市飞龙电器有限责任公司.Id, 内江经营部对象.Id);

        //    AddR3CustomerPlace(_泸州市纳溪区大渡龙氏电器经营部.Id, "资阳市和平路北段18号");
        //    AddR3CustomerPlace(_四川泸州万福商贸有限公司.Id, "泸县福集镇");
        //    AddR3CustomerPlace(_资阳市飞龙电器有限责任公司.Id, "资阳市和平路北段18号");
        //}
        //private void AddR3CustomerPlace(int CustomerLegalId, String place)
        //{
        //    var result = uso.R3CustomerPlace.FirstOrDefault(p => p.CustomerLegalId == CustomerLegalId && p.UnloadPlace == place);
        //    if (result == null)
        //    {
        //        result = new R3CustomerPlace()
        //        {
        //            CustomerLegalId = CustomerLegalId,
        //            UnloadPlace = place
        //        };
        //        uso.R3CustomerPlace.Add(result);
        //    }
        //}
        //private void AddCustomerLegalOrg(int CustomerLegalId, int OrganizationId)
        //{
        //    var result = uso.CustomerLegalOrgs.FirstOrDefault(p => p.CustomerLegalId == CustomerLegalId && p.OrganizationId == OrganizationId);
        //    if (result == null)
        //    {
        //        result = new CustomerLegalOrg()
        //        {
        //            CustomerLegalId = CustomerLegalId,
        //            OrganizationId = OrganizationId
        //        };
        //        uso.CustomerLegalOrgs.Add(result);
        //    }
        //}
        //private CustomerLegal AddCustomerLegal(String code,String name)
        //{
        //    var result = uso.CustomerLegals.FirstOrDefault(p => p.CustomerName == name);
        //    if (result == null)
        //    {
        //        result = new CustomerLegal()
        //        {
        //            LegalNumber = code,
        //            CustomerName = name,
        //            LegalPerson = "张三",
        //            BisinessLicense = "123"
        //        };
        //        uso.CustomerLegals.Add(result);
        //    }
        //    return result;
        //}
        //private Organization AddOrganization(String orgname, int parent, OrganizationType type)
        //{
             
        //     var result = uso.Organizations.FirstOrDefault(p => p.Name == orgname);
        //     if (result == null)
        //     {
        //         result = new Organization()
        //         {
        //             Name = orgname,
        //             Description = orgname,
        //             SortOrder = 1,
        //             ParentId = parent,
        //             NodeId = new Guid(),
        //             CreatedOn = DateTime.Now,
        //             CreatedBy = 1,
        //             ModifiedOn = DateTime.Now,
        //             ModifiedBy = 1,
        //             OrganizationType = OrganizationType.Head,
        //         };
        //         uso.Organizations.Add(result);
        //     };
        //     return result;
        //}

    }
}
