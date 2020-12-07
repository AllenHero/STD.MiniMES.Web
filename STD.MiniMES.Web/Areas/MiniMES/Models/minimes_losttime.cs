using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_losttimeService : ServiceBase<minimes_losttime>
    {
       
    }

    public class minimes_losttime : ModelBase
    {
        [PrimaryKey]   
        public int ID { get; set; }
        public string LineCode { get; set; }
        public string Station { get; set; }
        public string Date { get; set; }
        public string ShiftCode { get; set; }

        public string ProductCode { get; set; }
        public string ProductModel { get; set; }
        public int? LostTime { get; set; }
        public int? Person { get; set; }
        public int? AllLostTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? CreateDate { get; set; }
        public string DepartCode { get; set; }
        public string ReasonCode { get; set; }
        public string Remark { get; set; }
        public string TenantID { get; set; }
    }
}
