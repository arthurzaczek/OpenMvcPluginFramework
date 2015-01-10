using System.Collections.Generic;
using System.Web.Mvc;

namespace OpenMvcPluginFramework.ActionFilters
{
    public class CompoundActionResult : ActionResult
    {
        List<ActionResult> _results = new List<ActionResult>();

        public void AddResult(ActionResult result)
        {
            _results.Add(result);
        }
        public void AddResultToBegining(ActionResult result)
        {
            _results.Insert(0, result);
        }
        public void WrapResults(ActionResult beginResult, ActionResult endResult)
        {
            AddResultToBegining(beginResult);
            AddResult(endResult);
        }
        public void WrapResults(string beginResult, string endResult)
        {
            AddResultToBegining(new ContentResult() { Content = beginResult });
            AddResult(new ContentResult() { Content = endResult });
        }

        public override void ExecuteResult(ControllerContext context)
        {
            foreach (ActionResult result in _results)
                if (result != null)
                    result.ExecuteResult(context);
        }
    }
}
