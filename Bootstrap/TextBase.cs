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
    public abstract class TextBase<TControl> : PreAppend<TControl, string>
        where TControl : TextBase<TControl>
    {
        protected TextBase(TextBaseContext context, string name)
            : base(context, name, true)
        {
            Context = context;
        }

        public new TextBaseContext Context { get; private set; }

        public override void Initialize<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            base.Initialize(htmlHelper, expression);
            Context.MinLength = ValidationAttribute("length-min", 0);
            Context.MaxLength = ValidationAttribute("length-max", int.MaxValue);
            Context.MustMatch = ValidationAttribute("equalto-other", (string)null);
        }

        #region Control Properties

        public TControl MinLength(int newValue)
        {
            Context.MinLength = newValue;
            return (TControl)this;
        }

        public TControl MaxLength(int newValue)
        {
            Context.MaxLength = newValue;
            return (TControl)this;
        }

        public TControl MustMatch(string newValue)
        {
            Context.MustMatch = newValue;
            return (TControl)this;
        }

        #endregion

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeNotNullAttribute("value", Context.Value);
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

            if (!string.IsNullOrWhiteSpace(Context.MustMatch))
            {
                string name;
                if (Context.MustMatch.Substring(0, 2) == "*.")
                {
                    name = Context.MustMatch.Substring(2);
                }
                else
                {
                    name = Context.MustMatch;
                }
                var metadata = ModelMetadata.FromStringExpression(name, HtmlHelper.ViewData);
                if (!string.IsNullOrWhiteSpace(metadata.ShortDisplayName) && metadata.ShortDisplayName != metadata.DisplayName)
                {
                    name = metadata.ShortDisplayName;
                }
                else if (!string.IsNullOrWhiteSpace(metadata.DisplayName))
                {
                    name = metadata.DisplayName;
                }
                tag.MergeAttribute("data-val-equalto", ValidationMessage("Must be same", "Please enter the same value as the " + name.ToLowerInvariant() + ""));
                result = tag.MergeNotNullAttribute("data-val-equalto-other", Context.MustMatch);
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
    }

    public class TextBaseContext : PreAppendContext<string>
    {
        public int MinLength { get; internal set; }
        public int MaxLength { get; internal set; } = int.MaxValue;
        public string MustMatch { get; internal set; }
    }

}
