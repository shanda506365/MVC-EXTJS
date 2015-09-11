
namespace USO.Mvc.Extensions
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using MvcExtensions;
    using USO.Domain;
    using USO.Dto;

    public static class ModelStateDictionaryExtensions
    {
        public static void Merge(this ModelStateDictionary instance, IEnumerable<RuleViolation> ruleViolations)
        {
          

            ruleViolations.Each(violation => instance.AddModelError(violation.ParameterName, violation.ErrorMessage));
        }
    }
}
