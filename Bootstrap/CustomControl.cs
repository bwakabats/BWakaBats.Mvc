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
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using BWakaBats.Extensions;

namespace BWakaBats.Bootstrap
{
    public abstract class CustomControl<TControl> : Control<TControl, object>, IContainer<TControl>
        where TControl : CustomControl<TControl>
    {
        private bool _begun;

        protected CustomControl(ControlContext<object> context) : base(context, null) { }

        public TControl Begin()
        {
            if (_begun)
                throw new Exception("Already called Begin");

            string name = Context.Name;
            string id = Context.Id;

            var div = new HtmlTagBuilder("div");
            if (!string.IsNullOrWhiteSpace(name))
            {
                div.Attributes.Add("data-container-for", name);
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                div.Attributes.Add("data-group", id);
            }
            if (Context.IsHidden)
            {
                div.Attributes.Add("style", "display: none;");
            }
            if (Context.IsInvisible)
            {
                div.AddCssClass("invisible");
            }

            div.AddCssClass("form-group");
            div.AddCssClass("has-feedback");

            var context = HtmlHelper.ViewContext;
            context.Writer.Write(div.ToString(TagRenderMode.StartTag));

            string header = Context.Header;
            if (header != null)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    context.Writer.Write("<label>");
                }
                else
                {
                    context.Writer.Write("<label for='" + name + "'>");
                }
                context.Writer.Write(header + "</label>");
                if (!string.IsNullOrWhiteSpace(name))
                {
                    context.Writer.Write("<span class='field-validation-valid' data-valmsg-for='" + name + "' data-valmsg-replace='true'></span>");
                }
            }
            string description = Context.Description;
            if (!string.IsNullOrWhiteSpace(description))
            {
                context.Writer.Write("<div class='help-block'>" + description + "</div>");
            }

            var tag = new HtmlTagBuilder(TagType);
            tag.MergeAttributes(Context.HtmlAttributes);
            tag.MergeIfAttribute("readonly", "readonly", Context.IsReadOnly);
            tag.MergeIfAttribute("disabled", "disabled", Context.IsDisabled);
            context.Writer.Write(tag.ToString(TagRenderMode.StartTag));
            _begun = true;
            return (TControl)this;
        }

        public TControl End()
        {
            if (!_begun)
                throw new Exception("Cannot call End without Begin");

            var context = HtmlHelper.ViewContext;
            context.Writer.Write("</" + TagType + ">");
            context.Writer.Write("</div>");
            _begun = false;
            return (TControl)this;
        }

        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            if (_begun)
            {
                End();
            }
            GC.SuppressFinalize(this);
        }

        protected override string TagType
        {
            get { return "div"; }
        }
    }

    public sealed class CustomControl : CustomControl<CustomControl>
    {
        internal CustomControl() : base(new ControlContext<object>()) { }
    }

    public static partial class HtmlHelperExtensions
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static CustomControl BootstrapCustomControl(this HtmlHelper htmlHelper, string header)
        {
            var control = new CustomControl();
            control.Initialize(htmlHelper);
            control.Header(header);
            return control;
        }
    }
}
