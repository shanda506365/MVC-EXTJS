using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Core;
using USO.Domain;
using USO.Dto;

namespace USO.Infrastructure.Mappers
{

    public interface ITombstoneMapper : IDependency
    {
        TombstoneDTO Map(Tombstone entity, bool includeAll);
    }

    public class TombstoneMapper : ITombstoneMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public TombstoneMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public TombstoneDTO Map(Tombstone entity, bool includeAll)
        {
            var dto = LoadEntityData(entity, includeAll);
            return dto;
        }

        private TombstoneDTO LoadEntityData(Tombstone entity, bool includeAll)
        {

            var myDto = new TombstoneDTO()
            {
                Id = entity.Id,
                Name = entity.Name,
                AreaId = entity.AreaId,
                RowId = entity.RowId,
                ColumnId = entity.ColumnId,
                ParentId = entity.ParentId,
                Alias = entity.Alias,
                Remark = entity.Remark,
                CustomerId = entity.CustomerId,
                CustomerName = entity.CustomerName,
                StoneText = entity.StoneText,
                ExpiryDate = entity.ExpiryDate,
                BuyDate = entity.BuyDate,
                LastPaymentDate = entity.LastPaymentDate,
                BuryDate = entity.BuryDate,
                Width = entity.Width,
                Height = entity.Height,
                Acreage = entity.Acreage,
                SecurityLevelId = entity.SecurityLevelId,
                Image = entity.Image,
                ServiceLevelId = entity.ServiceLevelId,
                TypeId = entity.TypeId,
                PaymentStatusId = entity.PaymentStatusId,
                SortNum = entity.SortNum,
                CustomerBuryDtos = new List<CustomerDTO>(),
                AreaSort = "DESC",
                SupperManage = entity.SupperManage,
                ManageLimit = entity.ManageLimit
            };

            //包括所有关系
            if (includeAll)
            {
                if (entity.AreaId > 0)
                {
                    var cemeteryAreas = _databaseContext.CemeteryAreas.AsNoTracking().FirstOrDefault(a => a.Id == entity.AreaId);
                    CemeteryAreasDTO cemeteryAreasDto = null;
                    if (cemeteryAreas != null)
                    {
                        cemeteryAreasDto = new CemeteryAreasDTO
                        {
                            Id = cemeteryAreas.Id,
                            Name = cemeteryAreas.Name,
                            Alias = cemeteryAreas.Alias,
                            Remark = cemeteryAreas.Remark,
                            RowSort = cemeteryAreas.RowSort ?? "ASC"
                        };
                    }
                    myDto.AreaEntity = cemeteryAreasDto;
                    myDto.AreaSort = cemeteryAreasDto.RowSort;
                }

                if (entity.RowId > 0)
                {
                    var rows = _databaseContext.CemeteryRows.AsNoTracking().FirstOrDefault(a => a.Id == entity.RowId);
                    CemeteryRowsDTO rowsDto = null;
                    if (rows != null)
                    {
                        rowsDto = new CemeteryRowsDTO
                        {
                            Id = rows.Id,
                            Name = rows.Name,
                            Alias = rows.Alias,
                            Remark = rows.Remark
                        };
                    }
                    myDto.RowEntity = rowsDto;

                }
                if (entity.ColumnId > 0)
                {
                    var cols = _databaseContext.CemeteryColumns.AsNoTracking().FirstOrDefault(a => a.Id == entity.ColumnId);
                    CemeteryColumnsDTO closDto = null;
                    if (cols != null)
                    {
                        closDto = new CemeteryColumnsDTO
                        {
                            Id = cols.Id,
                            Name = cols.Name,
                            Alias = cols.Alias,
                            Remark = cols.Remark
                        };
                    }
                    myDto.ColumnEntity = closDto;

                }
                if (entity.PaymentStatusId > 0)
                {
                    var paymentStatus = _databaseContext.PaymentStatus.AsNoTracking().FirstOrDefault(a => a.Id == entity.PaymentStatusId);
                    PaymentStatusDTO paymentStatusDto = null;
                    if (paymentStatus != null)
                    {
                        paymentStatusDto = new PaymentStatusDTO
                        {
                            Id = paymentStatus.Id,
                            Name = paymentStatus.Name,
                            Alias = paymentStatus.Alias,
                            Remark = paymentStatus.Remark
                        };
                    }
                    myDto.PaymentStatusEntity = paymentStatusDto;

                }
                if (entity.SecurityLevelId > 0)
                {
                    var securityLevels = _databaseContext.SecurityLevels.AsNoTracking().FirstOrDefault(a => a.Id == entity.SecurityLevelId);
                    if (securityLevels != null)
                    {
                        myDto.SecurityLevelName = securityLevels.Name;
                    }
                }
                if (entity.ServiceLevelId > 0)
                {
                    var serviceLevels = _databaseContext.ServiceLevels.AsNoTracking().FirstOrDefault(a => a.Id == entity.ServiceLevelId);
                    if (serviceLevels != null)
                    {
                        myDto.ServiceLevelName = serviceLevels.Name;
                    }
                }

                //装载该墓碑落葬人
                var tombstoneBuriedPeopleMaps = _databaseContext.TombstoneBuriedPeopleMaps.Where(a => a.TombstoneId == myDto.Id);
                foreach (var tombstoneBuriedPeopleMap in tombstoneBuriedPeopleMaps)
                {
                    var customer = _databaseContext.Customers.FirstOrDefault(a => a.Id == tombstoneBuriedPeopleMap.BuriedCustomerId);
                    if (customer != null)
                    {
                        var customerDto = new CustomerDTO
                        {
                            Id = customer.Id,
                            FirstName = customer.FirstName,
                            MiddleName = customer.MiddleName,
                            LastName = customer.LastName
                        };
                        myDto.CustomerBuryDtos.Add(customerDto);
                    }
                }
            }

            return myDto;
        }
    }
}
