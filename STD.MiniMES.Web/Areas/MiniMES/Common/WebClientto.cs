using System;
using System.Net;

namespace STD.MiniMES.Web
{/// <summary>
 /// 
 /// </summary>
    public class WebClientto : WebClient
    {
        /// <summary>  
        /// 过期时间  
        /// </summary>  
        public int Timeout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        public WebClientto(int timeout)
        {
            Timeout = timeout;
        }

        /// <summary>  
        /// 重写GetWebRequest,添加WebRequest对象超时时间  
        /// </summary>  
        /// <param name="address"></param>  
        /// <returns></returns>  
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = Timeout;
            request.ReadWriteTimeout = Timeout;
            return request;
        }
    }
}