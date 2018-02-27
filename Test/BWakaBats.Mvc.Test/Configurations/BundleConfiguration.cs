using System.Web.Optimization;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(SalesWizard.Website.Configurations.BundleConfiguration), "Configuration")]

namespace SalesWizard.Website.Configurations
{
    public static class BundleConfiguration
    {
        public static void Configuration()
        {
#if !DEBUG && !TEST
            BundleTable.EnableOptimizations = true;
#endif
            var bundles = BundleTable.Bundles;
            bundles.UseCdn = true;

            var styleBundle = new StyleBundle("~/Content/Site.css");
            styleBundle.Include("~/Content/Site.css");
            bundles.Add(styleBundle);

            var scriptBundle = new ScriptBundle("~/bundles/head");
            scriptBundle.Include("~/Scripts/version.js",
                                 "~/Scripts/modernizr-{version}.js");
            bundles.Add(scriptBundle);

            scriptBundle = new ScriptBundle("~/bundles/main");
            scriptBundle.Include("~/Scripts/jquery-{version}.js",
                                 "~/Scripts/bootstrap.js",
                                 "~/Scripts/respond.js",
                                 "~/Scripts/typeahead.bundle.js",
                                 "~/Scripts/jquery.cookie.js",
                                 "~/Scripts/jquery.unobtrusive-ajax.js",
                                 "~/Scripts/jquery.validate.js",
                                 "~/Scripts/jquery.validate.unobtrusive.js",
                                 "~/Scripts/moment.js",
                                 "~/Scripts/leaflet-{version}.js",
                                 "~/Scripts/proj4.js",
                                 "~/Scripts/proj4leaflet.js",
                                 "~/Scripts/leaflet.awesome-markers.js",
                                 "~/Scripts/plupload/plupload.full.min.js",
                                 "~/Scripts/bootstrap-datetimepicker.js",
                                 "~/Scripts/bwakabats.bootstrap.js",
                                 "~/Scripts/bwakabats.bootstrap-colorpicker.js",
                                 "~/Scripts/bwakabats.bootstrap-filepicker.js",
                                 "~/Scripts/bwakabats.bootstrap-icon5picker-metadata.js",
                                 "~/Scripts/bwakabats.bootstrap-icon5picker.js",
                                 "~/Scripts/bwakabats.bootstrap-grid.js",
                                 "~/Scripts/bwakabats.bootstrap-map.js",
                                 "~/Scripts/bwakabats.bootstrap-postcodebox.js",
                                 "~/Scripts/fontawesome-all.js",
                                 "~/Scripts/htmlbox.full.js");
            bundles.Add(scriptBundle);
        }
    }
}
