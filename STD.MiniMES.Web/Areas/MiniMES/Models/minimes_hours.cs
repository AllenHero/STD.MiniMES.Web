using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_hoursService : ServiceBase<minimes_hours>
    {

    }

    public class minimes_hours : ModelBase
    {
        [PrimaryKey]
        public string ID { get; set; }
        public int Time { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TenantId { get; set; }
    }
}
