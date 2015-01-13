using System;
using System.Web;
using OpenMvcPluginFramework.Interfaces;

namespace OpenMvcPluginFramework.PluginResources
{
    /// <summary>
    /// Is used for representing embedded resources like css or javascript files.
    /// </summary>
    public class EmbeddedPluginResource : IPluginResource
    {
        public EmbeddedPluginResource(string url)
        {
            if (String.IsNullOrEmpty(url)) throw new ArgumentException("url");

            Url = VirtualPathUtility.ToAbsolute(url);
        }

        public string Url { get; private set; }

    }
}
