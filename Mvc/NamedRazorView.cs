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
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace BWakaBats.Mvc
{
    /// <summary>
    /// The class customises the standard RazorView to include the view Name.
    /// </summary>
    public class NamedRazorView : RazorView
    {
        /// <summary>
        /// Initializes a new instance of the NamedRazorView class using the view page activator.
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewPath"></param>
        /// <param name="layoutPath"></param>
        /// <param name="runViewStartPages"></param>
        /// <param name="viewStartFileExtensions"></param>
        /// <param name="viewPageActivator"></param>
        public NamedRazorView(ControllerContext controllerContext, string viewPath, string layoutPath, bool runViewStartPages, IEnumerable<string> viewStartFileExtensions, IViewPageActivator viewPageActivator = null)
            : base(controllerContext, viewPath, layoutPath, runViewStartPages, viewStartFileExtensions, viewPageActivator)
        {
        }

        /// <summary>
        /// Get and set the Name of the View
        /// </summary>
        public string Name { get; set; }

        public override void Render(ViewContext viewContext, TextWriter writer)
        {
            base.Render(viewContext, writer);
        }

        protected override void RenderView(ViewContext viewContext, TextWriter writer, object instance)
        {
            base.RenderView(viewContext, writer, instance);
        }
    }
}