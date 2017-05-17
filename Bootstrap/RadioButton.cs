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
    public abstract class RadioButton<TControl, TValue> : BoundControl<TControl, TValue>
            where TControl : RadioButton<TControl, TValue>
    {
        protected RadioButton(RadioButtonContext<TValue> context, string name)
            : base(context, name, true)
        {
            Context = context;
        }

        public new RadioButtonContext<TValue> Context { get; private set; }

        #region Control Properties

        public TControl IsInline(bool newValue = true)
        {
            Context.IsInline = newValue;
            return (TControl)this;
        }

        public TControl IsChecked(bool newValue = true)
        {
            Context.IsChecked = newValue;
            return (TControl)this;
        }

        public TControl AllowMultiple(bool newValue = true)
        {
            Context.AllowMultiple = newValue;
            return (TControl)this;
        }

        #endregion

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeAttribute("type", Context.AllowMultiple ? "checkbox" : "radio");
            tag.MergeNotNullAttribute("value", Context.Value);
            tag.MergeIfAttribute("checked", "checked", Context.IsChecked);
            return base.UpdateTag(tag);
        }

        public override string ToHtmlString()
        {
            var tag = CreateTag();

            var label = new HtmlTagBuilder("label");
            string id = Context.Id;
            if (!string.IsNullOrWhiteSpace(id) && !Context.AllowMultiple)
            {
                label.Attributes.Add("for", id);
            }
            if (Context.IsHidden)
            {
                label.MergeAttribute("style", "display: none;", true);
            }
            if (Context.IsInvisible)
            {
                label.AddCssClass("invisible");
            }
            label.InnerHtml = tag.ToString() + Context.Header;

            string name = Context.Name;
            if (Context.IsInline)
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    label.Attributes.Add("data-container-for", name);
                }
                label.AddCssClass("radio-inline");
                return label.ToString();
            }

            string output = "<div class='" + (Context.AllowMultiple ? "checkbox" : "radio") + "'";
            if (!string.IsNullOrWhiteSpace(name))
            {
                output += " data-container-for='" + name + "'";
            }
            output += ">" + label.ToString() + "</div>";

            string description = Context.Description;
            if (!string.IsNullOrWhiteSpace(description))
            {
                output += "<div class='help-block'>" + description + "</div>";
            }
            return output;
        }
    }

    public class RadioButtonContext<TValue> : BoundControlContext<TValue>
    {
        public bool IsInline { get; internal set; }
        public bool IsChecked { get; internal set; }
        public bool AllowMultiple { get; internal set; }
    }

    public sealed class RadioButton<TValue> : RadioButton<RadioButton<TValue>, TValue>
    {
        internal RadioButton(string name = null) : base(new RadioButtonContext<TValue>(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static RadioButton<object> BootstrapRadioButton(this HtmlHelper htmlHelper, string name)
        {
            var control = new RadioButton<object>(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static RadioButton<TProperty> BootstrapRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new RadioButton<TProperty>();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
