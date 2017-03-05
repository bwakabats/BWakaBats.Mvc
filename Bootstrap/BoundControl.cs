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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BWakaBats.Bootstrap
{
    public enum ShortMessageScreenSize
    {
        None,
        ExtraSmall,
        Small,
        Medium,
        Large
    }

    public abstract class BoundControl<TControl, TValue> : Control<TControl, TValue>, IHtmlString
        where TControl : BoundControl<TControl, TValue>
    {
        protected BoundControl(BoundControlContext<TValue> context, string name, bool isWide)
            : base(context, name)
        {
            Context = context;
            Context.IsWide = isWide;
        }

        public new BoundControlContext<TValue> Context { get; private set; }

        public override sealed void Initialize(HtmlHelper htmlHelper)
        {
            base.Initialize(htmlHelper);
        }

        public override void Initialize<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            base.Initialize(htmlHelper, expression);

            if (expression == null)
                return;

            Context.ValidationMessage = htmlHelper.ValidationMessageFor(expression).ToHtmlString();
            Context.ValidationAttributes = htmlHelper.GetUnobtrusiveValidationAttributes(Context.Name, Context.Metadata);
            Context.IsRequired = ValidationAttribute("required", (string)null) != null;
            Context.RemoteUrl = ValidationAttribute("remote-url", (string)null);
            Context.RemoteFields = ValidationAttribute("remote-additionalfields", (string)null);
        }

        #region Control Properties

        public TControl ShortMessage(ShortMessageScreenSize newValue)
        {
            Context.ShortMessage = newValue;
            return (TControl)this;
        }

        public TControl IsWide(bool newValue = true)
        {
            Context.IsWide = newValue;
            return (TControl)this;
        }

        public TControl IsRequired(bool newValue = true)
        {
            Context.IsRequired = newValue;
            return (TControl)this;
        }

        public TControl OnChange(string newValue)
        {
            Context.OnChange = newValue;
            return (TControl)this;
        }

        public TControl RemoteUrl(string newValue)
        {
            Context.RemoteUrl = newValue;
            return (TControl)this;
        }

        public TControl RemoteFields(string newValue)
        {
            Context.RemoteFields = newValue;
            return (TControl)this;
        }

        #endregion

        protected TResult ValidationAttribute<TResult>(string name, TResult defaultValue)
        {
            if (Context.ValidationAttributes == null)
                return defaultValue;

            object value;
            if (!Context.ValidationAttributes.TryGetValue("data-val-" + name, out value))
                return defaultValue;

            return ConvertProperty<TResult, object>(value);
        }

        protected string ValidationMessage(string shortMessage, string longMessage)
        {
            switch (Context.ShortMessage)
            {
                case ShortMessageScreenSize.ExtraSmall:
                    return "<span class='visible-xs'>" + shortMessage + "</span><span class='hidden-xs'>" + longMessage + "</span>";
                case ShortMessageScreenSize.Small:
                    return "<span class='visible-xs visible-sm'>" + shortMessage + "</span><span class='hidden-xs hidden-sm'>" + longMessage + "</span>";
                case ShortMessageScreenSize.Medium:
                    return "<span class='hidden-lg'>" + shortMessage + "</span><span class='visible-lg'>" + longMessage + "</span>";
                case ShortMessageScreenSize.Large:
                    return shortMessage;
                case ShortMessageScreenSize.None:
                    return longMessage;
            }
            throw new InvalidOperationException();
        }

        protected override bool UpdateTag(TagBuilder tag)
        {
            if (!string.IsNullOrWhiteSpace(Context.OnChange))
            {
                tag.MergeAttribute("onchange", "javascript: " + Context.OnChange);
            }
            bool result = tag.MergeNotNullAttribute("data-val-remote-url", Context.RemoteUrl);
            result = tag.MergeNotNullAttribute("data-val-remote-additionalfields", Context.RemoteFields) || result;
            tag.MergeIfAttribute("data-val-remote", ValidationMessage("Invalid", "Please correct this field."), result);
            if (Context.IsRequired)
            {
                tag.MergeAttribute("data-val-required", ValidationMessage("Required", "This field is required"));
                result = true;
            }
            return base.UpdateTag(tag) || result;
        }

        protected override string TagType
        {
            get { return "input"; }
        }

        public override string ToString()
        {
            return ToHtmlString();
        }

        public virtual string ToHtmlString()
        {
            var tag = CreateTag();

            string id = Context.Id;
            bool requiresContainer = false;
            string output = "";
            string feedBackStyle;
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
                feedBackStyle = "";
            }
            else
            {
                feedBackStyle = " style='top: 0px;'";
            }

            if (tag.IsValidated)
            {
                if (!string.IsNullOrWhiteSpace(Context.ValidationMessage))
                {
                    output += Context.ValidationMessage;
                }
                else if (!string.IsNullOrWhiteSpace(FullHtmlFieldName))
                {
                    output += "<span class='field-validation-valid' data-valmsg-for='" + FullHtmlFieldName + "' data-valmsg-replace='true'></span>";
                }
            }

            output += WrapTag(tag) + AppendToTag();

            if (Context.IsRequired)
            {
                requiresContainer = true;
                output += "<span class='fa fa-star form-control-feedback'" + feedBackStyle + "></span>";
            }
            else if (!string.IsNullOrWhiteSpace(id))
            {
                requiresContainer = true;
                if (string.IsNullOrWhiteSpace(feedBackStyle))
                {
                    feedBackStyle = "style='display: none;'";
                }
                else
                {
                    feedBackStyle = feedBackStyle.Substring(0, feedBackStyle.Length - 1) + ";display: none;'";
                }
                output += "<span class='glyphicon form-control-feedback' " + feedBackStyle + "></span>";
            }

            string description = Context.Description;
            if (!string.IsNullOrWhiteSpace(description))
            {
                requiresContainer = true;
                output += "<div class='help-block'>" + description + "</div>";
            }

            string name = Context.Name;
            HtmlTagBuilder container;
            if (requiresContainer)
            {
                container = new HtmlTagBuilder("div");
                container.AddCssClass("form-group");
                if (Context.IsRequired || !string.IsNullOrWhiteSpace(id))
                {
                    container.AddCssClass("has-feedback");
                }
            }
            else
            {
                container = tag; // *
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

            if (!requiresContainer)
                return WrapTag(container) + AppendToTag(); // container was set to tag (See * above) 

            container.InnerHtml = output + AppendToTag();
            return container.ToString();
        }

        protected virtual string WrapTag(TagBuilder tag)
        {
            return WrapTagPrivate(WrapTagInternal(tag.ToString()));
        }

        protected virtual string AppendToTag()
        {
            return "";
        }

        internal string WrapBaseTag(TagBuilder tag)
        {
            return WrapTagPrivate(tag.ToString());
        }

        internal string WrapTagInternal(string output)
        {
            if (Context.IsWide)
                return output;

            return "<div class='row'>"
                 + "<div class='col-xs-12 col-sm-6 clearfix'>"
                 + output
                 + "</div>"
                 + "</div>";
        }

        private string WrapTagPrivate(string tag)
        {
            if (!Context.IsReadOnly)
                return tag;

            // See Control.UpdateTag
            return HtmlHelper.Hidden(Context.Name, Context.Value) + tag;
        }
    }

    public class BoundControlContext<TValue> : ControlContext<TValue>
    {
        public ShortMessageScreenSize ShortMessage { get; internal set; } = ShortMessageScreenSize.ExtraSmall;
        public bool IsWide { get; internal set; }
        public bool IsRequired { get; internal set; }
        public string OnChange { get; internal set; }
        public string RemoteUrl { get; internal set; }
        public string RemoteFields { get; internal set; }
        public string ValidationMessage { get; internal set; }
        public IDictionary<string, object> ValidationAttributes { get; internal set; }
    }

    public static partial class HtmlHelperExtensions
    {
        public static IHtmlString BootstrapEditorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            IHtmlString editor;

            editor = EditorFromDataType(htmlHelper, expression);
            if (editor != null)
                return editor;

            editor = EditorFromTypeCode(htmlHelper, expression);
            if (editor != null)
                return editor;

            return htmlHelper.BootstrapTextBoxFor(expression);
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private static IHtmlString EditorFromDataType<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var Metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            switch (Metadata.DataTypeName)
            {
                case "DateTime":
                    return htmlHelper.BootstrapDateTimePickerFor(expression);
                case "Date":
                    return htmlHelper.BootstrapDatePickerFor(expression);
                case "Time":
                case "Duration":
                    return htmlHelper.BootstrapTimePickerFor(expression);
                case "PhoneNumber":
                    return htmlHelper.BootstrapTextBoxFor(expression);
                case "Currency":
                    return htmlHelper.BootstrapCurrencyBoxFor(expression);
                case "Text":
                    return htmlHelper.BootstrapTextBoxFor(expression);
                case "Html":
                    return htmlHelper.BootstrapHtmlAreaFor(expression);
                case "MultilineText":
                    return htmlHelper.BootstrapTextAreaFor(expression);
                case "EmailAddress":
                    return htmlHelper.BootstrapEmailBoxFor(expression);
                case "Password":
                    return htmlHelper.BootstrapPasswordBoxFor(expression);
                case "Url":
                case "ImageUrl":
                    return htmlHelper.BootstrapUrlBoxFor(expression);
                case "CreditCard":
                    return htmlHelper.BootstrapTextBoxFor(expression);
                case "PostalCode":
                    return htmlHelper.BootstrapPostcodeBoxFor(expression);
                case "Upload":
                    return htmlHelper.BootstrapFilePickerFor(expression);
            }
            return null;
        }

        private static IHtmlString EditorFromTypeCode<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var propertyType = typeof(TProperty);
            switch (Type.GetTypeCode(propertyType))
            {
                case TypeCode.Boolean:
                    return htmlHelper.BootstrapCheckBoxFor(expression);
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return htmlHelper.BootstrapDigitBoxFor(expression);
                case TypeCode.Double:
                case TypeCode.Single:
                    return htmlHelper.BootstrapNumberBoxFor(expression);
                case TypeCode.Decimal:
                    return htmlHelper.BootstrapCurrencyBoxFor(expression);
            }
            return null;
        }
    }
}
