using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Core;
using USO.Domain;
using USO.Dto;

namespace USO.Infrastructure.Mappers
{
    public interface ICemeteryAreasMapper : IDependency
    {
        CemeteryAreasDTO Map(CemeteryAreas entity);
    }

    public class CemeteryAreasMapper : ICemeteryAreasMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public CemeteryAreasMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public CemeteryAreasDTO Map(CemeteryAreas entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
        }

        private CemeteryAreasDTO LoadEntityData(CemeteryAreas entity)
        {

            var myDto = new CemeteryAreasDTO()
            {
                Id = entity.Id,
                Name = entity.Name,
                Alias = entity.Alias,
                Remark = entity.Remark,
                RowSort = entity.RowSort ?? "ASC"
            };
            myDto.TotalCount = _databaseContext.Tombstones.Count(a => a.AreaId == entity.Id);
            myDto.OrderCount = _databaseContext.Tombstones.Count(a => a.AreaId == entity.Id && a.PaymentStatusId == 2);
            myDto.SaleCount = _databaseContext.Tombstones.Count(a => a.AreaId == entity.Id && a.PaymentStatusId == 3);
            myDto.BuryCount = _databaseContext.Tombstones.Count(a => a.AreaId == entity.Id && a.PaymentStatusId == 5);

            return myDto;
        }
    }
}
