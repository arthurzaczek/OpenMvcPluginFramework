# OpenMvcPluginFramework

##Table of Content:
1. General
2. MvcPluginSiteProject
3. MvcPluginProject
4. GetStarted

## General

The OpenMvcPluginFramework is developed to support building modular web application with ASP.NET MVC 4.
The concept is simple, the user is creating via the framework plugins, which can dynamically loaded by a hosting application
called site. The site is providing storage, authentication or third party services, which are abstracted via interfaces and used
by the plugins. The independed plugin development with losely coupling via interfaces to necessary core functionality is a big advantage
in the concept.

The framework consits of following components:

- OpenMvcPluginFramework assemblies:
The framework consists of two assemblies, the OpenMvcPluginFramework.dll and the OpenMvcPluginFramework.Interfaces assembly.
The OpenMvcPluginFramework.Inferfaces assembly includes are Interfaces and Data Transfer Objects, which are used by the framework.
The OpenMvcPluginFramework assembly includes the actual implement of the OpenMvcPluginFramework.

- Visual Studio 2012 Projekt Templates
The framework provides two project templates, the MvcPluginProject and the MvcPluginSiteProject template. The MvcPluginProject template 
is used to create an OpenMvcPluginFramework Plugin. The templates provides a default configuration, which can be easly adjusted.
The MvcPluginSiteProject template provides a basis setup for using created plugin via the framework.


## MvcPluginSiteProject

The MvcPluginSiteProject template provides a basis setup for using OpenMvcPluginFramework plugins.

The following adjustments are necessary to run the hosting application successfully:

- Register dependencies for loaded plugins
If some of the plugins are depending of funcationality provided by the site application, the dependencies have to be registered.
The framework is using the AutoFac dependency injection framework for resloving these dependencies.
The following code has to be adjusted and added to the Global.asax.cs file in the site application.

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Has to be adjused with dependencies
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new BlogDataAccess()).As<IBlogDataAccess>();
            OpenMvcPluginFramework.PluginManager.Instance.RegisterDependencyContainer(builder.Build());

            OpenMvcPluginFramework.PluginManager.Instance.LoadPlugins();
        }

- Render loaded plugins
Plugin assemblies located in \bin\Plugins are loaded automatically during start up. If a plugin should be 
used the Render method of the PluginManager class has to be called in the dedicated view. See example in Index.cshtml.

@{ OpenMvcPluginFramework.PluginManager.Instance.Render(Html, "Plugin"); }

## MvcPluginProject
The MvcPluginProject template provides a basis setup for creating an OpenMvcPluginFramework plugin.

The following adjustments are necessary to run successfully a plugin.

- Adjust assembly naming
It is necessary that the plugin assembly has a .plugin postfix. That mean in the project properties in section Application
the assembly name and namespace has to be adjusted to following naming convention.

AssemblyName: {PluginName}.plugin.dll
Namespace: {PluginName}.plugin.{namespacename}

It .plugin postfix is necessary to recognize during the plugin loading the relevent assemblies.

- Adjust build event
The default plugin project setup is not building successfully, because the post build event has to be adjusted.
The post build event is copying the successfully build plugin to the site plugin folder. The build event has to be adjusted
in following way.

    xcopy "$(ProjectDir)bin\{PluginName}.plugin.dll" "{PluginSiteLocation}bin\Plugins\" /Y /F

- Register plugin elements
The created views, actions, scirpts and stylesheets have to be register as element, so these resources can be used in the 
site application. Please take care that scripts, stylesheets and views are marked as embedded resources. 
The following lines has to be adjusted in the Plugin class.

    [Export(typeof(IPlugin))]
    [PluginMetaData("Plugin", "Demo Example")] <-- Set plugin name for access
    public class Plugin: PluginBase
    {
        public Plugin()
        {
            //Replace the examples with actual plugin resources
            base.AddScriptResource(new EmbeddedPluginResource(GetResourceLocation("Scripts", "script1.js")));
            base.AddCssResource(new EmbeddedPluginResource(GetResourceLocation("Css", "style1.css")));
            base.AddElement(new PluginElement() { Id = "Home", Controller = "PluginHome", Action = "Index", ActionFilter = new NullActionFilter() });
        }
    }

## GetStarted

- Install Templates
The templaes located in GitHub repository under DotNet/Visul Studio Templates/ have tobe copied to ...\Documents\Visual Studio 2012\Templates\ProjectTemplates.
If templates are copied to this location they can be used via Visual Studio to create new projects.

- Example Project WebDashboard
There is example project located in GitHub repository unter DotNet/Examples/. The project is called WebDashboard and is 
illustrating the used of the OpenMvcPluginFramework. Please take a look.
