using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Dto
{
    public class TombstoneDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// 全名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 所属区域Id
        /// </summary>
        public int? AreaId { get; set; }
        /// <summary>
        /// 所属区域名称
        /// </summary>
        public CemeteryAreasDTO AreaEntity { get; set; }
        /// <summary>
        /// 所属排序字段
        /// </summary>
        public string AreaSort { get; set; }
        /// <summary>
        /// 所属行Id
        /// </summary>
        public int? RowId { get; set; }
        /// <summary>
        /// 所属行名称
        /// </summary>
        public CemeteryRowsDTO RowEntity { get; set; }


        /// <summary>
        /// 所属列Id
        /// </summary>
        public int? ColumnId { get; set; }
        /// <summary>
        /// 所属列名称
        /// </summary>
        public CemeteryColumnsDTO ColumnEntity { get; set; }


        /// <summary>
        /// 所属父墓碑Id
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// 所属墓碑
        /// </summary>
        public TombstoneDTO Parent { get; set; }


        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 所属客户Id
        /// </summary>
        public int? CustomerId { get; set; }
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
        /// 到期日期(用于计算管理费到期)
        /// </summary>
        public DateTime? ExpiryDate { get; set; }
        public string ExpiryDateString
        {
            get
            {
                return ExpiryDate.HasValue ? ((DateTime)ExpiryDate).ToString("yyyy-MM-dd HH:mm:ss") : "";
            }
        }
        public string ExpiryDateShortString
        {
            get
            {
                return ExpiryDate.HasValue ? ((DateTime)ExpiryDate).ToString("yyyy-MM-dd") : "";
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
        public int? SecurityLevelId { get; set; }
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
        public int? ServiceLevelId { get; set; }
        /// <summary>
        /// 服务等级名称
        /// </summary>
        public string ServiceLevelName { get; set; }

        /// <summary>
        /// 墓碑类别Id
        /// </summary>
        public int? TypeId { get; set; }
        /// <summary>
        /// 保墓碑类别名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 付款状态Id
        /// </summary>
        public int? PaymentStatusId { get; set; }
        /// <summary>
        /// 付款状态
        /// </summary>
        public PaymentStatusDTO PaymentStatusEntity { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public int SortNum { get; set; }


        /// <summary>
        /// 管理费催缴 0是，1否//无限期管理年限 0否，1是
        /// </summary>
        public int? SupperManage { get; set; }

        /// <summary>
        /// 管理年限
        /// </summary>
        public int? ManageLimit { get; set; }

        /// <summary>
        /// 该墓碑包含的落葬人
        /// </summary>
        private List<CustomerDTO> _customerBuryDtos;
        /// <summary>
        /// 该墓碑包含的落葬人
        /// </summary>
        public List<CustomerDTO> CustomerBuryDtos
        {
            get
            {
                if (this._customerBuryDtos != null)
                {
                    return this._customerBuryDtos;
                }
                else
                {
                    return null;
                }

            }
            set { this._customerBuryDtos = value; }
        }

        /// <summary>
        /// 该角色包含的功能文本
        /// </summary>
        public string CustomerBuryString
        {
            get
            {
                var customerName = from p in CustomerBuryDtos select p.LastName + p.MiddleName + p.FirstName;
                string str = string.Join(",", customerName.ToArray());
                return str;
            }
        } 
    }
}
