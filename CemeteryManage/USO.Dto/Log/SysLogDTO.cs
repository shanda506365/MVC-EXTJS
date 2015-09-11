using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Domain;

namespace USO.Dto
{
    public class SysLogDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType Type { get; set; }
        public string TypeString
        {
            get { return Type.DescriptionFor(); }
        }
        /// <summary>
        /// 操作记录
        /// </summary>
        public string ControlName { get; set; }
        /// <summary>
        /// 日志文本
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        public int UserId { get; set; }
      
        public UserDTO UserEntity { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? Date { get; set; }

        public string DateString
        {
            get
            {
                return Date.HasValue ? ((DateTime)Date).ToString("yyyy-MM-dd HH:mm:ss") : "";
            }
        }

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
        public int? ControllTid { get; set; }
        /// <summary>
        /// 操作的编号数组
        /// </summary>
        public string ControllIds { get; set; }
        /// <summary>
        /// 逝者
        /// </summary>
        public string BuryMan { get; set; }
        /// <summary>
        /// 启用日期(落葬日期)
        /// </summary>
        public DateTime? BuryDate { get; set; }
        public string BuryDateString
        {
            get
            {
                return BuryDate.HasValue ? ((DateTime)BuryDate).ToString("yyyy-MM-dd HH:mm:ss") : "";
            }
        }
        /// <summary>
        /// 申请人备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 落葬备注
        /// </summary>
        public string Remark2 { get; set; }
        #endregion
        #region 虚拟字段
        /// <summary>
        /// 管理费催缴 0是，1否
        /// </summary>
        public int? SupperManage { get; set; }

        /// <summary>
        /// 管理年限
        /// </summary>
        public int? ManageLimit { get; set; }

        /// <summary>
        /// 管理费时间区间
        /// </summary>
        public string ManageDate { get; set; }

        /// <summary>
        /// 上次缴费日期(补交日期)
        /// </summary>
        public DateTime? LastPaymentDate { get; set; }
        public string LastPaymentDateString
        {
            get
            {
                return LastPaymentDate.HasValue ? ((DateTime)LastPaymentDate).ToString("yyyy-MM-dd HH:mm:ss") : "";
            }
        }
        /// <summary>
        /// 购买日期(出售日期)
        /// </summary>
        public DateTime? BuyDate { get; set; }
        public string BuyDateString
        {
            get
            {
                return BuyDate.HasValue ? ((DateTime)BuyDate).ToString("yyyy-MM-dd HH:mm:ss") : "";
            }
        }
       
        #endregion
        /// <summary>
        /// 操作的墓碑信息
        /// </summary>
        public TombstoneDTO TombstoneEntity { get; set; }
    }
}
