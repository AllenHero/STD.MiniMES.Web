using System;
using System.Web;
using System.Web.Mvc;

namespace STD.MiniMES.Web
{
    public class MvcDisposeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            GC.Collect();
        }
    }
}