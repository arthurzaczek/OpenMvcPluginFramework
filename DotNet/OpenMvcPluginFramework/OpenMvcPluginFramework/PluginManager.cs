using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMvcPluginFramework.Interfaces;

namespace OpenMvcPluginFramework
{
    public class PluginManager : PluginManagerBase
    {
        public static PluginManager Instance = new PluginManager();

        public IEnumerable<Lazy<IPlugin>> Plugins 
        {
            get { return _plugins; }
        }
    }
}
