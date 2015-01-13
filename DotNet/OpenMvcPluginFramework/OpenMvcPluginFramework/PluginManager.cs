using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMvcPluginFramework.Interfaces;

namespace OpenMvcPluginFramework
{
    public class PluginManager : PluginManagerBase
    {
        public static IPluginManager Instance = new PluginManager();
    }
}
