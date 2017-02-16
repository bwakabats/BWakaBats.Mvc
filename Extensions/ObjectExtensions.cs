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
using System.Web;

namespace BWakaBats.Extensions
{
    public static class ObjectExtensions
    {
        public static IHtmlString ToHtmlString(this object value)
        {
            return new HtmlString(value == null ? string.Empty : value.ToString());
        }
    }
}