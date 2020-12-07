using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_dayplanService : ServiceBase<minimes_dayplan>
    {
       
    }

    public class minimes_dayplan : ModelBase
    {
        [PrimaryKey]   
        public string ID { get; set; }
        public string Date { get; set; }
        public string LineCode { get; set; }
        public string ShiftCode { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public int? PlanQty { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }
        public string TenantID { get; set; }
    }
}
