﻿/*************************************************************************
 * 文件名称 ：ApiData.cs                          
 * 描述说明 ：取得API数据
 * 
 * 创建信息 : 
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 深圳市数本科技开发有限公司 www.stdlean.com
**************************************************************************/

using System;
using System.Dynamic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using STD.Framework.Core;

namespace STD.MiniMES.Web
{
    public class ApiData : IDataGetter
    {
        public object GetData(HttpContext context)
        {
            dynamic data = null;
            var url = context.Request.Form["dataAction"];
            var param = JsonConvert.DeserializeObject<dynamic>(context.Request.Form["dataParams"]);

            var route = url.Replace("/api/", "").Split('/'); // route[0]=mms,route[1]=send,route[2]=get
            var type = Type.GetType(String.Format("STD.MiniMES.Areas.{0}.Controllers.{1}ApiController,STD.MiniMES.Web", route), false, true);
            if (type != null)
            {
                var instance = Activator.CreateInstance(type);

                var action = route.Length > 2 ? route[2] : "Get";
                var methodInfo = type.GetMethod(action);
                var parameters = new object[] { new RequestWrapper().SetRequestData(param) };
                data = methodInfo.Invoke(instance, parameters);

                if (data.GetType() == typeof(ExpandoObject))
                {
                    if ((data as ExpandoObject).Where(x => x.Key == "rows").Count() > 0)
                        data = data.rows;
                }
            }

            return data;
        }
    }
}
