using System.Web.Mvc;
using BWakaBats.Mvc;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof($rootnamespace$.Configurations.ViewEngineConfiguration), "Configuration")]

namespace $rootnamespace$.Configurations
{
    public static class ViewEngineConfiguration
    {
        public static void Configuration()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new NamedRazorViewEngine());
        }
    }
}
