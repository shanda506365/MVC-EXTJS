using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Domain;
using USO.Dto;
using USO.Infrastructure.Mappers;

namespace USO.Infrastructure.Services
{
    public class FunctionService :IFunctionService
    {
         private readonly IDatabaseContext _databaseContext;
         private readonly FunctionMapper _functionMapper;

         public FunctionService(IDatabaseContext databaseContext, FunctionMapper functionMapper)
        {
            _databaseContext = databaseContext;
            _functionMapper = functionMapper;
        }

        /// <summary>
        /// 查询功能
        /// </summary>
         /// <param name="functionQuery"></param>
        /// <returns></returns>
         public PagedResult<FunctionDTO> Find(FunctionQuery functionQuery)
        {
            Check.Argument.IsNotNull(functionQuery, "functionQuery");

            //Apply filtering    
            var query = _databaseContext.Functions.Where(functionQuery);
           
            var total = query.Count();
            query = SortMemeberHelper.SortingAndPaging<Function>(query, functionQuery.sort, functionQuery.dir
                , functionQuery.page, functionQuery.limit);


            var resultSet = query.AsNoTracking().ToList().Select(r => _functionMapper.Map(r)).ToList();

            return new PagedResult<FunctionDTO>(resultSet, total);
        }
         /// <summary>
         /// 根据Id获取功能
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
        public FunctionDTO GetFunctionById(int id)
        {
            var dto = new FunctionDTO();
            var function = _databaseContext.Functions.FirstOrDefault(a=>a.Id == id);
            if (function != null)
            {
                dto = new FunctionDTO
                    {
                        Id = function.Id,
                        Code = function.Code,
                        Name = function.Name,
                        ParentId = function.ParentId,
                        Url = function.Url
                    };
            }
            return dto;
        }
    }
}
