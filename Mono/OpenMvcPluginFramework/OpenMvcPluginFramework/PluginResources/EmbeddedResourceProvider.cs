using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using OpenMvcPluginFramework.PluginResources.Helper;

namespace OpenMvcPluginFramework.PluginResources
{
    public class EmbeddedResourceProvider : VirtualPathProvider
    {
        protected ConcurrentDictionary<string, Assembly> _assembliesByName = new ConcurrentDictionary<string, Assembly>();
        protected List<string> _embeddedResourceLocations = new List<string>();

        protected static ConcurrentBag<string> _foundPaths = new ConcurrentBag<string>();

        public EmbeddedResourceProvider()
        {
            _embeddedResourceLocations.Add(HttpContext.Current.Server.MapPath("~/bin/Plugins"));
        }

        private bool IsPluginResourcePath(string virtualPath)
        {
            String checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith("~/Plugins/", StringComparison.InvariantCultureIgnoreCase);
        }

        public override VirtualFile GetFile(string virtualPath)
        {

            if (IsPluginResourcePath(virtualPath))
                return new EmbeddedResourceVirtualFile(virtualPath);
            else
                return base.GetFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (IsPluginResourcePath(virtualPath))
                return null;

            return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        protected string FindAssembly(string name)
        {
            string fullName = null;

            if (_embeddedResourceLocations.Any(l => File.Exists(fullName = Path.Combine(l, name))))
            {
                return fullName;
            }

            return null;
        }

        public override bool FileExists(string virtualPath)
        {
            if (_foundPaths.Contains(virtualPath))
                return true;

            if (IsPluginResourcePath(virtualPath))
                return TryLoadPluginRessource(virtualPath);

            return base.FileExists(virtualPath);
        }

        private bool TryLoadPluginRessource(string virtualPath)
        {
            string path = VirtualPathUtility.ToAppRelative(virtualPath);
            string[] parts = path.Split('/');
            if (parts.Length >= 4)
            {
                string assemblyName = AssemblyPathParser.GetAssemblyName(path);
                string resourceName = AssemblyPathParser.GetResourceName(path);

                Assembly assembly = null;
                assemblyName = FindAssembly(assemblyName);

                if (!_assembliesByName.TryGetValue(assemblyName, out assembly))
                {
                    byte[] assemblyBytes = File.ReadAllBytes(assemblyName);
                    assembly = Assembly.Load(assemblyBytes);
                    _assembliesByName.TryAdd(assemblyName, assembly);
                }

                if (assembly != null)
                {
                    bool found = assembly.GetManifestResourceNames().Contains(resourceName);

                    if (found)
                        _foundPaths.Add(virtualPath);

                    return found;
                }
            }

            return false;
        }
    }

    public class EmbeddedResourceVirtualFile : System.Web.Hosting.VirtualFile
    {
        private string _path;

        public EmbeddedResourceVirtualFile(string virtualPath)
            : base(virtualPath)
        {
            _path = VirtualPathUtility.ToAppRelative(virtualPath);
        }

        public override Stream Open()
        {
            string assemblyName = AssemblyPathParser.GetAssemblyName(_path);
            string resourceName = AssemblyPathParser.GetResourceName(_path);

            assemblyName = Path.Combine(HttpContext.Current.Server.MapPath("~/bin/Plugins"), assemblyName);
            byte[] assemblyBytes = File.ReadAllBytes(assemblyName);
            Assembly assembly = Assembly.Load(assemblyBytes);

            if (assembly != null)
                return assembly.GetManifestResourceStream(resourceName);

            return null;
        }
    }
}
