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
using System.Dynamic;
using log4net;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    [AllowAnonymous]
    [MvcMenuFilter(false)]
    public class SummaryBoardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取车间信息
        /// </summary>
        /// <param name="TenantID"></param>
        /// <param name="WorkShopId"></param>
        /// <returns></returns>
        public ActionResult LoadWorkshopName(string TenantID, string WorkShopId)
        {
            string result = "信息化车间看板";
            List<dynamic> WorkShop = ApiDataSource.GetWorkShopList(TenantID, WorkShopId).ToObject<List<dynamic>>();
            if (WorkShop.Count > 0)
                result = WorkShop[0].WorkShopName;
            return Json(result, "application/json", JsonRequestBehavior.AllowGet);
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

       
    }
    public class SummaryBoardApiController : System.Web.Http.ApiController
    {
        /// <summary>
        /// 获取当前车间各机台生产状态
        /// </summary>
        /// <param name="WorkShopId"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public dynamic GetLineStatus(string WorkShopId, string TenantID)
        {
            
            List<minimes_order_record> result = new List<minimes_order_record>();
            try
            {
                List<dynamic> linelist = ApiDataSource.GetLineList(TenantID, WorkShopId, null, null, null).ToObject<List<dynamic>>();
                result = new List<minimes_order_record>();
                minimes_order_recordService reportService = new minimes_order_recordService();
                List<minimes_order_record> reportList = reportService.GetModelList(ParamQuery.Instance().AndWhere("State", 1).AndWhere("TenantID", TenantID));
                foreach (var item1 in linelist)
                {
                    //屏蔽
                    if (item1.LineId != "2d6a9f23-c1dc-4a6d-baa7-473011e4954c" && item1.LineId != "d3bb37bd-0aff-4c6a-96a0-d544c136a018")
                    {
                        minimes_order_record row = new minimes_order_record();
                        row.LineId = item1.LineId + "";//产线ID
                        row.LineName = item1.LineName + "";//产线名称
                        row.Color = "grey";//状态颜色
                        row.StateName = "未切单";//状态名称

                        result.Add(row);

                        foreach (var item in reportList)
                        {
                            if (row.LineId == item.LineId)
                            {
                                row.Qty = item.Qty;
                                row.PlanQty = item.PlanQty;
                                row.ReserveQty = item.ReserveQty;

                                minimes_resttimeService resttimeService = new minimes_resttimeService();
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

                                if (row.PlanQty + row.ReserveQty <= row.Qty)//已完成 注;实际产量大于等于计划产量加备存数据 工单才完工
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 获取工单进度
        /// </summary>
        /// <param name="WorkShopId"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public dynamic GetOrderNoProgress(string WorkShopId, string TenantID)
        {
            LineBoardController lineboardController = new LineBoardController();

            List<dynamic> result = new List<dynamic>();
            try
            {
                minimes_order_recordService reportService = new minimes_order_recordService();
                List<minimes_order_record> reportList = reportService.GetModelList(ParamQuery.Instance().AndWhere("WorkShopId", WorkShopId).AndWhere("State", 1).AndWhere("TenantID", TenantID).OrderBy("LineId ASC"));
                foreach (var item in reportList)
                {
                    minimes_order_record row = new minimes_order_record();

                    row.LineId = item.LineId;//产线ID
                    row.LineName = item.LineName;//产线名称
                    row.OrderNo = item.OrderNo;//工单编号
                    row.ProductCode = item.ProductCode;//产品编号
                    row.ProductName = item.ProductName;//产品名称
                    row.Qty = item.Qty;//实际产量
                    row.PlanQty = item.PlanQty;//计划产量
                    row.StopCount = item.StopTime;//停机时间

                    if (row.PlanQty == 0)
                    {
                        row.PlanRatio = "0%";
                    }
                    else
                    {
                        row.PlanRatio = Math.Round((double)row.Qty * 100 / row.PlanQty, 2) + "%";//计划达成率
                    }

                    //生产时间
                    int ProductTime= lineboardController.GetProductTime(row.OrderNo, TenantID) - lineboardController.GetLostTime(row.OrderNo, TenantID);//分钟

                    //稼动率
                    double Utilization = 0;
                    if (ProductTime!=0)
                    {
                        Utilization =Math.Round((double)(ProductTime - row.StopTime) * 100 / (ProductTime),2);
                    }
                    row.Utilization = Utilization + "%";
                    result.Add(row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 获取设备运行时间&效率
        /// </summary>
        /// <param name="WorkShopId"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public dynamic GetEquipmentData(string WorkShopId, string TenantID)
        {
            
            List<dynamic> result = new List<dynamic>();
            minimes_status_recordService statusrecordService = new minimes_status_recordService();
            List<dynamic> List = statusrecordService.GetData(WorkShopId, TenantID);
            if (List.Count > 0 && List != null)
            {
                foreach (var item in List)
                {
                    dynamic obj = new ExpandoObject();

                    obj.Date = item.Date;
                    obj.StopTime = item.StopTime;
                    if ((item.StopTime + item.ProduceTime == 0))
                    {
                        obj.Rate = 0;
                    }
                    else
                    {
                        obj.Rate = Math.Round((item.ProduceTime * 100 / (item.StopTime + item.ProduceTime)), 2);
                    }
                    result.Add(obj);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取当天异常
        /// </summary>
        /// <param name="WorkShopId"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public dynamic GetUndisposedException(string WorkShopId, string TenantID)
        {
            string WorkShopCode = "";
            List<dynamic> WorkShop = ApiDataSource.GetWorkShopList(TenantID, WorkShopId).ToObject<List<dynamic>>();
            if (WorkShop.Count > 0)
                WorkShopCode = WorkShop[0].WorkShopCode;
            return   ApiDataSource.GetUndisposedException(WorkShopCode, TenantID);
        }

        /// <summary>
        /// 获取7天内异常
        /// </summary>
        /// <param name="WorkShopId"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public dynamic GetExceptionWeekAnalysis(string WorkShopId, string TenantID)
        {
            string WorkShopCode = "";
            List<dynamic> WorkShop = ApiDataSource.GetWorkShopList(TenantID, WorkShopId).ToObject<List<dynamic>>();
            if (WorkShop.Count > 0)
                WorkShopCode = WorkShop[0].WorkShopCode;
            return ApiDataSource.GetExceptionWeekAnalysis(WorkShopCode, TenantID);
        }

        /// <summary>
        /// 获质量问题
        /// </summary>
        /// <param name="WorkShopId"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public dynamic GetFirstCheckByWorkShopCode(string WorkShopId, string TenantID)
        {
            string WorkShopCode = "";
            List<dynamic> WorkShop = ApiDataSource.GetWorkShopList(TenantID, WorkShopId).ToObject<List<dynamic>>();
            if (WorkShop.Count > 0)
                WorkShopCode = WorkShop[0].WorkShopCode;
            return ApiDataSource.GetFirstCheckByWorkShopCode(WorkShopCode, TenantID);
        }

        /// <summary>
        /// 获质量问题
        /// </summary>
        /// <param name="WorkShopId"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public dynamic GetRandomPltoInfo(string WorkShopId, string TenantID)
        {
            string WorkShopCode = "";
            List<dynamic> WorkShop = ApiDataSource.GetWorkShopList(TenantID, WorkShopId).ToObject<List<dynamic>>();
            if (WorkShop.Count > 0)
                WorkShopCode = WorkShop[0].WorkShopCode;
            return ApiDataSource.GetRandomPltoInfo(WorkShopCode, TenantID);
        }

        /// <summary>
        /// 获取工单列表
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public dynamic GetWorkSheetList(string LineId, string TenantID)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                dynamic WorkNoList = ApiDataSource.GetChangeOrderNoList(TenantID,LineId);
                if (WorkNoList != null && WorkNoList.Count > 0)
                {
                    foreach (var item in WorkNoList)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + item.WorkSheetNo + "</td>");
                        sb.Append("<td>" + item.ProductCode + "</td>");
                        sb.Append("<td>" + item.ProductName + "</td>");
                        sb.Append("<td>" + item.LineName + "</td>");
                        sb.Append("<td>" + item.PlanStartDate.ToString("yyyy-MM-dd") + "</td>");
                        sb.Append("<td>" + item.PlanCount + "</td>");
                        sb.Append("<td style=\"display: none\">" + item.LineId + "</td>");
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    sb.Append("<tr><td colspan=\"10\">没有数据</td></tr>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 工单查询
        /// </summary>
        /// <param name="WorkSheetNo"></param>
        /// <param name="PlanDate"></param>
        /// <param name="Line_id"></param>
        /// <param name="ProductCode"></param>
        /// <returns></returns>
        public dynamic GetWorkSheet(string WorkSheetNo, string PlanDate, string Line_id, string ProductCode)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (WorkSheetNo == null) WorkSheetNo = "";
                if (ProductCode == null) ProductCode = "";
                if (PlanDate == null) PlanDate = "";
                if (string.IsNullOrEmpty(Line_id)) Line_id = "";
                string TenantId = SysHelper.GetTenantId();
                dynamic WorkNoList = ApiDataSource.GetWorkSheetNoList(TenantId, WorkSheetNo,Line_id, ProductCode, PlanDate);

                if (WorkNoList != null && WorkNoList.Count > 0)
                {
                    foreach (var item in WorkNoList)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + item.WorkSheetNo + "</td>");
                        sb.Append("<td>" + item.ProductCode + "</td>");
                        sb.Append("<td>" + item.ProductName + "</td>");
                        sb.Append("<td>" + item.LineName + "</td>");
                        sb.Append("<td>" + item.PlanStartDate.ToString("yyyy-MM-dd") + "</td>");
                        sb.Append("<td>" + item.PlanCount + "</td>");
                        sb.Append("<td style=\"display: none\">" + item.LineId + "</td>");
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    sb.Append("<tr><td colspan=\"10\">没有数据</td></tr>");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }

        /// <summary>
        /// 获取产线信息
        /// </summary>
        /// <param name="workshopId"></param>
        /// <returns></returns>
        public string GetCurrentLineList(string workshopId)
        {
            StringBuilder sb = new StringBuilder();
            dynamic list = ApiDataSource.GetLineList(SysHelper.GetTenantId(), null, null, null, null);
            foreach (var item in list)
            {
                sb.Append("<option selected=\"selected\" value=\"" + item.LineId + "\">" + item.LineName + "</option>");
            }
            return sb.ToString();
        }
    }
}
