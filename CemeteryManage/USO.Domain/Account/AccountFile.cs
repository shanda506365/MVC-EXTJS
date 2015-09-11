using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Domain
{
    public class AccountFile
    {
        public AccountFile()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// 账户Id
        /// </summary>
        public long AccountId { get; set; }
        /// <summary>
        /// 存放路径
        /// </summary>
        public string PathUrl { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public AccountFileType FileType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public long CreatedBy { get; set; }
    }

    public enum AccountFileType
    {
        /// <summary>
        /// 普通文件
        /// </summary>
        Common = 0,

        /// <summary>
        /// Logo文件
        /// </summary>
        Logo = 1,

    }
}




