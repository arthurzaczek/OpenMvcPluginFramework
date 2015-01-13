using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;

namespace OpenMvcPluginFramework.Interfaces
{
    public interface IPluginManager
    {
        IContainer DependencyContainer { get; }
        IEnumerable<Lazy<IPlugin>> Plugins { get; }
        void RegisterDependencyContainer(IContainer container);
        void RegisterRoutes(RouteCollection routes);
        void LoadPlugins();
        void Render(HtmlHelper html, string pluginName);
        IHtmlString RenderScripts(HtmlHelper html);
        IHtmlString RenderCss(HtmlHelper html);

    }
}
