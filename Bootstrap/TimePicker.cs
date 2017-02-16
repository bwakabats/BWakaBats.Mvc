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
    public abstract class TimePicker<TControl> : RangeBase<TControl, TimeSpan?>
        where TControl : TimePicker<TControl>
    {
        protected TimePicker(RangeBaseContext<TimeSpan?> context, string name) : base(context, name, TimeSpan.MinValue, TimeSpan.MaxValue) { }

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeAttribute("type", "text");
            tag.MergeAttribute("data-type", "time");
            TimeSpan? value = Context.Value;
            if (value.HasValue)
            {
                if (string.IsNullOrEmpty(Context.Format))
                {
                    tag.MergeAttribute("value", value.Value.ToString());
                }
                else
                {
                    tag.MergeAttribute("value", value.Value.ToString(Context.Format, CultureInfo.InvariantCulture), true);
                }
            }
            return base.UpdateTag(tag);
        }

        protected override string DefaultDisplayFormat
        {
            get { return "hh':'mm"; }
        }
    }

    public sealed class TimePicker : TimePicker<TimePicker>
    {
        internal TimePicker(string name = null) : base(new RangeBaseContext<TimeSpan?>(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static TimePicker BootstrapTimePicker(this HtmlHelper htmlHelper, string name)
        {
            var control = new TimePicker(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static TimePicker BootstrapTimePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new TimePicker();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
