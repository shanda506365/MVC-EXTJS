using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Domain;

namespace USO.Dto
{
    public class CustomerDTO
    {
        
        public int Id { get; set; }
        /// <summary>
        /// 全名
        /// </summary>
        public string FullName { get; set; }
        public string FullNameString
        {
            get { return LastName + MiddleName + FirstName; }
        }

        /// <summary>
        /// 姓氏
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 中间名
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 移动电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 名称2
        /// </summary>
        public string OtherPhone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 客户类型Id
        /// </summary>
        public int? CustomerTypeId { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>
        public string CustomerTypeName { get; set; }

        /// <summary>
        /// 联系人Id
        /// </summary>
        public int? LinkCustomerId { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkCustomerName { get; set; }

        /// <summary>
        /// 下葬日期
        /// </summary>
        public DateTime? BuryDate { get; set; }
        public string BuryDateString {
            get
            {
                return BuryDate.HasValue ? ((DateTime)BuryDate).ToString("yyyy-MM-dd HH:mm:ss") : "";
            }
        }

        /// <summary>
        /// 死亡日期
        /// </summary>
        public DateTime? DeathDate { get; set; }

        /// <summary>
        /// 死亡日期
        /// </summary>
        public string DeathDateString
        {
            get
            {
                return DeathDate.HasValue ? ((DateTime)DeathDate).ToString("yyyy-MM-dd HH:mm:ss") : "";
            }
        }

        /// <summary>
        /// 国籍Id
        /// </summary>
        public int? NationalityId { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        public string NationalityName { get; set; }


        /// <summary>
        /// 客户状态Id
        /// </summary>
        public int? CustomerStatusId { get; set; }

        /// <summary>
        /// 客户状态
        /// </summary>
        public string CustomerStatusName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDNumber { get; set; }
    }
}
