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
    public abstract class EmailBox<TControl> : TextBase<TControl>
        where TControl : EmailBox<TControl>
    {
        protected EmailBox(TextBaseContext context, string name) : base(context, name) { }

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeAttribute("type", "email");
            tag.MergeAttribute("data-val-email", ValidationMessage("Invalid email", "Please enter a valid email address"));
            tag.AddCssClass("text-lower");
            return base.UpdateTag(tag) || true;
        }
    }

    public sealed class EmailBox : EmailBox<EmailBox>
    {
        internal EmailBox(string name = null) : base(new TextBaseContext(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static EmailBox BootstrapEmailBox(this HtmlHelper htmlHelper, string name)
        {
            var control = new EmailBox(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static EmailBox BootstrapEmailBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new EmailBox();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
