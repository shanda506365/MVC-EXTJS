using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcExtensions
{
    /// <summary>
    /// Ajax 自定义错误信息处理。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class AjaxExceptionAttribute : ActionFilterAttribute, IExceptionFilter
    {

        /// <summary>
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnException(ExceptionContext filterContext)
        {   
            if (!filterContext.HttpContext.Request.IsAjaxRequest())    
                return; 
            filterContext.Result = AjaxError(filterContext.Exception.Message, filterContext);
            
            //Let the system know that the exception has been handled   
            filterContext.ExceptionHandled = true;      
        }  

        /// <summary> 
        /// Ajaxes the error.  
        /// </summary> 
        /// <param name="message">The message.</param>
        ///<param name="filterContext">The filter context.</param>
        ///<returns>JsonResult</returns>
        protected JsonResult AjaxError(string message, ExceptionContext filterContext)
        { 
            //If message is null or empty, then fill with generic message
            if (String.IsNullOrEmpty(message))
                message = "Something went wrong while processing your request. Please refresh the page and try again.";
            //Set the response status code to 500
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //Needed for IIS7.0 
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;  
            return new JsonResult   
            {  
                //can extend more properties 
                Data = new
                {
                    ErrorMessage = message
                },   
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };  
        }
    }
}
