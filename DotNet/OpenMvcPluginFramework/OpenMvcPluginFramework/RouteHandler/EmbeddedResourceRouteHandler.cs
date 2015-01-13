using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace OpenMvcPluginFramework.RouteHandler
{
    /// <summary>
    /// Customized RoutHandler for supporting embedded resources in plugins like css or javascript resources. 
    /// </summary>
    public class EmbeddedResourceRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new EmbeddedResourceHttpHandler(requestContext.RouteData);
        }
    }

    /// <summary>
    /// Customized HttpHandler for supporting embedded resources in plugins like css or javascript resources. 
    /// </summary>
    public class EmbeddedResourceHttpHandler : IHttpHandler
    {
        private readonly RouteData _routeData;

        public EmbeddedResourceHttpHandler(RouteData routeData)
        {
            _routeData = routeData;
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var routeDataValues = _routeData.Values;
            var fileName = routeDataValues["file"].ToString();
            var typeName = routeDataValues["type"].ToString();
            var resourceName = routeDataValues["resource"].ToString();
            var fileExtension = routeDataValues["extension"].ToString();
            string manifestResourceName = string.Format("{0}.plugin.{1}.{2}.{3}",typeName, resourceName, fileName, fileExtension);
            var assembly = Assembly.LoadFrom(FindAssembly(String.Format("Plugins\\{0}.plugin.dll", typeName)));
            var stream = assembly.GetManifestResourceStream(manifestResourceName);
            context.Response.Clear();
            context.Response.ContentType = "text/css"; // default
            if (fileExtension == "js")
                context.Response.ContentType = "text/javascript";
            stream.CopyTo(context.Response.OutputStream);
        }

        protected string FindAssembly(string name)
        {
            string fullName = Path.Combine(HttpRuntime.BinDirectory, name);
            
            if (File.Exists(fullName))
               return fullName;

            return null;
        }
    }
}
