using System.Web.Http;
using System.Web.Mvc;
using BWakaBats.Attributes;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof($rootnamespace$.Configurations.FilterConfiguration), "Configuration")]

namespace $rootnamespace$.Configurations
{
    public static class FilterConfiguration
    {
        public static void Configuration()
        {
            GlobalFilters.Filters.Add(new ElmahHandleErrorAttribute());
            GlobalConfiguration.Configuration.Filters.Add(new JsonExceptionFilterAttribute());
        }
    }
}