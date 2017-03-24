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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BWakaBats.Extensions;

namespace BWakaBats.Bootstrap
{
    public abstract class FieldSet<TControl> : Element<TControl>, IContainer<TControl>
        where TControl : FieldSet<TControl>
    {
        private bool _begun;

        internal FieldSet(FieldSetContext context)
            : base(context, null)
        {
            Context = (FieldSetContext)base.Context;
        }

        public new FieldSetContext Context { get; private set; }

        #region Control Properties

        public TControl ExcludeValidationSummary(bool newValue = true)
        {
            Context.ExcludeValidationSummary = newValue;
            return (TControl)this;
        }

        #endregion

        protected override string TagType
        {
            get { return "fieldset"; }
        }

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeAttribute("role", "form");
            return base.UpdateTag(tag);
        }

        public TControl Begin()
        {
            if (_begun)
                throw new Exception("Already called Begin");

            var tag = CreateTag();

            var context = HtmlHelper.ViewContext;
            context.Writer.Write(tag.ToString(TagRenderMode.StartTag));
            string header = Context.Header;
            if (header != null)
            {
                context.Writer.Write("<legend>" + header + "</legend>");
            }
            if (!Context.ExcludeValidationSummary)
            {
                context.Writer.Write(GetSummary(HtmlHelper));
            }
            _begun = true;
            return (TControl)this;
        }

        public TControl End()
        {
            if (!_begun)
                throw new Exception("Cannot call End without Begin");

            var context = HtmlHelper.ViewContext;
            context.Writer.Write("</" + TagType + ">");
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

        private static string GetSummary(HtmlHelper htmlHelper)
        {
            var viewContext = htmlHelper.ViewContext;
            System.Web.Mvc.FormContext formContext = viewContext.ClientValidationEnabled ? viewContext.FormContext : null;

            if (htmlHelper.ViewData.ModelState.IsValid && formContext == null)
                return null;

            var modelState = GetModelStateList(htmlHelper);
            StringBuilder listBuilder = new StringBuilder();
            foreach (var state in modelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    string message = error.ErrorMessage;
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        listBuilder.Append("<li>");

                        string key = state.Key;
                        if (!string.IsNullOrWhiteSpace(key) && !message.Contains(key))
                        {
                            int lastDot = key.LastIndexOf('.');
                            if (lastDot != -1)
                            {
                                string suffix = key.Substring(lastDot + 1);
                                if (message.Contains(suffix))
                                {
                                    key = key.Substring(0, lastDot);
                                }
                            }
                            listBuilder.Append("<strong>");
                            listBuilder.Append(key.Replace('.', ' ').ToWords(true));
                            listBuilder.Append("</strong> ");
                        }

                        listBuilder.Append(message);
                        listBuilder.AppendLine("</li>");
                    }
                }
            }
            if (listBuilder.Length == 0)
            {
                if (viewContext.UnobtrusiveJavaScriptEnabled || formContext == null)
                    return null;

                listBuilder.AppendLine("<li style=\"display:none\"></li>");
            }

            var listTag = new HtmlTagBuilder("ul");
            listTag.InnerHtml = listBuilder.ToString();

            var divBuilder = new HtmlTagBuilder("div");
            divBuilder.AddCssClass(htmlHelper.ViewData.ModelState.IsValid ? HtmlHelper.ValidationSummaryValidCssClassName : HtmlHelper.ValidationSummaryCssClassName);
            divBuilder.AddCssClass("alert alert-danger fade in");
            if (formContext != null)
            {
                if (viewContext.UnobtrusiveJavaScriptEnabled)
                {
                    divBuilder.MergeAttribute("data-valmsg-summary", "true");
                }
                else
                {
                    divBuilder.GenerateId("validationSummary");
                    formContext.ValidationSummaryId = divBuilder.Attributes["id"];
                    formContext.ReplaceValidationSummary = true;
                }
            }
            divBuilder.InnerHtml = "<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>"
                                 + listTag.ToString();

            return divBuilder.ToString();
        }

        private static IEnumerable<KeyValuePair<string, ModelState>> GetModelStateList(HtmlHelper htmlHelper)
        {
            var modelState = htmlHelper.ViewData.ModelState;
            var modelMetadata = htmlHelper.ViewData.ModelMetadata;
            if (modelMetadata == null)
                return modelState;

            var metadataDictionary = modelMetadata.Properties.ToDictionary(p => p.PropertyName);
            return htmlHelper.ViewData.ModelState
                .Select(kv =>
                {
                    ModelMetadata metadata;
                    string key;
                    int order;
                    if (metadataDictionary.TryGetValue(kv.Key, out metadata))
                    {
                        key = metadata.DisplayName ?? kv.Key;
                        order = metadata.Order;
                    }
                    else
                    {
                        key = kv.Key;
                        order = 0x2710;
                    }
                    return new
                    {
                        Key = key,
                        Value = kv.Value,
                        Order = order,
                    };
                })
                .OrderBy(kv => kv.Order)
                .Select(kv => new KeyValuePair<string, ModelState>(kv.Key, kv.Value));
        }
    }

    public class FieldSetContext : ElementContext
    {
        public bool ExcludeValidationSummary { get; internal set; }
    }

    public sealed class FieldSet : FieldSet<FieldSet>
    {
        internal FieldSet() : base(new FieldSetContext()) { }
    }

    public static partial class HtmlHelperExtensions
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static FieldSet BootstrapFieldSet(this HtmlHelper htmlHelper, string header = null)
        {
            var control = new FieldSet();
            control.Initialize(htmlHelper);
            control.Header(header);
            return control;
        }
    }
}
