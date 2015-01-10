using System;
using System.Text;

namespace OpenMvcPluginFramework.PluginResources.Helper
{
    public static class AssemblyPathParser
    {
        public static string GetAssemblyName(string path)
        {
            string assemblyName = null;

            string[] parts = path.Split('/');
            if (parts.Length >= 4)
            {
                assemblyName = parts[2];

                if (!assemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
                {
                    assemblyName = String.Format("{0}.dll", assemblyName);
                }
            }

            return assemblyName;
        }

        public static string GetResourceName(string path)
        {
            string resourceName = null;

            string[] parts = path.Split('/');
            if (parts.Length >= 4)
            {
                string assemblyName = parts[2];
                resourceName = parts[3];
                if (parts.Length > 4)
                {
                    var buff = new StringBuilder(parts[3]);
                    for (var p = 4; p < parts.Length; p++)
                        buff.Append(".").Append(parts[p]);
                    resourceName = buff.ToString();
                }

                if (!assemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
                {
                    resourceName = String.Format("{0}.{1}", assemblyName, resourceName);
                }
            }

            return resourceName;
        }
    }
}
