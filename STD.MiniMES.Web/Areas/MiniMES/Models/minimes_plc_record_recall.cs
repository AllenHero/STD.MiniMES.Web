using STD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_plc_record_recallService : ServiceBase<minimes_plc_record_recall>
    {
    }

    public class minimes_plc_record_recall : ModelBase
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string OrderNO { get; set; }
        public string LineId { get; set; }
        public string PlanDate { get; set; }
        public int Qty { get; set; }
        public string CreateDate { get; set; }

    }

}