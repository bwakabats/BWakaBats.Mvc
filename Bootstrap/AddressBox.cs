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
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    [Flags]
    public enum AddressBoxParts
    {
        None = 0,
        AddressLine1 = 1,
        AddressLine2 = 2,
        AddressLine3 = 4,
        AddressLine4 = 8,
        City = 16,
        County = 32,
        Country = 64,
    }

    public abstract class AddressBox<TControl> : TextBase<AddressBox>
        where TControl : AddressBox<TControl>
    {
        protected AddressBox(AddressBoxContext context, string name)
            : base(context, name)
        {
            Context = context;
        }

        public new AddressBoxContext Context { get; private set; }

        #region Control Properties

        public TControl Part(AddressBoxParts newValue)
        {
            Context.Part = newValue;
            return (TControl)this;
        }

        #endregion

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeAttribute("type", "text");
            tag.MergeAttribute("data-address-part", Context.Part.ToString().ToLowerInvariant());
            return base.UpdateTag(tag);
        }
    }

    public class AddressBoxContext : TextBaseContext
    {
        public AddressBoxParts Part { get; internal set; }
    }

    public sealed class AddressBox : AddressBox<AddressBox>
    {
        internal AddressBox(string name = null) : base(new AddressBoxContext(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static AddressBox BootstrapAddressBox(this HtmlHelper htmlHelper, string name)
        {
            var control = new AddressBox(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static AddressBox BootstrapAddressBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new AddressBox();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
