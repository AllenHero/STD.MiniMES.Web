using STD.MiniMES.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    public class TESTController : Controller
    {
        //
        // GET: /MiniMES/TEST/
        [AllowAnonymous]
        [MvcMenuFilter(false)]
        public ActionResult Index()
        {
            return View();
        }

    }
}
