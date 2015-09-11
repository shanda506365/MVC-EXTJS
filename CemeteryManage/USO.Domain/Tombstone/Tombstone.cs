using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Domain
{
    /// <summary>
    /// 墓碑表
    /// </summary>
    [Table("Tombstone")]
    public class Tombstone
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
        [ForeignKey("AreaId")]
        public virtual CemeteryAreas Area { get; set; }
        /// <summary>
        /// 所属行Id
        /// </summary>
        public int RowId { get; set; }
        [ForeignKey("RowId")]
        public virtual CemeteryRows Row { get; set; }
        /// <summary>
        /// 所属列Id
        /// </summary>
        public int ColumnId { get; set; }
        [ForeignKey("ColumnId")]
        public virtual CemeteryColumns Column { get; set; }
        /// <summary>
        /// 所属父墓碑Id
        /// </summary>
        public int ParentId { get; set; }


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
        /// 购买日期(出售日期)
        /// </summary>
        public DateTime BuyDate { get; set; }

        /// <summary>
        /// 上次缴费日期(补交日期)
        /// </summary>
        public DateTime LastPaymentDate { get; set; }

        /// <summary>
        /// 启用日期(落葬日期)
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
        /// 保密等级
        /// </summary>
        public int SecurityLevelId { get; set; }

        /// <summary>
        /// 墓碑图片
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 服务等级
        /// </summary>
        public int ServiceLevelId { get; set; }

        /// <summary>
        /// 墓碑类别
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 付款状态Id
        /// </summary>
        public int PaymentStatusId { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public int SortNum { get; set; }

        /// <summary>
        /// 管理费催缴 0是，1否
        /// </summary>
        public int? SupperManage { get; set; }

        /// <summary>
        /// 管理年限
        /// </summary>
        public int? ManageLimit { get; set; }

    }
}
