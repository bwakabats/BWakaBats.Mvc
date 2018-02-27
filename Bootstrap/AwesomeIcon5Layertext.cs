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
using BWakaBats.Extensions;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    internal class AwesomeIcon5LayerText : IAwesomeIcon5Layer
    {
        private string _value { get; }
        private object _htmlAttributes { get; }

        public AwesomeIcon5LayerText(string value, object htmlAttributes)
        {
            _value = value;
            _htmlAttributes = htmlAttributes;
        }

        public string ToAwesomeIcon5LayerString()
        {
            if (_htmlAttributes == null)
                return "<span class='fa-layers-text'>" + _value + "</span>";

            var tag = new TagBuilder("span");
            tag.MergeAttributes(_htmlAttributes);
            tag.AddCssClass("fa-layers-text");
            tag.InnerHtml = _value;
            return tag.ToString();
        }
    }
}