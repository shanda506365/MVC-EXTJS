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
    public interface ICemeteryColumnsMapper : IDependency
    {
        CemeteryColumnsDTO Map(CemeteryColumns entity);
    }

    public class CemeteryColumnsMapper : ICemeteryColumnsMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public CemeteryColumnsMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public CemeteryColumnsDTO Map(CemeteryColumns entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
        }

        private CemeteryColumnsDTO LoadEntityData(CemeteryColumns entity)
        {

            var myDto = new CemeteryColumnsDTO()
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
