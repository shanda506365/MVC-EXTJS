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
    public interface INationalityMapper : IDependency
    {
        NationalityDTO Map(Nationality entity);
    }

    public class NationalityMapper : INationalityMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public NationalityMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public NationalityDTO Map(Nationality entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
        }

        private NationalityDTO LoadEntityData(Nationality entity)
        {

            var myDto = new NationalityDTO()
            {
                Id = entity.Id,
                Name = entity.Name
            };


            return myDto;
        }
    }
}
