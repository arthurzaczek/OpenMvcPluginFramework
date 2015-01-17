using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpenMvcPluginFramework.Attributes;

namespace PhotoPlugin.Controllers
{
    [PluginActionFilter]
    public class PluginPhotoController : Controller
    {
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}
