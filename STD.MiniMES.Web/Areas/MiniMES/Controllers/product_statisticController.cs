using Newtonsoft.Json;
using STD.Framework.Core;
using STD.MiniMES.Models;
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
    public class product_statisticController : Controller
    {
        //
        // GET: /MiniMES/product_statistic/

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult GetProductStatisticData(string LineId, string TenantId, string Date)
        {
            DataTable dt = GetTable(LineId, TenantId, Date);
            return Json(this.ConvertDataTableToJson(dt), "application/json", JsonRequestBehavior.AllowGet);
        }

        private DataTable GetTable(string LineId, string TenantId, string Date)
        {
            string sql = string.Format(@"select OrderNo,ProductCode,PlanDate,ProductName,sum(Qty) as Qty,sum(QtyNG) as QtyNG,PlanQty,TTQty,DetailedNG,Remark FROM 
(select a.ID, a.OrderNo,a.ProductCode,a.PlanDate,b.TimeDisplay,a.ProductName,a.Qty,a.QtyNG,sum(c.PlanQty) as PlanQty,sum(c.Qty) as TTQty,a.DetailedNG,a.Remark from minimes_hoursreport a 
left join minimes_time b on a.Time=b.Time
left join minimes_order_record c on a.OrderNo=c.OrderNo and a.LineId=c.LineId and a.TenantId=c.TenantId
where a.LineId = '{0}' and a.PlanDate = '{1}' and a.TenantId = '{2}' 
group by a.ID, a.OrderNo,a.ProductCode,a.PlanDate,b.TimeDisplay,a.ProductName,a.Qty,a.QtyNG,a.DetailedNG,a.Remark) d
group by OrderNo,ProductCode,PlanDate,ProductName,PlanQty,TTQty,DetailedNG,Remark; ", LineId, Convert.ToDateTime(Date).Date, TenantId);
            List<dynamic> reportList = new List<dynamic>();
            using (var db = Db.Context("MiniMES"))
            {
                reportList = db.Sql(sql).QueryMany<dynamic>();
            }

            DataTable dt = this.CreateDTTitle();
            foreach (var row in reportList)
            {
                int PlanQty = 0;
                int TTQty = 0;
                try
                {
                    PlanQty = Convert.ToInt32(row.PlanQty);
                    TTQty = Convert.ToInt32(row.TTQty);
                }
                catch
                { }
                DataRow dr = dt.NewRow();
                dr["PlanDate"] = (Convert.ToDateTime(row.PlanDate)).ToString("yyyy-MM-dd");
                dr["OrderNo"] = row.OrderNo + "";
                dr["ProductCode"] = row.ProductCode + "";
                dr["PlanQty"] = row.PlanQty + "";
                dr["Qty"] = row.Qty + "";
                dr["TTQty"] = row.TTQty + "";
                if (PlanQty == 0)
                    dr["Rate"] = 0;
                else
                    dr["Rate"] = Math.Round((decimal)TTQty*100 / PlanQty, 2);
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private DataTable CreateDTTitle()
        {
            DataTable dt = new DataTable("DataTable");
            dt.Columns.Add("PlanDate");
            dt.Columns.Add("OrderNo");
            dt.Columns.Add("ProductCode");
            dt.Columns.Add("PlanQty");
            dt.Columns.Add("Qty");
            dt.Columns.Add("TTQty");
            dt.Columns.Add("Rate");
            return dt;
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
        /// 导出
        /// </summary>
        /// <param name="LineCode"></param>
        /// <param name="Date"></param>
        /// <param name="Station"></param>
        /// <param name="Shift"></param>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        public FileResult Export(string LineId, string Date, string TenantId)
        {
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //获取list数据
            var dt = this.GetTable(LineId, TenantId, Date);
            var ds = new DataSet();
            ds.Tables.Add(dt);
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("日期");
            row1.CreateCell(1).SetCellValue("工单号");
            row1.CreateCell(2).SetCellValue("产品编号");
            row1.CreateCell(3).SetCellValue("工单数量");
            row1.CreateCell(4).SetCellValue("当天产出");
            row1.CreateCell(5).SetCellValue("累加产出");
            row1.CreateCell(6).SetCellValue("生产达成率");
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(ds.Tables[0].Rows[i]["PlanDate"] + "");
                rowtemp.CreateCell(1).SetCellValue(ds.Tables[0].Rows[i]["OrderNo"] + "");
                rowtemp.CreateCell(2).SetCellValue(ds.Tables[0].Rows[i]["ProductCode"] + "");
                rowtemp.CreateCell(3).SetCellValue(ds.Tables[0].Rows[i]["PlanQty"] + "");
                rowtemp.CreateCell(4).SetCellValue(ds.Tables[0].Rows[i]["Qty"] + "");
                rowtemp.CreateCell(5).SetCellValue(ds.Tables[0].Rows[i]["TTQty"] + "");
                rowtemp.CreateCell(6).SetCellValue(ds.Tables[0].Rows[i]["Rate"] + "");
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "生产统计.xls");
        }
    }
}
