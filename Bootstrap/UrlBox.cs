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
using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public abstract class UrlBox<TControl> : TextBase<TControl>
        where TControl : UrlBox<TControl>
    {
        protected UrlBox(TextBaseContext context, string name) : base(context, name) { }

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeAttribute("type", "url");
            tag.MergeAttribute("data-val-url", ValidationMessage("Invalid address", "Please enter a valid address"));
            return base.UpdateTag(tag) || true;
        }
    }

    public sealed class UrlBox : UrlBox<UrlBox>
    {
        internal UrlBox(string name = null) : base(new TextBaseContext(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static UrlBox BootstrapUrlBox(this HtmlHelper htmlHelper, string name)
        {
            var control = new UrlBox(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static UrlBox BootstrapUrlBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new UrlBox();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
