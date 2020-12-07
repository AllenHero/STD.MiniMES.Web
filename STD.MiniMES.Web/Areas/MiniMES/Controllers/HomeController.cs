using STD.MiniMES.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    //[AllowAnonymous]
    //[MvcMenuFilter(false)]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

    }
}
