using STD.MiniMES.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    [AllowAnonymous]
    [MvcMenuFilter(false)]
    public class KanBanController : Controller
    {
        //
        // GET: /MiniMES/KanBan/

        
        public ActionResult Index()
        {
            return View();
        }

        #region 1.当前生产信息

        /// <summary>
        /// 1.当前生产信息
        /// </summary>
        /// <returns></returns>
        //public ActionResult pro_information()
        //{
        //    return View();
        //}

        #endregion

        #region 2.生产统计

        /// <summary>
        /// 2.生产统计
        /// </summary>
        /// <returns></returns>
        //public ActionResult pro_statistics()
        //{
        //    return View();
        //}

        #endregion

        #region 3.LOB管理

        /// <summary>
        /// 3.LOB管理
        /// </summary>
        /// <returns></returns>
        //public ActionResult LOB()
        //{
        //    return View();
        //}

        #endregion

        #region 4.小时产量

        /// <summary>
        /// 4.小时产量
        /// </summary>
        /// <returns></returns>
        //public ActionResult productivity_hour()
        //{
        //    return View();
        //}

        #endregion
        
        #region 5.损失工时

        /// <summary>
        /// 5.损失工时
        /// </summary>
        /// <returns></returns>
        //public ActionResult lost_time()
        //{
        //    return View();
        //}

        #endregion

        #region 6.打包扫描

        /// <summary>
        /// 6.打包扫描
        /// </summary>
        /// <returns></returns>
        //public ActionResult dbindex()
        //{
        //    return View();
        //}

        #endregion

        #region 7.效率统计

        /// <summary>
        /// 7.效率统计
        /// </summary>
        /// <returns></returns>
        //public ActionResult efficiency_statistics()
        //{
        //    return View();
        //}

        #endregion

        #region 8. 报表管理
        //public ActionResult Efficiency_report()
        //{
        //    return View();
        //}

        //public ActionResult ef_Losttime()
        //{
        //    return View();
        //}

        #endregion

        #region 9.班组管理

        //public ActionResult Group_management()
        //{
        //    return View();
        //}
        #endregion
    }
}
