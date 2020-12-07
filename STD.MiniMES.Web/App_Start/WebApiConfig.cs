using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using STD.Framework.Core;

namespace STD.MiniMES.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Change ControllerSuffix from default string "Controller" to "ApiController"
            var suffix = typeof(DefaultHttpControllerSelector).GetField("ControllerSuffix", BindingFlags.Static | BindingFlags.Public);
            if (suffix != null) suffix.SetValue(null, "ApiController");
 
             //支持命名空间
            config.Services.Replace(typeof(IHttpControllerSelector), 
                new NamespaceHttpControllerSelector(config));
 
            config.ParameterBindingRules.Insert(0, param => {
                if (param.ParameterType == typeof(RequestWrapper))
                    return new RequestWrapperParameterBinding(param);
               
                return null;
            });
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
           //config.Filters.Add(new AuthorizeAttribute());
           config.Filters.Add(new WebApiExceptionFilter());
           config.Filters.Add(new WebApiDisposeFilter());
        }
    }
}
