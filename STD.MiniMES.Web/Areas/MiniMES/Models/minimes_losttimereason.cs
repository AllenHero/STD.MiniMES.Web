using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_losttimereasonService : ServiceBase<minimes_losttimereason>
    {
       
    }

    public class minimes_losttimereason : ModelBase
    {
        [PrimaryKey]   
        public string ID { get; set; }
        public string DepartCode { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string TenantID { get; set; }
    }
}
