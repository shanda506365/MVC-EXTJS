using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Core;
using USO.Domain;
using USO.Dto;
using USO.Infrastructure.Mappers;

namespace USO.Infrastructure.Services.BaseNum
{
    public interface IComboStoreService : IDependency
    {
        /// <summary>
        /// 获取所有的角色
        /// </summary>
        /// <returns></returns>
        List<RoleDTO> GetAllRoles();
        /// <summary>
        /// 获取所有的部门
        /// </summary>
        /// <returns></returns>
        List<DepartmentDTO> GetAllDepartments();
        /// <summary>
        /// 获取所有的付款状态
        /// </summary>
        /// <returns></returns>
        List<PaymentStatusDTO> GetAllPaymentStatus();
        /// <summary>
        /// 获取所有的墓碑类别
        /// </summary>
        /// <returns></returns>
        List<TombstoneTypeDTO> GetAllTombstoneType();
        /// <summary>
        /// 获取所有的区域
        /// </summary>
        /// <returns></returns>
        List<CemeteryAreasDTO> GetAllArea();
        /// <summary>
        /// 获取所有的行
        /// </summary>
        /// <returns></returns>
        List<CemeteryRowsDTO> GetAllRow();
        /// <summary>
        /// 获取所有的列
        /// </summary>
        /// <returns></returns>
        List<CemeteryColumnsDTO> GetAllColumn();
        /// <summary>
        /// 获取所有的保密级别
        /// </summary>
        /// <returns></returns>
        List<SecurityLevelDTO> GetAllSecurityLevel();
        /// <summary>
        /// 获取所有的服务级别
        /// </summary>
        /// <returns></returns>
        List<ServiceLevelDTO> GetAllServiceLevel();
        /// <summary>
        /// 获取所有的客户类型
        /// </summary>
        /// <returns></returns>
        List<CustomerTypeDTO> GetAllCustomerType();
        /// <summary>
        /// 获取所有的国籍
        /// </summary>
        /// <returns></returns>
        List<NationalityDTO> GetAllNationality();
        /// <summary>
        /// 获取所有的客户状态
        /// </summary>
        /// <returns></returns>
        List<CustomerStatusDTO> GetAllCustomerStatus();
    }

    public class ComboStoreService : IComboStoreService
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly CustomerTypeMapper _customerTypeMapper;
        private readonly NationalityMapper _nationalityMapper;
        private readonly CustomerStatusMapper _customerStatusMapper;
        private readonly CemeteryAreasMapper _cemeteryAreasMapper;
        private readonly CemeteryRowsMapper _cemeteryRowsMapper;
        private readonly CemeteryColumnsMapper _cemeteryColumnsMapper;
        private readonly SecurityLevelMapper _securityLevelMapper;
        private readonly ServiceLevelMapper _serviceLevelMapper;
        private readonly TombstoneTypeMapper _tombstoneTypeMapper;
        private readonly PaymentStatusMapper _paymentStatusMapper;
        private readonly IDepartmentMapper _departmentMapper;
        private readonly RoleMapper _roleMapper;

        public ComboStoreService(IDatabaseContext databaseContext, CustomerTypeMapper customerTypeMapper
            , NationalityMapper nationalityMapper, CustomerStatusMapper customerStatusMapper
            , CemeteryAreasMapper cemeteryAreasMapper, CemeteryRowsMapper cemeteryRowsMapper
            , CemeteryColumnsMapper cemeteryColumnsMapper, SecurityLevelMapper securityLevelMapper
            ,ServiceLevelMapper serviceLevelMapper,TombstoneTypeMapper tombstoneTypeMapper
            , PaymentStatusMapper paymentStatusMapper, IDepartmentMapper departmentMapper
            ,RoleMapper roleMapper)
        {
            _databaseContext = databaseContext;
            _customerTypeMapper = customerTypeMapper;
            _nationalityMapper = nationalityMapper;
            _customerStatusMapper = customerStatusMapper;
            _cemeteryAreasMapper = cemeteryAreasMapper;
            _cemeteryRowsMapper = cemeteryRowsMapper;
            _cemeteryColumnsMapper = cemeteryColumnsMapper;
            _securityLevelMapper = securityLevelMapper;
            _serviceLevelMapper = serviceLevelMapper;
            _tombstoneTypeMapper = tombstoneTypeMapper;
            _paymentStatusMapper = paymentStatusMapper;
            _departmentMapper = departmentMapper;
            _roleMapper = roleMapper;
        }

