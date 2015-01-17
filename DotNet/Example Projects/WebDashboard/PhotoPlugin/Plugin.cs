using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ComponentModel.Composition;
using OpenMvcPluginFramework;
using OpenMvcPluginFramework.ActionFilters;
using OpenMvcPluginFramework.Interfaces;
using OpenMvcPluginFramework.PluginResources;

namespace PhotoPlugin
{
    [Export(typeof(IPlugin))]
    [PluginMetaData("PhotoPlugin", "WebDashboard Photo Plugin")]
    public class Plugin : PluginBase
    {
        public Plugin()
        {
            base.AddScriptResource(new EmbeddedPluginResource(GetResourceLocation("Scripts", "jquery.cycle.all.js")));
            base.AddCssResource(new EmbeddedPluginResource(GetResourceLocation("Css", "PhotoStyle.css")));
            base.AddElement(new PluginElement() { Id = "Photo", Controller = "PluginPhoto", Action = "Index", ActionFilter = new NullActionFilter() });
        }
    }
}