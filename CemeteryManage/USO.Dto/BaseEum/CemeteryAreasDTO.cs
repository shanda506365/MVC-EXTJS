using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Dto
{
    /// <summary>
    /// 墓碑区域表
    /// </summary>
    public class CemeteryAreasDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 总墓碑数量
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 已订墓碑数量
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 已售墓碑数量
        /// </summary>
        public int SaleCount { get; set; }
        /// <summary>
        /// 落葬墓碑数量
        /// </summary>
        public int BuryCount { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public int ElseCount
        {
            get
            {
                if (TotalCount > 0)
                {
                    return TotalCount - OrderCount - SaleCount - BuryCount;
                }
                else
                {
                    return 0;
                }
                
            }
        }

        /// <summary>
        /// 排显示顺序
        /// </summary>
        public string RowSort { get; set; }
        /// <summary>
        /// 排显示顺序
        /// </summary>
        public string RowSortString
        {
            get
            {
                if (RowSort == "DESC")
                {
                    return "倒序";
                }
                else
                {
                    return "正序";
                }
            }
        }
    }

}
