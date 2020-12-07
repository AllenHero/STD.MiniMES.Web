using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_weight_recordService : ServiceBase<minimes_weight_record>
    {
    }

    public class minimes_weight_record : ModelBase
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string OrderNO { get; set; }
        public string LineId { get; set; }
        public string PlanDate { get; set; }
        public string LineName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public double Weight { get; set; }
        public string CreateDate { get; set; }
        public string TenantID { get; set; }
        public string IP { get; set; }

    }
}