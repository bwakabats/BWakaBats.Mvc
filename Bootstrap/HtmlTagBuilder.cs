// *****************************************************
//               MVC EXPANSION - BOOTSTRAP
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
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public class HtmlTagBuilder : TagBuilder
    {
        public HtmlTagBuilder(string tagName)
            : base(tagName)
        {
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(InnerHtml))
                return base.ToString();

            switch (TagName)
            {
                case "span":
                case "div":
                case "textarea":
                case "select":
                case "label":
                case "ul":
                case "ol":
                    return base.ToString();
            }

            return base.ToString(TagRenderMode.SelfClosing);
        }
    }
}
