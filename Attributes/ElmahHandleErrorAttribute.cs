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
using System.Web.Mvc;
using Elmah;

namespace BWakaBats.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class ElmahHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                JsonExceptionFilterAttribute.UpdateFilterContext(filterContext);
            }
            else
            {
                base.OnException(filterContext);
            }
        }
    }
}