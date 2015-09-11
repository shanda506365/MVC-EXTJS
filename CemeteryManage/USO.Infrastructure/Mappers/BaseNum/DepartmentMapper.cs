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
    public interface IDepartmentMapper : IDependency
    {
        DepartmentDTO Map(Department entity);
    }

    public class DepartmentMapper : IDepartmentMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public DepartmentMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public DepartmentDTO Map(Department entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
        }

        private DepartmentDTO LoadEntityData(Department entity)
        {

            var myDto = new DepartmentDTO()
            {
                Id = entity.Id,
                Name = entity.Name
            };


            return myDto;
        }
    }
}
