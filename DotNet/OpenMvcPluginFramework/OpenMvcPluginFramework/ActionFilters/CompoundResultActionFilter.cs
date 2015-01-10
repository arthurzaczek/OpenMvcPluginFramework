using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace OpenMvcPluginFramework.ActionFilters
{
    public class CompoundResultActionFilter : ActionFilterAttribute
    {
        protected string _controller;
        protected string _action;
        protected bool _atBegining = false;

        public CompoundResultActionFilter()
        {
        }

        public CompoundResultActionFilter(string controller, string action, bool atBegining = false)
        {
            _controller = controller;
            _action = action;
            _atBegining = atBegining;
        }

        protected CompoundActionResult CompoundResult(dynamic context)
        {
            var result = context.Result as CompoundActionResult;
            if (result == null)
            {
                result = new CompoundActionResult();
                result.AddResult(context.Result);
                context.Result = result;
            }
            return result;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!string.IsNullOrWhiteSpace(_action) && !string.IsNullOrWhiteSpace(_controller))
                AddActionResponse(_action, _controller, filterContext);
        }

        protected void AddActionResponse(string action, string controller, dynamic context)
        {
            HtmlHelper helper = GetHtmlHelper(context.Controller as Controller);
            var result = new ContentResult() { Content = helper.Action(action, controller).ToString() };

            if (_atBegining)
                CompoundResult(context).AddResultToBegining(result);
            else
                CompoundResult(context).AddResult(result);
        }

        public HtmlHelper GetHtmlHelper(Controller controller)
        {
            var viewContext = new ViewContext(controller.ControllerContext, new NullView(), controller.ViewData, controller.TempData, TextWriter.Null);
            return new HtmlHelper(viewContext, new ViewPage());
        }

        public class NullView : IView
        {
            public void Render(ViewContext viewContext, TextWriter writer)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
