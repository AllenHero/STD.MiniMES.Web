using Newtonsoft.Json;
using STD.Framework.Core;
using STD.MiniMES.Models;
using STD.MiniMES.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    [AllowAnonymous]
    [MvcMenuFilter(false)]
    public class ScanBarCodeController : Controller
    {
        //
        // GET: /MiniMES/ScanBarCode/

        public ActionResult Index()
        {
            return View();
        }


        //获取线体
        public ActionResult GetLine(string TenantID, string LineId)
        {
            //List<dynamic> result = ApiDataSource.GetWorkSheetList(TenantID, DateTime.Now.Date, null, null, null).ToObject<List<dynamic>>();
            List<dynamic> result = ApiDataSource.GetWorkSheetList(TenantID, null, LineId, null, null, null, null, null, null, null).ToObject<List<dynamic>>();
           // List<dynamic> worksheetlist = result.Where(x => x.LineId == LineId).ToList();
            string js = JsonConvert.SerializeObject(result);
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        //获取工单
        public ActionResult GetOrderNo(string TenantID, string LineId)
        {
            // List<dynamic> result = ApiDataSource.GetWorkSheetList(TenantID, DateTime.Now.Date, null, null, null).ToObject<List<dynamic>>();
            List<dynamic> result = ApiDataSource.GetWorkSheetList(TenantID, null, LineId, null, null, null, null, null, null, null).ToObject<List<dynamic>>();
            //List<dynamic> worksheetlist = result.Where(x => x.LineId == LineId).ToList();
            string js = JsonConvert.SerializeObject(result);
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        //获取工单详情
        public ActionResult GetOrderData(string OrderNo, string TenantID, string LineId)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                WebApiController controller = new WebApiController();
                dynamic result = controller.GetOrderNoDetail(LineId, OrderNo, DateTime.Now.Date.ToString("yyyy-MM-dd"), TenantID);
                string status = result.status + "";
                string message = result.message + "";
                if (status == "error")
                {
                    sb.Append("{\"ok\":false,");
                    sb.Append("\"message\":\"" + message + "\"}");
                    return Json(sb.ToString(), "application/json", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    dynamic rows = result.rows;
                    int TTQty = rows.Qty;
                    int TodayQty = GetQty(TenantID, LineId, OrderNo);
                    if (TTQty < TodayQty)
                        TTQty = TodayQty;
                    string Rate = Math.Round((decimal)TTQty * 100 / Convert.ToInt32(rows.PlanQty), 2) + "%";
                    sb.Append("{\"ok\":true,");
                    sb.Append("\"OrderNo\":\"" + rows.OrderNo + "\",");
                    sb.Append("\"ProductCode\":\"" + rows.ProductCode + "\",");
                    sb.Append("\"ProductName\":\"" + rows.ProductName + "\",");
                    sb.Append("\"Qty\":\"" + TodayQty + "\",");
                    sb.Append("\"QtyNG\":\"" + rows.QtyNG + "\",");
                    sb.Append("\"PlanQty\":\"" + rows.PlanQty + "\",");
                    sb.Append("\"Ratio\":\"" + rows.Ratio + "\",");
                    sb.Append("\"StandUsers\":\"" + rows.StandUsers + "\",");
                    sb.Append("\"TTQty\":\"" + TTQty + "\",");
                    sb.Append("\"Rate\":\"" + Rate + "\"}");
                    return Json(sb.ToString(), "application/json", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                sb.Append("{\"ok\":false,");
                sb.Append("\"message\":\"" + ex.Message + "\"}");
                return Json(sb.ToString(), "application/json", JsonRequestBehavior.AllowGet);
            }
        }


        //扫码
        public ActionResult GetScanBarcode(string TenantID, string Barcode, string LineId, string OrderNo, string TallyRatio, string Person, string PlanQty)
        {
            StringBuilder sb = new StringBuilder();
            int Qty = 0;
            if (!int.TryParse(TallyRatio, out Qty))
            {
                sb.Append("{\"ok\":false,");
                sb.Append("\"message\":\"单次计数必须为整数\"}");
                return Json(sb.ToString(), "application/json", JsonRequestBehavior.AllowGet);
            }
            try
            {
                dynamic data = new
                {
                    OrderNo = OrderNo,
                    LineId = LineId,
                    Date = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                    Barcode = Barcode,
                    TallyRatio = TallyRatio,
                    Qty = Qty,
                    StandUsers = Person,
                    SignID = Guid.NewGuid().ToString(),
                    TenantId = TenantID,
                    CreateDate = DateTime.Now,
                    CreateUser = ""
                };
                WebApiController controller = new WebApiController();
                dynamic result = controller.UpdateScan(data);
                string status = result.status + "";
                string message = result.message + "";
                if (status == "error")
                {
                    sb.Append("{\"ok\":false,");
                    sb.Append("\"message\":\"" + message + "\"}");
                    return Json(sb.ToString(), "application/json", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string refQtyNG = result.QtyNG + "";
                    int TTQty = result.Qty;
                    int TodayQty = GetQty(TenantID, LineId, OrderNo);
                    if (TTQty < TodayQty)
                        TTQty = TodayQty;
                    string Rate = Math.Round((decimal)TTQty * 100 / Convert.ToInt32(PlanQty), 2) + "%";
                    sb.Append("{\"ok\":true,");
                    sb.Append("\"Qty\":\"" + TodayQty + "\",");
                    sb.Append("\"QtyNG\":\"" + refQtyNG + "\",");
                    sb.Append("\"TTQty\":\"" + TTQty + "\",");
                    sb.Append("\"Rate\":\"" + Rate + "\",");
                    sb.Append("\"message\":\"保存成功。\"}");
                    return Json(sb.ToString(), "application/json", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                sb.Append("{\"ok\":false,");
                sb.Append("\"message\":\"" + ex.Message + "\"}");
                return Json(sb.ToString(), "application/json", JsonRequestBehavior.AllowGet);
            }
        }


        private int GetQty(string TenantID, string LineId, string OrderNo)
        {
            int result = 0;
            try
            {
                minimes_hoursreportService reportService = new minimes_hoursreportService();
                List<minimes_hoursreport> reportList = reportService.GetModelList(
        ParamQuery.Instance()
        .AndWhere("LineId", LineId)
         .AndWhere("OrderNo", OrderNo)
        .AndWhere("TenantID", TenantID)
         .AndWhere("PlanDate", DateTime.Now.Date)
        );
                result = reportList.Sum(v => v.Qty);
            }
            catch
            { }
            return result;
        }

    }
}
