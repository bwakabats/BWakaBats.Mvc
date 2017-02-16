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
using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public abstract class InputBox<TControl, TValue> : BoundControl<TControl, TValue>
        where TControl : InputBox<TControl, TValue>
    {
        protected InputBox(InputBoxContext<TValue> context, string name, bool isWide)
            : base(context, name, isWide)
        {
            Context = context;
        }

        public new InputBoxContext<TValue> Context { get; private set; }

        public override void Initialize<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            base.Initialize<TModel, TProperty>(htmlHelper, expression);
            FindHtmlAttribute("placeholder", v => Placeholder(v));
        }

        #region Control Properties

        public TControl Placeholder(string newValue)
        {
            Context.Placeholder = newValue;
            return (TControl)this;
        }

        #endregion

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeNotNullAttribute("placeholder", GeneratePlaceholder());
            return base.UpdateTag(tag);
        }

        public sealed override string ToHtmlString()
        {
            // This method does nothing but is used to seal it
            // However, it was previously sealed
            // (for some reason I now can't remember)
            // and then unsealed (again I can't rememeber why)
            return base.ToHtmlString();
        }

        protected virtual string GeneratePlaceholder()
        {
            if (Context.Placeholder != null)
                return Context.Placeholder;

            if (Context.IsReadOnly || Context.IsDisabled)
                return "";

            string name = Context.Header ?? Context.Name.ToWords(true);
            return DefaultPlaceholder(name);
        }

        protected virtual string DefaultPlaceholder(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Context.IsRequired ? "Required" : "Optional";
            return Context.IsRequired ? name.Substring(0, 1) + name.Substring(1).ToLowerInvariant() + " is required" : "Please enter " + name.ToLowerInvariant();
        }
    }

    public class InputBoxContext<TValue> : BoundControlContext<TValue>
    {
        public string Placeholder { get; internal set; }
    }
}
