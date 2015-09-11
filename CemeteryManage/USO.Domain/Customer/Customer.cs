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
    /// 客户表
    /// </summary>
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 全名
        /// </summary>
        [StringLength(200)]
        public string FullName { get; set; }

        /// <summary>
        /// 姓氏
        /// </summary>
        [StringLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// 中间名
        /// </summary>
        [StringLength(50)]
        public string MiddleName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 移动电话
        /// </summary>
        [StringLength(50)]
        public string Telephone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(50)]
        public string Phone { get; set; }

        /// <summary>
        /// 名称2
        /// </summary>
        [StringLength(50)]
        public string OtherPhone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        public int? CustomerTypeId { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public int? LinkCustomerId { get; set; }

        /// <summary>
        /// 下葬日期
        /// </summary>
        public DateTime? BuryDate { get; set; }

        /// <summary>
        /// 死亡日期
        /// </summary>
        public DateTime? DeathDate { get; set; }

        /// <summary>
        /// 国籍Id
        /// </summary>
        public int? NationalityId { get; set; }

        /// <summary>
        /// 客户状态Id
        /// </summary>
        public int? CustomerStatusId { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [StringLength(200)]
        public string IDNumber { get; set; }
    }


}
