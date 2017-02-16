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

namespace BWakaBats.Bootstrap
{
    //Unobtrusive Validation fails from data-val-creditcard


    //public abstract class CreditCardBox<TControl> : TextBase<TControl>
    //    where TControl : CreditCardBox<TControl>
    //{
    //    protected CreditCardBox(TextBaseContext context, string name) : base(context, name) { }

    //    protected override bool UpdateTag(TagBuilder tag)
    //    {
    //        tag.MergeAttribute("type", "text");
    //        tag.MergeAttribute("data-val-creditcard", ValidationMessage("Invalid number", "Please enter a valid credit card"));
    //        tag.AddCssClass("text-lower");
    //        return base.UpdateTag(tag) || true;
    //    }
    //}

    //public sealed class CreditCardBox : CreditCardBox<CreditCardBox>
    //{
    //    internal CreditCardBox(string name = null) : base(new TextBaseContext(), name) { }
    //}

    //public static partial class HtmlHelperExtensions
    //{
    //    public static CreditCardBox BootstrapCreditCardBox(this HtmlHelper htmlHelper, string name)
    //    {
    //        var control = new CreditCardBox(name);
    //        control.Initialize(htmlHelper);
    //        return control;
    //    }

    //    public static CreditCardBox BootstrapCreditCardBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
    //    {
    //        var control = new CreditCardBox();
    //        control.Initialize(htmlHelper, expression);
    //        return control;
    //    }
    //}
}
