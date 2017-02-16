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
    public abstract class TextArea<TControl> : InputBox<TControl, string>
        where TControl : TextArea<TControl>
    {
        protected TextArea(TextAreaContext context, string name)
            : base(context, name, true)
        {
            Context = context;
        }

        public new TextAreaContext Context { get; private set; }

        public override void Initialize<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            base.Initialize(htmlHelper, expression);
            Context.MinLength = ValidationAttribute("length-min", 0);
            Context.MaxLength = ValidationAttribute("length-max", int.MaxValue);
        }

        #region Control Properties

        public TControl MinLength(double newValue)
        {
            Context.MinLength = newValue;
            return (TControl)this;
        }

        public TControl MaxLength(double newValue)
        {
            Context.MaxLength = newValue;
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
            tag.MergeIfAttribute("maxlength", Context.MaxLength, Context.MaxLength != int.MaxValue);

            bool result;
            result = tag.MergeIfAttribute("data-val-length-min", Context.MinLength, Context.MinLength != 0);
            result = tag.MergeIfAttribute("data-val-length-max", Context.MaxLength, Context.MaxLength != int.MaxValue) || result;
            if (result)
            {
                if (Context.MinLength != 0)
                {
                    if (Context.MaxLength != int.MaxValue)
                    {
                        tag.MergeAttribute("data-val-length", ValidationMessage("" + Context.MinLength + " to " + Context.MaxLength + " characters", "Please enter a value between " + Context.MinLength + " and " + Context.MaxLength + " characters long"));

                    }
                    else
                    {
                        tag.MergeAttribute("data-val-length", ValidationMessage("At least " + Context.MinLength + " characters", "Please enter at least " + Context.MinLength + " characters"));
                    }
                }
                else
                {
                    tag.MergeAttribute("data-val-length", ValidationMessage("Maximum " + Context.MaxLength + " characters", "Please enter no more than " + Context.MaxLength + " characters"));
                }
            }

            tag.MergeIfAttribute("rows", Context.Rows, Context.Rows != 2);
            return base.UpdateTag(tag) || result;
        }

        protected override string DefaultCssClass
        {
            get { return "form-control"; }
        }
    }

    public class TextAreaContext : InputBoxContext<string>
    {
        public double MinLength { get; internal set; }
        public double MaxLength { get; internal set; } = int.MaxValue;
        public int Rows { get; internal set; } = 5;
    }

    public sealed class TextArea : TextArea<TextArea>
    {
        internal TextArea(string name = null) : base(new TextAreaContext(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static TextArea BootstrapTextArea(this HtmlHelper htmlHelper, string name)
        {
            var control = new TextArea(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static TextArea BootstrapTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new TextArea();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
