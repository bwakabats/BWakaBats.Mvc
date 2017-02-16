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
using System.Web.Routing;
using BWakaBats.Extensions;

namespace BWakaBats.Bootstrap
{
    public abstract class Form<TControl> : Element<TControl>, IContainer<TControl>
        where TControl : Form<TControl>
    {
        private bool _begun;

        protected Form(FormContext context)
            : base(context, null)
        {
            Context = context;
        }

        public new FormContext Context { get; private set; }

        #region Control Properties

        public TControl ActionName(string newValue)
        {
            Context.ActionName = newValue;
            return (TControl)this;
        }

        public TControl ControllerName(string newValue)
        {
            Context.ControllerName = newValue;
            return (TControl)this;
        }

        public TControl RouteValues(object newValue)
        {
            Context.RouteValues = newValue;
            return (TControl)this;
        }

        public TControl Method(FormMethod newValue)
        {
            Context.Method = newValue;
            return (TControl)this;
        }

        public TControl IsMultipart(bool newValue = true)
        {
            Context.IsMultipart = newValue;
            return (TControl)this;
        }

        public TControl ExcludeAntiForgeryToken(bool newValue = true)
        {
            Context.ExcludeAntiForgeryToken = newValue;
            return (TControl)this;
        }

        #endregion

        public TControl Begin()
        {
            if (_begun)
                throw new Exception("Already called Begin");

            var context = HtmlHelper.ViewContext;
            context.Writer.Write(CreateTag().ToString(TagRenderMode.StartTag));
            if (!Context.ExcludeAntiForgeryToken)
            {
                context.Writer.Write(HtmlHelper.AntiForgeryToken());
            }
            context.FormContext = new System.Web.Mvc.FormContext();
            _begun = true;
            return (TControl)this;
        }

        public TControl End()
        {
            if (!_begun)
                throw new Exception("Cannot call End without Begin");

            var context = HtmlHelper.ViewContext;
            context.Writer.Write("</" + TagType + ">");
            context.OutputClientValidation();
            context.FormContext = null;
            _begun = false;
            return (TControl)this;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _begun)
            {
                End();
            }
        }

        protected override string TagType
        {
            get { return "form"; }
        }

        protected override bool UpdateTag(TagBuilder tag)
        {
            string action;
            if (string.IsNullOrWhiteSpace(Context.ActionName) && string.IsNullOrWhiteSpace(Context.ControllerName) && Context.RouteValues == null)
            {
                action = HtmlHelper.ViewContext.HttpContext.Request.RawUrl;
            }
            else
            {
                action = UrlHelper.GenerateUrl(null, Context.ActionName, Context.ControllerName, new RouteValueDictionary(Context.RouteValues), HtmlHelper.RouteCollection, HtmlHelper.ViewContext.RequestContext, true);
            }
            tag.MergeNotNullAttribute("action", action);
            tag.MergeNotNullAttribute("method", Context.Method);
            tag.MergeIfAttribute("enctype", "multipart/form-data", Context.IsMultipart);
            return base.UpdateTag(tag);
        }
    }

    public class FormContext : ElementContext
    {
        public string ActionName { get; internal set; }
        public string ControllerName { get; internal set; }
        public object RouteValues { get; internal set; }
        public FormMethod Method { get; internal set; } = FormMethod.Post;
        public bool IsMultipart { get; internal set; }
        public bool ExcludeAntiForgeryToken { get; internal set; }
    }
    public sealed class Form : Form<Form>
    {
        internal Form() : base(new FormContext()) { }
    }

    public static partial class HtmlHelperExtensions
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static Form BootstrapForm(this HtmlHelper htmlHelper)
        {
            var control = new Form();
            control.Initialize(htmlHelper);
            return control;
        }
    }

}
