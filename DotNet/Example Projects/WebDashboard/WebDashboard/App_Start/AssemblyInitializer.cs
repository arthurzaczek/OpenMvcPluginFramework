using System;
using System.IO;
using System.Reflection;
using System.Web.Compilation;
using System.Web;

[assembly: PreApplicationStartMethod(typeof(WebDashboard.AssemblyInitializer), "Initialize")]
namespace WebDashboard
{
    public static class AssemblyInitializer
    {
        public static void Initialize()
        {
            var pluginFolder = new DirectoryInfo(Path.Combine(HttpRuntime.AppDomainAppPath, "bin\\Plugins"));
            var pluginAssemblies = pluginFolder.GetFiles("*.dll", SearchOption.AllDirectories);
            foreach (var pluginAssemblyFile in pluginAssemblies)
            {
                var asm = Assembly.LoadFrom(pluginAssemblyFile.FullName);
                BuildManager.AddReferencedAssembly(asm);
            }

        }
    }
}