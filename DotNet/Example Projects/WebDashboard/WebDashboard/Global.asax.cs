using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using WebDashboard.DataAccess;
using WebDashboard.DataAccess.Interfaces;

namespace WebDashboard
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Wire dependencies
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new BlogDataAccess()).As<IBlogDataAccess>();

            OpenMvcPluginFramework.PluginManager.Instance.RegisterDependencyContainer(builder.Build());

            OpenMvcPluginFramework.PluginManager.Instance.LoadPlugins();
        }
    }
}