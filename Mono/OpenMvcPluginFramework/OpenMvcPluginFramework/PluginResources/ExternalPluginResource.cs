using System;
using OpenMvcPluginFramework.Interfaces;

namespace OpenMvcPluginFramework.PluginResources
{
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
