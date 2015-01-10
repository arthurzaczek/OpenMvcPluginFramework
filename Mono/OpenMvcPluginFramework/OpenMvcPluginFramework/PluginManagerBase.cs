using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.UI;
using OpenMvcPluginFramework.Interfaces;
using OpenMvcPluginFramework.PluginResources;
using OpenMvcPluginFramework.RouteHandler;
using OpenMvcPluginFramework.ViewEngines;

namespace OpenMvcPluginFramework
{
    public abstract class PluginManagerBase
    {
        protected static string _scriptLink = "<script src='{0}' ></script>";
        protected static string _cssLink = "<link href='{0}' media='screen' rel='stylesheet' type='text/css' />";

        protected PluginRazorViewEngine _viewEngine;
        protected CompositionContainer _container;

        [ImportMany]
        protected IEnumerable<Lazy<IPlugin, IPluginMetaData>> _plugins = null;

        protected PluginManagerBase()
        {
            HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedResourceProvider());

            LoadPlugins();

            System.Web.Mvc.ViewEngines.Engines.Clear();
            System.Web.Mvc.ViewEngines.Engines.Add(_viewEngine);
        }

        public virtual IHtmlString RenderCss(HtmlHelper html)
        {
            var resources = new StringBuilder();

            if (_plugins != null)
            {
                foreach (var plugin in _plugins)
                {
                    var cssLinks = plugin.Value.CssResources.Select(r => String.Format(_cssLink, r.Url));
                    resources.Append(String.Join(Environment.NewLine, cssLinks));
                }
            }

            return html.Raw(resources.ToString());
        }

        public virtual IHtmlString RenderScripts(HtmlHelper html)
        {
            var resources = new StringBuilder();

            if (_plugins != null)
            {
                foreach (var plugin in _plugins)
                {
                    var scriptLinks = plugin.Value.ScriptResources.Select(r => String.Format(_scriptLink, r.Url));
                    resources.Append(String.Join(Environment.NewLine, scriptLinks));
                }
            }

            return html.Raw(resources.ToString());
        }

        public virtual void Render(HtmlHelper html, string pluginName)
        {
            if (_plugins != null)
            {
                var plugin = _plugins.FirstOrDefault(p => p.Metadata.Name == pluginName);
                if (plugin != null)
                    plugin.Value.Render(html);
            }
        }

        protected IList<string> GetPluginViewLocations()
        {
            var viewLocations = new List<string>();

            foreach (var plugin in _plugins)
            {
                if (!viewLocations.Contains(plugin.Value.ViewLocation))
                    viewLocations.Add(plugin.Value.ViewLocation);
            }

            return viewLocations;
        }

        protected void LoadPlugins()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(HttpContext.Current.Server.MapPath("~/bin/Plugins"), "*plugin*.dll"));

            _container = new CompositionContainer(catalog);
            _container.ComposeParts(this);

            var viewLocations = GetPluginViewLocations();
            _viewEngine = new PluginRazorViewEngine(viewLocations);
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.Insert(0, new Route("{resource}/{type}.plugin.{file}.{extension}",
                                   new RouteValueDictionary(new { }),
                                   new RouteValueDictionary(new { extension = "css|js" }),
                                   new EmbeddedResourceRouteHandler()
                             ));
        }
    }
}
