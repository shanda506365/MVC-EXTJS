using System;

namespace USO.Domain
{
    public class AccountAddress
    {
        public AccountAddress()
        {
            CreatedOn = DateTime.UtcNow;
            ModifiedOn = DateTime.UtcNow;
        }

        public long Id { get; set; }
        public long AccountId { get; set; }
        public string ContactName { get; set; }
        public string Name { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string StateOrProvince { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string PostOfficeBox { get; set; }
        public string PostalCode { get; set; }
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
        public string Description { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
