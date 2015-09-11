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
    public interface IFunctionMapper : IDependency
    {
        FunctionDTO Map(Function entity);
    }

    public class FunctionMapper : IFunctionMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public FunctionMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public FunctionDTO Map(Function entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
        }

        private FunctionDTO LoadEntityData(Function entity)
        {

            var myDto = new FunctionDTO()
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                ParentId = entity.ParentId,
                Url = entity.Url
            };
          

            return myDto;
        }
    }
}
