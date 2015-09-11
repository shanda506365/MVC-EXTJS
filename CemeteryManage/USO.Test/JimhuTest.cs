using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NUnit.Framework;
using USO.Core;
using USO.Domain;
using USO.Dto;
using USO.Infrastructure;
using USO.Infrastructure.Mappers;
using USO.Infrastructure.Services;
using USO.Infrastructure.Services.Address;

namespace USO.Test
{
    [TestFixture]
    public class JimhuTest
    {
        readonly IDatabaseContext _databaseContext = new USOEntities();
        readonly IAddressMapper _addressMapper = new AddressMapper();
        private readonly IAddressService _addressService;


        [SetUp]
        public void Init()
        {
            //_addressService = _addressService;
        }

        [TearDown]
        public void Clear()
        {
            //_addressService = null;
        }
        public void Jimhuaddress()
        {
            GetAllParentList(7);
            //var id = 65375;
            //var result = _databaseContext.Address.SingleOrDefault(r => r.Id == id);
            //if (result == null)
            //    return null;
            //var info = _addressMapper.Map(result);
            //return info;
            //var countryId = 1;
            //var childlist = new List<AddressDTO>();
            //var provinces = _databaseContext.Address.Where(a => a.ParentId == countryId).AsNoTracking().ToList().Select(r => _addressMapper.Map(r)).ToList();
            //if (provinces.Count > 0)
            //{
            //    childlist.AddRange(provinces);//加入省列表

            //    foreach (var province in provinces)//取到市区列表
            //    {
            //        var provincetemp = province;
            //        var citys =
            //            _databaseContext.Address.Where(a => a.ParentId == provincetemp.Id)
            //                            .AsNoTracking()
            //                            .ToList()
            //                            .Select(r => _addressMapper.Map(r))
            //                            .ToList();
            //        if (citys.Count > 0)
            //        {
            //            childlist.AddRange(citys);
            //            foreach (var city in citys) //取到县列表
            //            {
            //                var citytemp = city;
            //                var districts =
            //                    _databaseContext.Address.Where(a => a.ParentId == citytemp.Id)
            //                                    .AsNoTracking()
            //                                    .ToList()
            //                                    .Select(r => _addressMapper.Map(r))
            //                                    .ToList();
            //                if (districts.Count > 0)
            //                {
            //                    childlist.AddRange(districts);
            //                    foreach (var district in districts)//取到街道列表
            //                    {
            //                        var districttemp = district;
            //                        var streets =
            //                            _databaseContext.Address.Where(a => a.ParentId == districttemp.Id)
            //                                            .AsNoTracking()
            //                                            .ToList()
            //                                            .Select(r => _addressMapper.Map(r))
            //                                            .ToList();
            //                        if (streets.Count > 0)
            //                        {
            //                            childlist.AddRange(streets);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //return childlist;

            //var type = 5;
            //if ((int)type > 1)
            //{
            //    var result = _databaseContext.Address.Where(a => a.AddressType == (AddressType)((int)type - 1))
            //        .AsNoTracking()
            //        .ToList()
            //        .Select(r => _addressMapper.Map(r))
            //        .ToList();
            //}
            //var countrys =
            //    _databaseContext.Address.Where(a => a.AddressType == AddressType.Country).AsNoTracking().ToList();
            //ar a = _addressService.GetCountryList();
            //_addressService.GetAllChildList(1);
        }

        [Test] public void Hybo()
        {
            var result =
                from a in
                    _databaseContext.CustomerLegalVendorProductGroups.Where(t => t.CustomerLegalVendorId == 105)
                                    .AsNoTracking()
                join b in _databaseContext.R3ProductLines on a.R3ProductLineId equals b.Id
                join c in _databaseContext.SystemProductCompanies on b.SystemProductCompanyId equals c.Id
                select a;
        }


