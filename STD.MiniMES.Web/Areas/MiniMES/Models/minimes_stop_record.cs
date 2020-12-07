using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_stop_recordService : ServiceBase<minimes_stop_record>
    {
        public dynamic GetAllData(string WorkShopId, string LineId, string Shift, string Date)
        {
            dynamic result = null;
            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT  ID,WorkShopId,LineId,Type,BeginTime,EndTime,Shift,Remark,Date,round(TIMESTAMPDIFF(SECOND,BeginTime,EndTime)/60,2) as StopTime");
            sb.Append(" from minimes_stop_record");
            sb.Append(" where 1=1 ");
            if (WorkShopId.Length > 0) sb.AppendFormat(" and  WorkShopId = '{0}' ", WorkShopId);
            if (LineId.Length > 0) sb.AppendFormat(" and LineId = '{0}' ", LineId);
            if (Shift.Length > 0) sb.AppendFormat(" and Shift ='{0}' ", Shift);
            if (Date.Length > 0) sb.AppendFormat(" and Date ='{0}' ", Date);
            result = db.Sql(sb.ToString()).QueryMany<dynamic>();
            return result;
        }
    }

    public class minimes_stop_record : ModelBase
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string WorkShopId { get; set; }
        public string WorkShopName { get; set; }
        public string LineId { get; set; }
        public string LineName { get; set; }
        public string Type { get; set; }
        public string OrderNo { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Shift { get; set; }
        public string Operator { get; set; }
        public string Remark { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? CreateDate { get; set; }
        public string TenantID { get; set; }
    }
}
