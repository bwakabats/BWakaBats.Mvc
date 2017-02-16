// *****************************************************
//                     MVC EXPANSION
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
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BWakaBats.Extensions
{
    public static partial class HtmlHelperExtensions
    {
        #region ActionLink

        #region ActionLinkRaw

        public static MvcHtmlString ActionLinkRaw(this HtmlHelper htmlHelper, string linkText, string actionName)
        {
            return htmlHelper.ActionLinkRaw(linkText, actionName, null, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString ActionLinkRaw(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            return htmlHelper.ActionLinkRaw(linkText, actionName, null, new RouteValueDictionary(routeValues), new RouteValueDictionary());
        }

        public static MvcHtmlString ActionLinkRaw(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            return htmlHelper.ActionLinkRaw(linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString ActionLinkRaw(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            return htmlHelper.ActionLinkRaw(linkText, actionName, null, routeValues, new RouteValueDictionary());
        }

        public static MvcHtmlString ActionLinkRaw(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            return htmlHelper.ActionLinkRaw(linkText, actionName, null, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString ActionLinkRaw(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.ActionLinkRaw(linkText, actionName, null, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ActionLinkRaw(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            return htmlHelper.ActionLinkRaw(linkText, actionName, controllerName, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString ActionLinkRaw(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (string.IsNullOrEmpty(linkText))
            {
                throw new ArgumentNullException("linkText");
            }
            return MvcHtmlString.Create(GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, null, actionName, controllerName, null, null, null, routeValues, htmlAttributes, false));
        }

        public static MvcHtmlString ActionLinkRaw(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return htmlHelper.ActionLinkRaw(linkText, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString ActionLinkRaw(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (string.IsNullOrEmpty(linkText))
            {
                throw new ArgumentNullException("linkText");
            }
            return MvcHtmlString.Create(GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, null, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes, false));
        }

        #endregion

        #region ActionLink HtmlString

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string actionName)
        {
            return htmlHelper.ActionLink(linkHtmlString, actionName, null, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string actionName, object routeValues)
        {
            return htmlHelper.ActionLink(linkHtmlString, actionName, null, new RouteValueDictionary(routeValues), new RouteValueDictionary());
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string actionName, string controllerName)
        {
            return htmlHelper.ActionLink(linkHtmlString, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string actionName, RouteValueDictionary routeValues)
        {
            return htmlHelper.ActionLink(linkHtmlString, actionName, null, routeValues, new RouteValueDictionary());
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string actionName, object routeValues, object htmlAttributes)
        {
            return htmlHelper.ActionLink(linkHtmlString, actionName, null, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.ActionLink(linkHtmlString, actionName, null, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            return htmlHelper.ActionLink(linkHtmlString, actionName, controllerName, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (linkHtmlString == null)
            {
                throw new ArgumentNullException("linkHtmlString");
            }
            return MvcHtmlString.Create(GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkHtmlString.ToString(), null, actionName, controllerName, null, null, null, routeValues, htmlAttributes, false));
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return htmlHelper.ActionLink(linkHtmlString, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (linkHtmlString == null)
            {
                throw new ArgumentNullException("linkHtmlString");
            }
            return MvcHtmlString.Create(GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkHtmlString.ToString(), null, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes, false));
        }

        #endregion

        #endregion

        #region RouteLink

        #region RouteLinkRaw

        public static MvcHtmlString RouteLinkRaw(this HtmlHelper htmlHelper, string linkText, object routeValues)
        {
            return htmlHelper.RouteLinkRaw(linkText, new RouteValueDictionary(routeValues));
        }

        public static MvcHtmlString RouteLinkRaw(this HtmlHelper htmlHelper, string linkText, string routeName)
        {
            return htmlHelper.RouteLinkRaw(linkText, routeName, null);
        }

        public static MvcHtmlString RouteLinkRaw(this HtmlHelper htmlHelper, string linkText, RouteValueDictionary routeValues)
        {
            return htmlHelper.RouteLinkRaw(linkText, routeValues, new RouteValueDictionary());
        }

        public static MvcHtmlString RouteLinkRaw(this HtmlHelper htmlHelper, string linkText, object routeValues, object htmlAttributes)
        {
            return htmlHelper.RouteLinkRaw(linkText, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString RouteLinkRaw(this HtmlHelper htmlHelper, string linkText, string routeName, object routeValues)
        {
            return htmlHelper.RouteLinkRaw(linkText, routeName, new RouteValueDictionary(routeValues));
        }

        public static MvcHtmlString RouteLinkRaw(this HtmlHelper htmlHelper, string linkText, string routeName, RouteValueDictionary routeValues)
        {
            return htmlHelper.RouteLinkRaw(linkText, routeName, routeValues, new RouteValueDictionary());
        }

        public static MvcHtmlString RouteLinkRaw(this HtmlHelper htmlHelper, string linkText, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.RouteLinkRaw(linkText, null, routeValues, htmlAttributes);
        }

        public static MvcHtmlString RouteLinkRaw(this HtmlHelper htmlHelper, string linkText, string routeName, object routeValues, object htmlAttributes)
        {
            return htmlHelper.RouteLinkRaw(linkText, routeName, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString RouteLinkRaw(this HtmlHelper htmlHelper, string linkText, string routeName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (string.IsNullOrEmpty(linkText))
            {
                throw new ArgumentNullException("linkText");
            }
            return MvcHtmlString.Create(GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, routeName, null, null, null, null, null, routeValues, htmlAttributes, false));
        }

        public static MvcHtmlString RouteLinkRaw(this HtmlHelper htmlHelper, string linkText, string routeName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return htmlHelper.RouteLinkRaw(linkText, routeName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString RouteLinkRaw(this HtmlHelper htmlHelper, string linkText, string routeName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (string.IsNullOrEmpty(linkText))
            {
                throw new ArgumentNullException("linkText");
            }
            return MvcHtmlString.Create(GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, routeName, null, null, protocol, hostName, fragment, routeValues, htmlAttributes, false));
        }

        #endregion

        #region RouteLink HtmlString

        public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, object routeValues)
        {
            return htmlHelper.RouteLink(linkHtmlString, new RouteValueDictionary(routeValues));
        }

        public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string routeName)
        {
            return htmlHelper.RouteLink(linkHtmlString, routeName, null);
        }

        public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, RouteValueDictionary routeValues)
        {
            return htmlHelper.RouteLink(linkHtmlString, routeValues, new RouteValueDictionary());
        }

        public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, object routeValues, object htmlAttributes)
        {
            return htmlHelper.RouteLink(linkHtmlString, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string routeName, object routeValues)
        {
            return htmlHelper.RouteLink(linkHtmlString, routeName, new RouteValueDictionary(routeValues));
        }

        public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string routeName, RouteValueDictionary routeValues)
        {
            return htmlHelper.RouteLink(linkHtmlString, routeName, routeValues, new RouteValueDictionary());
        }

        public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.RouteLink(linkHtmlString, null, routeValues, htmlAttributes);
        }

        public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string routeName, object routeValues, object htmlAttributes)
        {
            return htmlHelper.RouteLink(linkHtmlString, routeName, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string routeName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (linkHtmlString == null)
            {
                throw new ArgumentNullException("linkHtmlString");
            }
            return MvcHtmlString.Create(GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkHtmlString.ToString(), routeName, null, null, null, null, null, routeValues, htmlAttributes, false));
        }

        public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string routeName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return htmlHelper.RouteLink(linkHtmlString, routeName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, IHtmlString linkHtmlString, string routeName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (linkHtmlString == null)
            {
                throw new ArgumentNullException("linkHtmlString");
            }
            return MvcHtmlString.Create(GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkHtmlString.ToString(), routeName, null, null, protocol, hostName, fragment, routeValues, htmlAttributes, false));
        }

        #endregion

        #endregion

        private static string GenerateLink(RequestContext requestContext, RouteCollection routeCollection, string linkText, string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, bool includeImplicitMvcValues)
        {
            string href = UrlHelper.GenerateUrl(routeName, actionName, controllerName, protocol, hostName, fragment, routeValues, routeCollection, requestContext, includeImplicitMvcValues);

            var anchor = new TagBuilder("a")
            {
                InnerHtml = !string.IsNullOrEmpty(linkText) ? linkText : string.Empty
            };
            anchor.MergeAttributes<string, object>(htmlAttributes);
            anchor.MergeAttribute("href", href);
            return anchor.ToString(TagRenderMode.Normal);
        }
    }
}
