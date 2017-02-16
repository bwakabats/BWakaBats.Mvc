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
    public abstract class DigitBox<TControl> : RangeBase<TControl, int?>
        where TControl : DigitBox<TControl>
    {
        protected DigitBox(RangeBaseContext<int?> context, string name) : base(context, name, int.MinValue, int.MaxValue) { }

        protected override bool UpdateTag(TagBuilder tag)
        {
            int? value = Context.Value;
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
            tag.MergeAttribute("data-val-digits", ValidationMessage("Invalid number", "Please enter a valid whole number"));
            if (Context.SliderClass != null)
            {
                tag.MergeAttribute(Context.SliderClass + "step", "1");
            }
            return base.UpdateTag(tag) || true;
        }

        protected override string DefaultDisplayFormat
        {
            get { return null; }
        }
    }

    public sealed class DigitBox : DigitBox<DigitBox>
    {
        internal DigitBox(string name = null) : base(new RangeBaseContext<int?>(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static DigitBox BootstrapDigitBox(this HtmlHelper htmlHelper, string name)
        {
            var control = new DigitBox(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static DigitBox BootstrapDigitBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new DigitBox();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
