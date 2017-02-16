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
using BWakaBats.Extensions;

namespace BWakaBats.Bootstrap
{
    public abstract class Control<TControl, TValue> : Element<TControl>
        where TControl : Control<TControl, TValue>
    {
        protected Control(ControlContext<TValue> context, string name)
            : base(context, name)
        {
            Context = context;
        }

        public new ControlContext<TValue> Context { get; private set; }

        public override void Initialize<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            base.Initialize(htmlHelper, expression);

            if (expression == null)
                return;

            var model = Context.Metadata.Model;

            TValue propertyValue = ConvertProperty<TValue, TProperty>(model);
            Value(propertyValue);
            if (!string.IsNullOrWhiteSpace(Context.Metadata.ShortDisplayName) && Context.Metadata.ShortDisplayName != Context.Metadata.DisplayName)
            {
                Description(Context.Metadata.DisplayName);
            }
            FindHtmlAttribute("description", v => Description(v));
        }

        #region Control Properties

        public TControl Value(TValue newValue)
        {
            Context.Value = newValue;
            return (TControl)this;
        }

        public TControl Description(string newValue)
        {
            Context.Description = newValue;
            return (TControl)this;
        }

        public TControl IsDisabled(bool newValue = true)
        {
            Context.IsDisabled = newValue;
            return (TControl)this;
        }

        public TControl IsReadOnly(bool newValue = true)
        {
            Context.IsReadOnly = newValue;
            return (TControl)this;
        }

        public TControl IsHidden(bool newValue = true)
        {
            Context.IsHidden = newValue;
            return (TControl)this;
        }

        public TControl IsInvisible(bool newValue = true)
        {
            Context.IsInvisible = newValue;
            return (TControl)this;
        }

        public TControl OnClick(string newValue)
        {
            Context.OnClick = newValue;
            return (TControl)this;
        }

        #endregion

        protected override bool UpdateTag(TagBuilder tag)
        {
            if (!string.IsNullOrWhiteSpace(Context.OnClick))
            {
                tag.MergeAttribute("onclick", "javascript: " + Context.OnClick);
            }

            tag.MergeIfAttribute("readonly", "readonly", Context.IsReadOnly);
            // So that ReadOnly looks the same for the user as disabld, we disable it, but then add a Hidden so it is passed back to the server
            // (See BoundControl.WrapTag)
            tag.MergeIfAttribute("disabled", "disabled", Context.IsReadOnly || Context.IsDisabled);
            return base.UpdateTag(tag);
        }
    }

    public class ControlContext<TValue> : ElementContext
    {
        public TValue Value { get; internal set; }
        public string Description { get; internal set; }
        public bool IsDisabled { get; internal set; }
        public bool IsReadOnly { get; internal set; }
        public bool IsHidden { get; internal set; }
        public bool IsInvisible { get; internal set; }
        public string OnClick { get; internal set; }
    }
}
