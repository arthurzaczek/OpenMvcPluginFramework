using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using OpenMvcPluginFramework;
using OpenMvcPluginFramework.ActionFilters;
using OpenMvcPluginFramework.Interfaces;
using OpenMvcPluginFramework.PluginResources;

namespace BlogPlugin
{
    [Export(typeof(IPlugin))]
    [PluginMetaData("BlogPlugin", "WebDashboard Blog")]
    public class Plugin : PluginBase
    {
        public Plugin()
        {
            base.AddCssResource(new EmbeddedPluginResource(GetResourceLocation("Css", "BlogStyle.css")));
            base.AddElement(new PluginElement() { Id = "Blog", Controller = "PluginBlog", Action = "Index", ActionFilter = new NullActionFilter() });
        }
    }
}