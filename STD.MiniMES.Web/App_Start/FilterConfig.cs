﻿using System.Web;
using System.Web.Mvc;

namespace STD.MiniMES.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new MvcHandleErrorAttribute());
            filters.Add(new System.Web.Mvc.AuthorizeAttribute());
            filters.Add(new MvcDisposeFilter());
            filters.Add(new MvcMenuFilter());
        }
    }
}