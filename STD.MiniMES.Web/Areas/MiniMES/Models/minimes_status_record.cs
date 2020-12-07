using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_status_recordService : ServiceBase<minimes_status_record>
    {
        public dynamic GetData(string WorkShopId,string TenantID)
        {
          
            string sql = "";
            //7
            sql += " SELECT '"+ DateTime.Now.AddDays(-6).ToString("dd") + "'as Date, (SELECT IFNULL(round(sum(TIMESTAMPDIFF(SECOND, StartTime, EndTime)) / 60,2), 0) FROM minimes_status_record WHERE WorkShopId = '" + WorkShopId + "' and TenantID='" + TenantID + "' and PlanDate = '" + DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd") + "' and State = 0) AS StopTime,(SELECT IFNULL(round(sum(TIMESTAMPDIFF(SECOND, StartTime, EndTime)) / 60,2), 0) FROM minimes_status_record WHERE WorkShopId = '" + WorkShopId + "' and TenantID='" + TenantID + "' and PlanDate = '" + DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd") + "' and State = 1)AS ProduceTime";

            sql += " UNION ALL";

            //6
            sql += " SELECT '" + DateTime.Now.AddDays(-5).ToString("dd") + "'as Date, (SELECT IFNULL(round(sum(TIMESTAMPDIFF(SECOND, StartTime, EndTime)) / 60,2), 0) FROM minimes_status_record WHERE WorkShopId = '" + WorkShopId + "' and TenantID='" + TenantID + "' and PlanDate = '" + DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd") + "' and State = 0) AS StopTime,(SELECT IFNULL(round(sum(TIMESTAMPDIFF(SECOND, StartTime, EndTime)) / 60,2), 0) FROM minimes_status_record WHERE WorkShopId = '" + WorkShopId + "' and TenantID='" + TenantID + "' and PlanDate = '" + DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd") + "' and State = 1)AS ProduceTime";

            sql += " UNION ALL";

            //5
            sql += " SELECT '" + DateTime.Now.AddDays(-4).ToString("dd") + "'as Date, (SELECT IFNULL(round(sum(TIMESTAMPDIFF(SECOND, StartTime, EndTime)) / 60,2), 0) FROM minimes_status_record WHERE WorkShopId = '" + WorkShopId + "' and TenantID='" + TenantID + "' and PlanDate = '" + DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd") + "' and State = 0) AS StopTime,(SELECT IFNULL(round(sum(TIMESTAMPDIFF(SECOND, StartTime, EndTime)) / 60,2), 0) FROM minimes_status_record WHERE WorkShopId = '" + WorkShopId + "' and TenantID='" + TenantID + "' and PlanDate = '" + DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd") + "' and State = 1)AS ProduceTime";

            sql += " UNION ALL";

            //4
            sql += " SELECT '" + DateTime.Now.AddDays(-3).ToString("dd") + "'as Date, (SELECT IFNULL(round(sum(TIMESTAMPDIFF(SECOND, StartTime, EndTime)) / 60,2), 0) FROM minimes_status_record WHERE WorkShopId = '" + WorkShopId + "' and TenantID='" + TenantID + "' and PlanDate = '" + DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd") + "' and State = 0) AS StopTime,(SELECT IFNULL(round(sum(TIMESTAMPDIFF(SECOND, StartTime, EndTime)) / 60,2), 0) FROM minimes_status_record WHERE WorkShopId = '" + WorkShopId + "' and TenantID='" + TenantID + "' and PlanDate = '" + DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd") + "' and State = 1)AS ProduceTime";

            sql += " UNION ALL";

            //3
            sql += " SELECT '" + DateTime.Now.AddDays(-2).ToString("dd") + "'as Date, (SELECT IFNULL(round(sum(TIMESTAMPDIFF(SECOND, StartTime, EndTime)) / 60,2), 0) FROM minimes_status_record WHERE WorkShopId = '" + WorkShopId + "' and TenantID='" + TenantID + "' and PlanDate = '" + DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") + "' and State = 0) AS StopTime,(SELECT IFNULL(round(sum(TIMESTAMPDIFF(SECOND, StartTime, EndTime)) / 60,2), 0) FROM minimes_status_record WHERE WorkShopId = '" + WorkShopId + "' and TenantID='" + TenantID + "' and PlanDate = '" + DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") + "' and State = 1)AS ProduceTime";

            sql += " UNION ALL";

            //2
            sql += " SELECT '" + DateTime.Now.AddDays(-1).ToString("dd") + "'as Date, (SELECT IFNULL(round(sum(TIMESTAMPDIFF(SECOND, StartTime, EndTime)) / 60,2), 0) FROM minimes_status_record WHERE WorkShopId = '" + WorkShopId + "' and TenantID='" + TenantID + "' and PlanDate = '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "' and State = 0) AS StopTime,(SELECT IFNULL(round(sum(TIMESTAMPDIFF(SECOND, StartTime, EndTime)) / 60,2), 0) FROM minimes_status_record WHERE WorkShopId = '" + WorkShopId + "' and TenantID='" + TenantID + "' and PlanDate = '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "' and State = 1)AS ProduceTime";

            sql += " UNION ALL";


            //1
            sql += " SELECT '" + DateTime.Now.ToString("dd") + "'as Date, (SELECT IFNULL(round(sum(TIMESTAMPDIFF(SECOND, StartTime, EndTime)) / 60,2), 0) FROM minimes_status_record WHERE WorkShopId = '" + WorkShopId + "' and TenantID='"+ TenantID + "' and PlanDate = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and State = 0) AS StopTime,(SELECT IFNULL(round(sum(TIMESTAMPDIFF(SECOND, StartTime, EndTime)) / 60,2), 0) FROM minimes_status_record WHERE WorkShopId = '" + WorkShopId + "' and TenantID='" + TenantID + "' and PlanDate = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and State = 1)AS ProduceTime";
           
            List<dynamic> List = db.Sql(sql).QueryMany<dynamic>();
            return List;
        }
    }


    public class minimes_status_record : ModelBase
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string WorkShopId { get; set; }
        public string WorkShopName { get; set; }
        public string LineId { get; set; }
        public string LineName { get; set; }
        public string State { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TenantID { get; set; }
    }
}