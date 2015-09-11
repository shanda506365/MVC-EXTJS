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
    public interface ICemeteryRowsMapper : IDependency
    {
        CemeteryRowsDTO Map(CemeteryRows entity);
    }

    public class CemeteryRowsMapper : ICemeteryRowsMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public CemeteryRowsMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public CemeteryRowsDTO Map(CemeteryRows entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
        }

        private CemeteryRowsDTO LoadEntityData(CemeteryRows entity)
        {

            var myDto = new CemeteryRowsDTO()
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
