
namespace USO.Dto.Accounts
{
    using System;
    using System.Collections.Generic;
    using USO.Domain;

    public class AccountDTO
    {
        public long Id { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        
        public string Name { get; set; }

        /// <summary>
        /// R3客户代码
        /// </summary>
       
        public string R3Code { get; set; }

        /// <summary>
        /// 客户描述
        /// </summary>
        public string Description { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}

