using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using STD.MiniMES.Models;
using STD.Framework.Core;

namespace STD.MiniMES.Web
{
    public class CommonHelper
    {
        public static string GetHostAddress()
        {
            string userIP = "127.0.0.1";
            try
            {
                if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Request == null || System.Web.HttpContext.Current.Request.ServerVariables == null)
                    return "";
                string CustomerIP = "";
                //CDN加速后取到的IP 
                CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }
                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(CustomerIP))
                    return CustomerIP;

                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (CustomerIP == null)
                        CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                }

                if (string.Compare(CustomerIP, "unknown", true) == 0)
                    return System.Web.HttpContext.Current.Request.UserHostAddress;
                return CustomerIP;
            }
            catch { }

            return userIP;
        }

    }

    public class SimpleProductInfo
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int? UPH { get; set; }

        public SimpleProductInfo(string ProductCode,string ProductName,int? UPH)
        {
            this.ProductCode = ProductCode;
            this.ProductName = ProductName;
            this.UPH = UPH;
        }
    }

    public class SimplePlanInfo
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public int? PlanQty { get; set; }

        public SimplePlanInfo(string ProductCode,string ProductName,int?PlanQty)
        {
            this.ProductCode = ProductCode;
            this.ProductName = ProductName;
            this.PlanQty = PlanQty;
        }
    }
}