using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_hoursreportService : ServiceBase<minimes_hoursreport>
    {
        public dynamic GetAllData(string LineId, string StartTime)
        {
            dynamic result = null;
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  distinct LineId,LineName");
            sb.Append(" from minimes_hoursreport");
            sb.Append(" where 1=1 ");
            if (LineId.Length > 0) sb.AppendFormat(" and  LineId = '{0}' ", LineId);
            if (StartTime.Length > 0) sb.AppendFormat(" and StartTime like '%{0}%' ", StartTime);
            result = db.Sql(sb.ToString()).QueryMany<dynamic>();
            return result;
        }
    }

    public class minimes_hoursreport : ModelBase
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string OrderNO { get; set; }
        public string PlanDate { get; set; }
        public string WorkShopId { get; set; }
        public string WorkShopName { get; set; }
        public string LineId { get; set; }
        public string LineName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Time { get; set; }
        public int QtyNG { get; set; }
        public int Qty { get; set; }
        public int WorkMinute { get; set; }
        public int WorkMinuteMax { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime LastTime { get; set; }
        public string TenantId { get; set; }
        public string DetailedNG { get; set; }
        public string Remark { get; set; }
    }
}
