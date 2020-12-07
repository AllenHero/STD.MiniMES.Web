using STD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_mappingService : ServiceBase<minimes_mapping>
    {

    }
    public class minimes_mapping : ModelBase
    {
        [PrimaryKey]
        public string LineId { get; set; }
        public string LineName { get; set; }
        public string WorkShopId { get; set; }
        public string WorkShopName { get; set; }
        public DateTime? StartTime { get; set; }
    }
}