        /// <summary>
        /// 获取所有的角色
        /// </summary>
        /// <returns></returns>
        public List<RoleDTO> GetAllRoles()
        {
            var dtoList = _databaseContext.Roles.AsNoTracking().ToList().Select(r => _roleMapper.Map(r)).ToList();
            return dtoList;
        }

        /// <summary>
        /// 获取所有的部门
        /// </summary>
        /// <returns></returns>
        public List<DepartmentDTO> GetAllDepartments()
        {
            var dtoList = _databaseContext.Departments.AsNoTracking().ToList().Select(r => _departmentMapper.Map(r)).ToList();
            return dtoList;
        }

        /// <summary>
        /// 获取所有的付款状态
        /// </summary>
        /// <returns></returns>
        public List<PaymentStatusDTO> GetAllPaymentStatus()
        {
            var paymentStatusDtoList = _databaseContext.PaymentStatus.AsNoTracking().ToList().Select(r => _paymentStatusMapper.Map(r)).ToList();
            return paymentStatusDtoList;
        }

        /// <summary>
        /// 获取所有的墓碑类别
        /// </summary>
        /// <returns></returns>
        public List<TombstoneTypeDTO> GetAllTombstoneType()
        {
            var tombstoneDtoList = _databaseContext.TombstoneTypes.AsNoTracking().ToList().Select(r => _tombstoneTypeMapper.Map(r)).ToList();
            return tombstoneDtoList;
        }
        /// <summary>
        /// 获取所有的区域
        /// </summary>
        /// <returns></returns>
        public List<CemeteryAreasDTO> GetAllArea()
        {
            var cemeteryAreasDtoList = _databaseContext.CemeteryAreas.AsNoTracking().ToList().Select(r => _cemeteryAreasMapper.Map(r)).ToList();
            return cemeteryAreasDtoList;
        }
        /// <summary>
        /// 获取所有的行
        /// </summary>
        /// <returns></returns>
        public List<CemeteryRowsDTO> GetAllRow()
        {
            var cemeteryRowsDtoList = _databaseContext.CemeteryRows.AsNoTracking().ToList().Select(r => _cemeteryRowsMapper.Map(r)).ToList();
            return cemeteryRowsDtoList;
        }
        /// <summary>
        /// 获取所有的列
        /// </summary>
        /// <returns></returns>
        public List<CemeteryColumnsDTO> GetAllColumn()
        {
            var cemeteryColumnsDtoList = _databaseContext.CemeteryColumns.AsNoTracking().ToList().Select(r => _cemeteryColumnsMapper.Map(r)).ToList();
            return cemeteryColumnsDtoList;
        }
        /// <summary>
        /// 获取所有的保密级别
        /// </summary>
        /// <returns></returns>
        public List<SecurityLevelDTO> GetAllSecurityLevel()
        {
            var securityLevelDtoList = _databaseContext.SecurityLevels.AsNoTracking().ToList().Select(r => _securityLevelMapper.Map(r)).ToList();
            return securityLevelDtoList;
        }
        /// <summary>
        /// 获取所有的服务级别
        /// </summary>
        /// <returns></returns>
        public List<ServiceLevelDTO> GetAllServiceLevel()
        {
            var serviceLevelDtoList = _databaseContext.ServiceLevels.AsNoTracking().ToList().Select(r => _serviceLevelMapper.Map(r)).ToList();
            return serviceLevelDtoList;
        }

        /// <summary>
        /// 获取所有的客户类型
        /// </summary>
        /// <returns></returns>
        public List<CustomerTypeDTO> GetAllCustomerType()
        {
            var customerTypeDtoList = _databaseContext.CustomerTypes.AsNoTracking().ToList().Select(r => _customerTypeMapper.Map(r)).ToList();
            return customerTypeDtoList;
        }
        /// <summary>
        /// 获取所有的国籍
        /// </summary>
        /// <returns></returns>
        public List<NationalityDTO> GetAllNationality()
        {
            var nationalityDtoList = _databaseContext.Nationalitys.AsNoTracking().ToList().Select(r => _nationalityMapper.Map(r)).ToList();
            return nationalityDtoList;
        }
        /// <summary>
        /// 获取所有的客户状态
        /// </summary>
        /// <returns></returns>
        public List<CustomerStatusDTO> GetAllCustomerStatus()
        {
            var customerStatusDtoList = _databaseContext.CustomerStatus.AsNoTracking().ToList().Select(r => _customerStatusMapper.Map(r)).ToList();
            return customerStatusDtoList;
        }

       
    }
}
