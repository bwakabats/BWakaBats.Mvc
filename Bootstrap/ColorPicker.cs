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
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public enum ColorPickerRange
    {
        Light,
        Normal,
        Dark
    }

    public enum ColorPickerType
    {
        Simple,
        Advanced,
        Full
    }

    public enum ColorPickerAdditionalType
    {
        None,
        Advanced,
        Full
    }

    public abstract class ColorPicker<TControl> : PreAppend<TControl, string>
        where TControl : ColorPicker<TControl>
    {
        internal ColorPicker(ColorPickerContext context, string name)
            : base(context, name, true)
        {
            Context = (ColorPickerContext)base.Context;
        }

        public new ColorPickerContext Context { get; private set; }

        public override void Initialize<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            base.Initialize(htmlHelper, expression);
            Context.MaxLength = ValidationAttribute("length-max", 6);
        }

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

        public TControl OptionalLabel(string newValue)
        {
            Context.OptionalLabel = newValue;
            return (TControl)this;
        }

        public TControl Range(ColorPickerRange newValue)
        {
            Context.Range = newValue;
            return (TControl)this;
        }

        public TControl NormalType(ColorPickerType newValue)
        {
            Context.NormalType = newValue;
            return (TControl)this;
        }

        public TControl AdditionalType(ColorPickerAdditionalType newValue)
        {
            Context.AdditionalType = newValue;
            return (TControl)this;
        }

        public TControl MaxLength(int newValue)
        {
            Context.MaxLength = newValue;
            return (TControl)this;
        }

        public TControl ShowHexadecimal(bool newValue = true)
        {
            Context.ShowHexadecimal = newValue;
            return (TControl)this;
        }

        #endregion

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeAttribute("type", "hidden");
            tag.MergeAttribute("maxlength", Context.MaxLength.ToString(CultureInfo.InvariantCulture));
            tag.MergeNotNullAttribute("value", Context.Value);
            tag.MergeNotNullAttribute("data-colorpicker", Context.NormalType.ToString().ToLowerInvariant());
            bool more = Context.AdditionalType == ColorPickerAdditionalType.Advanced && Context.NormalType != ColorPickerType.Advanced
                     || Context.AdditionalType == ColorPickerAdditionalType.Full && Context.NormalType != ColorPickerType.Full;
            if (more)
            {
                tag.MergeNotNullAttribute("data-colorpicker-additional", Context.AdditionalType.ToString().ToLowerInvariant());
            }
            tag.MergeNotNullAttribute("data-colorpicker-range", Context.Range.ToString().ToLowerInvariant());
            return base.UpdateTag(tag);
        }

        protected override string WrapTag(TagBuilder tag)
        {
            string value = Context.Value;
            string background = value ?? "fff";
            string foreground;
            if (value == null)
            {
                foreground = "ccc";
            }
            else if (value.Length >= 5)
            {
                foreground = (HexToNumber(value[0]) + HexToNumber(value[1]) + HexToNumber(value[2]) > 30) ? "333" : "FFF";
            }
            else if (value.Length >= 5)
            {
                foreground = (HexToNumber(value[0]) + HexToNumber(value[2]) * 2 + HexToNumber(value[4]) > 30) ? "333" : "FFF";
            }
            else
            {
                foreground = "fff";
            }
            string id = Context.Id;
            string buttonStyle = "btn-" + (Context.Style == ButtonStyle.Information ? "info" : Context.Style.ToString().ToLowerInvariant());
            string optional = (Context.IsRequired ? "" : "<div class='colorpicker-none'><a href='#'>" + (Context.OptionalLabel ?? "(none)") + "</a></div>");
            bool more = Context.AdditionalType == ColorPickerAdditionalType.Advanced && Context.NormalType != ColorPickerType.Advanced
                     || Context.AdditionalType == ColorPickerAdditionalType.Full && Context.NormalType != ColorPickerType.Full;
            string changeType = more ? "<div class='colorpicker-more'><a href='#'>More</a></div>" : "";
            string hex = Context.ShowHexadecimal ? "<div class='colorpicker-hex'>" + background + "</div>" : "";
            string hasBottom = !Context.IsRequired || more || Context.ShowHexadecimal ? " class='colorpicker-bottom'" : "";

            var group = new TagBuilder("div");
            if (Context.Append.Count + Context.Prepend.Count > 0)
            {
                group.AddCssClass("input-group-btn");
            }
            else
            {
                group.AddCssClass("btn-group");
            }
            group.InnerHtml = "<button type='button' class='btn " + buttonStyle + " btn-colorpicker dropdown-toggle' data-toggle='dropdown' id='" + id + "_button' style='color: #" + foreground + ";background: #" + background + "'>"
                + Context.Icon.FixedWidth().ToTag()
                + " <span class='caret'></span>"
                + "</button>"
                + "<div id='" + id + "_popup' class='colorpicker-popup'>"
                + "<div class='colorpicker-image colorpicker-grey'></div>"
                + "<div class='colorpicker-image colorpicker-hue'></div>"
                + "<div class='colorpicker-image colorpicker-spectrum'></div>"
                + "<div" + hasBottom + ">"
                + optional
                + changeType
                + hex
                + "</div>"
                + "</div>"
                + "<span id='" + id + "_ColorName'>"
                + GeneratePlaceholder()
                + "</span>";

            return tag.ToString() + base.WrapTag(group);
        }

        private static int HexToNumber(char v)
        {
            // Convert      0-9 => 0-9           A-F => 10-15   a-f => 10-15
            return v < 'A' ? (v - '0') : (v < 'a' ? (v - '7') : (v - 'W'));
        }

        protected override string WrapGroupStyle
        {
            get { return "width:0;display:block;"; }
        }
    }

    public class ColorPickerContext : PreAppendContext<string>
    {
        public string OptionalLabel { get; internal set; }
        public AwesomeIcon Icon { get; internal set; } = AwesomeIcon.Tint;
        public ButtonStyle Style { get; internal set; }
        public ColorPickerRange Range { get; internal set; }
        public ColorPickerType NormalType { get; internal set; }
        public ColorPickerAdditionalType AdditionalType { get; internal set; }
        public int MaxLength { get; internal set; } = 6;
        public bool ShowHexadecimal { get; internal set; }
    }

    public sealed class ColorPicker : ColorPicker<ColorPicker>
    {
        internal ColorPicker(string name = null) : base(new ColorPickerContext(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static ColorPicker BootstrapColorPicker(this HtmlHelper htmlHelper, string name)
        {
            var control = new ColorPicker(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static ColorPicker BootstrapColorPickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new ColorPicker();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
