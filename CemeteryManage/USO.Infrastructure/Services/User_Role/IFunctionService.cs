using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Core;
using USO.Domain;
using USO.Dto;

namespace USO.Infrastructure.Services
{
    public interface IFunctionService : IDependency
    {
        /// <summary>
        /// 查询功能
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        PagedResult<FunctionDTO> Find(FunctionQuery query);
        /// <summary>
        /// 根据Id获取功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FunctionDTO GetFunctionById(int id);
    }
}
