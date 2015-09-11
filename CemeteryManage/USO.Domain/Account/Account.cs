namespace USO.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 客户账户信息
    /// </summary>
    public class Account
    {
        public Account()
        {
            CreatedOn = DateTime.UtcNow;
            ModifiedOn = DateTime.UtcNow;
            Organizations=new List<Organization>();
            AccountFiles=new List<AccountFile>();
            Products=new List<Product>();
           
        }


        [Key]
        public long Id { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// R3客户代码
        /// </summary>
        [StringLength(100)]
        [Required]
        public string R3Code { get; set; }

        /// <summary>
        /// 客户描述
        /// </summary>
        [StringLength(1000)]
        public string Description { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// 客户组织（一个客户可能属于多个组织）
        /// </summary>
        public IList<Organization> Organizations { get; set; }

        /// <summary>
        /// 客户文件（一个客户可以对应多个文件）
        /// </summary>
        public IList<AccountFile> AccountFiles { get; set; }

        /// <summary>
        /// 客户日志（一个客户可以有多条日志）
        /// </summary>
        public IList<AccountAnnotation> AccountAnnotations { get; set; }

        /// <summary>
        /// 客户产品（一个客户可以指定多个产品）
        /// </summary>
        public IList<Product> Products { get; set; }

    }
}