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
    public class CapacityController : Controller
    {
        public ActionResult Index()
        {
            List<dynamic> LineList = new List<dynamic>();
            dynamic Lines = ApiDataSource.GetLineList(SysHelper.GetTenantId(), null, null, null, null);
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
                    query = "/api/MiniMES/Capacity",
                },
                resx = new
                {
                    noneSelect = "请先选择一条数据！",
                    editSuccess = "保存成功！",
                    auditSuccess = "单据已审核！"
                },
                form = new
                {
                    LineId = "",
                    WorkShopId="",
                    StartTime = ""
                },
                defaultRow = new
                {
                },
                setting = new
                {
                    idField = "",
                    postListFields = new string[] { }
                }
            };
            return View(model);
        }
    }

    public class CapacityApiController : ApiController
    {
        public dynamic Get(RequestWrapper query)
        {
            try
            {
                query.LoadSettingXmlString(@"
            <settings defaultOrderBy='StartTime DESC'>
                <select>DISTINCT LineId,LineName,WorkShopName,WorkShopId</select>
                <from>minimes_hoursreport</from>
                <where defaultForAll='true' defaultCp='equal' defaultIgnoreEmpty='true' >
                     <field name='LineId'		cp='equal'></field>    
                     <field name='StartTime'		cp='DateRange'></field>   
                     <field name='WorkShopId'		cp='equal'></field>   
                </where>
            </settings>");
                var service = new minimes_hoursreportService();
                var pQuery = query.ToParamQuery();
                string StartTime = query["StartTime"];
                if (string.IsNullOrEmpty(StartTime))
                {
                    StartTime = DateTime.Now.ToString("yyyy-MM-dd"); 
                }
                var result = service.GetDynamicListWithPaging(pQuery);
                foreach (var item in result.rows)
                {
                    using (var db = Db.Context("MiniMES"))
                    {
                        //白班开始时间
                        string DayShiftBeginDate =Convert.ToDateTime(StartTime).AddDays(1).AddHours(-8).ToString("yyyy-MM-dd") + " 08:00:00";
                        //白班结束时间
                        string DayShiftEndDate = Convert.ToDateTime(StartTime).AddDays(1).AddHours(-8).ToString("yyyy-MM-dd") + " 20:00:00";
                        //晚班开始时间
                        string EveningShiftBeginDate = Convert.ToDateTime(StartTime).AddDays(1).AddHours(-8).ToString("yyyy-MM-dd") + " 20:00:00";
                        //晚班开始时间
                        string EveningShiftEndDate = Convert.ToDateTime(DayShiftBeginDate).AddDays(1).ToString("yyyy-MM-dd hh:mm:ss");
                        int DayShiftQty = 0;
                        int DayShiftTime = 0;
                        int DayShiftStopCount = 0;
                        int DayShiftStopTime = 0;
                        int EveningShiftQty = 0;
                        int EveningShiftTime = 0;
                        int EveningShiftStopCount = 0;
                        int EveningShiftStopTime = 0;
                        List<dynamic> DayShiftList = new List<dynamic>();
                        List<dynamic> DayShiftList1 = new List<dynamic>();
                        List<dynamic> EveningShiftList = new List<dynamic>();
                        List<dynamic> EveningShiftList1 = new List<dynamic>();

                        #region 白班

                        ((IDictionary<string, object>)item).Add("StartTime", StartTime);

                        //班别
                        ((IDictionary<string, object>)item).Add("DayShift", "白班");
                        string DayShiftSql = string.Format(@" SELECT SUM(Qty) as 'DayShiftQty',round(sum(TIMESTAMPDIFF(SECOND,StartTime,LastTime))/60,2) as 'DayShiftTime' FROM minimes_hoursreport WHERE LineId = '{0}' AND StartTime >= '{1}' AND LastTime <= '{2}'", item.LineId, DayShiftBeginDate, DayShiftEndDate);
                        DayShiftList = db.Sql(DayShiftSql).QueryMany<dynamic>();
                        if (DayShiftList.Count != 0)
                        {
                            DayShiftQty = Convert.ToInt32(DayShiftList[0].DayShiftQty);
                            DayShiftTime = Convert.ToInt32(DayShiftList[0].DayShiftTime);
                        }
                        //产能
                        ((IDictionary<string, object>)item).Add("DayQty", DayShiftQty);
                        //生产时间
                        ((IDictionary<string, object>)item).Add("DayTime", DayShiftTime);
                        string DayShiftSql1 = string.Format(@"select round(sum(TIMESTAMPDIFF(SECOND,StartTime,EndTime))/60,2) as DayShiftStopTime,count(*) as DayShiftStopCount  from minimes_status_record where LineId='{0}' and State=0 and StartTime>='{1}' and EndTime<= '{2}'GROUP BY LineName;", item.LineId, DayShiftBeginDate, DayShiftEndDate);
                        DayShiftList1 = db.Sql(DayShiftSql1).QueryMany<dynamic>();
                        if (DayShiftList1.Count != 0)
                        {
                            DayShiftStopCount = Convert.ToInt32(DayShiftList1[0].DayShiftStopCount);
                            DayShiftStopTime = Convert.ToInt32(DayShiftList1[0].DayShiftStopTime);
                        }
                        //停机次数
                        ((IDictionary<string, object>)item).Add("DayStopCount", DayShiftStopCount);
                        //停机时间
                        ((IDictionary<string, object>)item).Add("DayStopTime", DayShiftStopTime);

                        #endregion

                        #region 晚班

                        //班别
                        ((IDictionary<string, object>)item).Add("EveningShift", "晚班");
                        string EveningShiftSql = string.Format(@" SELECT SUM(Qty) as 'EveningShiftQty',round(sum(TIMESTAMPDIFF(SECOND,StartTime,LastTime))/60,2) as 'EveningShiftTime' FROM minimes_hoursreport WHERE LineId = '{0}' AND StartTime >= '{1}' AND LastTime <= '{2}'", item.LineId, EveningShiftBeginDate, EveningShiftEndDate);
                        EveningShiftList = db.Sql(EveningShiftSql).QueryMany<dynamic>();
                        if (EveningShiftList.Count != 0)
                        {
                            EveningShiftQty = Convert.ToInt32(EveningShiftList[0].EveningShiftQty);
                            EveningShiftTime = Convert.ToInt32(EveningShiftList[0].EveningShiftTime);
                        }
                        //产能
                        ((IDictionary<string, object>)item).Add("EveningQty", EveningShiftQty);
                        //生产时间
                        ((IDictionary<string, object>)item).Add("EveningTime", EveningShiftTime);
                        string EveningShiftSql1 = string.Format(@"select round(sum(TIMESTAMPDIFF(SECOND,StartTime,EndTime))/60,2) as EveningShiftStopTime,count(*) as EveningShiftStopCount  from minimes_status_record where LineId='{0}' and State=0  and StartTime>='{1}' and EndTime<= '{2}'GROUP BY LineName;", item.LineId, EveningShiftBeginDate, EveningShiftEndDate);
                        EveningShiftList1 = db.Sql(EveningShiftSql1).QueryMany<dynamic>();
                        if (EveningShiftList1.Count != 0)
                        {
                            EveningShiftStopCount = Convert.ToInt32(EveningShiftList1[0].EveningShiftStopCount);
                            EveningShiftStopTime = Convert.ToInt32(EveningShiftList1[0].EveningShiftStopTime);
                        }
                        //停机次数
                        ((IDictionary<string, object>)item).Add("EveningStopCount", EveningShiftStopCount);
                        //停机时间
                        ((IDictionary<string, object>)item).Add("EveningStopTime", EveningShiftStopTime);

                        #endregion
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
