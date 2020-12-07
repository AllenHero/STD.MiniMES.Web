using STD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_order_recordService : ServiceBase<minimes_order_record>
    {
        public dynamic GetAllData(string LineId, string OrderPlanDate, string OrderNo, string ProductName,string ProductCode)
        {
            dynamic result = null;
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  LineId,LineName,OrderNo,ProductName,ProductCode,PlanDate,OrderPlanDate,Qty,PlanQty,(Qty*100/PlanQty) as RatioQty");
            sb.Append(" from minimes_order_record");
            sb.Append(" where 1=1 ");
            sb.Append(" and  State = 1 ");
            if (LineId.Length > 0) sb.AppendFormat(" and  LineId = '{0}' ", LineId);
            if (OrderPlanDate.Length > 0) sb.AppendFormat(" and OrderPlanDate = '{0}' ", OrderPlanDate);
            if (OrderNo.Length > 0) sb.AppendFormat(" and OrderNo ='{0}' ", OrderNo);
            if (ProductName.Length > 0) sb.AppendFormat(" and ProductName ='{0}' ", ProductName);
            if (ProductCode.Length > 0) sb.AppendFormat(" and ProductCode ='{0}' ", ProductName);
            sb.Append(" GROUP BY LineId");
            result = db.Sql(sb.ToString()).QueryMany<dynamic>();
            return result;
        }

        
    }

    public class minimes_order_record : ModelBase
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string OrderNo { get; set; }
        public string WorkShopId { get; set; }
        public string WorkShopName { get; set; }
        public string LineId { get; set; }
        public string LineName { get; set; }
        public string Color { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public DateTime PlanDate { get; set; }
        public DateTime OrderPlanDate { get; set; }
        public int UPH { get; set; }
        /// <summary>
        /// 单次计数拼版数
        /// </summary>
        public int Ratio { get; set; }
        public double Weight { get; set; }
        public int Qty { get; set; }
        public int QtyNG { get; set; }
        public int PlanQty { get; set; }
        public int State { get; set; }
        public int MachineState { get; set; }
        public string StateName { get; set; }
        public int StopCount { get; set; }
        public int StopTime { get; set; }
        public int Person { get; set; }
        public int PlanPerson { get; set; }
        public DateTime CreateDate { get; set; }

        public string CreateDateStr { get; set; }
        public string TenantID { get; set; }

        /// <summary>
        /// 时间稼动率
        /// </summary>
        public string Utilization { get; set; }
        /// <summary>
        /// 计划完成率
        /// </summary>
        public string RatioQty { get; set; }
        /// <summary>
        /// 实际生产时间
        /// </summary>
        public int ProductTime { get; set; }
        /// <summary>
        /// 差异人数
        /// </summary>
        public int DifferPerson { get; set; }
        /// <summary>
        /// 计划达成率
        /// </summary>
        public string PlanRatio { get; set; }
        public int DifferQty { get; set; }
        /// <summary>
        /// 标准CT
        /// </summary>
        public string PlanCT { get; set; }
        /// <summary>
        /// 实际CT
        /// </summary>
        public string CT { get; set; }
        /// <summary>
        /// 实际UPH
        /// </summary>
        public int CurUPH { get; set; }

        //计划开工时间
        public DateTime PlanStartDate { get; set; }
        //计划完工时间
        public DateTime PlanEndDate { get; set; }
        public DateTime RealStartDate { get; set; }
        public DateTime RealEndDate { get; set; }
        public DateTime RecoverDate { get; set; }
        public DateTime RealStopDate { get; set; }

        /// <summary>
        /// 备存产量
        /// </summary>
        public int ReserveQty { get; set; }

        /// <summary>
        /// 备存计划达成率
        /// </summary>
        public string ReserveRate { get; set; }

        /// <summary>
        /// 实际模穴
        /// </summary>
        public int ActualMould { get; set; }

        /// <summary>
        /// 标准模穴
        /// </summary>
        public int StandardMould { get; set; }

        /// <summary>
        /// 模穴率
        /// </summary>
        public string MouldRate { get; set; }
    }
}