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
using System.IO;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace BWakaBats.Mvc
{
    /// <summary>
    /// Defines a synchronous HTTP handler for JS files
    /// </summary>
    public class CssHttpHandler : CachedHttpHandler
    {
        private static Minifier _minifier = new Minifier();

        protected override string ContentType
        {
            get { return "text/css"; }
        }

        protected override object Process(HttpContext context, FileInfo fileInfo, string physicalPath)
        {
            var content = File.ReadAllText(physicalPath);
            return _minifier.MinifyStyleSheet(content);
        }
    }
}
