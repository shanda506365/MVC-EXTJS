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
    /// 系统日志表
    /// </summary>
    [Table("SysLog")]
    public class SysLog
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType Type { get; set; }
        /// <summary>
        /// 操作记录
        /// </summary>
        [StringLength(200)]
        public string ControlName { get; set; }
        /// <summary>
        /// 日志文本
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Date { get; set; }


        #region 附加业务信息
        /// <summary>
        /// 申请人
        /// </summary>
        public string Applicanter { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDNumber { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Money { get; set; }
        /// <summary>
        /// 操作的墓碑编号
        /// </summary>
        public int ControllTid { get; set; }
        /// <summary>
        /// 操作的编号数组
        /// </summary>
        public string ControllIds { get; set; }
        /// <summary>
        /// 逝者
        /// </summary>
        public string BuryMan { get; set; }

        /// <summary>
        /// 落葬时间
        /// </summary>
        public DateTime? BuryDate { get; set; }


        /// <summary>
        /// 申请人备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 落葬备注
        /// </summary>
        public string Remark2 { get; set; }
        #endregion
    }


}
