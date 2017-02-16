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
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public abstract class NumberBox<TControl> : RangeBase<NumberBox, double?>
        where TControl : NumberBox<TControl>
    {
        protected NumberBox(RangeBaseContext<double?> context, string name) : base(context, name, double.MinValue, double.MaxValue) { }

        protected override bool UpdateTag(TagBuilder tag)
        {
            double? value = Context.Value;
            if (value.HasValue)
            {
                if (string.IsNullOrEmpty(Context.Format))
                {
                    tag.MergeAttribute("value", value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    tag.MergeAttribute("value", value.Value.ToString(Context.Format, CultureInfo.InvariantCulture), true);
                }
            }
            tag.MergeAttribute("type", "text");
            tag.MergeAttribute("data-val-number", ValidationMessage("Invalid number", "Please enter a valid number"));
            return base.UpdateTag(tag) || true;
        }

        protected override string DefaultDisplayFormat
        {
            get { return null; }
        }
    }

    public sealed class NumberBox : NumberBox<NumberBox>
    {
        internal NumberBox(string name = null) : base(new RangeBaseContext<double?>(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static NumberBox BootstrapNumberBox(this HtmlHelper htmlHelper, string name)
        {
            var control = new NumberBox(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static NumberBox BootstrapNumberBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new NumberBox();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
