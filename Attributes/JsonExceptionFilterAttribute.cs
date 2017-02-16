// *****************************************************
//                     MVC EXPANSION
//                  by  Shane Whitehead
//                  bwakabats@gmail.com
// *****************************************************
//      The software is released under the GNU GPL:
//          http://www.gnu.org/licenses/gpl.txt
//
// Feel free to use, modify and distribute this software
// I only ask you to keep this comment intact.
// Please contact me with bugs, ideas, modification etc.
// *****************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Web.Http.Filters;
using System.Web.Mvc;
using BWakaBats.Extensions;
using Elmah;

namespace BWakaBats.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class JsonExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                ErrorSignal.FromCurrentContext().Raise(actionExecutedContext.Exception);
            }

            //if (actionExecutedContext.Request.IsAjaxRequest())
            //{
            UpdateFilterContext(actionExecutedContext);
            //}
            //else
            //{
            //    base.OnException(actionExecutedContext);
            //}
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private static void UpdateFilterContext(HttpActionExecutedContext actionExecutedContext)
        {
            string exception = actionExecutedContext.Exception.Message;
            actionExecutedContext.Exception = null;
            actionExecutedContext.Response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new System.Net.Http.StringContent("{ \"Result\": false, \"Error\": \"" + exception.Replace("\"", "'").Replace("\r", "\\r").Replace("\n", "\\n") + "\" }", Encoding.ASCII, "text/json")
            };
        }

        public static void UpdateFilterContext(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                filterContext.HttpContext.ClearError();
                filterContext.Result = filterContext.Exception.ToJsonResult();
                filterContext.Exception = null;
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }
        }

        public static JsonResult CreateJsonResult(Exception exception, string propertyName = null)
        {
            IEnumerable<string> innerExceptions;

            var list = new List<string>();
            var innerException = exception.InnerException;
            while (innerException != null)
            {
                list.Add(innerException.Message);
                innerException = innerException.InnerException;
            }
            innerExceptions = list;

            var errorData = new
            {
                error = exception.Message,
                stackTrace = exception.StackTrace,
                source = exception.Source,
                innerExceptions = innerExceptions,
                propertyName = propertyName,
            };
            return new JsonResult() { Data = errorData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}