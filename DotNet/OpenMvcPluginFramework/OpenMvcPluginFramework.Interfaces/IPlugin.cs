using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Autofac;

namespace OpenMvcPluginFramework.Interfaces
{
    public interface IPlugin
    {        
        IEnumerable<PluginElement> Elements { get; }
        IEnumerable<IPluginResource> CssResources { get; }
        IEnumerable<IPluginResource> ScriptResources { get; }
        void AddElement(PluginElement element);
        void AddCssResource(IPluginResource resource);
        void AddScriptResource(IPluginResource resource);
        string ViewLocation { get; }
        void Render(HtmlHelper html);
    }
}
