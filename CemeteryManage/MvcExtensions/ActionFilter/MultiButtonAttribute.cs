using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcExtensions.ActionFilter
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)] 
    public class MultiButtonAttribute : ActionNameSelectorAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public MultiButtonAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Determines whether the action name is valid in the specified controller context.
        /// </summary>
        /// <returns>
        /// true if the action name is valid in the specified controller context; otherwise, false.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="actionName">The name of the action.</param><param name="methodInfo">Information about the action method.</param>
        public override bool IsValidName(ControllerContext controllerContext,
            string actionName, System.Reflection.MethodInfo methodInfo)
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                return false;
            }
            return controllerContext.HttpContext.Request.Form.AllKeys.Contains(this.Name);
        }
    }
}
