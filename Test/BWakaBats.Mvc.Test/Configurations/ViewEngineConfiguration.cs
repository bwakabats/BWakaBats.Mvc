using System.Web.Mvc;
using BWakaBats.Mvc;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(BWakaBats.Mvc.Test.Configurations.ViewEngineConfiguration), "Configuration")]

namespace BWakaBats.Mvc.Test.Configurations
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
