using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using STD.Framework.Core;
using STD.MiniMES.Models;
using STD.MiniMES.Web;

namespace STD.MiniMES.Areas.MiniMES.Controllers
{
    [AllowAnonymous]
    [MvcMenuFilter(false)]
    public class productivity_hourController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// liuqiang@m3lean.com
        /// 将datatable转换成json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
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
            dt.Columns.Add("ID");
            dt.Columns.Add("TimeDisplay");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("UPH");
            dt.Columns.Add("Qty");
            dt.Columns.Add("QtyNG");
            dt.Columns.Add("DetailedNG");
            dt.Columns.Add("Remark");
            return dt;
        }

        public ActionResult GetDataSource(string LineId, string TenantId,string Date)
        {
            DataTable dt = GetTable(LineId, TenantId, Date);
            return Json(this.ConvertDataTableToJson(dt), "application/json", JsonRequestBehavior.AllowGet);
        }

        private DataTable GetTable(string LineId, string TenantId, string Date)
        {
            string sql = string.Format(@"select a.ID, a.OrderNo,a.PlanDate,b.TimeDisplay,a.ProductName,c.UPH,a.Qty,a.QtyNG,a.DetailedNG,a.Remark from minimes_hoursreport a 
left join minimes_time b on a.Time=b.Time
left join minimes_order_record c on a.OrderNo=c.OrderNo
where a.LineId = '{0}' and a.PlanDate = '{1}' and a.TenantId = '{2}' and (a.Qty>0 or a.QtyNG>0) order by b.ID; ", LineId, Convert.ToDateTime(Date).Date, TenantId);
            List<dynamic> reportList = new List<dynamic>();
            using (var db = Db.Context("MiniMES"))
            {
                reportList = db.Sql(sql).QueryMany<dynamic>();
            }

            DataTable dt = this.CreateDTTitle();
            foreach (var row in reportList)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = row.ID;
                dr["TimeDisplay"] = row.TimeDisplay + "";
                dr["ProductName"] = row.ProductName + "";
                dr["UPH"] = row.UPH + "";
                dr["Qty"] = row.Qty + "";
                dr["QtyNG"] = row.QtyNG + "";
                dr["DetailedNG"] = row.DetailedNG + "";
                dr["Remark"] = row.Remark + "";
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public ActionResult SaveRemark(string ID, string DetailedNG, string Remark)
        {
            string msg = "";
            try
            {
                using (var db = Db.Context("MiniMes"))

                        db.Update("minimes_hoursreport")
                        .Column("DetailedNG", DetailedNG)
                        .Column("Remark", Remark)
                        .Where("ID", ID)
                        .Execute();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(msg);
            return Json(sb.ToString(), "application/json", JsonRequestBehavior.AllowGet);
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
            row1.CreateCell(0).SetCellValue("时间段");
            row1.CreateCell(1).SetCellValue("生产型号");
            row1.CreateCell(2).SetCellValue("计划产能");
            row1.CreateCell(3).SetCellValue("实际产能");
            row1.CreateCell(4).SetCellValue("不良数");
            row1.CreateCell(5).SetCellValue("不良明细");
            row1.CreateCell(6).SetCellValue("备注");
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(ds.Tables[0].Rows[i]["TimeDisplay"] + "");
                rowtemp.CreateCell(1).SetCellValue(ds.Tables[0].Rows[i]["ProductName"] + "");
                rowtemp.CreateCell(2).SetCellValue(ds.Tables[0].Rows[i]["UPH"] + "");
                rowtemp.CreateCell(3).SetCellValue(ds.Tables[0].Rows[i]["Qty"] + "");
                rowtemp.CreateCell(4).SetCellValue(ds.Tables[0].Rows[i]["QtyNG"] + "");
                rowtemp.CreateCell(5).SetCellValue(ds.Tables[0].Rows[i]["DetailedNG"] + "");
                rowtemp.CreateCell(6).SetCellValue(ds.Tables[0].Rows[i]["Remark"] + "");
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "小时产量.xls");
        }


    }
}
