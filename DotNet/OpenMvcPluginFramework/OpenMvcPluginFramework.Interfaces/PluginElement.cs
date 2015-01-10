using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace OpenMvcPluginFramework.Interfaces
{
    public class PluginElement
    {
        public string Id { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public IActionFilter ActionFilter { get; set; }
    }
}
