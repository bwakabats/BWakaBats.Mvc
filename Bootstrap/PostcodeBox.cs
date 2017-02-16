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
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace BWakaBats.Bootstrap
{
    public abstract class PostcodeBox<TControl> : TextBase<PostcodeBox>
        where TControl : PostcodeBox<TControl>
    {
        protected PostcodeBox(PostcodeBoxContext context, string name)
            : base(context, name)
        {
            Context = context;
        }

        public new PostcodeBoxContext Context { get; private set; }

        #region Control Properties

        public TControl LookupUrl(string newValue)
        {
            Context.LookupUrl = newValue;
            return (TControl)this;
        }

        public TControl ConversionFunction(string newValue)
        {
            Context.ConversionFunction = newValue;
            return (TControl)this;
        }


        public TControl ExternalUrl(string newValue)
        {
            /*
             e.g.
                Open Street Map:    https://www.openstreetmap.org/#map=19/{lat}/{lng}
                Bing:               https://www.bing.com/maps?cp={lat}~{lng}&lvl=19
                Google:             https://www.google.co.uk/maps/@{lat},{lng},19z
                OS:                 https://osmaps.ordnancesurvey.co.uk/osmaps/{lat},{lng},13
             */
            Context.ExternalUrl = newValue;
            return (TControl)this;
        }

        public TControl ButtonIcon(AwesomeIcon newValue)
        {
            Context.ButtonIcon = newValue;
            return (TControl)this;
        }

        public TControl ButtonStyle(ButtonStyle newValue)
        {
            Context.ButtonStyle = newValue;
            return (TControl)this;
        }

        public TControl HideButton(bool newValue = true)
        {
            Context.HideButton = newValue;
            return (TControl)this;
        }

        #endregion

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        protected override bool UpdateTag(TagBuilder tag)
        {
            if (!Context.HideButton)
            {
                Context.Append.Add(HtmlHelper.BootstrapButton(Context.ButtonIcon).Behavior(ButtonBehavior.Button).Style(Context.ButtonStyle).Id(Context.Id + "_Button").HtmlAttributes(new { data_address_part = "url" }));
            }

            tag.MergeAttribute("type", "text");
            tag.AddCssClass("text-upper");
            tag.MergeAttribute("data-address-part", "postcode");
            if (!string.IsNullOrWhiteSpace(Context.LookupUrl))
            {
                if (Context.LookupUrl.IndexOf("{pc}", StringComparison.Ordinal) == -1)
                    throw new System.InvalidOperationException("Lookup Url must contain {pc} for the Postcode Box " + Context.Name);

                tag.MergeAttribute("data-address-lookupurl", Context.LookupUrl);
                tag.MergeAttribute("data-address-label", Context.Header);
                tag.MergeAttribute("data-address-func", Context.ConversionFunction);
            }
            tag.MergeAttribute("data-address-externalurl", Context.ExternalUrl);
            return base.UpdateTag(tag);
        }
    }

    public class PostcodeBoxContext : TextBaseContext
    {
        public string LookupUrl { get; internal set; }
        public string ConversionFunction { get; internal set; }
        public string ExternalUrl { get; internal set; } = "https://www.google.co.uk/maps/@{lat},{lng},19z";
        public AwesomeIcon ButtonIcon { get; internal set; } = AwesomeIcon.ExternalLink;
        public ButtonStyle ButtonStyle { get; internal set; } = ButtonStyle.Information;
        public bool HideButton { get; internal set; }
    }

    public sealed class PostcodeBox : PostcodeBox<PostcodeBox>
    {
        internal PostcodeBox(string name = null) : base(new PostcodeBoxContext(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static PostcodeBox BootstrapPostcodeBox(this HtmlHelper htmlHelper, string name)
        {
            var control = new PostcodeBox(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static PostcodeBox BootstrapPostcodeBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new PostcodeBox();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
