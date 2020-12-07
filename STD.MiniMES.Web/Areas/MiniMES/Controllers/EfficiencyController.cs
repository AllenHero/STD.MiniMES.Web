using STD.Framework.Core;
using STD.Framework.Utils;
using STD.Framework.Utils.EPPlus;
using STD.Framework.Utils.EPPlus.Style;
using STD.MiniMES.Models;
using STD.MiniMES.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    public class EfficiencyController : Controller
    {
        public ActionResult Index()
        {
            List<dynamic> LineList = new List<dynamic>();
            dynamic Lines = ApiDataSource.GetLineList(SysHelper.GetTenantId(), null,null,null,null);
            for (int i = 0; i < Lines.Count; i++)
            {
                LineList.Add(new { value = Lines[i].LineId.Value, text = Lines[i].LineName.Value });
            }

            List<dynamic> WorkShopList = new List<dynamic>();
            dynamic WorkShops = ApiDataSource.GetWorkShopList(SysHelper.GetTenantId(), null);
            for (int i = 0; i < WorkShops.Count; i++)
            {
                WorkShopList.Add(new { value = WorkShops[i].WorkShopId.Value, text = WorkShops[i].WorkShopName.Value });
            }
            var model = new
            {
                dataSource = new
                {
                    dsLineName = LineList,
                    dsWrokShopName = WorkShopList,
                },
                urls = new
                {
                    query = "/api/MiniMES/Efficiency",
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    WorkShopId = "",
                    LineId = "",
                    OrderPlanDate = "",
                    OrderNo = "",
                    ProductName = "",
                    ProductCode="",
                },
                defaultRow = new
                {
                },
                setting = new
                {
                    idField = "ID",
                    postListFields = new string[] { }
                }
            };
            return View(model);
        }

        public ActionResult ReportIndex()
        {
            //var code = new linepatrol_codeService();
            var model = new
            {
                dataSource = new
                {
                    dsLine = ApiDataSource.GetLineList(SysHelper.GetTenantId(), null, null, null, null),

                },
                urls = new
                {
                    query = "/api/MiniMES/Efficiency/GetReport",
                    newkey = "/api/MiniMES/Efficiency/getnewkey",
                    edit = "/api/MiniMES/Efficiency/edit"
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    OrderNo = "",
                    StartTime = DateTime.Now.ToString("yyyy-MM-dd"),
                    LineId = "",
                    ProductCode = "",
                    ProductName = "",


                },
                defaultRow = new
                {

                },
                setting = new
                {
                    idField = "WorkNO",
                    postListFields = new string[] { "OrderNo", "StartTime", "LineId", "LineName", "ProductCode", "ProductName", "PlanCount", "SumCount", "NGCount", "NGRate" }
                }
            };

            return View(model);
        }
    }

    public class EfficiencyApiController : ApiController
    {
        LineBoardController lineboardController = new LineBoardController();
        public dynamic Get(RequestWrapper query)
        {
            try
            {
                query.LoadSettingXmlString(@"
            <settings defaultOrderBy='OrderPlanDate DESC'>
                <select>WorkShopName,WorkShopId,LineId,LineName,OrderNo,ProductName,ProductCode,
                PlanDate,OrderPlanDate,Qty,PlanQty,ActualMould,StandardMould,StopTime,TenantID, UPH</select>
                <from> minimes_order_record</from>
                <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
                     <field name='WorkShopId'		cp='equal'></field>   
                     <field name='LineId'		cp='equal'></field>   
                     <field name='OrderPlanDate'		cp='DateRange'></field>   
                     <field name='OrderNo'		cp='equal'></field>   
                     <field name='ProductName'		cp='equal'></field>
                     <field name='ProductCode'		cp='equal'></field>   
                </where>
            </settings>");
                var service = new minimes_order_recordService();
                var pQuery = query.ToParamQuery();
                var result = service.GetDynamicListWithPaging(pQuery);
                foreach (var item in result.rows)
                {
                    #region 时间稼动率&实际UPH&生产效率

                    //生产时间
                    int ProductTime = lineboardController.GetProductTime(item.OrderNo, item.TenantID) - lineboardController.GetLostTime(item.OrderNo, item.TenantID);
                    double Utilization = 0;
                    if (ProductTime!=0)
                    {
                        Utilization =Math.Round((double)(ProductTime - item.StopTime) * 100 / (ProductTime),2);
                    }
                    //时间稼动率
                    ((IDictionary<string, object>)item).Add("Utilization", Utilization);

                    //生产时间
                    ((IDictionary<string, object>)item).Add("ProductTime", ProductTime);

                    double PlanRatio = 0;
                    int CurUPH = 0;
                    if (ProductTime!=0)
                    {
                        CurUPH = item.Qty / ProductTime * 60;
                    }
                    if (item.UPH > 0)
                    {
                        PlanRatio = Math.Round((double)CurUPH * 100 / item.UPH, 2);
                    }
                    //实际UPH
                    ((IDictionary<string, object>)item).Add("CurUPH",CurUPH);

                    //生产效率
                    ((IDictionary<string, object>)item).Add("PlanRatio", PlanRatio);

                    #endregion 

                    #region 计划达成率&备存数量&备存计划达成率

                    double RatioQty = 0;
                    double ReserveRate = 0;
                    int ReserveQty = 0;

                    if (item.Qty<=item.PlanQty)
                    {
                        RatioQty = Math.Round((double)item.Qty * 100 / item.PlanQty, 2);
                        ReserveRate = 0;
                    }
                    else
                    {
                        RatioQty = 100;
                        if (item.Qty> item.PlanQty)//当实际产量>计划产量时，才有备存数量
                        {
                            ReserveQty = item.Qty - item.PlanQty;
                        }
                        ReserveRate = Math.Round((double)ReserveQty * 100 / item.PlanQty, 2);
                    }
                    //计划达成率
                    ((IDictionary<string, object>)item).Add("RatioQty", RatioQty);

                    //备存数量
                    ((IDictionary<string, object>)item).Add("ReserveQty", ReserveQty);

                    //备存计划达成率
                    ((IDictionary<string, object>)item).Add("ReserveRate", ReserveRate);

                    #endregion 

                    #region 模穴率

                    //模穴率
                    double MouldRate = 0;
                    if (item.StandardMould!=0)
                    {
                        MouldRate= Math.Round((double)item.ActualMould * 100 / item.StandardMould, 2);
                    }
                    ((IDictionary<string, object>)item).Add("MouldRate", MouldRate);

                    #endregion
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }  
        }

        public dynamic GetReport(RequestWrapper query)
        {
            query.LoadSettingXmlString(@"
            <settings defaultOrderBy='OrderNo ASC'>
                <select>OrderNo,DATE_FORMAT(StartTime, '%Y-%m-%d') StartTime,LineId,LineName,Max(ProductCode) ProductCode,Max(ProductName) ProductName,'0' PlanCount,SUM(Qty) SumCount,'0' NGCount,'' NGRate</select>
                <from>minimes_hoursreport</from>
                <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' > 
                    <field name='minimes_hoursreport.StartTime'		cp='DateRange'></field>   
                    <field name='minimes_hoursreport.LineId'		cp='equal'></field> 
                    <field name='minimes_hoursreport.ProductCode'		cp='equal'></field>   
                    <field name='minimes_hoursreport.ProductName'		cp='equal'></field>   
                    <field name='minimes_hoursreport.OrderNo'		cp='equal'></field>   
                </where> 
            </settings>");
            var pQuery = query.ToParamQuery();
            pQuery.GroupBy("DATE_FORMAT(StartTime,'%Y-%m-%d'),LineName,LineId,OrderNo ");
            pQuery.OrderBy("DATE_FORMAT(StartTime,'%Y-%m-%d'),LineName,LineId,OrderNo");
            var service = new minimes_hoursreportService();
            var result = service.GetDynamicListWithPaging(pQuery);
            if (result != null && result.rows != null && result.rows.Count != 0)
            {
                ParamQuery orderparam = new ParamQuery();
                string orderWhere = string.IsNullOrEmpty(pQuery.GetData().WhereSql) ? "1=1" : pQuery.GetData().WhereSql;

                orderparam.Select(" minimes_order_record.OrderNo,minimes_order_record.PlanQty").From("minimes_hoursreport join minimes_order_record on minimes_hoursreport.OrderNo=minimes_order_record.OrderNo").AndWhere(orderWhere + " and 1", "1");
                var orderList = service.GetDynamicList(orderparam);
                List<dynamic> allcheck = new List<dynamic>();
                using (var db = Db.Context("QualityControl"))
                {
                    ParamQuery paramQuery = new ParamQuery();
                   
                    if (!string.IsNullOrEmpty(query["StartTime"]))
                    {
                        paramQuery.AndWhere("WorkDate", query["StartTime"], Cp.DateRange);
                    }
                    string where = string.IsNullOrEmpty(paramQuery.GetData().WhereSql) ? "1=1" : paramQuery.GetData().WhereSql;
                    string sql = $"SELECT WorkNO, DATE_FORMAT(WorkDate, '%Y-%m-%d') WorkDate, LineCode,  MAX(ProductCode) ProductCode,   MAX(ProductName) ProductName, ProductType,  MAX(PlanCount) PlanCount, SUM(ItemCount) ItemCount  from allproductcheck where CheckType=1 AND {where}  GROUP BY WorkNO, LineCode, ProductType, DATE_FORMAT(WorkDate, '%Y-%m-%d') order by DATE_FORMAT(WorkDate, '%Y-%m-%d'),WorkNO  ";
                    allcheck = db.Sql(sql).QueryMany<dynamic>();

                } 

                List<dynamic> dsLine = ApiDataSource.GetLineList(SysHelper.GetTenantId(), null, null, null, null).ToObject<List<dynamic>>();
                foreach (var item in result.rows)
                {
                    string OrderNo = item.OrderNo;
                    string StartTime = item.StartTime;
                    string LineId = item.LineId;
                    string LineCode = "";
                    if (dsLine != null && dsLine.Count > 0)
                    {
                        List<dynamic> line = dsLine.FindAll(o => (Convert.ToString(o.LineId).Equals(LineId)));
                        if (line != null && line.Count > 0)
                        {
                            LineCode = line[0].LineCode; 
                        }
                    }
                    double PlanCount = 0;
                    double SumCount = 0;
                    double NGCount = 0;
                    string NGRate = "0%";

                    double.TryParse(Convert.ToString(item.SumCount),out SumCount);
                    if (!string.IsNullOrEmpty(OrderNo) && allcheck != null && allcheck.Count > 0)
                    {
                        List<dynamic> findCheck = allcheck.FindAll(o => (Convert.ToString(o.WorkNO).Equals(OrderNo) && Convert.ToString(o.LineCode).Equals(LineCode) && Convert.ToString(o.WorkDate).Equals(StartTime) && Convert.ToString(o.ProductType).Equals("0")));
                        if (findCheck != null && findCheck.Count > 0)
                        {
                            NGCount = Convert.ToDouble(findCheck[0].ItemCount);
                        }
                    }

                    if (!string.IsNullOrEmpty(OrderNo) && orderList != null && orderList.Count > 0)
                    {
                        List<dynamic> findCheck = orderList.FindAll(o => (Convert.ToString(o.OrderNo).Equals(OrderNo)));
                        if (findCheck != null && findCheck.Count > 0)
                        {
                            PlanCount = Convert.ToDouble(findCheck[0].PlanQty);
                        }
                    }

                    item.PlanCount = PlanCount;
                    item.SumCount = SumCount;
                    item.NGCount = NGCount;
                    if (SumCount != 0 && NGCount != 0)
                    {
                        item.NGRate = ((NGCount / SumCount) * 100).ToString("0.00") + "%";
                    }
                    else
                    {
                        item.NGRate = NGRate;
                    }


                }
            }
            return result;
        }
    }
}
