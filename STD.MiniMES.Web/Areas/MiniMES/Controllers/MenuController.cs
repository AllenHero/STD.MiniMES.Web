using Newtonsoft.Json;
using STD.Framework.Utils;
using STD.MiniMES.Web;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    public class MenuController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }

    public class MenuApiController : ApiController
    {
        // GET api/menu
        public IEnumerable<dynamic> Get()
        {
            var UserCode = SysHelper.GetUserCode();
            var TenantId = SysHelper.GetTenantId();
            
            dynamic result = ZCache.GetCache("MenuData");

            if (result == null && !string.IsNullOrEmpty(UserCode) && !string.IsNullOrEmpty(TenantId))
            {
                var postdata = new
                {
                    AppCode = "EPS",
                    ApiCode = "GetUserMenuData",
                    TenantId = TenantId,
                    UserCode = UserCode,
                    RequestAppCode = "MiniMES"
                };

                result = HttpHelper.PostWebApi(ZConfig.GetConfigString("APIGatewayUrl"), JsonConvert.SerializeObject(postdata), 18000);

                ZCache.SetCache("MenuData", result);
            }

            return result;
        }
    }
}
