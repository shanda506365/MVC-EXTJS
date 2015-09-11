using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USO.Core.XmlConfig
{
    [XmlRoot("WFConfig")]
    public class SpecialAuditWFTemplate
    {
        [XmlArray("WorkFlowTypes"), XmlArrayItem("WorkFlowType")]
        public WorkFlowType[] WorkFlowTypes { get; set; }

        [XmlArray("AuditItems"), XmlArrayItem("AuditItem")]
        public AuditItem[] AuditItems { get; set; }
    }


    [XmlRoot("WorkFlowType")]
    public class WorkFlowType
    {
        [XmlAttribute("TypeName")]
        public string TypeName { get; set; }

        [XmlAttribute("TypeId")]
        public int TypeId { get; set; }

        [XmlAttribute("WorkStatus")]
        public int WorkStatus { get; set; }

        [XmlAttribute("CompanyId")]
        public int CompanyId { get; set; }
    }

    [XmlRoot("AuditItem")]
    public class AuditItem
    {
        [XmlAttribute("RoleId")]
        public int RoleId { get; set; }

        [XmlAttribute("RoleName")]
        public string RoleName { get; set; }

        [XmlAttribute("OrgTypeId")]
        public int OrgTypeId { get; set; }

        [XmlAttribute("OrgTypeName")]
        public string OrgTypeName { get; set; }

        [XmlAttribute("Status")]
        public int Status { get; set; }
    }

}
