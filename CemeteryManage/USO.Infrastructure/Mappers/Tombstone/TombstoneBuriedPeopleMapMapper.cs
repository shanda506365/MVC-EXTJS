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
    public interface ITombstoneBuriedPeopleMapMapper : IDependency
    {
        TombstoneBuriedPeopleMapDTO Map(TombstoneBuriedPeopleMap entity, bool includeAll);
    }

    public class TombstoneBuriedPeopleMapMapper : ITombstoneBuriedPeopleMapMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public TombstoneBuriedPeopleMapMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public TombstoneBuriedPeopleMapDTO Map(TombstoneBuriedPeopleMap entity, bool includeAll)
        {
            var dto = LoadEntityData(entity, includeAll);
            return dto;
        }

        private TombstoneBuriedPeopleMapDTO LoadEntityData(TombstoneBuriedPeopleMap entity, bool includeAll)
        {

            var myDto = new TombstoneBuriedPeopleMapDTO()
            {
                Id = entity.Id,
                BuriedCustomerId = entity.BuriedCustomerId,
                TombstoneId = entity.TombstoneId,
                Remark = entity.Remark
            };
            //包括所有关系
            if (includeAll)
            {
                if (entity.BuriedCustomerId > 0)
                {
                    myDto.BuriedCustomer = null;

                }
                if (entity.TombstoneId > 0)
                {
                    myDto.Tombstone = null;
                }
               
            }

            return myDto;
        }
    }
}
