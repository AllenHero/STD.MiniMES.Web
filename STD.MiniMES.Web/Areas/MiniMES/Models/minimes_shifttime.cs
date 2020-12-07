using STD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STD.MiniMES.Web.Areas.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_shifttimeService : ServiceBase<minimes_shifttime>
    {

    }

    public class minimes_shifttime : ModelBase
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string WorkShopId { get; set; }
        public string WorkShopName { get; set; }
        public string ProductionDate { get; set; }
        public double? ShiftTime1 { get; set; }
        public double? ShiftTime2 { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatePerson { get; set; }
        public string TenantID { get; set; }
    }
}