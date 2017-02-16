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
    public abstract class PasswordBox<TControl> : TextBase<TControl>
        where TControl : PasswordBox<TControl>
    {
        protected PasswordBox(TextBaseContext context, string name) : base(context, name) {}

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeAttribute("type", "password");
            return base.UpdateTag(tag);
        }
    }

    public sealed class PasswordBox : PasswordBox<PasswordBox>
    {
        internal PasswordBox(string name = null) : base(new TextBaseContext(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static PasswordBox BootstrapPasswordBox(this HtmlHelper htmlHelper, string name)
        {
            var control = new PasswordBox(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static PasswordBox BootstrapPasswordBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new PasswordBox();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