        public void ExceptTest()
        {
            var a = new[] { 1, 2, 3 };
            var b = new[] { 1, 2 };
            var c = a.Except(b);
            var d = new[] { 3, 4, 5, 6 };
            var e = a.Except(d);
            Console.WriteLine(c.Count());
            Console.WriteLine(e.Count());
        }
        public List<AddressDTO> GetAllParentList(int id)
        {
            var prentList = new List<AddressDTO>();
            var address = _databaseContext.Address.SingleOrDefault(r => r.Id == id);
            if (address == null)
                return null;
            var info = _addressMapper.Map(address);
            var result = PrevFoo(info.ParentId, prentList);
            return result;
        }
        private List<AddressDTO> PrevFoo(int parentId, List<AddressDTO> prentList)
        {
            var panrent = _databaseContext.Address.SingleOrDefault(a => a.Id == parentId);
            if (panrent != null)
            {
                if (panrent.Id == parentId)
                {
                    var paddress = _addressMapper.Map(panrent);
                    prentList.Add(paddress);
                    PrevFoo(panrent.ParentId, prentList);
                }
                else
                {
                    int parentid = panrent.ParentId;
                    var address = _databaseContext.Address.SingleOrDefault(a => a.Id == panrent.ParentId);
                    if (address != null)
                    {
                        var paddress = _addressMapper.Map(address);
                        prentList.Add(paddress);
                    }
                    PrevFoo(parentid, prentList);
                }
            }
            return prentList;
        }
        public PagedResult<AddressDTO> Find(UsoAddressQuery addressQuery)
        {
            Check.Argument.IsNotNull(addressQuery, "addressQuery");
            var query = _databaseContext.Address
                            .Where(addressQuery);
            var total = query.Count();
            query = query.OrderByDescending(m => m.Id);
            if (addressQuery.PageSize > 0)
            {
                query = query.Skip((addressQuery.Page - 1) * addressQuery.PageSize);
            }
            query = query.Take(addressQuery.PageSize);


            var resultSet = query.AsNoTracking().ToList().Select(r => _addressMapper.Map(r)).ToList();
            return new PagedResult<AddressDTO>(resultSet, total);
        }

        //private List<AddressDTO> PrevFoo(int parentId, List<AddressDTO> prentList)
        //{
        //    var panrent = _databaseContext.Address.SingleOrDefault(a => a.Id == parentId);
        //    if (panrent != null)
        //    {
        //        int parentid = panrent.ParentId;
        //        var address = _databaseContext.Address.SingleOrDefault(a => a.Id == panrent.ParentId);
        //        if (address != null)
        //        {
        //            var paddress = _addressMapper.Map(address);
        //            prentList.Add(paddress);
        //        }
        //        PrevFoo(parentid, prentList);
        //    }
        //    return prentList;
        //}
        public static string GetFirstLetter()
        {
            var chineseStr = "中国";
            byte[] cBs = Encoding.Default.GetBytes(chineseStr);

            if (cBs.Length < 2)
                return chineseStr;

            string strFirstLetter = string.Empty;

            for (int i = 0; i < cBs.Length; i++)
            {

                if (cBs[i] < 0x80)
                {
                    strFirstLetter += Encoding.Default.GetString(cBs, i, 1);
                    continue;
                }

                var ucHigh = cBs[i];
                var ucLow = cBs[i + 1];
                if (ucHigh < 0xa1 || ucLow < 0xa1)
                    continue;
                int nCode = (ucHigh - 0xa0) * 100 + ucLow - 0xa0;

                string str = FirstLetter(nCode);
                strFirstLetter += str != string.Empty ? str : Encoding.Default.GetString(cBs, i, 2);
                i++;
            }
            return strFirstLetter;
        }

        /// <summary>
        /// Get the first letter of pinyin according to specified Chinese character code
        /// </summary>
        /// <param name="nCode">the code of the chinese character</param>
        /// <returns>receive the string of the first letter</returns>
        public static string FirstLetter(int nCode)
        {
            string strLetter = string.Empty;

            if (nCode >= 1601 && nCode < 1637) strLetter = "A";
            if (nCode >= 1637 && nCode < 1833) strLetter = "B";
            if (nCode >= 1833 && nCode < 2078) strLetter = "C";
            if (nCode >= 2078 && nCode < 2274) strLetter = "D";
            if (nCode >= 2274 && nCode < 2302) strLetter = "E";
            if (nCode >= 2302 && nCode < 2433) strLetter = "F";
            if (nCode >= 2433 && nCode < 2594) strLetter = "G";
            if (nCode >= 2594 && nCode < 2787) strLetter = "H";
            if (nCode >= 2787 && nCode < 3106) strLetter = "J";
            if (nCode >= 3106 && nCode < 3212) strLetter = "K";
            if (nCode >= 3212 && nCode < 3472) strLetter = "L";
            if (nCode >= 3472 && nCode < 3635) strLetter = "M";
            if (nCode >= 3635 && nCode < 3722) strLetter = "N";
            if (nCode >= 3722 && nCode < 3730) strLetter = "O";
            if (nCode >= 3730 && nCode < 3858) strLetter = "P";
            if (nCode >= 3858 && nCode < 4027) strLetter = "Q";
            if (nCode >= 4027 && nCode < 4086) strLetter = "R";
            if (nCode >= 4086 && nCode < 4390) strLetter = "S";
            if (nCode >= 4390 && nCode < 4558) strLetter = "T";
            if (nCode >= 4558 && nCode < 4684) strLetter = "W";
            if (nCode >= 4684 && nCode < 4925) strLetter = "X";
            if (nCode >= 4925 && nCode < 5249) strLetter = "Y";
            if (nCode >= 5249 && nCode < 5590) strLetter = "Z";
            return strLetter;
        }

    }
}
