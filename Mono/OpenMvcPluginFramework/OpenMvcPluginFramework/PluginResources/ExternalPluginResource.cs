using System;
using OpenMvcPluginFramework.Interfaces;

namespace OpenMvcPluginFramework.PluginResources
{
    /// <summary>
    /// Is used for representing external resources like css or javascript files on a webserver.
    /// </summary>
    public class ExternalPluginResource : IPluginResource
    {
        public ExternalPluginResource(string url)
        {
            if (String.IsNullOrEmpty(url)) throw new ArgumentException("url");

            Url = url;
        }

        public string Url { get; private set; }
    }
}
