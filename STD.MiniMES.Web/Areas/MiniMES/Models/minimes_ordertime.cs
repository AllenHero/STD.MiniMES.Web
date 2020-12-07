using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_ordertimeService : ServiceBase<minimes_ordertime>
    {
       
    }

    public class minimes_ordertime : ModelBase
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string OrderNo { get; set; }
        public string LineId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? CreateDate { get; set; }
        public string TenantID { get; set; }
    }
}
