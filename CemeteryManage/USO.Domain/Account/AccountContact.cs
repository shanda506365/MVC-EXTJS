using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USO.Domain
{
    public class AccountContact
    {
        public AccountContact()
        {
            CreatedOn = DateTime.UtcNow;
            ModifiedOn = DateTime.UtcNow;
        }

        [Key]
        public long Id { get; set; }

        /// <summary>
        /// 此联系人所属的Account
        /// </summary>
        public long AccountId { get; set; }
        public string FirstName { get; set; }
        public string NickName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }
        public string GenderId { get; set; }
        public string Phone1Country { get; set; }
        public string Phone1Area { get; set; }
        public string Phone1 { get; set; }
        public string Phone1Ext { get; set; }
        public string Phone2Country { get; set; }
        public string Phone2Area { get; set; }
        public string Phone2 { get; set; }
        public string Phone2Ext { get; set; }
        public string FaxCountry { get; set; }
        public string FaxArea { get; set; }
        public string Fax { get; set; }
        public string FaxExt { get; set; }
        public string MobilePhoneCountry { get; set; }
        public string MobilePhoneArea { get; set; }
        public string MobilePhone { get; set; }
        public string EMail1 { get; set; }
        public string EMail2 { get; set; }
        public string Description { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
