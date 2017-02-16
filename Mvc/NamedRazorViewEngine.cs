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
using System.Web.Mvc;

namespace BWakaBats.Mvc
{
    /// <summary>
    /// Represents a view engine that is used to render a Web page that uses the ASP.NET Razor syntax.
    /// This has been customered to only include cshtml files and to create a CustomerRazorView
    /// </summary>
    public class NamedRazorViewEngine : RazorViewEngine
    {
        /// <summary>
        /// Create a Custom Razor View Engine with jsut the cshtml formats
        /// </summary>
        public NamedRazorViewEngine()
        {
            var areaLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.cshtml" };
            base.AreaViewLocationFormats = areaLocationFormats;
            base.AreaMasterLocationFormats = areaLocationFormats;
            base.AreaPartialViewLocationFormats = areaLocationFormats;

            var locationFormats = new string[] { "~/Views/{1}/{0}.cshtml", "~/Views/Shared/{0}.cshtml" };
            base.ViewLocationFormats = locationFormats;
            base.MasterLocationFormats = locationFormats;
            base.PartialViewLocationFormats = locationFormats;

            base.FileExtensions = new string[] { "cshtml" };
        }

        /// <summary>
        /// Finds the specified view by using the specified controller context and master view name.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewName">The name of the view.</param>
        /// <param name="masterName">The name of the master view.</param>
        /// <param name="useCache">true to use the cached view.</param>
        /// <returns>The page view.</returns>
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var result = base.FindView(controllerContext, viewName, masterName, useCache);
            if (result != null && result.View != null)
            {
                var view = result.View as NamedRazorView;
                if (view != null)
                {
                    view.Name = viewName;
                }
            }
            return result;
        }

        /// <summary>
        /// Creates a custom view by using the specified controller context and the paths of the view and master view.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewPath">The path to the view.</param>
        /// <param name="masterPath">The path to the master view.</param>
        /// <returns>The custom view.</returns>
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new NamedRazorView(controllerContext, viewPath, masterPath, true, base.FileExtensions, base.ViewPageActivator);
        }
    }
}