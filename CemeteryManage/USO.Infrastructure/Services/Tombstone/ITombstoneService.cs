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
    public interface ITombstoneService : IDependency
    {
        /// <summary>
        /// 查询客户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        PagedResult<TombstoneDTO> Find(TombstoneQuery query);

        /// <summary>
        /// 新建客户
        /// </summary>
        /// <param name="tbDto"></param>
        /// <returns></returns>
        DataControlResult<TombstoneDTO> Create(TombstoneDTO tbDto);

        /// <summary>
        /// 批量添加墓碑
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="rowId"></param>
        /// <param name="typeId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        DataControlResult<TombstoneDTO> CreateList(int areaId, int rowId, int typeId, int count);
        /// <summary>
        /// 更新客户信息
        /// </summary>
        /// <param name="tbDto"></param>
        /// <returns></returns>
        DataControlResult<TombstoneDTO> Update(TombstoneDTO tbDto);
        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="tbDto"></param>
        /// <returns></returns>
        DataControlResult<TombstoneDTO> Delete(List<TombstoneDTO> tbDtoList);

        /// <summary>
        /// 墓碑排序
        /// </summary>
        /// <param name="csDtoList"></param>
        /// <returns></returns>
        DataControlResult<TombstoneDTO> SorTombstone(string[] idList);

        /// <summary>
        /// 墓碑落葬
        /// </summary>
        /// <param name="tombstoneId"></param>
        /// <param name="customerIds"></param>
        /// <returns></returns>
        DataControlResult<TombstoneBuriedPeopleMapDTO> BuryPeopleTombstone(int tombstoneId, List<int> customerIds);

        /// <summary>
        /// 墓碑解除落葬
        /// </summary>
        /// <param name="tombstoneId"></param>
        /// <param name="customerIds"></param>
        /// <returns></returns>
        DataControlResult<TombstoneBuriedPeopleMapDTO> UnburyPeopleTombstone(int tombstoneId, List<int> customerIds);
        /// <summary>
        /// 墓碑预定
        /// </summary>
        /// <param name="tombstoneId"></param>
        /// <returns></returns>
        DataControlResult<TombstoneDTO> OrderTombstone(int tombstoneId, DateTime lastPaymentDate);
        /// <summary>
        /// 墓碑出售
        /// </summary>
        /// <param name="tombstoneId"></param>
        /// <returns></returns>
        DataControlResult<TombstoneDTO> SaleTombstone(int tombstoneId, DateTime buyDate);
        /// <summary>
        /// 墓碑落葬
        /// </summary>
        /// <param name="tombstoneId"></param>
        /// <returns></returns>
        DataControlResult<TombstoneDTO> BuryTombstone(int tombstoneId, DateTime buryDate, int manageLimit,
            bool supperManage);

        /// <summary>
        /// 更新墓碑管理费到期时间
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        DataControlResult<TombstoneDTO> UpdateExpiryDate(TombstoneDTO csDto);

        /// <summary>
        /// 续交管理费
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        DataControlResult<TombstoneDTO> RenewManageLimit(TombstoneDTO csDto);
    }
}
