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
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public abstract class PreAppend<TControl, TValue> : InputBox<TControl, TValue>
        where TControl : PreAppend<TControl, TValue>
    {
        protected PreAppend(PreAppendContext<TValue> context, string name, bool isWide)
            : base(context, name, isWide)
        {
            Context = context;
        }

        public new PreAppendContext<TValue> Context { get; private set; }

        public override void Initialize<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            base.Initialize<TModel, TProperty>(htmlHelper, expression);
            FindHtmlAttribute("prepend", v => Prepend(v));
            FindHtmlAttribute("append", v => Append(v));
        }

        #region Control Properties

        public TControl Prepend(string newValue)
        {
            Context.Prepend.Add(new PreAppenderString(newValue));
            return (TControl)this;
        }

        public TControl Prepend(IPreAppender newValue)
        {
            Context.Prepend.Add(newValue);
            return (TControl)this;
        }

        public TControl Prepend(IEnumerable<IPreAppender> newValues)
        {
            Context.Prepend.Add(newValues);
            return (TControl)this;
        }

        public TControl Append(string newValue)
        {
            Context.Append.Add(new PreAppenderString(newValue));
            return (TControl)this;
        }

        public TControl Append(IPreAppender newValue)
        {
            Context.Append.Add(newValue);
            return (TControl)this;
        }

        public TControl Append(IEnumerable<IPreAppender> newValues)
        {
            Context.Append.Add(newValues);
            return (TControl)this;
        }

        #endregion

        protected override sealed string TagType
        {
            get { return base.TagType; }
        }

        protected override string DefaultPlaceholder(string name)
        {
            if (!Context.IsWide && (Context.Prepend.Count + Context.Append.Count > 0))
                return Context.IsRequired ? "Required" : "Please enter";

            return base.DefaultPlaceholder(name);
        }

        protected override string WrapTag(TagBuilder tag)
        {
            if (Context.Prepend.Count + Context.Append.Count == 0)
                return base.WrapTag(tag);

            string wrapGroupStyle = WrapGroupStyle;
            if (wrapGroupStyle != null)
            {
                wrapGroupStyle = " style='" + wrapGroupStyle + "'";
            }
            string appended = Context.Append.Count == 0 ? "" : " appended";
            var output = new StringBuilder("<div class='input-group" + appended + "'" + wrapGroupStyle + ">");
            output.Append(GetPreAppendString(Context.Prepend));
            output.Append(base.WrapBaseTag(tag));
            output.Append(GetPreAppendString(Context.Append));
            output.Append("</div>");
            return base.WrapTagInternal(output.ToString());
        }

        protected virtual string WrapGroupStyle
        {
            get { return null; }
        }

        protected static StringBuilder GetPreAppendString(IEnumerable<IPreAppender> preAppenders)
        {
            var output = new StringBuilder();
            foreach (var preAppender in preAppenders)
            {
                output.Append(preAppender.ToPreAppenderString());
            }
            return output;
        }
    }

    public class PreAppendContext<TValue> : InputBoxContext<TValue>
    {
        public ICollection<IPreAppender> Prepend { get; internal set; } = new List<IPreAppender>();
        public ICollection<IPreAppender> Append { get; internal set; } = new List<IPreAppender>();
    }

}
