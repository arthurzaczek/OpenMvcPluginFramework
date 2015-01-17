using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using WebDashboard.DataAccess.Interfaces;

namespace BlogPlugin.Controllers
{
    public class PluginBlogController : Controller
    {
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult Save(string blogEntryText)
        {
            using (var scope = OpenMvcPluginFramework.PluginManager.Instance.DependencyContainer.BeginLifetimeScope())
            {
                var dataAccess = scope.Resolve<IBlogDataAccess>();
                dataAccess.SaveBlogEntry(blogEntryText);
            }

            return PartialView();
        }

    }
}
