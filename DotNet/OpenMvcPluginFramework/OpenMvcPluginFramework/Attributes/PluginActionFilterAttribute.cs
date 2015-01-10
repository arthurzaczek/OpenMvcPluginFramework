using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using OpenMvcPluginFramework.Interfaces;

namespace OpenMvcPluginFramework.Attributes
{
    public class PluginActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            foreach (Lazy<IPlugin> plugin in PluginManager.Instance.Plugins)
                foreach (var filter in plugin.Value.Elements)
                    if (System.String.Compare(filter.Action, filterContext.ActionDescriptor.ActionName, System.StringComparison.OrdinalIgnoreCase) == 0 &&
                        System.String.Compare(filter.Controller, filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, System.StringComparison.OrdinalIgnoreCase) == 0)
                        filter.ActionFilter.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            foreach (Lazy<IPlugin> plugin in PluginManager.Instance.Plugins)
                foreach (var filter in plugin.Value.Elements)
                    if (System.String.Compare(filter.Action, filterContext.ActionDescriptor.ActionName, System.StringComparison.OrdinalIgnoreCase) == 0 &&
                        System.String.Compare(filter.Controller, filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, System.StringComparison.OrdinalIgnoreCase) == 0)
                        filter.ActionFilter.OnActionExecuted(filterContext);
        } 
    }
}
