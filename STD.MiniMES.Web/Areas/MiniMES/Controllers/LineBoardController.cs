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
using log4net;
using STD.MiniMES.Web.Areas.MiniMES.Models;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    [AllowAnonymous]
    [MvcMenuFilter(false)]
    public class LineBoardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取主界面数据
        /// </summary>
        /// <param name="LineId">线体ID</param>
        /// <param name="TenantID">企业ID</param>
        /// <returns></returns>
        public ActionResult GetLineBoardData(string LineId, string TenantID)
        {
            List<dynamic> linelist = ApiDataSource.GetLineList(TenantID, null, null, LineId, null).ToObject<List<dynamic>>();
            string js = JsonConvert.SerializeObject(linelist);
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取固定数据
        /// </summary>
        /// <param name="LineId">线体ID</param>
        /// <param name="TenantID">企业ID</param>
        /// <returns></returns>
        public ActionResult GetLineData(string LineId, string TenantID)
        {
            ResponseObject result = new ResponseObject();
            minimes_order_recordService reportService = new minimes_order_recordService();
            List<minimes_order_record> reportList = reportService.GetModelList(
    ParamQuery.Instance()
    .AndWhere("LineId", LineId)
     //.AndWhere("PlanDate", DateTime.Now.Date)
     .AndWhere("State", 1)
    .AndWhere("TenantID", TenantID)
    );
            if (reportList.Count > 0)
            {
                minimes_order_record row = reportList[0];
                List<dynamic> linelist = ApiDataSource.GetLineList(TenantID, null, null, LineId, null).ToObject<List<dynamic>>();
                foreach (var item in linelist)
                {
                    row.DifferPerson = item.StandUsers - row.Person;
                    row.PlanPerson = item.StandUsers;
                }

                row.DifferQty = row.PlanQty - row.Qty;
                row.CreateDateStr = row.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                if (row.UPH > 0)//标准UPH
                    row.PlanCT = Math.Round((double)3600 / row.UPH, 2).ToString();//标准CT
                else
                    row.PlanCT = "0";

                dynamic rows = new
                {
                    Person = row.Person,//实际人数
                    ProductName = row.ProductName,//产品名称
                    OrderNo = row.OrderNo,//工单编号
                    ProductCode = row.ProductCode,//产品编号
                    PlanQty = row.PlanQty,//计划产量
                    Ratio = row.Ratio,//单次计数
                    PlanUPH = row.UPH,// 标准UPH
                    PlanCT = row.PlanCT//标准C/T
                };
                result.status = true;
                result.message = "";
                result.rows = rows;
            }
            else
            {
                result.message = "获取不到数据。";
            }
            string js = JsonConvert.SerializeObject(result);
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取实时数据
        /// </summary>
        /// <param name="LineId">线体ID</param>
        /// <param name="TenantID">企业ID</param>
        /// <returns></returns>
        public ActionResult GetLineOrderData(string LineId, string TenantID)
        {
            ResponseObject result = new ResponseObject();
            minimes_order_recordService reportService = new minimes_order_recordService();
            List<minimes_order_record> reportList = reportService.GetModelList(
    ParamQuery.Instance()
    .AndWhere("LineId", LineId)
     //.AndWhere("PlanDate", DateTime.Now.Date)
     .AndWhere("State", 1)
    .AndWhere("TenantID", TenantID)
    );
            if (reportList.Count > 0)
            {
                minimes_order_record row = reportList[0];
                string WorkSheetNo = row.OrderNo;
                List<dynamic> worksheetlist = ApiDataSource.GetWorkSheetDetail(TenantID, WorkSheetNo).ToObject<List<dynamic>>();

                string WorkShopId = "";
                minimes_resttimeService resttimeService = new minimes_resttimeService();
                dynamic LineList = ApiDataSource.GetLineList(TenantID, null, null, LineId, null);
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

                if (row.PlanQty + row.ReserveQty <= row.Qty)//已完成 注;实际产量大于等于计划产量加备存数据 工单才完工
                {
                    row.StateName = "已完成";
                    row.Color = "green";
                }
                else if (row.MachineState == 0 && IsRest == "1")//设备停机且当前时间在休息时间内
                {
                    row.StateName = "休息中";
                    row.Color = "orange";
                }
                else if (row.MachineState == 0)
                {
                    row.StateName = "停机中";
                    row.Color = "red";
                }
                else
                {
                    row.StateName = "生产中";
                    row.Color = "blue";
                }

                //生产时间
                row.ProductTime = GetProductTime(row.OrderNo, TenantID) - GetLostTime(row.OrderNo, TenantID);//分钟
                double Utilization = 0;
                if (row.ProductTime !=0)
                {
                    //稼动率
                    Utilization =Math.Round((double)(row.ProductTime - row.StopTime) * 100 / (row.ProductTime),2);
                }
                row.Utilization = Utilization + "%";

                #region 计划达成率&备存计划达成率

                int ReserveQty = 0;
                if (row.Qty<= row.PlanQty)
                {
                    if (row.PlanQty==0)
                    {
                        row.RatioQty = "0%";
                    }
                    else
                    {
                        row.RatioQty = Math.Round((double)row.Qty * 100 / row.PlanQty, 2) + "%";//计划达成率
                    }
                    row.ReserveRate = "0%";
                }
                else
                {
                    row.RatioQty = "100%";
                    if (row.PlanQty==0)
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

                #region  模穴率=实际模穴/标准模穴

                if (row.StandardMould==0)
                {
                    row.MouldRate = "0%";
                }
                else
                {
                    row.MouldRate = Math.Round((double)(row.ActualMould * 100/ row.StandardMould),2)+"%";
                }

                #endregion 

                row.DifferQty = row.PlanQty - row.Qty;
                row.CreateDateStr = row.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                if (row.ProductTime == 0)
                {
                    row.CurUPH = 0;
                    row.CT = "0";
                }
                else
                {
                    row.CurUPH = (row.Qty / row.ProductTime)*60;//实际UPH
                    row.CT = Math.Round((double)3600 / row.CurUPH, 2).ToString();//实际CT
                }
                if (row.UPH > 0)
                    row.PlanRatio = Math.Round((double)row.CurUPH * 100 / row.UPH, 2) + "%";//生产效率

                #region 日计划
                string dayRatioQty = "";
                int dayPlanQty = 0;
                int dayQty = 0;
                if (worksheetlist!=null)
                {
                    string Scheduling = worksheetlist[0].Scheduling;
                    string Production = worksheetlist[0].Production;
                    if (Scheduling!=null&&!string.IsNullOrWhiteSpace(Scheduling))
                    {
                        string[] array = Scheduling.Split(new char[] { '|' },StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in array)
                        {
                            string[] val = item.Split(new char[] { ',' });
                            if (val!=null&&val.Length==2)
                            {
                                DateTime date = DateTime.Now;
                                int iCount = 0;
                                if (DateTime.TryParse(val[0],out date)&&int.TryParse(val[1],out iCount))
                                {
                                    if (date.ToString("yyyy-MM-dd").Equals(DateTime.Now.ToString("yyyy-MM-dd")))
                                    {
                                        dayPlanQty = iCount;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (Production != null && !string.IsNullOrWhiteSpace(Production))
                    {
                        string[] array = Production.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in array)
                        {
                            string[] val = item.Split(new char[] { ',' });
                            if (val != null && val.Length == 2)
                            {
                                DateTime date = DateTime.Now;
                                int iCount = 0;
                                if (DateTime.TryParse(val[0], out date) && int.TryParse(val[1], out iCount))
                                {
                                    if (date.ToString("yyyy-MM-dd").Equals(DateTime.Now.ToString("yyyy-MM-dd")))
                                    {
                                        dayQty = iCount;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (dayPlanQty == 0)
                    {
                        dayRatioQty = "0%";
                    }
                    else
                    {
                        dayRatioQty = Math.Round((double)dayQty * 100 / dayPlanQty, 2) + "%";//计划达成率
                    }
                }
                #endregion
                dynamic rows = new
                {
                    DayRatioQty = dayRatioQty, 
                    DayPlanQty = dayPlanQty, 
                    DayQty = dayQty, 

                    StateName = row.StateName,//设备状态
                    RatioQty = row.RatioQty,//计划达成率
                    Qty = row.Qty,//实际产量
                    DifferQty = row.DifferQty,//差异产量
                    PlanRatio = row.PlanRatio,//生产效率
                    CurUPH = row.CurUPH,//实际UPH
                    CurCT = row.CT,// 实际C/T
                    CreateDateStr = row.CreateDateStr,//开始时间
                    Utilization = row.Utilization,//时间稼动率
                    ProductTime = row.ProductTime,//生产时间
                    StopCount = row.StopCount,//停机次数
                    StopTime = row.StopTime,//停机时间
                    Color = row.Color,//颜色
                    ReserveRate=row.ReserveRate,//备存计划达成率
                    ActualMould=row.ActualMould,//实际模穴
                    StandardMould=row.StandardMould,//标准模穴
                    MouldRate=row.MouldRate,//模穴率

                };
                result.status = true;
                result.message = "";
                result.rows = rows;
            }
            else
            {
                result.message = "获取不到数据。";
            }
            string js = JsonConvert.SerializeObject(result);
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 点击切换计划
        /// </summary>
        /// <param name="LineId">线体ID</param>
        /// <param name="TenantID">企业ID</param>
        // / <returns></returns>
        public ActionResult ChangeOrder(string LineId, string TenantID)
        {
            try
            {
                //获取待切工单
                //List<dynamic> worksheetlist = ApiDataSource.GetWorkSheetList(TenantID, null, LineId, null, null, null, null, null, null, 1).ToObject<List<dynamic>>();//根据产线来获取
                List<dynamic> worksheetlist = ApiDataSource.GetChangeOrderNoList(TenantID, LineId).ToObject<List<dynamic>>();//根据车间来获取
                foreach (var item in worksheetlist)
                {
                    int Seq = 100;
                    int LineSeq = 100;
                    if (Convert.ToString(item.LineId).Equals(LineId))
                    {
                        LineSeq = 1;
                    }
                    if (int.TryParse(Convert.ToString(item.Remark), out Seq))
                    {
                        item.Seq = Seq;
                    }
                    else
                    {
                        item.Seq = 100;
                    }
                    item.LineSeq = LineSeq;
                }
                List<dynamic> worksheetlists = worksheetlist.OrderBy(a => a.LineSeq).ThenBy(a => a.Seq).ToList(); //按照备注正叙排序
                string js = JsonConvert.SerializeObject(worksheetlists);
                return Json(js, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, "application/json", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取小时产量
        /// </summary>
        /// <param name="LineId">线体ID</param>
        /// <param name="TenantID">企业ID</param>
        // / <returns></returns>
        public ActionResult GetHourData(string LineId, string TenantID)
        {
            ResponseObject result = new ResponseObject();
            try
            {
                List<dynamic> reportList = new List<dynamic>();
                string sql = "";
                if (DateTime.Now.Hour >= 8)
                {
                    sql = string.Format(@"select a.ID, a.OrderNo,a.PlanDate,b.TimeDisplay,b.StartTime,a.ProductName,c.UPH,a.Qty,a.QtyNG,a.Qty+a.QtyNG  as QtySum,a.DetailedNG,a.Remark from minimes_hoursreport a left join minimes_time b on a.Time=b.Time left join minimes_order_record c on a.OrderNo=c.OrderNo where a.LineId = '{0}' and a.PlanDate = '{1}' and a.TenantId = '{2}' and (a.Qty>0 or a.QtyNG>0) order by b.ID; ", LineId, DateTime.Now.Date, TenantID);
                }
                else
                {
                    sql = string.Format(@"select a.ID, a.OrderNo,a.PlanDate,b.TimeDisplay,b.StartTime,a.ProductName,c.UPH,a.Qty,a.QtyNG,a.Qty+a.QtyNG as QtySum,a.DetailedNG,a.Remark from minimes_hoursreport a left join minimes_time b on a.Time=b.Time left join minimes_order_record c on a.OrderNo=c.OrderNo where a.LineId = '{0}' and (a.PlanDate = '{1}' or (a.PlanDate = '{2}' and a.Time>=8)) and a.TenantId = '{3}' and (a.Qty>0 or a.QtyNG>0) order by b.ID; ", LineId, DateTime.Now.Date, DateTime.Now.Date.AddDays(-1), TenantID);

                }
                using (var db = Db.Context("MiniMES"))
                {
                    reportList = db.Sql(sql).QueryMany<dynamic>();
                }
                result.status = true;
                result.total = reportList.Count;
                result.message = "";
                result.rows = reportList;
            }
            catch (Exception ex)
            {
                result.message = ex.Message;

            }
            string js = JsonConvert.SerializeObject(result);
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 下拉框改变
        /// </summary>
        /// <param name="ProductCode">产品型号</param>
        /// <param name="TenantID">企业ID</param>
        /// <returns></returns>
        public ActionResult SelectChange(string ProductCode, string TenantID,string OrderNo)
        {
            int Ratio = 1;
            int UPH = 1;
            int StandardMould = 1;
            int Person = 1;
            string ActualMould = "";
            try
            {
                //minimes_productService reportService = new minimes_productService();
                //List<minimes_product> reportList = reportService.GetModelList(
                //ParamQuery.Instance()
                //.AndWhere("InventoryCode", ProductCode)
                //.AndWhere("TenantId", TenantID)
                //);
                //if (reportList.Count > 0)
                //    Ratio = reportList[0].TallyRatio.Value;
                //List<dynamic> worksheetlist = ApiDataSource.GetInventoryList(TenantID, ProductCode, null, null, null, null).ToObject<List<dynamic>>();
                //if (worksheetlist.Count > 0)
                //    UPH = worksheetlist[0].StandardUPH;
                minimes_uphService uphService = new minimes_uphService();
                List<minimes_uph> uphList = uphService.GetModelList(ParamQuery.Instance().AndWhere("ProductCode", ProductCode).AndWhere("TenantID", TenantID));
                if (uphList.Count != 0)
                {
                    int iStandardUPH = 0;
                    if (uphList[0].StandardUPH!=null)
                    {
                        if (int.TryParse(Convert.ToString(uphList[0].StandardUPH), out iStandardUPH))
                        {
                            UPH = iStandardUPH;
                        }
                       
                    }
                    int iRatio = 0;
                    if (uphList[0].Ratio != null)
                    {
                        if (int.TryParse(Convert.ToString(uphList[0].Ratio), out iRatio))
                        {
                            StandardMould = iRatio; ;
                        }
                    } 

                }

               
                minimes_order_recordService orderrecordService = new minimes_order_recordService();
                List<minimes_order_record> orderrecordList = orderrecordService.GetModelList(ParamQuery.Instance().AndWhere("OrderNo", OrderNo));
                if (orderrecordList.Count != 0)
                { 
                    Ratio = orderrecordList[0].Ratio;
                    UPH = orderrecordList[0].UPH;
                    StandardMould = orderrecordList[0].StandardMould;
                    Person = orderrecordList[0].Person;
                    ActualMould = orderrecordList[0].ActualMould.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            var result = new
            {
                Ratio = Ratio,
                UPH = UPH,
                StandardMould= StandardMould,
                Person = Person,
                ActualMould = ActualMould
            };
            string js = JsonConvert.SerializeObject(result);
            return Json(js, "application/json", JsonRequestBehavior.AllowGet); ;
        }


        /// <summary>
        /// 确认切单
        /// </summary>
        /// <param name="WorkSheetNo">工单号</param>
        /// <param name="UPH">UPH</param>
        /// <param name="Ratio">拼板数</param>
        /// <param name="Person">人数</param>
        /// <param name="PlanPerson">计划人数</param>
        /// <param name="LineId">线体ID</param>
        /// <param name="TenantID">企业ID</param>
        /// <returns></returns>
        public ActionResult OrderOK(string WorkSheetNo, string UPH, string Ratio, string Person, string PlanPerson, string LineId, string TenantID,string ActualMould,string StandardMould)
        {
            string js = "";
            try
            {
                if (!string.IsNullOrEmpty(WorkSheetNo))
                {
                    DateTime OrderPlanDate = DateTime.Now.Date;
                    //List < dynamic > worksheetlist = ApiDataSource.GetWorkSheetList(TenantID, OrderPlanDate, null, WorkSheetNo, null).ToObject<List<dynamic>>();
                    //List<dynamic> worksheetlist = ApiDataSource.GetWorkSheetList(TenantID, WorkSheetNo, null, null, null, null, null, null, null, null).ToObject<List<dynamic>>();
                    //获取工单详细信息

                  
                    List<dynamic> worksheetlist = ApiDataSource.GetWorkSheetDetail(TenantID, WorkSheetNo).ToObject<List<dynamic>>();
                    string OrderNo = worksheetlist[0].WorkSheetNo;
                    string LineName = worksheetlist[0].LineName;
                    string ProductCode = worksheetlist[0].ProductCode;
                    string MaterialCode = worksheetlist[0].MaterialCode;
                    string ProductName = worksheetlist[0].ProductName;
                    DateTime PlanDate = DateTime.Now.Date;
                    int PlanQty = worksheetlist[0].PlanCount;
                    int ReserveQty = worksheetlist[0].BackupsCount;//备存数量
                    

                    string APSLineId = worksheetlist[0].LineId;
                    string APSLineName = worksheetlist[0].LineName;

                    List<dynamic> linelist = ApiDataSource.GetLineList(TenantID, null, null, LineId, null).ToObject<List<dynamic>>();
                    if (linelist != null && linelist.Count > 0)
                    {
                        LineName = linelist[0].LineName;
                    }

                    string WorkShopId = "";
                    string WorkShopName = "";
                    dynamic LineList = ApiDataSource.GetLineList(TenantID, null, null, LineId, null);
                    foreach (var item_line in LineList)
                    {
                        WorkShopId = item_line.WorkShopId;
                        WorkShopName = item_line.WorkShopName;
                    } 
                    minimes_order_recordService reportService = new minimes_order_recordService();
                    using (var db = Db.Context("MiniMes"))
                    {

                        List<minimes_order_record> reportList = reportService.GetModelList(
                       ParamQuery.Instance()
                       //.AndWhere("LineId", LineId)
                       //.AndWhere("OrderPlanDate", OrderPlanDate)
                       .AndWhere("OrderNo", WorkSheetNo)
                       .AndWhere("TenantID", TenantID)
                       );
                        //修改状态
                        db.Update("minimes_order_record")
                        .Column("State", 0)
                        .Where("LineId", LineId)
                        .Execute();
                        if (reportList.Count > 0)//该工单已经生产过
                        {
                            if (!reportList[0].LineId.Equals(LineId))
                            { 
                                db.Update("minimes_hoursreport").Column("LineId", LineId).Column("LineName", LineName).Where("OrderNo", OrderNo).Execute();
                                ApiDataSource.EditWorkSheetCount(TenantID, OrderNo, LineId, LineName);
                                #region 记录换线的工单

                                db.Insert("minimes_changeorderno")
                                         .Column("ID", Guid.NewGuid())
                                         .Column("BeforeLineId", APSLineId)//换线之前产线ID
                                         .Column("BeforeLineName", APSLineName)//换线之前产线名称
                                         .Column("AfterLineId", LineId)//换线之后产线ID
                                         .Column("AfterLineName", LineName)//换线之后产线名称
                                         .Column("OrderNo", OrderNo)//工单号
                                         .Column("CreateDate", DateTime.Now)//创建时间
                                         .Execute();

                                #endregion
                            }

                            db.Update("minimes_order_record")
                            .Column("UPH", UPH)
                            .Column("Ratio", Ratio)
                            .Column("State", 1)
                            .Column("LineId", LineId)
                            .Column("LineName", LineName)
                            .Column("PlanDate", PlanDate)
                            .Column("Person", Person)
                            .Column("PlanPerson", PlanPerson)
                            .Where("ID", reportList[0].ID)
                            .Execute();
                        }
                        else//该工单未生产过
                        {
                            db.Insert("minimes_order_record")
                                 .Column("OrderNo", OrderNo)
                                 .Column("WorkShopId", WorkShopId)//车间ID
                                 .Column("WorkShopName", WorkShopName)//车间名称
                                 .Column("LineId", LineId)
                                 .Column("LineName", LineName)
                                 .Column("ProductCode", ProductCode)
                                 .Column("ProductName", ProductName)
                                 .Column("PlanDate", PlanDate)
                                 .Column("OrderPlanDate", OrderPlanDate)
                                 .Column("UPH", UPH)
                                 .Column("Ratio", Ratio)
                                 .Column("QtyNG", 0)
                                 .Column("Qty", 0)
                                 .Column("PlanQty", PlanQty)
                                 .Column("ReserveQty", ReserveQty)
                                 .Column("StandardMould", StandardMould)//标准模穴
                                 .Column("ActualMould", ActualMould)//实际模穴
                                 .Column("State", 1)
                                 .Column("MachineState", 1)
                                 .Column("StopCount", 0)
                                 .Column("StopTime", 0)
                                 .Column("Person", Person)
                                 .Column("PlanPerson", PlanPerson)
                                 .Column("CreateDate", DateTime.Now)
                                 .Column("TenantID", TenantID)
                                 .Execute();
                        } 
                    }  
                }
                else
                {
                    js = "无产品数据";
                }
            }
            catch (Exception ex)
            {
                js = ex.Message;

                #region log

                ILog log = LogManager.GetLogger("logs");
                log.Error("nizeheng 时间：" + DateTime.Now + "----" + "【工单】：" + WorkSheetNo + " 【异常】：" + ex.Message);

                #endregion
            }
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 判断停止开始状态
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="TenantID"></param>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public ActionResult Show(string LineId, string TenantID, string OrderNo)
        {
            string Data = null;
            minimes_ordertimeService ordertimeService = new minimes_ordertimeService();
            List<minimes_ordertime> ordertimeList = ordertimeService.GetModelList(ParamQuery.Instance().AndWhere("LineId", LineId).AndWhere("OrderNo", OrderNo).AndWhere("TenantID", TenantID).OrderBy(" CreateDate Desc"));
            if (ordertimeList.Count > 0 && ordertimeList[0].EndTime == null)
            {
                Data = "开始";
            }
            else
            {
                Data = "停止";
            }
            var result = new
            {
                Data
            };
            string js = JsonConvert.SerializeObject(result);
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 工单开始
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="TenantId"></param>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public ActionResult StartTime(string LineId, string TenantId, string OrderNo)
        {
            string js = "";
            try
            {
                using (var db = Db.Context("MiniMes"))
                {
                    db.Insert("minimes_ordertime")
                    .Column("ID", Guid.NewGuid())
                    .Column("OrderNo", OrderNo)
                    .Column("LineId", LineId)
                    .Column("StartTime", DateTime.Now)
                    .Column("EndTime", null)
                    .Column("CreateDate", DateTime.Now)
                    .Column("TenantID", TenantId)
                    .Execute();
                }
            }
            catch (Exception ex)
            {
                js = ex.Message;
            }
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 工单暂停
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="TenantId"></param>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public ActionResult EndTime(string LineId, string TenantId, string OrderNo)
        {
            string js = "";
            string EndTime = DateTime.Now.ToString();
            minimes_ordertimeService ordertimeService = new minimes_ordertimeService();
            List<minimes_ordertime> ordertimeList = ordertimeService.GetModelList(ParamQuery.Instance().AndWhere("LineId", LineId).AndWhere("OrderNo", OrderNo).AndWhere("TenantID", TenantId).OrderBy(" CreateDate Desc"));
            try
            {
                using (var db = Db.Context("MiniMes"))
                {
                    db.Update("minimes_ordertime")
                         .Column("EndTime", EndTime)
                         .Where("ID", ordertimeList[0].ID)
                         .Execute();
                }
            }
            catch (Exception ex)
            {
                js = ex.Message;
            }
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 设备
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="TenantId"></param>
        /// <param name="OrderNo"></param>
        /// <param name="State"></param>
        /// <param name="Info"></param>
        /// <returns></returns>
        public ActionResult EquipmentControl(string LineId, string TenantId, string OrderNo, int State, string Info)
        {
            string js = "";
            try
            {
                minimes_commandService commandService = new minimes_commandService();
                CommandApiController CommandController = new CommandApiController();
                CommandController.EditPutCommandData(new { LineId = LineId, LineName = "1", UserId = "1", UserCode = "1", UserName = "1", CommandState = State, CommandModule = "1", CommandInfo = OrderNo, Remark = Info, TenantID = TenantId, GId = Guid.NewGuid().ToString() });

            }
            catch (Exception ex)
            {
                js = ex.Message;
            }
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取产线产能和生产时间
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public ActionResult GetLineQtyAndTime(string LineId, string TenantID)
        {
            ResponseObject result = new ResponseObject();
            //白班开始时间
            string DayShiftBeginDate = DateTime.Now.AddHours(-8).ToString("yyyy-MM-dd") + " 08:00:00";
            //白班结束时间
            string DayShiftEndDate = DateTime.Now.AddHours(-8).ToString("yyyy-MM-dd") + " 20:00:00";
            //晚班开始时间
            string EveningShiftBeginDate = DateTime.Now.AddHours(-8).ToString("yyyy-MM-dd") + " 20:00:00";
            //晚班开始时间
            string EveningShiftEndDate = Convert.ToDateTime(DayShiftBeginDate).AddDays(1).ToString("yyyy-MM-dd hh:mm:ss");
            int DayShiftQty = 0;
            int DayShiftTime = 0;
            int EveningShiftQty = 0;
            int EveningShiftTime = 0;
            List<dynamic> DayShiftList = new List<dynamic>();
            List<dynamic> EveningShiftList = new List<dynamic>();
            try
            {
                string DayShiftSql = string.Format(@" SELECT SUM(Qty) as 'DayShiftQty',round(sum(TIMESTAMPDIFF(SECOND,StartTime,LastTime))/60,2) as 'DayShiftTime' FROM minimes_hoursreport WHERE LineId = '{0}' AND TenantID = '{1}' AND StartTime >= '{2}' AND LastTime <= '{3}'",
                 LineId, TenantID, DayShiftBeginDate, DayShiftEndDate);
                using (var db = Db.Context("MiniMES"))
                {
                    DayShiftList = db.Sql(DayShiftSql).QueryMany<dynamic>();
                }
                if (DayShiftList.Count != 0)
                {
                    DayShiftQty = Convert.ToInt32(DayShiftList[0].DayShiftQty);
                    DayShiftTime = Convert.ToInt32(DayShiftList[0].DayShiftTime);
                }

                string EveningShiftSql = string.Format(@" SELECT SUM(Qty) as 'EveningShiftQty',round(sum(TIMESTAMPDIFF(SECOND,StartTime,LastTime))/60,2) as 'EveningShiftTime' FROM minimes_hoursreport WHERE LineId = '{0}' AND TenantID = '{1}' AND StartTime >= '{2}' AND LastTime <= '{3}'",
                 LineId, TenantID, EveningShiftBeginDate, EveningShiftEndDate);
                using (var db = Db.Context("MiniMES"))
                {
                    EveningShiftList = db.Sql(EveningShiftSql).QueryMany<dynamic>();
                }
                if (EveningShiftList.Count != 0)
                {
                    EveningShiftQty = Convert.ToInt32(EveningShiftList[0].EveningShiftQty);
                    EveningShiftTime = Convert.ToInt32(EveningShiftList[0].EveningShiftTime);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (DayShiftList.Count > 0 && EveningShiftList.Count > 0)
            {
                dynamic rows = new
                {
                    DayShiftQty = DayShiftQty,//白班产能
                    DayShiftTime = DayShiftTime,//白班生产时间
                    EveningShiftQty = EveningShiftQty,//晚班产能
                    EveningShiftTime = EveningShiftTime,//晚班生产时间

                };
                result.status = true;
                result.message = "";
                result.rows = rows;
            }
            else
            {
                result.message = "获取不到数据。";
            }
            string js = JsonConvert.SerializeObject(result);
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 判断状态
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="TenantID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public ActionResult ShowStatus(string LineId, string TenantID, string Type,string OrderNo)
        {
            string ChangeOverData = null;
            string HandleExceptionData = null;
            string MachineMaintenanceData = null;
            minimes_stop_recordService ordertimeService = new minimes_stop_recordService();
            List<minimes_stop_record> stoprecordList = ordertimeService.GetModelList(ParamQuery.Instance().AndWhere("LineId", LineId).AndWhere("OrderNo", OrderNo).AndWhere("TenantID", TenantID).AndWhere("Type", Type).OrderBy("CreateDate Desc"));
            if (stoprecordList.Count > 0 && stoprecordList[0].EndTime == null)
            {
                if (Type == "1")
                {
                    ChangeOverData = "换模结束";
                }
                else if (Type == "2")
                {
                    HandleExceptionData = "处理异常结束";
                }
                else if (Type == "3")
                {
                    MachineMaintenanceData = "机台维修结束";
                }
            }
            else
            {
                if (Type == "1")
                {
                    ChangeOverData = "换模开始";
                }
                else if (Type == "2")
                {
                    HandleExceptionData = "处理异常开始";
                }
                else if (Type == "3")
                {
                    MachineMaintenanceData = "机台维修开始";
                }
            }
            var result = new
            {
                ChangeOverData,
                HandleExceptionData,
                MachineMaintenanceData
            };
            string js = JsonConvert.SerializeObject(result);
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 开始操作
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="TenantId"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public ActionResult StartOperate(string LineId, string TenantId, string Type,string UserName,string OrderNo)
        {
            string js = "";
            string WorkShopId = "";
            string WorkShopName = "";
            string LineName = "";
            List<dynamic> Linelist = ApiDataSource.GetLineList(TenantId, null, null, LineId, null).ToObject<List<dynamic>>();
            if (Linelist.Count > 0 && Linelist != null)
            {
                WorkShopId = Linelist[0].WorkShopId;
                WorkShopName = Linelist[0].WorkShopName;
                LineName = Linelist[0].LineName;
            }
            string Shift = "晚班";
            DateTime NowTime = DateTime.Now;
            //白班开始时间
            DateTime DayShiftBeginDate = Convert.ToDateTime(DateTime.Now.AddHours(-8).ToString("yyyy-MM-dd") + " 08:00:00");
            //白班结束时间
            DateTime DayShiftEndDate = Convert.ToDateTime(DateTime.Now.AddHours(-8).ToString("yyyy-MM-dd") + " 20:00:00");
            if (NowTime > DayShiftBeginDate && NowTime < DayShiftEndDate)
            {
                Shift = "白班";
            }
            try
            {
                using (var db = Db.Context("MiniMes"))
                {
                    db.Insert("minimes_stop_record")
                    .Column("ID", Guid.NewGuid())
                    .Column("WorkShopId", WorkShopId)
                    .Column("WorkShopName", WorkShopName)
                    .Column("LineId", LineId)
                    .Column("LineName", LineName)
                    .Column("OrderNo", OrderNo)
                    .Column("Type", Type)
                    .Column("BeginTime", DateTime.Now)
                    .Column("EndTime", null)
                    .Column("Shift", Shift)
                    .Column("Operator", UserName)
                    .Column("Remark", "")
                    .Column("Date", DateTime.Now.ToString("yyyy-MM-dd"))
                    .Column("CreateDate", DateTime.Now)
                    .Column("TenantID", TenantId)
                    .Execute();
                }
            }
            catch (Exception ex)
            {
                js = ex.Message;
            }
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 结束操作
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="TenantId"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public ActionResult EndOperate(string LineId, string TenantId, string Type,string OrderNo)
        {
            string js = "";
            string EndTime = DateTime.Now.ToString();
            minimes_stop_recordService stop_recordService = new minimes_stop_recordService();
            List<minimes_stop_record> stop_recordList = stop_recordService.GetModelList(ParamQuery.Instance().AndWhere("LineId", LineId).AndWhere("OrderNo", OrderNo).AndWhere("Type", Type).AndWhere("TenantID", TenantId).OrderBy(" CreateDate Desc"));
            try
            {
                using (var db = Db.Context("MiniMes"))
                {
                    db.Update("minimes_stop_record")
                         .Column("EndTime", EndTime)
                         .Where("ID", stop_recordList[0].ID)
                         .Execute();
                }
            }
            catch (Exception ex)
            {
                js = ex.Message;
            }
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public ActionResult GetData(string OrderNo)
        {
            int UPH = 0;
            int Person = 0;
            int Ratio = 1;
            minimes_order_recordService orderrecordService = new minimes_order_recordService();
            List<minimes_order_record> orderrecordList = orderrecordService.GetModelList(ParamQuery.Instance().AndWhere("OrderNo", OrderNo));
            if (orderrecordList.Count != 0)
            {
                UPH = orderrecordList[0].UPH;
                Person = orderrecordList[0].Person;
                Ratio = orderrecordList[0].Ratio;
            }
            var result = new
            {
                UPH = UPH,
                Person = Person,
                Ratio = Ratio
            };
            string js = JsonConvert.SerializeObject(result);
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改切单参数
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="UPH"></param>
        /// <param name="Person"></param>
        /// <param name="Ratio"></param>
        /// <returns></returns>
        public ActionResult EditData(string OrderNo, string UPH, string Person, string Ratio)
        {
            string js = "";
            try
            {
                using (var db = Db.Context("MiniMes"))
                {
                    db.Update("minimes_order_record")
                         .Column("UPH", UPH)
                         .Column("Person", Person)
                         .Column("Ratio", Ratio)
                         .Where("OrderNo", OrderNo)
                         .Execute();
                }
            }
            catch (Exception ex)
            {
                js = ex.Message;
            }
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取模数信息
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public ActionResult GetMouldData(string OrderNo)
        {
            int ActualMould = 0;
            int StandardMould = 0;
            minimes_order_recordService orderrecordService = new minimes_order_recordService();
            List<minimes_order_record> orderrecordList = orderrecordService.GetModelList(ParamQuery.Instance().AndWhere("OrderNo", OrderNo));
            if (orderrecordList.Count != 0)
            {
                ActualMould = orderrecordList[0].ActualMould;
                StandardMould = orderrecordList[0].StandardMould;
            }
            var result = new
            {
                ActualMould = ActualMould,
                StandardMould = StandardMould,
            };
            string js = JsonConvert.SerializeObject(result);
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改模穴数据
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="ActualMould"></param>
        /// <param name="StandardMould"></param>
        /// <returns></returns>
        public ActionResult EditMould(string OrderNo, string ActualMould, string StandardMould)
        {
            string js = "";
            try
            {
                using (var db = Db.Context("MiniMes"))
                {
                    db.Update("minimes_order_record")
                         .Column("ActualMould", ActualMould)
                         .Column("StandardMould", StandardMould)
                         .Where("OrderNo", OrderNo)
                         .Execute();
                }
            }
            catch (Exception ex)
            {
                js = ex.Message;
            }
            return Json(js, "application/json", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取生产时间
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public int GetProductTime(string OrderNo, string TenantID)
        {
            int ProductTime = 0;
            List<dynamic> List = new List<dynamic>();
            string Sql = string.Format(@"SELECT round(sum(TIMESTAMPDIFF(SECOND,StartTime,LastTime))/60,2) as 'ProductTime' FROM minimes_hoursreport WHERE OrderNo = '{0}' AND TenantID = '{1}'",
                OrderNo, TenantID);
            using (var db = Db.Context("MiniMES"))
            {
                List = db.Sql(Sql).QueryMany<dynamic>();
            }
            if (List.Count != 0)
            {
                ProductTime = Convert.ToInt32(List[0].ProductTime);
            }
            return ProductTime;
        }

        /// <summary>
        /// 获取损失时间
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public int GetLostTime(string OrderNo, string TenantID)
        {
            int LostTime = 0;
            List<dynamic> List = new List<dynamic>();
            string Sql = string.Format(@"SELECT round(sum(TIMESTAMPDIFF(SECOND,BeginTime,EndTime))/60,2) as 'LostTime' FROM minimes_stop_record WHERE OrderNo = '{0}' AND TenantID = '{1}'",
                OrderNo, TenantID);
            using (var db = Db.Context("MiniMES"))
            {
                List = db.Sql(Sql).QueryMany<dynamic>();
            }
            if (List.Count != 0)
            {
                LostTime = Convert.ToInt32(List[0].LostTime);
            }
            return LostTime;
        }
    }
}
