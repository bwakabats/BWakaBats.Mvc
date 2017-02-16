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
    public abstract class ImageMap<TControl> : Map<TControl, string>
        where TControl : ImageMap<TControl>
    {
        internal ImageMap(MapContext<string> context, string name)
            : base(context, name)
        {
        }

        protected override bool UpdateTag(TagBuilder tag)
        {
            tag.MergeNotNullAttribute("data-map-image", Context.Value);
            return base.UpdateTag(tag);
        }

        protected override string DefaultCssClass
        {
            get { return "map imagemap"; }
        }
    }

    public sealed class ImageMap : ImageMap<ImageMap>
    {
        internal ImageMap(string name = null) : base(new MapContext<string>(), name) { }
    }

    public static partial class HtmlHelperExtensions
    {
        public static ImageMap BootstrapImageMap(this HtmlHelper htmlHelper, string name)
        {
            var control = new ImageMap(name);
            control.Initialize(htmlHelper);
            return control;
        }

        public static ImageMap BootstrapImageMapFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var control = new ImageMap();
            control.Initialize(htmlHelper, expression);
            return control;
        }
    }
}
