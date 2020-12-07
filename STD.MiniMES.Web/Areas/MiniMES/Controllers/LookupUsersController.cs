using Newtonsoft.Json;
using STD.Framework.Core;
using STD.Framework.Utils;
using STD.MiniMES.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    [System.Web.Mvc.AllowAnonymous]
    [MvcMenuFilter(false)]
    public class LookupUsersController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

    }
    public class LookupUsersApiController : ApiController
    {
        static string APIGatewayUrl = ZConfig.GetConfigString("APIGatewayUrl");
        public dynamic GetUserOrganizeMap(RequestWrapper query)
        {
            string TenantId = string.IsNullOrEmpty(query["TenantId"])?SysHelper.GetTenantId(): query["TenantId"];
            string OrganizeId = query["OrganizeId"];
            string UserName = query["UserName"];
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetUserOrganizeMap",
                TenantId = TenantId,
                OrganizeId = OrganizeId,
                UserName = UserName
            };
            dynamic list = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            dynamic result = null;
            if (list != null)
            {
                result = list.rows;
            }
            return result;
        }  
        
        public dynamic GetAllOrganize(RequestWrapper query)
        {
            string TenantId = string.IsNullOrEmpty(query["TenantId"]) ? SysHelper.GetTenantId() : query["TenantId"];
            if (string.IsNullOrEmpty(TenantId))
            {
                TenantId = SysHelper.GetTenantId();
            }
            dynamic result = null;
            var postdata = new
            {
                AppCode = "EPS",
                ApiCode = "GetAllOrganize",
                TenantId = TenantId
            };
            result = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(postdata), 18000);
            return result;
        }        
    }
}
