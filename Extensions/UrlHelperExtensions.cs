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
using System.Diagnostics.CodeAnalysis;
using System.Web.Hosting;
using System.Web.Mvc;

namespace BWakaBats.Extensions
{
    public static class UrlHelperExtensions
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "url")]
        public static bool Exists(this UrlHelper url, string contentPath)
        {
            string serverPath;
            if (contentPath[0] == '~')
            {
                serverPath = HostingEnvironment.ApplicationPhysicalPath + contentPath.Substring(2).Replace('/', '\\');
            }
            else
            {
                serverPath = HostingEnvironment.ApplicationPhysicalPath + contentPath.Replace('/', '\\');
            }
            return System.IO.File.Exists(serverPath);
        }
    }
}
