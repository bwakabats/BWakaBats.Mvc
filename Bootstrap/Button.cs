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
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    #region enums

    public enum ButtonStyle
    {
        Primary,
        Default,
        Success,
        Information,
        Warning,
        Danger,
    }

    #endregion

    public abstract class Button<TControl> : Control<TControl, string>, IHtmlString, IPreAppender
        where TControl : Button<TControl>
    {
        protected Button(ButtonContext context)
            : base(context, null)
        {
            Context = (ButtonContext)base.Context;
        }

        public new ButtonContext Context { get; private set; }

        #region Control Properties

        public TControl Icon(AwesomeIcon newValue)
        {
            Context.Icon = newValue;
            return (TControl)this;
        }

        public TControl Style(ButtonStyle newValue)
        {
            Context.Style = newValue;
            return (TControl)this;
        }

        public TControl Behavior(ButtonBehavior newValue)
        {
            Context.Behavior = newValue;
            return (TControl)this;
        }

        public TControl Href(string newValue)
        {
            Context.Href = newValue;
            return (TControl)this;
        }

        public TControl NewWindow(bool newValue = true)
        {
            Context.NewWindow = newValue;
            return (TControl)this;
        }

        public TControl Interrupt(string newValue = "true")
        {
            Context.Interrupt = newValue;
            return (TControl)this;
        }

        #endregion

        protected override string TagType
        {
            get { return "button"; }
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        protected override bool UpdateTag(TagBuilder tag)
        {
            string header = Context.Header;
            if (string.IsNullOrWhiteSpace(header))
            {
                tag.InnerHtml = Context.Icon.FixedWidth(true).ToTag().ToHtmlString();
            }
            else
            {
                tag.InnerHtml = Context.Icon.FixedWidth(true).ToTag().ToHtmlString() + " " + header;
            }

            tag.MergeNotNullAttribute("value", Context.Value);
            if (Context.Behavior == ButtonBehavior.Automatic)
            {
                tag.MergeAttribute("type", string.IsNullOrWhiteSpace(Context.OnClick) && string.IsNullOrWhiteSpace(Context.Href) && !Context.IsReadOnly ? "submit" : "button");
            }
            else
            {
                tag.MergeAttribute("type", Context.Behavior.ToString().ToLowerInvariant());
            }

            tag.MergeNotNullAttribute("data-href", Context.Href);
            tag.MergeNotNullAttribute("data-interrupt", Context.Interrupt);
            if (string.IsNullOrWhiteSpace(Context.OnClick) && !string.IsNullOrWhiteSpace(Context.Href))
            {
                if (Context.NewWindow)
                {
                    tag.MergeAttribute("onclick", "javascript: window.open('" + Context.Href + "')");
                }
                else
                {
                    tag.MergeAttribute("onclick", "javascript: location.href='" + Context.Href + "'");
                }
            }
            tag.MergeIfAttribute("style", "display: none;", Context.IsHidden);
            if (Context.IsInvisible)
            {
                tag.AddCssClass("invisible");
            }

            return base.UpdateTag(tag);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        protected override string DefaultCssClass
        {
            get
            {
                if (Context.Style == ButtonStyle.Information)
                    return "btn btn-info";
                return "btn btn-" + Context.Style.ToString().ToLowerInvariant();
            }
        }

        public override string ToString()
        {
            return ToHtmlString();
        }

        public string ToHtmlString()
        {
            var tag = CreateTag();

            string name = Context.Name;
            string description = Context.Description;
            if (description == null)
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    tag.Attributes.Add("data-container-for", name);
                }
                return tag.ToString() + Environment.NewLine;
            }

            tag.AddCssClass("btn-block");
            string output = "<div class='row'";
            if (!string.IsNullOrWhiteSpace(name))
            {
                output += " data-container-for='" + name + "'";
            }
            return output + "><div class='col-xs-4 col-sm-3'>" + tag.ToString() + "</div>"
                 + "<div class='col-xs-8 col-sm-9'>" + description + "</div>"
                 + "<div class='clearfix'></div>"
                 + "</div>" + Environment.NewLine;
        }

        public string ToPreAppenderString()
        {
            return "<span class='input-group-btn'>" + ToString() + "</span>";
        }
    }

    public class ButtonContext : ControlContext<string>
    {
        public AwesomeIcon Icon { get; internal set; }
        public ButtonStyle Style { get; internal set; }
        public ButtonBehavior Behavior { get; internal set; }
        public string Href { get; internal set; }
        public bool NewWindow { get; internal set; }
        public string Interrupt { get; internal set; }
    }

    public sealed class Button : Button<Button>
    {
        internal Button() : base(new ButtonContext()) { }
    }

    public static partial class HtmlHelperExtensions
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static Button BootstrapButton(this HtmlHelper htmlHelper, AwesomeIcon icon)
        {
            var control = new Button();
            control.Initialize(htmlHelper);
            control.Icon(icon);
            return control;
        }
    }
}
