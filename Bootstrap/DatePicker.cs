﻿// *****************************************************
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
    public abstract class DatePicker<TControl> : RangeBase<TControl, DateTime?>
        where TControl : DatePicker<TControl>
    {
        protected DatePicker(RangeBaseContext<DateTime?> context, string name) : base(context, name, DateTime.MinValue, DateTime.MaxValue) { }

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeAttribute("type", "text");
            tag.MergeAttribute("data-type", "date");
            DateTime? value = Context.Value;
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
            return base.UpdateTag(tag);
        }

        protected override string DefaultDisplayFormat
        {
            get { return "dd/MM/yyyy"; }
        }
    }

    public sealed class DatePicker : DatePicker<DatePicker>
    {
        internal DatePicker(string name = null) : base(new RangeBaseContext<DateTime?>(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static DatePicker BootstrapDatePicker(this HtmlHelper htmlHelper, string name)
        {
            var control = new DatePicker(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static DatePicker BootstrapDatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new DatePicker();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
