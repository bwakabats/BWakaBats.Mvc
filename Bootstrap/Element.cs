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
using BWakaBats.Attributes;
using BWakaBats.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public abstract class Element<TControl>
        where TControl : Element<TControl>
    {
        private UrlHelper _urlHelper;

        protected HtmlHelper HtmlHelper { get; private set; }

        protected UrlHelper UrlHelper
        {
            get
            {
                if (_urlHelper == null)
                {
                    _urlHelper = new UrlHelper(HtmlHelper.ViewContext.RequestContext);
                }
                return _urlHelper;
            }
        }

        protected string FullHtmlFieldName { get; private set; }

        protected Element(ElementContext context, string name)
        {
            Context = context;
            Context.Name = name;
        }

        public ElementContext Context { get; private set; }

        public virtual void Initialize(HtmlHelper htmlHelper)
        {
            HtmlHelper = htmlHelper;
        }

        public virtual void Initialize<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            Initialize(htmlHelper);

            if (expression == null)
                return;

            string name = ExpressionHelper.GetExpressionText(expression);
            Name(name);

            Context.Metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (!string.IsNullOrWhiteSpace(Context.Metadata.ShortDisplayName) && Context.Metadata.ShortDisplayName != Context.Metadata.DisplayName)
            {
                Header(Context.Metadata.ShortDisplayName);
            }
            else if (!string.IsNullOrWhiteSpace(Context.Metadata.DisplayName))
            {
                Header(Context.Metadata.DisplayName);
            }
            else
            {
                Header(name);
            }

            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                var attributes = (HtmlAttributeAttribute[])memberExpression.Member.GetCustomAttributes(typeof(HtmlAttributeAttribute), true);
                var dictionary = attributes.ToDictionary(v => v.Key, v => (object)v.Value);
                HtmlAttributes(dictionary);
                FindHtmlAttribute("title", v => Tooltip(v));
            }

            //var additionalValues = Context.Metadata.AdditionalValues;
            //if (additionalValues != null)
            //{
            //    var dictionary = additionalValues.ToDictionary(v => v.Key, v => (object)((HtmlAttributeAttribute)v.Value).Value);
            //    HtmlAttributes(dictionary);
            //    FindHtmlAttribute("title", v => Tooltip(v));
            //}
        }

        protected void FindHtmlAttribute(string key, Action<string> callCustomProperty)
        {
            var dictionary = Context.HtmlAttributes as IDictionary<string, object>;
            if (dictionary == null)
                return;

            object value;
            if (!dictionary.TryGetValue(key, out value))
                return;

            dictionary.Remove(key);
            callCustomProperty(value.ToString());
        }

        #region Control Properties

        public TControl Id(string newValue)
        {
            Context.Id = TagBuilder.CreateSanitizedId(newValue);
            if (newValue != null && Context.Name == null)
                return Name(newValue);

            return (TControl)this;
        }

        public TControl Name(string newValue)
        {
            Context.Name = newValue;
            if (newValue != null && Context.Id == null)
                return Id(newValue);

            return (TControl)this;
        }

        public TControl Header(string newValue)
        {
            Context.Header = newValue;
            return (TControl)this;
        }

        public TControl Tooltip(string newValue)
        {
            Context.Tooltip = newValue;
            return (TControl)this;
        }

        public TControl HtmlAttributes(object newValue)
        {
            Context.HtmlAttributes = newValue;
            return (TControl)this;
        }

        public TControl HtmlAttributes(IDictionary<string, object> newValue)
        {
            Context.HtmlAttributes = newValue;
            return (TControl)this;
        }

        public TControl CssClass(string newValue)
        {
            Context.CssClass = newValue;
            return (TControl)this;
        }

        #endregion

        protected virtual ValidatedTagBuilder CreateTag()
        {
            var tag = new ValidatedTagBuilder(TagType);
            if (UpdateTag(tag))
            {
                tag.MergeAttribute("data-val", "true");
                tag.IsValidated = true;
            }

            if (!string.IsNullOrWhiteSpace(Context.Name))
            {
                FullHtmlFieldName = HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(Context.Name);
                tag.MergeNotNullAttribute("name", FullHtmlFieldName);
                if (string.IsNullOrWhiteSpace(Context.Id))
                {
                    Id(TagBuilder.CreateSanitizedId(FullHtmlFieldName));
                }
            }
            else
            {
                FullHtmlFieldName = null;
            }
            string id = Context.Id;

            if (!string.IsNullOrWhiteSpace(id))
            {
                tag.MergeNotNullAttribute("id", id);
            }
            tag.MergeNotNullAttribute("title", Context.Tooltip);
            tag.MergeAttributes(Context.HtmlAttributes);

            string cssClass = Context.CssClass ?? DefaultCssClass;
            if (!string.IsNullOrWhiteSpace(cssClass))
            {
                tag.AddCssClass(cssClass);
            }

            return tag;
        }

        protected virtual bool UpdateTag(TagBuilder tag)
        {
            return false;
        }

        protected abstract string TagType { get; }

        protected virtual string DefaultCssClass
        {
            get { return ""; }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        protected static TResult ConvertProperty<TResult, TInput>(object model)
        {
            if (model == null)
                return default(TResult);

            var inputType = typeof(TInput);
            var resultType = typeof(TResult);

            var nullablePropertyType = Nullable.GetUnderlyingType(inputType);
            if (nullablePropertyType != null)
            {
                model = inputType.GetProperty("Value").GetValue(model);
                inputType = nullablePropertyType;
            }

            if (inputType == resultType)
                return (TResult)model;

            if (resultType == typeof(string))
            {
                // I'll shoehorn the fecker in...
                if (inputType == typeof(Guid))
                {
                    return (TResult)((object)(((Guid)model).ToString("N")));
                }
                return (TResult)((object)(model.ToString()));
            }

            var nullableValueType = Nullable.GetUnderlyingType(resultType);
            if (nullableValueType != null)
            {
                model = System.Convert.ChangeType(model, nullableValueType, CultureInfo.InvariantCulture);
            }

            if (resultType == typeof(bool))
            {
                if (inputType == typeof(string))
                {
                    return (TResult)((object)(!string.IsNullOrWhiteSpace((string)model)));
                }
                return (TResult)((object)((int)model != 0));
            }

            return (TResult)model;
        }
    }

    public abstract class ElementContext
    {
        protected ElementContext() { }

        public ModelMetadata Metadata { get; internal set; }
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string Header { get; internal set; }
        public string Tooltip { get; internal set; }
        public string CssClass { get; internal set; }
        public object HtmlAttributes { get; internal set; }
    }
}
