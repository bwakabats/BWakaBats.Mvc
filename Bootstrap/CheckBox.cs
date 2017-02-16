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
    public abstract class CheckBox<TControl> : BoundControl<TControl, bool>
        where TControl : CheckBox<TControl>
    {
        protected CheckBox(CheckBoxContext context, string name)
            : base(context, name, true)
        {
            Context = (CheckBoxContext)base.Context;
        }

        public new CheckBoxContext Context { get; private set; }

        #region Control Properties

        public TControl IsInline(bool newValue = true)
        {
            Context.IsInline = newValue;
            return (TControl)this;
        }

        #endregion

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeAttribute("type", "checkbox");
            tag.MergeAttribute("value", "true");
            tag.MergeIfAttribute("checked", "checked", Context.Value);
            return base.UpdateTag(tag);
        }

        public override string ToHtmlString()
        {
            var tag = CreateTag();

            var label = new HtmlTagBuilder("label");
            string id = Context.Id;
            if (!string.IsNullOrWhiteSpace(id))
            {
                label.Attributes.Add("for", id);
            }
            label.InnerHtml = tag.ToString() + Context.Header;

            string name = Context.Name;
            HtmlTagBuilder container;
            if (!Context.IsInline)
            {
                label.AddCssClass("clearfix");
                container = new HtmlTagBuilder("div");
                container.AddCssClass("form-group");
            }
            else
            {
                label.AddCssClass("checkbox-inline");
                container = label;
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                container.MergeAttribute("data-container-for", name);
            }
            if (Context.IsHidden)
            {
                container.MergeAttribute("style", "display: none;", true);
            }
            if (Context.IsInvisible)
            {
                container.AddCssClass("invisible");
            }

            if (!Context.IsInline)
            {
                string output = "<div class='checkbox'>" + label.ToString() + "</div>";
                string description = Context.Description;
                if (!string.IsNullOrWhiteSpace(description))
                {
                    output += "<div class='help-block'>" + description + "</div>";
                }
                container.InnerHtml = output;
            }

            return container.ToString();
        }
    }

    public class CheckBoxContext : BoundControlContext<bool>
    {
        public bool IsInline { get; internal set; }
    }

    public sealed class CheckBox : CheckBox<CheckBox>
    {
        internal CheckBox(string name = null) : base(new CheckBoxContext(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static CheckBox BootstrapCheckBox(this HtmlHelper htmlHelper, string name)
        {
            var control = new CheckBox(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static CheckBox BootstrapCheckBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new CheckBox();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
