using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using USO.Domain;
using USO.Dto;
using USO.Infrastructure.Mappers;

namespace USO.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly CustomerMapper _customerMapper;

        public CustomerService(IDatabaseContext databaseContext, CustomerMapper customerMapper)
        {
            _databaseContext = databaseContext;
            _customerMapper = customerMapper;
        }
        /// <summary>
        /// 查询客户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public PagedResult<CustomerDTO> Find(CustomerQuery customerQuery)
        {
            Check.Argument.IsNotNull(customerQuery, "customerQuery");

            //Apply filtering    
            var query = _databaseContext.Customers.Where(customerQuery);
            var total = query.Count();
            query = SortMemeberHelper.SortingAndPaging<Customer>(query, customerQuery.sort, customerQuery.dir
                , customerQuery.page, customerQuery.limit);
          

            var resultSet = query.AsNoTracking().ToList().Select(r => _customerMapper.Map(r, true)).ToList();

            return new PagedResult<CustomerDTO>(resultSet, total);
        }
        /// <summary>
        /// 新建客户
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        public DataControlResult<CustomerDTO> Create(CustomerDTO csDto)
        {
            var result = new DataControlResult<CustomerDTO>();
            try
            {
                #region 赋值
                var customer = new Customer
                {
                    FullName = csDto.FullName,
                    LastName = csDto.LastName,
                    FirstName = csDto.FirstName,
                    MiddleName = csDto.MiddleName,
                    Remark = csDto.Remark,
                    Telephone = csDto.Telephone,
                    Phone = csDto.Phone,
                    OtherPhone = csDto.OtherPhone,
                    Address = csDto.Address,
                    BuryDate = csDto.BuryDate,
                    DeathDate = csDto.DeathDate,
                    IDNumber = csDto.IDNumber
                };
                if (csDto.CustomerTypeId.HasValue && csDto.CustomerTypeId > 0)
                {
                    customer.CustomerTypeId = csDto.CustomerTypeId;
                }
                if (csDto.LinkCustomerId.HasValue && csDto.LinkCustomerId > 0)
                {
                    customer.LinkCustomerId = csDto.LinkCustomerId;
                }
                if (csDto.CustomerStatusId.HasValue && csDto.CustomerStatusId > 0)
                {
                    customer.CustomerStatusId = csDto.CustomerStatusId;
                }
                if (csDto.NationalityId.HasValue && csDto.NationalityId > 0)
                {
                    customer.NationalityId = csDto.NationalityId;
                }
                #endregion

                _databaseContext.Customers.Add(customer);
                _databaseContext.SaveChanges();
                result.ResultOutDto = _customerMapper.Map(customer, false);
                result.code = MyErrorCode.ResOK;
                result.msg = string.Empty;
                result.success = true;
            }
            catch (Exception ex)
            {
                result.code = MyErrorCode.ResDBError;
                result.msg = ex.Message;
                result.success = false;
                result.ResultOutDto = null;
                return result;
            }
            return result;
        }

        /// <summary>
        /// 新建客户(批量)
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        public PagedResult<CustomerDTO> Create(List<CustomerDTO> csDtoList)
        {
            var errDtoList = new List<CustomerDTO>();
            var sucTotal = 0;

            foreach (var customerDto in csDtoList)
            {
               var result = Create(customerDto);
                if (result.success)
                {
                    sucTotal++;
                }
                else
                {
                    errDtoList.Add(customerDto);
                }
            }
            return new PagedResult<CustomerDTO>(errDtoList, sucTotal);
        }

        /// <summary>
        /// 更新客户信息
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        public DataControlResult<CustomerDTO> Update(CustomerDTO csDto)
        {
            var result = new DataControlResult<CustomerDTO>();
            try
            {
                var customer = _databaseContext.Customers.SingleOrDefault(n => n.Id == csDto.Id);
                if (customer == null)
                {
                    result.success = false;
                    result.msg = "该客户不存在";
                    result.code = MyErrorCode.ResParamError;
                    return result;
                }
                #region 赋值
                customer.FullName = csDto.FullName;
                customer.LastName = csDto.LastName;
                customer.FirstName = csDto.FirstName;
                customer.MiddleName = csDto.MiddleName;
                customer.Remark = csDto.Remark;
                customer.Telephone = csDto.Telephone;
                customer.Phone = csDto.Phone;
                customer.OtherPhone = csDto.OtherPhone;
                customer.Address = csDto.Address;
                customer.BuryDate = csDto.BuryDate;
                customer.DeathDate = csDto.DeathDate;
                customer.IDNumber = csDto.IDNumber;
                if (csDto.CustomerTypeId.HasValue && csDto.CustomerTypeId > 0)
                {
                    customer.CustomerTypeId = csDto.CustomerTypeId;
                }
                if (csDto.LinkCustomerId.HasValue && csDto.LinkCustomerId > 0)
                {
                    customer.LinkCustomerId = csDto.LinkCustomerId;
                }
                if (csDto.CustomerStatusId.HasValue && csDto.CustomerStatusId > 0)
                {
                    customer.CustomerStatusId = csDto.CustomerStatusId;
                }
                if (csDto.NationalityId.HasValue && csDto.NationalityId > 0)
                {
                    customer.NationalityId = csDto.NationalityId;
                }

                #endregion
                _databaseContext.SaveChanges();
                result.ResultOutDto = _customerMapper.Map(customer, false); ;
                result.code = MyErrorCode.ResOK;
                result.msg = string.Empty;
                result.success = true;
            }
            catch (Exception ex)
            {
                result.code = MyErrorCode.ResDBError;
                result.msg = ex.Message;
                result.success = false;
                return result;
            }
            return result;
        }

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="csDtoList"></param>
        /// <returns></returns>
        public DataControlResult<CustomerDTO> Delete(List<CustomerDTO> csDtoList)
        {
            var result = new DataControlResult<CustomerDTO>();
            var stopFlag = false;
            try
            {
                foreach (var customerDto in csDtoList)
                {
                    var customer = _databaseContext.Customers.SingleOrDefault(n => n.Id == customerDto.Id);
                    if (customer == null)
                    {
                        stopFlag = true;
                        result.success = false;
                        result.msg = "该客户不存在";
                        result.code = MyErrorCode.ResParamError;
                        break;
                    }
                    _databaseContext.Customers.Remove(customer);
                }

                if (!stopFlag)
                {
                    result.code = MyErrorCode.ResOK;
                    result.msg = string.Empty;
                    result.success = true;
                    _databaseContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                result.code = MyErrorCode.ResDBError;
                result.msg = ex.Message;
                result.success = false;
                return result;
            }
            return result;
        }
    }
}
