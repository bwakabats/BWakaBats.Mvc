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
using System.Web.Mvc;

namespace BWakaBats.Extensions
{
    public static class ExceptionExtensions
    {
        public static JsonResult ToJsonResult(this Exception source, string propertyName = null)
        {
            IEnumerable<string> innerExceptions;

            var list = new List<string>();
            var innerException = source.InnerException;
            while (innerException != null)
            {
                list.Add(innerException.Message);
                innerException = innerException.InnerException;
            }
            innerExceptions = list;

            var errorData = new
            {
                error = source.Message,
                stackTrace = source.StackTrace,
                source = source.Source,
                innerExceptions = innerExceptions,
                propertyName = propertyName,
            };
            return new JsonResult() { Data = errorData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
