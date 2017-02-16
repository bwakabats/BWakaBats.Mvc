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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public enum TextBoxCapital
    {
        None,
        Upper,
        Lower,
        Mixed,
    }

    public abstract class TextBox<TControl> : TextBase<TextBox>
        where TControl : TextBox<TControl>
    {
        protected TextBox(TextBoxContext context, string name)
            : base(context, name)
        {
            Context = context;
        }

        public new TextBoxContext Context { get; private set; }

        public override void Initialize<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            base.Initialize(htmlHelper, expression);
            Context.RegularExpression = ValidationAttribute("regex-pattern", (string)null);
        }

        #region Control Properties

        public TControl Capital(TextBoxCapital newValue)
        {
            Context.Capital = newValue;
            return (TControl)this;
        }

        public TControl RegularExpression(string newValue)
        {
            Context.RegularExpression = newValue;
            return (TControl)this;
        }

        public TControl Items(IEnumerable<string> newValue)
        {
            Context.Items = newValue;
            return (TControl)this;
        }

        public TControl Items(IEnumerable newValue)
        {
            List<string> Items;
            if (newValue == null)
            {
                Items = null;
            }
            else
            {
                Items = new List<string>();
                foreach (var item in newValue)
                {
                    Items.Add(item.ToString());
                }
            }
            Context.Items = Items;
            return (TControl)this;
        }

        #endregion

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeAttribute("type", "text");
            if (Context.Capital != TextBoxCapital.None)
            {
                tag.AddCssClass("text-" + Context.Capital.ToString().ToLowerInvariant());
            }
            if (Context.Items != null)
            {
                string unusedDelimiter = FindDelimiter();
                tag.MergeAttribute("data-typeahead-delimiter", unusedDelimiter);
                tag.MergeAttribute("data-typeahead", string.Join(unusedDelimiter, Context.Items));
            }

            bool result = tag.MergeNotNullAttribute("data-val-regex-pattern", Context.RegularExpression);
            tag.MergeIfAttribute("data-val-regex", ValidationMessage("Invalid", "Field in wrong format"), result);

            return base.UpdateTag(tag) || result;
        }

        private string FindDelimiter()
        {
            string unusedDelimiter = FindDelimiter("", "");
            if (unusedDelimiter != null)
                return unusedDelimiter;

            int length = 1;
            while (true)
            {
                string padding = new string(' ', length);

                unusedDelimiter = FindDelimiter("", padding);
                if (unusedDelimiter != null)
                    return unusedDelimiter;

                unusedDelimiter = FindDelimiter(padding, "");
                if (unusedDelimiter != null)
                    return unusedDelimiter;

                unusedDelimiter = FindDelimiter(padding, padding);
                if (unusedDelimiter != null)
                    return unusedDelimiter;

                length++;

            }
        }

        private string FindDelimiter(string suffix, string prefix)
        {
            const string delimiters = " ,;:/!^*~&%#£$@?(){}[]<>0123456789abcdefghijklmnopqrstuvwxyzABCDEFHIJKLMNOPQRSTUVWXYZ";

            foreach (char oneDelimiter in delimiters)
            {
                bool found = false;
                string delimiter = suffix + oneDelimiter + prefix;
                foreach (string item in Context.Items)
                {
                    if (item.IndexOf(delimiter, StringComparison.Ordinal) > -1)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    return delimiter;
            }
            return null;
        }
    }

    public class TextBoxContext : TextBaseContext
    {
        public TextBoxCapital Capital { get; internal set; }
        public string RegularExpression { get; internal set; }
        public IEnumerable<string> Items { get; internal set; }
    }

    public sealed class TextBox : TextBox<TextBox>
    {
        internal TextBox(string name = null) : base(new TextBoxContext(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static TextBox BootstrapTextBox(this HtmlHelper htmlHelper, string name)
        {
            var control = new TextBox(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static TextBox BootstrapTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new TextBox();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
