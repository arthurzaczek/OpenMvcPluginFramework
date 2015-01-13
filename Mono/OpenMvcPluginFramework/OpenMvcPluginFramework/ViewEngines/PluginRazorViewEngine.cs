using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OpenMvcPluginFramework.ViewEngines
{
    /// <summary>
    /// Customized RazorViewEngine for supporting plugin views. 
    /// </summary>
    public class PluginRazorViewEngine : RazorViewEngine
    {
        public PluginRazorViewEngine(IList<string> viewLocations)
        {
            ViewLocationFormats = ViewLocationFormats.Union(viewLocations).ToArray();
            PartialViewLocationFormats = PartialViewLocationFormats.Union(viewLocations).ToArray();
        }

        private bool IsPluginResourcePath(string virtualPath)
        {
            String checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith("~/Plugins/", StringComparison.InvariantCultureIgnoreCase);
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            if (IsPluginResourcePath(virtualPath))
            {
                return System.Web.Hosting.HostingEnvironment.VirtualPathProvider.FileExists(virtualPath);
            }
            else
                return base.FileExists(controllerContext, virtualPath);
        }      
    }
}
