using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Dto.Customer
{
    public class TombstoneDTO
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 全名
        /// </summary>
        [StringLength(200)]
        public string Name { get; set; }
        /// <summary>
        /// 所属区域Id
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 所属区域名称
        /// </summary>
        public int AreaName { get; set; }

        /// <summary>
        /// 所属行Id
        /// </summary>
        public int RowId { get; set; }
        /// <summary>
        /// 所属行名称
        /// </summary>
        public int RowName { get; set; }


        /// <summary>
        /// 所属列Id
        /// </summary>
        public int ColumnId { get; set; }
        /// <summary>
        /// 所属列名称
        /// </summary>
        public int ColumnName { get; set; }


        /// <summary>
        /// 所属父墓碑Id
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 所属墓碑
        /// </summary>
        public TombstoneDTO Parent { get; set; }


        /// <summary>
        /// 别名
        /// </summary>
        [StringLength(200)]
        public string Alias { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 所属客户Id
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 所属客户
        /// </summary>
        public CustomerDTO Customer { get; set; }

        /// <summary>
        /// 所属客户名称
        /// </summary>
        [StringLength(200)]
        public string CustomerName { get; set; }

        /// <summary>
        /// 碑文
        /// </summary>
        public string StoneText { get; set; }

        /// <summary>
        /// 到期日期
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// 购买日期
        /// </summary>
        public DateTime BuyDate { get; set; }

        /// <summary>
        /// 上次缴费日期
        /// </summary>
        public DateTime LastPaymentDate { get; set; }

        /// <summary>
        /// 启用日期
        /// </summary>
        public DateTime BuryDate { get; set; }

        /// <summary>
        /// 墓碑宽度
        /// </summary>
        public decimal Width { get; set; }
        /// <summary>
        /// 墓碑高度
        /// </summary>
        public decimal Height { get; set; }

        /// <summary>
        /// 墓碑面积
        /// </summary>
        public decimal Acreage { get; set; }

        /// <summary>
        /// 保密等级Id
        /// </summary>
        public int SecurityLevelId { get; set; }
        /// <summary>
        /// 保密等级名称
        /// </summary>
        public string SecurityLevelName { get; set; }

        /// <summary>
        /// 墓碑图片
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 服务等级Id
        /// </summary>
        public int ServiceLevelId { get; set; }
        /// <summary>
        /// 服务等级名称
        /// </summary>
        public string ServiceLevelName { get; set; }

        /// <summary>
        /// 墓碑类别Id
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 保墓碑类别名称
        /// </summary>
        public string TypeName { get; set; }
    }
}
