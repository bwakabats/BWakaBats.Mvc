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
using System.Globalization;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public abstract class Display<TControl> : Element<TControl>, IHtmlString
        where TControl : Display<TControl>
    {
        protected Display(DisplayContext context)
            : base(context, null)
        {
            Context = (DisplayContext)base.Context;
        }

        public new DisplayContext Context { get; private set; }

        public override sealed void Initialize<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            Context = (DisplayContext)base.Context;
            base.Initialize<TModel, TProperty>(htmlHelper, expression);
        }

        public void Initialize<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format)
        {
            base.Initialize<TModel, TProperty>(htmlHelper, expression);

            object model = Context.Metadata.Model;
            string value;
            var propertyType = typeof(TProperty);
            if (propertyType == typeof(bool) || propertyType == typeof(bool?))
            {
                bool? boolProperty = (bool?)model;
                value = boolProperty.HasValue ? (boolProperty.Value ? AwesomeIcon.Check.ToTag().ToHtmlString() : AwesomeIcon.Remove.ToTag().ToHtmlString()) : null;
            }
            else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
            {
                var valueDateTime = (DateTime?)model;
                value = valueDateTime.HasValue ? valueDateTime.Value.ToString(format, CultureInfo.InvariantCulture) : null;
            }
            else if (propertyType == typeof(TimeSpan) || propertyType == typeof(TimeSpan?))
            {
                var valueTimeSpan = (TimeSpan?)model;
                value = valueTimeSpan.HasValue ? valueTimeSpan.Value.ToString(format, CultureInfo.InvariantCulture) : null;
            }
            else if (string.IsNullOrWhiteSpace(format))
            {
                value = model?.ToString();
            }
            else
            {
                try
                {
                    value = Convert.ToDouble(model, CultureInfo.InvariantCulture).ToString(format, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Format not supported for " + propertyType.Name, "format", ex);
                }
            }
            Value(value);
        }

        #region Control Properties

        public TControl Value(string newValue)
        {
            Context.Value = newValue;
            return (TControl)this;
        }

        public TControl Description(string newValue)
        {
            Context.Description = newValue;
            return (TControl)this;
        }

        public TControl IfIsNull(string newValue)
        {
            Context.IfIsNull = newValue;
            return (TControl)this;
        }

        #endregion

        public override string ToString()
        {
            return ToHtmlString();
        }

        public string ToHtmlString()
        {
            var tag = CreateTag();

            bool requiresContainer = false;
            string output = "";
            string header = Context.Header;
            if (header != null)
            {
                requiresContainer = true;
                if (string.IsNullOrWhiteSpace(Context.Id))
                {
                    output += "<label>";
                }
                else
                {
                    output += "<label for='" + Context.Id + "'>";
                }
                output += header + "</label>";
            }

            output += tag.ToString();

            string description = Context.Description;
            if (!string.IsNullOrWhiteSpace(description))
            {
                requiresContainer = true;
                output += "<div class='help-block'>" + description + "</div>";
            }

            string name = Context.Name;
            if (!requiresContainer)
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    tag.MergeAttribute("data-container-for", name);
                }
                return tag.ToString();
            }

            var div = new HtmlTagBuilder("div");
            if (!string.IsNullOrWhiteSpace(name))
            {
                div.Attributes.Add("data-container-for", name);
            }

            div.AddCssClass("form-group");
            div.InnerHtml = output;
            return div.ToString();
        }

        protected override string TagType
        {
            get { return "p"; }
        }

        protected override string DefaultCssClass
        {
            get { return "form-control-static"; }
        }

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.InnerHtml = Context.Value ?? Context.IfIsNull;
            return base.UpdateTag(tag);
        }
    }

    public class DisplayContext : ElementContext
    {
        public string Value { get; internal set; }
        public string Description { get; internal set; }
        public string IfIsNull { get; internal set; }
    }
    public sealed class Display : Display<Display>
    {
        internal Display() : base(new DisplayContext()) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static Display BootstrapDisplay(this HtmlHelper htmlHelper, string header = null)
        {
            var control = new Display();
            control.Initialize(htmlHelper);
            control.Header(header);
            return control;
        }

        public static Display BootstrapDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format = null)
        {
            var control = new Display();
            control.Initialize(htmlHelper, expression, format);
            return control;
        }
    }
}
