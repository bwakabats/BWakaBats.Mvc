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
    public enum HtmlAreaToolBar
    {
        Full,
        Minimum,
    }

    public abstract class HtmlArea<TControl> : InputBox<TControl, string>
        where TControl : HtmlArea<TControl>
    {
        protected HtmlArea(HtmlAreaContext context, string name)
            : base(context, name, true)
        {
            Context = context;
            Context.Rows = 15;
        }

        public new HtmlAreaContext Context { get; private set; }

        #region Control Properties

        public TControl ToolBar(HtmlAreaToolBar newValue)
        {
            Context.ToolBar = newValue;
            return (TControl)this;
        }

        public TControl Rows(int newValue)
        {
            Context.Rows = newValue;
            return (TControl)this;
        }

        #endregion

        protected override string TagType
        {
            get { return "textarea"; }
        }

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.InnerHtml = Context.Value;
            tag.MergeIfAttribute("rows", Context.Rows, Context.Rows != 2);
            tag.MergeAttribute("data-html", Context.ToolBar.ToString());
            return base.UpdateTag(tag);
        }

        protected override string DefaultCssClass
        {
            get { return "form-control"; }
        }
    }

    public class HtmlAreaContext : InputBoxContext<string>
    {
        public HtmlAreaToolBar ToolBar { get; internal set; }
        public int Rows { get; internal set; }
    }

    public sealed class HtmlArea : HtmlArea<HtmlArea>
    {
        internal HtmlArea(string name = null) : base(new HtmlAreaContext(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static HtmlArea BootstrapHtmlArea(this HtmlHelper htmlHelper, string name)
        {
            var control = new HtmlArea(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static HtmlArea BootstrapHtmlAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new HtmlArea();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
