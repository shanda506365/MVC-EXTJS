using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Dto
{
    /// <summary>
    /// 墓碑客户关系表
    /// </summary>
    public class TombstoneBuriedPeopleMapDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// 墓碑Id
        /// </summary>
        public int? TombstoneId { get; set; }
        /// <summary>
        /// 墓碑
        /// </summary>
        public TombstoneDTO Tombstone { get; set; }
        /// <summary>
        /// 埋葬人Id
        /// </summary>
        public int? BuriedCustomerId { get; set; }
        /// <summary>
        /// 埋葬人
        /// </summary>
        public CustomerDTO BuriedCustomer { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
