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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace BWakaBats.Extensions
{
    public enum ButtonBehavior
    {
        Automatic,
        Submit,
        Button,
        Reset,
    }
    public static partial class HtmlHelperExtensions
    {
        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string innerHtml, ButtonBehavior type, object htmlAttributes)
        {
            return htmlHelper.Button(innerHtml, null, type, null, null, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string innerHtml, ButtonBehavior type, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Button(innerHtml, null, type, null, null, htmlAttributes);
        }

        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string innerHtml, ButtonBehavior type, string name, object htmlAttributes)
        {
            return htmlHelper.Button(innerHtml, null, type, name, null, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string innerHtml, ButtonBehavior type, string name, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Button(innerHtml, null, type, name, null, htmlAttributes);
        }

        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string innerHtml, ButtonBehavior type, string name, string value, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Button(innerHtml, null, type, name, value, htmlAttributes);
        }

        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string innerHtml, ButtonBehavior type, string name = null, string value = null, object htmlAttributes = null)
        {
            return htmlHelper.Button(innerHtml, null, type, name, value, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string innerHtml, string url, ButtonBehavior type, object htmlAttributes)
        {
            return htmlHelper.Button(innerHtml, url, type, null, null, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string innerHtml, string url, ButtonBehavior type, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Button(innerHtml, url, type, null, null, htmlAttributes);
        }

        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string innerHtml, string url, ButtonBehavior type, string name, object htmlAttributes)
        {
            return htmlHelper.Button(innerHtml, url, type, name, null, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string innerHtml, string url, ButtonBehavior type, string name, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Button(innerHtml, url, type, name, null, htmlAttributes);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string innerHtml, string url, ButtonBehavior type, string name, string value, IDictionary<string, object> htmlAttributes)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            }
            var tag = new TagBuilder("button")
            {
                InnerHtml = innerHtml,
            };
            if (type != ButtonBehavior.Automatic)
            {
                tag.Attributes.Add("type", type.ToString().ToLowerInvariant());
            }
            else if (url == null)
            {
                tag.Attributes.Add("type", "submit");
            }
            else
            {
                tag.Attributes.Add("type", "button");
            }
            tag.MergeNotNullAttribute("name", name);
            tag.MergeNotNullAttribute("value", value);
            string id = TagBuilder.CreateSanitizedId(name);
            tag.MergeNotNullAttribute("id", id);
            tag.MergeNotNullAttribute("onclick", "javascript: location.href='" + url + "'");

            tag.MergeAttributes(htmlAttributes, true);
            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string innerHtml, string url = null, ButtonBehavior type = ButtonBehavior.Submit, string name = null, string value = null, object htmlAttributes = null)
        {
            return htmlHelper.Button(innerHtml, url, type, name, value, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
    }
}