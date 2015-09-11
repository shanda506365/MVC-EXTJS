using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using USO.Core;
using USO.Core.XmlConfig;

namespace USO.Core.Helper
{
    public class XmlConfigHelper
    {
        //特批审核配置文件(USO.Web\WFConfig\SpecialAuditWFConfig.xml)
        private static readonly string SpecialAuditWF_ConfigFile = ConfigurationManager.AppSettings["SpecialAuditWFConfig"];

        public XmlConfigHelper() { }

        public static List<int> GetOrgTypeIdByRoleId(int roleId)
        {
            List<int> orgTypeId=new List<int>();
            SpecialAuditWFTemplate template = null;
            try
            {
                template = XmlDeSerialize<SpecialAuditWFTemplate>(SpecialAuditWF_ConfigFile);
                orgTypeId = (from d in template.AuditItems where d.RoleId == roleId select d.OrgTypeId).ToList();
            }
            catch { }

            return orgTypeId;
        }

        public static int GetStatusByRoleId(int roleId)
        {
            int status = 0;
            SpecialAuditWFTemplate template = null;
            try
            {
                template = XmlDeSerialize<SpecialAuditWFTemplate>(SpecialAuditWF_ConfigFile);
                status = (from d in template.AuditItems where d.RoleId == roleId select d.Status).FirstOrDefault();
            }
            catch { }

            return status;
        }

        /// <summary>
        /// 获取工作流程
        /// </summary>
        /// <param name="workStatus">流程编号</param>
        /// <returns></returns>
        public static WorkFlowType GetWorkFlowId(int workStatus,int companyTypeId)
        {
            WorkFlowType result = null;
            SpecialAuditWFTemplate template=null;
            try
            {
                template = XmlDeSerialize<SpecialAuditWFTemplate>(SpecialAuditWF_ConfigFile);
                result = (from d in template.WorkFlowTypes where d.WorkStatus == workStatus && d.CompanyId==companyTypeId select d).FirstOrDefault();
            }
            catch{}
            
            return result;
        }


        /// <summary>
        /// 获取反序列化后的配置信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="configFile">xml文件</param>
        /// <returns></returns>
        public static T XmlDeSerialize<T>(string configFile) where T : class
        {
            T template=null;
            try
            {
                template = SerializationHelper.DeSerialize(typeof(T), HttpContext.Current.Server.MapPath(configFile)) as T;
            }
            catch { }

            return template;
        }
    }
}
