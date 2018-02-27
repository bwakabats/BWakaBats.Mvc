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
    public abstract class Icon5Picker<TControl> : PreAppend<TControl, string>
        where TControl : Icon5Picker<TControl>
    {
        protected Icon5Picker(Icon5PickerContext context, string name)
            : base(context, name, true)
        {
            Context = context;
        }

        public new Icon5PickerContext Context { get; private set; }

        #region Control Properties

        public TControl IsFree(bool newValue = true)
        {
            Context.IsFree = newValue;
            return (TControl)this;
        }

        public TControl Style(ButtonStyle newValue)
        {
            Context.Style = newValue;
            return (TControl)this;
        }

        public TControl OptionalLabel(string newValue)
        {
            Context.OptionalLabel = newValue;
            return (TControl)this;
        }

        #endregion

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeAttribute("data-icon5picker", Context.OptionalLabel);
            if (Context.IsFree)
            {
                tag.MergeAttribute("data-icon5picker-free", "true");
            }
            tag.MergeAttribute("type", "hidden");
            tag.MergeNotNullAttribute("value", Context.Value);
            return base.UpdateTag(tag);
        }

        protected override string WrapTag(TagBuilder tag)
        {
            var value = AwesomeIcon5.FromString(Context.Value ?? "fas fa-times").FixedWidth().Grow(8);
            string id = Context.Id;
            string buttonStyle = "btn-" + (Context.Style == ButtonStyle.Information ? "info" : Context.Style.ToString().ToLowerInvariant());

            var group = new TagBuilder("div");
            if (Context.Append.Count + Context.Prepend.Count > 0)
            {
                group.AddCssClass("input-group-btn");
            }
            else
            {
                group.AddCssClass("btn-group");
            }

            group.InnerHtml =
                  "<button id='" + id + "_Button' type='button' class='btn " + buttonStyle + " btn-icon5picker dropdown-toggle' data-toggle='dropdown'>"
                    + "<span id='" + id + "_Icon'"
                    + (Context.Value == null ? "class='fa-none'" : "")
                    + ">" + value + "</span>"
                    + "&nbsp;&nbsp;<span class='caret'></span>"
                + "</button>"
                + "<div id='" + id + "_Popup' class='icon5picker-popup'>"
                    + "<div class='icon5picker-popup-header form-inline'>"
                    + "<label>Search: </label><input type='text' class='form-control' />"
                    + "</div>"
                    + "<div class='icon5picker-popup-results'></div>"
                + "</div>"
                + "<span id='" + Context.Id + "_IconName'>"
                    + GeneratePlaceholder()
                + "</span>";

            return tag + base.WrapTag(group);
        }

        protected override string WrapGroupStyle
        {
            get { return "width:0;display:block;"; }
        }
    }

    public class Icon5PickerContext : PreAppendContext<string>
    {
        public bool IsFree { get; internal set; }
        public string OptionalLabel { get; internal set; }
        public ButtonStyle Style { get; internal set; }
    }

    public sealed class Icon5Picker : Icon5Picker<Icon5Picker>
    {
        internal Icon5Picker(string name = null) : base(new Icon5PickerContext(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static Icon5Picker BootstrapIcon5Picker(this HtmlHelper htmlHelper, string name)
        {
            var control = new Icon5Picker(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static Icon5Picker BootstrapIcon5PickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new Icon5Picker();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
