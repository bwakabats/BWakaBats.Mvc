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
    public abstract class CurrencyBox<TControl> : RangeBase<CurrencyBox, decimal?>
        where TControl : CurrencyBox<TControl>
    {
        protected CurrencyBox(CurrencyBoxContext context, string name)
            : base(context, name, int.MinValue, int.MaxValue)
        {
            Context = (CurrencyBoxContext)base.Context;
            Prepend((AwesomeIcon)context.Symbol);
        }

        public new CurrencyBoxContext Context { get; private set; }

        #region Control Properties

        public TControl Symbol(string newValue)
        {
            Context.Symbol = newValue;
            Context.Prepend.Clear();
            Context.Prepend.Add(new PreAppenderString(newValue));
            return (TControl)this;
        }

        public TControl Symbol(AwesomeIcon newValue)
        {
            Context.Symbol = newValue;
            Context.Prepend.Clear();
            Context.Prepend.Add(newValue);
            return (TControl)this;
        }

        #endregion

        protected override bool UpdateTag(TagBuilder tag)
        {
            decimal? value = Context.Value;
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
            get { return "#,0.00"; }
        }

        protected override string DefaultEditFormat
        {
            get { return "#0.00"; }
        }
    }

    public class CurrencyBoxContext : RangeBaseContext<decimal?>
    {
        public object Symbol { get; internal set; } = AwesomeIcon.GBP;
    }

    public sealed class CurrencyBox : CurrencyBox<CurrencyBox>
    {
        internal CurrencyBox(string name = null) : base(new CurrencyBoxContext(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static CurrencyBox BootstrapCurrencyBox(this HtmlHelper htmlHelper, string name)
        {
            var control = new CurrencyBox(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static CurrencyBox BootstrapCurrencyBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new CurrencyBox();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
