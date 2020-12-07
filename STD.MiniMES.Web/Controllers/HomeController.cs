using STD.Framework.Core;
using STD.MiniMES.Web;
using System.Collections.Generic;
using System.Web.Mvc;

namespace STD.MiniMES.Controllers
{
    [MvcMenuFilter(false)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var loginer = FormsAuth.GetUserData<LoginerBase>();
            ViewBag.Title = "生产实时监控";

            ViewBag.UserName = loginer.UserName;
            ViewBag.UserCode = loginer.UserCode;
            //ViewBag.Settings = new sys_userService().GetCurrentUserSettings();

            string TenantId = SysHelper.GetTenantId();
            if (!string.IsNullOrEmpty(TenantId))
            {
                
            }
            else
            {
                ViewBag.TenantCode = "";
            }

            var result = new Dictionary<string, object>();
            result.Add("theme", "gray");
            result.Add("navigation", "accordion");
            result.Add("gridrows", "20");
            result.Add("tenant", "std");

            ViewBag.Settings = result;

            return View();
        }
        
        public ActionResult Error() 
        {
            return View();
        }

        public void Download()
        {
            Exporter.Instance().Download();
        }
    }
}
