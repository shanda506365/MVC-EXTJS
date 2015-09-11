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
    public interface ICustomerService : IDependency
    {
        /// <summary>
        /// 查询客户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        PagedResult<CustomerDTO> Find(CustomerQuery query);

        /// <summary>
        /// 新建客户
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        DataControlResult<CustomerDTO> Create(CustomerDTO csDto);

        /// <summary>
        /// 新建客户(批量)
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        PagedResult<CustomerDTO> Create(List<CustomerDTO> csDtoList);
        /// <summary>
        /// 更新客户信息
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        DataControlResult<CustomerDTO> Update(CustomerDTO csDto);
        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        DataControlResult<CustomerDTO> Delete(List<CustomerDTO> csDtoList);
    }
}
