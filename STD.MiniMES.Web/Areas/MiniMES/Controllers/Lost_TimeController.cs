using STD.Framework.Core;
using STD.MiniMES.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    [AllowAnonymous]
    [MvcMenuFilter(false)]
    public class Lost_TimeController : Controller
    {
        //
        // GET: /MiniMES/Lost_Time/

        public ActionResult Index()
        {
            return View();
        }


        private string ConvertDataTableToJson(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            if (dt == null || dt.Rows.Count == 0)
            {
                //如果查询到的数据为空则返回标记ok:false
                sb.Append("{\"ok\":false}");
            }
            else
            {
                sb.Append("{\"ok\":true,");
                for (int j = 0; j < 1; j++)
                {
                    sb.Append(string.Format("\"{0}\":[", dt.TableName));

                    foreach (DataRow dr in dt.Rows)
                    {
                        sb.Append("{");
                        for (int i = 0; i < dr.Table.Columns.Count; i++)
                        {
                            sb.AppendFormat("\"{0}\":\"{1}\",", dr.Table.Columns[i].ColumnName.Replace("\"", "\\\"").Replace("\'", "\\\'"), dr[i].ToString().Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                        }
                        sb.Remove(sb.ToString().LastIndexOf(','), 1);
                        sb.Append("},");
                    }

                    sb.Remove(sb.ToString().LastIndexOf(','), 1);
                    sb.Append("],");
                }
                sb.Remove(sb.ToString().LastIndexOf(','), 1);
                sb.Append("}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// liuqiang@m3lean.com
        /// '小时产量'kanban数据源表头
        /// </summary>
        /// <returns></returns>
        private DataTable CreateDTTitle()
        {
            DataTable dt = new DataTable("DataTable");
            dt.Columns.Add("LineName");
            dt.Columns.Add("sTime");
            dt.Columns.Add("eTime");
            dt.Columns.Add("StopCount");
            dt.Columns.Add("StopTime");
            dt.Columns.Add("MAXStop");
            return dt;
        }

        public ActionResult GetLost_Time(string LineId, string TenantId, string sDate,string eDate)
        {
            DataTable dt = GetTable(LineId, TenantId, sDate,eDate);
            return Json(this.ConvertDataTableToJson(dt), "application/json", JsonRequestBehavior.AllowGet);
        }

        private DataTable GetTable(string LineId, string TenantId, string sDate, string eDate)
        {
            string sql = string.Format(@"select LineName,round(sum(TIMESTAMPDIFF(SECOND,StartTime,EndTime))/60,2) as StopTime,
count(*) as StopCount,round(max(TIMESTAMPDIFF(SECOND,StartTime,EndTime))/60,2) as MAXStop  
from minimes_status_record 
where LineId='{0}' and State=0
and PlanDate BETWEEN '{1}' and '{2}' 
and TenantId='{3}'
GROUP BY LineName; ", LineId, Convert.ToDateTime(sDate).Date, Convert.ToDateTime(eDate).Date, TenantId);
            List<dynamic> reportList = new List<dynamic>();
            using (var db = Db.Context("MiniMES"))
            {
                reportList = db.Sql(sql).QueryMany<dynamic>();
            }

            DataTable dt = this.CreateDTTitle();
            foreach (var row in reportList)
            {
                DataRow dr = dt.NewRow();
                dr["LineName"] = row.LineName + "";
                dr["sTime"] = Convert.ToDateTime(sDate).ToString("yyyy-MM-dd");
                dr["eTime"] = Convert.ToDateTime(eDate).ToString("yyyy-MM-dd");
                dr["StopCount"] = row.StopCount + "";
                dr["StopTime"] = row.StopTime + "";
                dr["MAXStop"] = row.MAXStop + "";
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="LineCode"></param>
        /// <param name="Date"></param>
        /// <param name="Station"></param>
        /// <param name="Shift"></param>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        public FileResult Export(string LineId, string sDate, string eDate, string TenantId)
        {
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //获取list数据
            var dt = this.GetTable(LineId, TenantId, sDate, eDate);
            var ds = new DataSet();
            ds.Tables.Add(dt);
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("线体");
            row1.CreateCell(1).SetCellValue("开始时间");
            row1.CreateCell(2).SetCellValue("结束时间");
            row1.CreateCell(3).SetCellValue("停机次数");
            row1.CreateCell(4).SetCellValue("停机时间（min）");
            row1.CreateCell(5).SetCellValue("最长停机时间（min）");
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(ds.Tables[0].Rows[i]["LineName"] + "");
                rowtemp.CreateCell(1).SetCellValue(ds.Tables[0].Rows[i]["sTime"] + "");
                rowtemp.CreateCell(2).SetCellValue(ds.Tables[0].Rows[i]["eTime"] + "");
                rowtemp.CreateCell(3).SetCellValue(ds.Tables[0].Rows[i]["StopCount"] + "");
                rowtemp.CreateCell(4).SetCellValue(ds.Tables[0].Rows[i]["StopTime"] + "");
                rowtemp.CreateCell(5).SetCellValue(ds.Tables[0].Rows[i]["MAXStop"] + "");
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "损失报表.xls");
        }

    }
}
