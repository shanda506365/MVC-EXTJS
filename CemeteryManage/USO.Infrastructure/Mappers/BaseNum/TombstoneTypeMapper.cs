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
    public interface ITombstoneTypeMapper : IDependency
    {
        TombstoneTypeDTO Map(TombstoneType entity);
    }

    public class TombstoneTypeMapper : ITombstoneTypeMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public TombstoneTypeMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public TombstoneTypeDTO Map(TombstoneType entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
        }

        private TombstoneTypeDTO LoadEntityData(TombstoneType entity)
        {

            var myDto = new TombstoneTypeDTO()
            {
                Id = entity.Id,
                Name = entity.Name,
                Alias = entity.Alias,
                Remark = entity.Remark
            };


            return myDto;
        }
    }
}
