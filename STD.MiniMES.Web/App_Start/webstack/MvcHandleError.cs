using System.Web;
using System.Web.Mvc;
using log4net;

namespace STD.MiniMES.Web
{
    public class MvcHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            ILog log = LogManager.GetLogger(filterContext.RequestContext.HttpContext.Request.Url.LocalPath);
            log.Error(filterContext.Exception);
            base.OnException(filterContext);
        }
    }
}