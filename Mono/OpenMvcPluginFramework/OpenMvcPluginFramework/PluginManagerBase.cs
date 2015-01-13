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
using Autofac;
using OpenMvcPluginFramework.Interfaces;
using OpenMvcPluginFramework.PluginResources;
using OpenMvcPluginFramework.RouteHandler;
using OpenMvcPluginFramework.ViewEngines;

namespace OpenMvcPluginFramework
{
    public abstract class PluginManagerBase : IPluginManager
    {
        protected static string _scriptLink = "<script src='{0}' ></script>";
        protected static string _cssLink = "<link href='{0}' media='screen' rel='stylesheet' type='text/css' />";

        protected PluginRazorViewEngine _viewEngine;
        protected CompositionContainer _pluginContainer;
        protected IContainer _dependencyCotainer;
       

        [ImportMany]
        protected IEnumerable<Lazy<IPlugin, IPluginMetaData>> _plugins = null;

        protected PluginManagerBase()
        {
            HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedResourceProvider());

            LoadPlugins();

            System.Web.Mvc.ViewEngines.Engines.Clear();
            System.Web.Mvc.ViewEngines.Engines.Add(_viewEngine);
        }

        /// <summary>
        /// Loaded plugins loaded via MEF. Are lazy loaded during start up by LoadPlugins.
        /// </summary>
        public virtual IEnumerable<Lazy<IPlugin>> Plugins
        {
            get { return _plugins; }
        }

        /// <summary>
        /// AutoFac dependency container for resolving third party dependencies
        /// </summary>
        public virtual IContainer DependencyContainer { get; private set; }

        /// <summary>
        /// Renders plugin css resources.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Renders plugin javascript resources.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Renders a selected plugin.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pluginName"></param>
        public virtual void Render(HtmlHelper html, string pluginName)
        {
            if (_plugins != null)
            {
                var plugin = _plugins.FirstOrDefault(p => p.Metadata.Name == pluginName);
                if (plugin != null)
                    plugin.Value.Render(html);
            }
        }

        /// <summary>
        /// Provides the view locations of injected plugins.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Loads plugins via MEF. Should be called during start up.
        /// </summary>
        public void LoadPlugins()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(HttpContext.Current.Server.MapPath("~/bin/Plugins"), "*plugin*.dll"));

            //Dynamic load of plugins via MEF
            _pluginContainer = new CompositionContainer(catalog);
            _pluginContainer.ComposeParts(this);

            var viewLocations = GetPluginViewLocations();
            _viewEngine = new PluginRazorViewEngine(viewLocations);
        }

        /// <summary>
        /// Registers routes for handling link ressources as scripts or style sheets. Should be called during start up.
        /// </summary>
        /// <param name="routes"></param>
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.Insert(0, new Route("{resource}/{type}.plugin.{file}.{extension}",
                                   new RouteValueDictionary(new { }),
                                   new RouteValueDictionary(new { extension = "css|js" }),
                                   new EmbeddedResourceRouteHandler()
                             ));
        }

        /// <summary>
        /// Registers AutoFac dependency container for resolving third party dependencies in plugins.
        /// </summary>
        /// <param name="container"></param>
        public void RegisterDependencyContainer(IContainer container)
        {
            _dependencyCotainer = container;
        }
    }
}
