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
    /// 功能表
    /// </summary>
    [Table("Function")]
    public class Function
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(200)]
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(200)]
        public  string Code { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [StringLength(200)]
        public string Url { get; set; }
        /// <summary>
        /// 父操作Id
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 父操作功能
        /// </summary>
        [ForeignKey("ParentId")]
        public Function Parent { get; set; }
    }
}
