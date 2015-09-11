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
    /// 国籍表
    /// </summary>
    [Table("Nationality")]
    public class Nationality
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(200)]
        public string Name { get; set; }
    }
}
