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
using System.Linq.Expressions;
using System.Web.Mvc;
using BWakaBats.Extensions;

namespace BWakaBats.Bootstrap
{
    public abstract class ListBox<TControl, TValue> : SelectBox<TControl, TValue>
        where TControl : ListBox<TControl, TValue>
    {
        private static int _uniqueId;

        protected ListBox(ListBoxContext<TValue> context, string name)
            : base(context, name)
        {
            Context = context;
        }

        public new ListBoxContext<TValue> Context { get; private set; }

        #region Control Properties

        public TControl Rows(int newValue)
        {
            Context.Rows = newValue;
            return (TControl)this;
        }

        public TControl AllowMultiple(bool newValue = true)
        {
            Context.AllowMultiple = newValue;
            return (TControl)this;
        }

        public TControl MobileCompatible(bool newValue = true)
        {
            Context.MobileCompatible = newValue;
            return (TControl)this;
        }

        #endregion

        public override string ToHtmlString()
        {
            if (!Context.MobileCompatible)
                return base.ToHtmlString();

            if (string.IsNullOrWhiteSpace(Context.Name))
            {
                Context.Id = "bootstrap_listbox_id_" + _uniqueId++;
            }

            string html = base.ToHtmlString();

            int start = html.IndexOf("<select", StringComparison.Ordinal);
            int end = html.IndexOf("</select>", start, StringComparison.Ordinal);
            string prefix = html.Substring(0, start);
            string suffix = html.Substring(end + 9);
            string original = html.Substring(start, end - start + 9);
            string newDiv = original;

            string id = Context.Id;
            string onclick = "onclick='javascript: var s=\"selected\",t=$(this),p=t.parent(),h=$(\"#" + id + "\"),v";
            if (Context.AllowMultiple)
            {
                onclick += ";t.attr(s,function(index, attr){return attr == s ? null : s;});v=p.children(\"[\"+s+\"]\").map(function(){ return $(this).attr(\"value\"); }).get()";
            }
            else
            {
                onclick += "=t.attr(\"value\");p.children().removeAttr(s);t.attr(s,s)";
            }
            onclick += ";h.val(v)'";


            string height = "height:" + (Context.Rows * 1.6666) + "em;";
            int styleIndex = newDiv.IndexOf("style=\"", StringComparison.Ordinal);
            if (styleIndex == -1)
            {
                newDiv = newDiv.Replace("<select", "<div style='max-" + height + "min-" + height + height + "'");
            }
            else
            {
                newDiv = newDiv.Substring(0, styleIndex + 7)
                       + "max-" + height + "min-" + height + height
                       + newDiv.Substring(styleIndex + 7);
                newDiv = newDiv.Replace("<select", "<div");
            }
            newDiv = newDiv.Replace("</select", "</div");
            newDiv = newDiv.Replace("data-val=\"true\"", "");
            newDiv = newDiv.Replace("hidden", "listbox");
            newDiv = newDiv.Replace("id=\"" + id + "\"", "id=\"" + Context.Id + "_visible\"");
            if (!string.IsNullOrWhiteSpace(FullHtmlFieldName))
            {
                newDiv = newDiv.Replace("name=\"" + FullHtmlFieldName + "\"", "");
            }
            newDiv = newDiv.Replace("<option>", "<div class='option' " + onclick + ">");
            newDiv = newDiv.Replace("</option>", "</div>");
            newDiv = newDiv.Replace("<option ", "<div class='option' " + onclick + " ");

            return prefix + original + newDiv + suffix;
        }

        protected override bool UpdateTag(TagBuilder tag)
        {
            if (Context.MobileCompatible)
            {
                tag.AddCssClass("hidden");
            }
            else
            {
                tag.MergeIfAttribute("size", Context.Rows, Context.Rows > 1);
            }
            return base.UpdateTag(tag);
        }

        protected override bool AllowMultipleOverride
        {
            get { return Context.AllowMultiple; }
        }
    }

    public class ListBoxContext<TValue> : SelectBoxContext<TValue>
    {
        public int Rows { get; internal set; } = 4;
        public bool AllowMultiple { get; internal set; }
        public bool MobileCompatible { get; internal set; }
    }

    public sealed class ListBox<TValue> : ListBox<ListBox<TValue>, TValue>
    {
        internal ListBox(string name = null) : base(new ListBoxContext<TValue>(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static ListBox<object> BootstrapListBox(this HtmlHelper htmlHelper, string name)
        {
            var control = new ListBox<object>(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static ListBox<TProperty> BootstrapListBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new ListBox<TProperty>();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
