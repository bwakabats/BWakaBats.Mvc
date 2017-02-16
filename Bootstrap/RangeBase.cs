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
    public abstract class RangeBase<TControl, TValue> : PreAppend<TControl, TValue>
        where TControl : RangeBase<TControl, TValue>
    {
        protected RangeBase(RangeBaseContext<TValue> context, string name, TValue minValue, TValue maxValue)
            : base(context, name, false)
        {
            Context = context;
            Context.MinDefault = minValue;
            Context.MaxDefault = maxValue;
        }

        public new RangeBaseContext<TValue> Context { get; private set; }

        public override void Initialize<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            base.Initialize(htmlHelper, expression);
            Context.MinValue = ValidationAttribute("range-min", Context.MinDefault);
            Context.MaxValue = ValidationAttribute("range-max", Context.MaxDefault);
        }

        #region Control Properties

        public TControl MinValue(TValue newValue)
        {
            Context.MinValue = newValue;
            return (TControl)this;
        }

        public TControl MaxValue(TValue newValue)
        {
            Context.MaxValue = newValue;
            return (TControl)this;
        }

        public TControl Range(TValue minValue, TValue maxValue)
        {
            Context.MinValue = minValue;
            Context.MaxValue = maxValue;
            return (TControl)this;
        }

        public TControl Format(string newValue)
        {
            Context.Format = newValue;
            return (TControl)this;
        }

        public TControl SliderClass(string newValue)
        {
            Context.SliderClass = newValue;
            return (TControl)this;
        }

        #endregion

        protected override ValidatedTagBuilder CreateTag()
        {
            if (Context.Format == null)
            {
                if (Context.IsDisabled || Context.IsReadOnly)
                {
                    Context.Format = DefaultDisplayFormat;
                }
                else
                {
                    Context.Format = DefaultEditFormat;
                }
            }
            return base.CreateTag();
        }

        protected override bool UpdateTag(TagBuilder tag)
        {
            bool result;
            bool minSet = Context.MinValue != null && !Context.MinValue.Equals(Context.MinDefault);
            bool maxSet = Context.MaxValue != null && !Context.MaxValue.Equals(Context.MaxDefault);

            result = tag.MergeIfAttribute("data-val-range-min", Context.MinValue, minSet);
            result = tag.MergeIfAttribute("data-val-range-max", Context.MaxValue, maxSet) || result;
            if (Context.SliderClass != null)
            {
                tag.MergeNotNullAttribute(Context.SliderClass + "id", Context.Id + "Slider");
                tag.MergeNotNullAttribute(Context.SliderClass + "name", FullHtmlFieldName + "Slider");
                tag.MergeNotNullAttribute(Context.SliderClass + "value", Context.Value.ToString());
                tag.MergeIfAttribute(Context.SliderClass + "min", Context.MinValue, minSet);
                tag.MergeIfAttribute(Context.SliderClass + "max", Context.MaxValue, maxSet);
            }
            if (result)
            {
                if (minSet)
                {
                    if (maxSet)
                    {
                        tag.MergeAttribute("data-val-range", ValidationMessage("" + Context.MinValue + " to " + Context.MaxValue + "", "Please enter a value between " + Context.MinValue + " and " + Context.MaxValue + ""));
                    }
                    else
                    {
                        tag.MergeAttribute("data-val-range", ValidationMessage("" + Context.MinValue + " or less", "Please enter a value less than or equal to " + Context.MinValue + ""));
                    }
                }
                else
                {
                    tag.MergeAttribute("data-val-range", ValidationMessage("" + Context.MinValue + " or more", "Please enter a value greater than or equal to " + Context.MaxValue + ""));
                }
            }
            return base.UpdateTag(tag) || result;
        }

        protected override sealed string WrapTag(TagBuilder tag)
        {
            return base.WrapTag(tag);
        }

        protected override sealed string DefaultCssClass
        {
            get { return "form-control"; }
        }

        protected abstract string DefaultDisplayFormat { get; }

        protected virtual string DefaultEditFormat
        {
            get { return DefaultDisplayFormat; }
        }
    }

    public class RangeBaseContext<TValue> : PreAppendContext<TValue>
    {
        public TValue MinDefault { get; internal set; }
        public TValue MaxDefault { get; internal set; }
        public TValue MinValue { get; internal set; }
        public TValue MaxValue { get; internal set; }
        public string Format { get; internal set; }
        public string SliderClass { get; internal set; }
    }

}
