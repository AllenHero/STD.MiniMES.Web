using Newtonsoft.Json;
using STD.Framework.Core;
using STD.Framework.Utils;
using STD.MiniMES.Web;
using System.Web.Mvc;

namespace STD.MiniMES.Controllers
{
    [AllowAnonymous]
    [MvcMenuFilter(false)]
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.CnName = "STD MiniMES系统";
            ViewBag.EnName = "STD MiniMES System";

            var Token = Request["Token"];
            if (!string.IsNullOrEmpty(Token))
            {
                string APIGatewayUrl = ZConfig.GetConfigString("APIGatewayUrl");
                var data = new
                {
                    AppCode = "EPS",
                    ApiCode = "PostValidateToken",
                    Token = Token
                };

                var result = HttpHelper.PostWebApi(APIGatewayUrl, JsonConvert.SerializeObject(data), 18000);
                if (result != null)
                {
                    if (result.status == true)
                    {
                        var UserInfo = result.UserInfo;

                        //调用框架中的登录机制
                        var loginer = new LoginerBase
                        {
                            UserId = UserInfo.UserId,
                            UserType = "2",
                            TenantId = UserInfo.TenantId,
                            UserCode = UserInfo.UserCode,
                            UserName = UserInfo.UserName,
                            ShiftId = UserInfo.ShiftId
                        };

                        var effectiveHours = ZConfig.GetConfigInt("LoginEffectiveHours");
                        FormsAuth.SignIn(loginer.UserCode, loginer, 60 * effectiveHours);

                        ZCache.SetCache("MenuData", result.MenuData);

                        return Redirect("Home");
                    }
                }

                //return View("授权Token验证失败！单击<a href='" + ZConfig.GetConfigString("GatewayServer") + "'>这里</a>返回登录页面。");
                return Redirect(ZConfig.GetConfigString("GatewayServer") + "/Login");
            }
            else
            {
                //return View("授权参数错误！单击<a href='" + ZConfig.GetConfigString("GatewayServer") + "'>这里</a>返回登录页面。");
                //return View();
                return Redirect(ZConfig.GetConfigString("GatewayServer") + "/Login");
            }
        }

        public ActionResult Logout()
        {
            FormsAuth.SingOut();

            var result = Redirect(ZConfig.GetConfigString("GatewayServer") + "/Login");
            return result;
        }
    }
}
