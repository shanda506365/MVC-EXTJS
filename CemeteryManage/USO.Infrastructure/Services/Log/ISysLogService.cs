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
    public interface ISysLogService : IDependency
    {
        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="sysLogQuery"></param>
        /// <returns></returns>
        PagedResult<SysLogDTO> Find(SysLogQuery sysLogQuery);

        /// <summary>
        /// 新建日志
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        DataControlResult<SysLogDTO> Create(SysLogDTO dto);
        /// <summary>
        /// 更新日志信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        DataControlResult<SysLogDTO> Update(SysLogDTO dto);
        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="dtoList"></param>
        /// <returns></returns>
        DataControlResult<SysLogDTO> Delete(List<SysLogDTO> dtoList);

        /// <summary>
        /// 查询墓碑相关业务信息
        /// </summary>
        /// <param name="tombstoneId"></param>
        /// <param name="controlName"></param>
        /// <returns></returns>
        PagedResult<SysLogDTO> GetJobInfoTombstone(int tombstoneId, string controlName);

        /// <summary>
        /// 查询交管理费续交记录
        /// </summary>
        /// <param name="tombstoneId"></param>
        /// <param name="controlName"></param>
        /// <returns></returns>
        PagedResult<SysLogDTO> GetRenewManageTombstone(int tombstoneId);
    }
}
