using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Domain
{
    public class AccountAnnotation
    {
        public AccountAnnotation()
        {
            CreatedOn = DateTime.Now;
        }
        
        [Key]
        public long Id { get; set; }
        /// <summary>
        /// 账户Id
        /// </summary>
        public long AccountId { get; set; }
        /// <summary>
        /// 日志信息
        /// </summary>
        public string NoteText { get; set; }
        /// <summary>
        /// 日志类型（手动、自动）
        /// </summary>
        public AccountAnnotationStatus Status { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
       
        public enum AccountAnnotationStatus
        {
            None=-1,
            /// <summary>
            /// 手动输入
            /// </summary>
            Manual=0,
            /// <summary>
            /// 系统生成
            /// </summary>
            Auto=1
        }
    }
}
