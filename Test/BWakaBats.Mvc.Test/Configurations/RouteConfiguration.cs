using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(SalesWizard.Website.Configurations.RouteConfiguration), "Configuration")]

namespace SalesWizard.Website.Configurations
{
    public static class RouteConfiguration
    {
        public const string DefaultController = "Main";
        public const string DefaultAction = "Index";

        public static void Configuration()
        {
            ApiConfiguration();
            MvcConfiguration();
        }

        public static void ApiConfiguration()
        {
            //GlobalConfiguration.Configuration.Routes.MapHttpRoute(
            //    name: "Api",
            //    routeTemplate: "a/{action}/{id}",
            //    defaults: new { controller = "Mobile", id = RouteParameter.Optional }
            //);
            //
            //GlobalConfiguration.Configuration.EnableCors();
        }

        public static void MvcConfiguration()
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("favicon.ico");
            RouteTable.Routes.IgnoreRoute("bundles/*");

            RouteTable.Routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}