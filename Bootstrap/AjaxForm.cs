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
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace BWakaBats.Bootstrap
{
    public abstract class AjaxForm<TControl> : Form<TControl>
        where TControl : AjaxForm<TControl>
    {
        protected AjaxForm(AjaxFormContext context)
            : base(context)
        {
            Context = (AjaxFormContext)base.Context;
        }

        public new AjaxFormContext Context { get; private set; }

        #region Control Properties

        public TControl AllowCache(bool newValue = true)
        {
            Context.AllowCache = newValue;
            return (TControl)this;
        }

        public TControl Confirm(string newValue)
        {
            Context.Confirm = newValue;
            return (TControl)this;
        }

        public TControl InsertionMode(InsertionMode newValue)
        {
            Context.InsertionMode = newValue;
            return (TControl)this;
        }

        public TControl LoadingElementDuration(int newValue)
        {
            Context.LoadingElementDuration = newValue;
            return (TControl)this;
        }

        public TControl LoadingElementId(string newValue)
        {
            Context.LoadingElementId = newValue;
            return (TControl)this;
        }

        public TControl OnBegin(string newValue)
        {
            Context.OnBegin = newValue;
            return (TControl)this;
        }

        public TControl OnComplete(string newValue)
        {
            Context.OnComplete = newValue;
            return (TControl)this;
        }

        public TControl OnFailure(string newValue)
        {
            Context.OnFailure = newValue;
            return (TControl)this;
        }

        public TControl OnSuccess(string newValue)
        {
            Context.OnSuccess = newValue;
            return (TControl)this;
        }

        public TControl UpdateTargetId(string newValue)
        {
            Context.UpdateTargetId = newValue;
            return (TControl)this;
        }

        #endregion

        protected override bool UpdateTag(TagBuilder tag)
        {
            var ajaxOptions = new AjaxOptions()
            {
                AllowCache = Context.AllowCache,
                Confirm = Context.Confirm,
                InsertionMode = Context.InsertionMode,
                LoadingElementDuration = Context.LoadingElementDuration,
                LoadingElementId = Context.LoadingElementId,
                OnBegin = Context.OnBegin,
                OnComplete = Context.OnComplete,
                OnFailure = Context.OnFailure,
                OnSuccess = Context.OnSuccess,
                UpdateTargetId = Context.UpdateTargetId,
                HttpMethod = Context.Method.ToString(),
            };
            tag.MergeAttributes(ajaxOptions.ToUnobtrusiveHtmlAttributes());
            return base.UpdateTag(tag);
        }
    }

    public class AjaxFormContext : FormContext
    {
        public bool AllowCache { get; internal set; }
        public string Confirm { get; internal set; }
        public InsertionMode InsertionMode { get; internal set; }
        public int LoadingElementDuration { get; internal set; }
        public string LoadingElementId { get; internal set; }
        public string OnBegin { get; internal set; }
        public string OnComplete { get; internal set; }
        public string OnFailure { get; internal set; }
        public string OnSuccess { get; internal set; }
        public string UpdateTargetId { get; internal set; }
    }

    public sealed class AjaxForm : AjaxForm<AjaxForm>
    {
        internal AjaxForm() : base(new AjaxFormContext()) { }
    }

    public static partial class HtmlHelperExtensions
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static AjaxForm BootstrapAjaxForm(this HtmlHelper htmlHelper)
        {
            var control = new AjaxForm();
            control.Initialize(htmlHelper);
            return control;
        }
    }
}
