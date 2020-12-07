using STD.Framework.Core;
using STD.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;

namespace STD.MiniMES.Web
{
    public class SysHelper
    {
        public static string GetewayServer= ConfigurationManager.AppSettings["GetewayServer"];
        public static string GetCookies(string name)
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(name);
            return cookie == null ? null : cookie.Value;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        /// <returns></returns>
        public static string GetUserId()
        {
            string UserId = FormsAuth.GetUserData().UserId;

            if (string.IsNullOrEmpty(UserId))
            {
                UserId = Guid.Empty.ToString();
            }

            return UserId;
        }

        /// <summary>
        /// 用户工号
        /// </summary>
        /// <returns></returns>
        public static string GetUserCode()
        {
            return FormsAuth.GetUserData().UserCode;
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        /// <returns></returns>
        public static string GetUserName()
        {
            return FormsAuth.GetUserData().UserName;
        }

        /// <summary>
        /// 用户TenantId
        /// </summary>
        /// <returns></returns>
        public static string GetTenantId()
        {
            string TenantId = "082d68ce-4d00-4f95-acee-a5b934f3eb9a";
            //string TenantId = FormsAuth.GetUserData().TenantId;
            if (string.IsNullOrEmpty(TenantId))
            {
                TenantId = Guid.Empty.ToString();
            }

            return TenantId;
        }

        /// <summary>
        /// 用户产线
        /// </summary>
        /// <returns></returns>
        public static string GetUserShopLineId()
        {
            if (FormsAuth.GetUserData().ShopLineId == null)
            {
                return "";
            }
            else
            {
                return FormsAuth.GetUserData().ShopLineId.ToString();
            }
        }

        /// <summary>
        /// 用户班次
        /// </summary>
        /// <returns></returns>
        public static string GetUserShiftId()
        {
            if (FormsAuth.GetUserData().ShiftId == null)
            {
                return "";
            }
            else
            {
                return FormsAuth.GetUserData().ShiftId.ToString();
            }
        }

        public static string GetCurrentProject()
        {
            return GetCookies("CurrentProject");
        }

        public static void ThrowHttpExceptionWhen(bool condition, string message, params object[] param)
        {
            if (condition)
                throw new HttpResponseException(new HttpResponseMessage() { Content = new StringContent(string.Format(message, param)) });
        }

        public static dynamic GetEditUrls(string controller, object extend = null)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["getdetail"] = string.Format("/api/mms/{0}/getdetail/", controller);
            expando["getmaster"] = string.Format("/api/mms/{0}/geteditmaster/", controller);
            expando["edit"] = string.Format("/api/mms/{0}/edit/", controller);
            expando["audit"] = string.Format("/api/mms/{0}/audit/", controller);
            expando["getrowid"] = string.Format("/api/mms/{0}/getnewrowid/", controller);
            expando["report"] = controller;

            if (extend != null)
                EachHelper.EachObjectProperty(extend, (i, name, value) => { expando[name] = value; });

            return expando;
        }

        public static dynamic GetEditResx(string billName, object extend = null)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["rejected"] = "已撤消修改！";
            expando["editSuccess"] = "保存成功！";
            expando["auditPassed"] = "单据已通过审核！";
            expando["auditReject"] = "单据已取消审核！";

            if (extend != null)
                EachHelper.EachObjectProperty(extend, (i, name, value) => expando[name] = value);

            return expando;
        }

        public static dynamic GetIndexUrls(string controller, object extend = null)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["query"] = string.Format("/api/mms/{0}", controller);
            expando["remove"] = string.Format("/api/mms/{0}/", controller);
            expando["billno"] = string.Format("/api/mms/{0}/getnewbillno", controller);
            expando["audit"] = string.Format("/api/mms/{0}/audit/", controller);
            expando["edit"] = string.Format("/mms/{0}/edit/", controller);
            if (extend != null)
                EachHelper.EachObjectProperty(extend, (i, name, value) => { expando[name] = value; });

            return expando;
        }

        public static dynamic GetIndexResx(string billName, object extend = null)
        {
            var expando = (IDictionary<string, object>)new ExpandoObject();
            expando["detailTitle"] = billName + "明细";
            expando["noneSelect"] = "请先选择一条" + billName + "！";
            expando["deleteConfirm"] = "确定要删除选中的" + billName + "吗？";
            expando["deleteSuccess"] = "删除成功！";
            expando["auditSuccess"] = "单据已审核！";

            if (extend != null)
                EachHelper.EachObjectProperty(extend, (i, name, value) => expando[name] = value);

            return expando;
        }

        public static string GetTenantPSD(string str)
         {
             byte[] data = Encoding.GetEncoding("GB18030").GetBytes(str);
             MD5 md5 = new MD5CryptoServiceProvider();
             byte[] OutBytes = md5.ComputeHash(data);
 
             string OutString = "";
             for (int i = 0; i<OutBytes.Length; i++)
             {
                OutString += OutBytes[i].ToString("x2");
             }
            // return OutString.ToUpper();
             return OutString.ToLower();
         }
    }
    public class IDCode
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}