using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using STD.Framework.Core;
using STD.MiniMES.Models;
using STD.MiniMES.Web;
using Newtonsoft.Json;
using STD.Framework.Utils;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    [AllowAnonymous]
    [MvcMenuFilter(false)]
    public class WorkshopBoardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadWorkshopName(string TenantID, string WorkShopId)
        {
            string result = "信息化车间看板";
            List<dynamic> WorkShop = ApiDataSource.GetWorkShopList(TenantID, WorkShopId).ToObject<List<dynamic>>();
            if (WorkShop.Count > 0)
                result = WorkShop[0].WorkShopName;
            return Json(result, "application/json", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取看板基础信息
        /// </summary>
        /// <param name="DepartCode"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public ActionResult LoadWorkshopBoard(string DepartCode, string TenantID)
        {
            List<minimes_order_record> result = new List<minimes_order_record>();
            List<dynamic> linelist = ApiDataSource.GetLineList(TenantID, DepartCode, null, null, null).ToObject<List<dynamic>>();
            minimes_order_recordService reportService = new minimes_order_recordService();
            List<minimes_order_record> reportList = reportService.GetModelList(
    ParamQuery.Instance()
    //.AndWhere("PlanDate", DateTime.Now.Date)
    .AndWhere("State", 1)
     .AndWhere("TenantID", TenantID)
    );
            foreach (var item1 in linelist)
            {
                //屏蔽
                if (item1.LineId != "2d6a9f23-c1dc-4a6d-baa7-473011e4954c" && item1.LineId != "d3bb37bd-0aff-4c6a-96a0-d544c136a018")
                {
                    minimes_order_record row = new minimes_order_record();
                    row.Color = "tag-grey";
                    row.LineId = item1.LineId + "";
                    row.LineName = item1.LineName + "";
                    row.OrderNo = "";
                    row.ProductCode = "";
                    row.Qty = 0;
                    row.PlanQty = 0;
                    row.State = 1;
                    row.StateName = "未切单";
                    row.StopCount = 0;
                    row.StopTime = 0;
                    row.CreateDateStr = "";
                    result.Add(row);

                    foreach (var item in reportList)
                    {

                        if (row.LineId == item.LineId)
                        {
                            row.OrderNo = item.OrderNo;
                            row.ProductCode = item.ProductCode;
                            row.Qty = item.Qty;
                            row.PlanQty = item.PlanQty;
                            row.StopCount = item.StopCount;
                            row.StopTime = item.StopTime;
                            row.CreateDateStr = item.CreateDate.ToString("yyyy-MM-dd hh:mm:ss");
                            row.ActualMould = item.ActualMould;
                            row.StandardMould = item.StandardMould;
                            row.ReserveQty = item.ReserveQty;

                            string WorkShopId = "";
                            minimes_resttimeService resttimeService = new minimes_resttimeService();
                            dynamic LineList = ApiDataSource.GetLineList(TenantID, null, null, row.LineId, null);
                            foreach (var item_line in LineList)
                            {
                                WorkShopId = item_line.WorkShopId;
                            }
                            List<minimes_resttime> resttimeList = resttimeService.GetModelList(ParamQuery.Instance().AndWhere("WorkShopId", WorkShopId).AndWhere("TenantId", TenantID));

                            string IsRest = "0";//默认值
                            if (resttimeList.Count != 0)
                            {
                                foreach (var item_rest in resttimeList)
                                {
                                    string Time = item_rest.RestTime;
                                    string[] TimeArr = Time.Split('-');
                                    DateTime StartTime = Convert.ToDateTime(TimeArr[0]);
                                    DateTime EndTime = Convert.ToDateTime(TimeArr[1]);
                                    DateTime nowTime = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
                                    if ((nowTime >= StartTime && nowTime <= EndTime))
                                    {
                                        IsRest = "1";//在休息时间区间
                                    }
                                }
                            }

                            if (row.PlanQty+row.ReserveQty <= row.Qty)//已完成 注;实际产量大于等于计划产量加备存数据 工单才完工
                            {
                                row.StateName = "已完成";
                                row.Color = "green";
                            }
                            else if (item.MachineState == 1)
                            {
                                row.StateName = "生产中";
                                row.Color = "blue";
                            }
                            else if (item.MachineState == 0 && IsRest == "1")
                            {
                                row.StateName = "休息中";
                                row.Color = "orange";
                            }
                            else
                            {
                                row.StateName = "停机中";
                                row.Color = "red";
                            }
                        }
                    }
                }
            }
            string js = JsonConvert.SerializeObject(result);
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取实时数据 
        /// </summary>
        /// <param name="DepartCode"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public ActionResult GetWorkshopBoardData(string DepartCode, string TenantID)
        {
            List<minimes_order_record> result = new List<minimes_order_record>();
            try
            {
                //缓存失效重新加载
                List<dynamic> linelist = ApiDataSource.GetLineList(TenantID, DepartCode, null, null, null).ToObject<List<dynamic>>();
                result = new List<minimes_order_record>();
                minimes_order_recordService reportService = new minimes_order_recordService();
                List<minimes_order_record> reportList = reportService.GetModelList(
        ParamQuery.Instance()
        //.AndWhere("PlanDate", DateTime.Now.Date)
        .AndWhere("State", 1)
         .AndWhere("TenantID", TenantID)
        );
                foreach (var item1 in linelist)
                {
                    //屏蔽
                    if (item1.LineId != "2d6a9f23-c1dc-4a6d-baa7-473011e4954c" && item1.LineId != "d3bb37bd-0aff-4c6a-96a0-d544c136a018")
                    {
                        minimes_order_record row = new minimes_order_record();
                        row.Color = "tag-grey";
                        row.LineId = item1.LineId + "";
                        row.LineName = item1.LineName + "";
                        row.OrderNo = "";
                        row.ProductCode = "";
                        row.Qty = 0;
                        row.PlanQty = 0;
                        row.State = 2;
                        row.StateName = "未切单";
                        row.StopCount = 0;
                        row.StopTime = 0;
                        row.CreateDateStr = "";
                        row.PlanRatio = "";
                        row.Weight = 1;

                        result.Add(row);

                        foreach (var item in reportList)
                        {
                            if (row.LineId == item.LineId)
                            {
                                row.OrderNo = item.OrderNo;
                                row.ProductCode = item.ProductCode;
                                row.Qty = item.Qty;
                                row.PlanQty = item.PlanQty;
                                row.StopCount = item.StopCount;
                                row.StopTime = item.StopTime;
                                row.CreateDateStr = item.CreateDate.ToString("yyyy-MM-dd hh:mm:ss");
                                row.ActualMould = item.ActualMould;
                                row.StandardMould = item.StandardMould;
                                row.ReserveQty = item.ReserveQty;

                                string WorkShopId = "";
                                minimes_resttimeService resttimeService = new minimes_resttimeService();
                                dynamic LineList = ApiDataSource.GetLineList(TenantID, null, null, row.LineId, null);
                                foreach (var item_line in LineList)
                                {
                                    WorkShopId = item_line.WorkShopId;
                                }
                                List<minimes_resttime> resttimeList = resttimeService.GetModelList(ParamQuery.Instance().AndWhere("WorkShopId", WorkShopId).AndWhere("TenantId", TenantID));

                                string IsRest = "0";//默认值
                                if (resttimeList.Count != 0)
                                {
                                    foreach (var item_rest in resttimeList)
                                    {
                                        string Time = item_rest.RestTime;
                                        string[] TimeArr = Time.Split('-');
                                        DateTime StartTime = Convert.ToDateTime(TimeArr[0]);
                                        DateTime EndTime = Convert.ToDateTime(TimeArr[1]);
                                        DateTime nowTime = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
                                        if ((nowTime >= StartTime && nowTime <= EndTime))
                                        {
                                            IsRest = "1";//在休息时间区间
                                        }
                                    }
                                }

                                if (row.PlanQty + row.ReserveQty <= row.Qty)//已完成  注;实际产量大于等于计划产量加备存数据 工单才完工
                                {
                                    row.StateName = "已完成";
                                    row.Color = "green";
                                }
                                else if (item.MachineState == 1)
                                {
                                    row.StateName = "生产中";
                                    row.Color = "blue";
                                }
                                else if (item.MachineState == 0 && IsRest == "1")
                                {
                                    row.StateName = "休息中";
                                    row.Color = "orange";
                                }
                                else
                                {
                                    row.StateName = "停机中";
                                    row.Color = "red";
                                }

                                #region  计划达成率&备存计划达成率

                                int ReserveQty = 0;
                                if (row.Qty <= row.PlanQty)
                                {
                                    if (row.PlanQty == 0)
                                    {
                                        row.PlanRatio = "0%";
                                    }
                                    else
                                    {
                                        row.PlanRatio = Math.Round((double)row.Qty * 100 / row.PlanQty, 2) + "%";//计划达成率
                                    }
                                    row.ReserveRate = "0%";
                                }
                                else
                                {
                                    row.PlanRatio = "100%";
                                    if (row.PlanQty == 0)
                                    {
                                        row.ReserveRate = "0%";
                                    }
                                    else
                                    {
                                        if (row.Qty > row.PlanQty)//当实际产量>计划产量时，才有备存数量
                                        {
                                            ReserveQty = row.Qty - row.PlanQty;
                                        }
                                        row.ReserveRate = Math.Round((double)ReserveQty * 100 / row.PlanQty, 2) + "%";//备存计划达成率
                                    }
                                }

                                #endregion

                                #region 模穴率=实际模穴/标准模穴

                                if (row.StandardMould==0)
                                {
                                    row.MouldRate = "0%";
                                }
                                else
                                {
                                    row.MouldRate = Math.Round((double)(row.ActualMould * 100 / row.StandardMould), 2) + "%";//模穴率
                                }

                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            string js = JsonConvert.SerializeObject(result);
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 获取工厂信息
        /// </summary>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public ActionResult LoadTenantName(string TenantID)
        {
            try
            {
                cloud_tenantService reportService = new cloud_tenantService();
                List<cloud_tenant> reportList = reportService.GetModelList(
        ParamQuery.Instance()
         .AndWhere("TenantId", TenantID)
        );
                string js = JsonConvert.SerializeObject(reportList);
                return Json(js, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, "application/json", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取系统当前
        /// </summary>
        /// <returns></returns>
        public string GetDate()
        {
            string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string Week = Day[Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"))].ToString();
            DateTime NowDate = DateTime.Now;
            string date = NowDate.ToString("yyyy-MM-dd HH:mm:ss") + " " + Week;
            return date;
        }

        /// <summary>
        /// 获取系统当前
        /// </summary>
        /// <returns></returns>
        public dynamic GetDateByType(string type)
        {
            dynamic result = null;
            DateTime dt = DateTime.Parse(DateTime.Now.ToShortDateString().ToString());  //当前时间  

            if (type == "日")
            {
                result = new { StartTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00", EndTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00", Date = DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日") };
            }
            if (type == "周")
            {
                DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一  

                result = new { StartTime = startWeek.ToString("yyyy-MM-dd") + " 00:00:00", EndTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00", Date = startWeek.ToString("yyyy年MM月dd日") + "-" + DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日") };
            }
            if (type == "月")
            {
                result = new { StartTime = DateTime.Now.ToString("yyyy-MM") + "-01 00:00:00", EndTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00", Date = DateTime.Now.ToString("yyyy年MM月") };
            }
            string js = JsonConvert.SerializeObject(result);
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}
