using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_stationService : ServiceBase<minimes_station>
    {
       
    }

    public class minimes_station : ModelBase
    {
        [PrimaryKey]   
        public string ID { get; set; }
        public string StationCode { get; set; }
        public string StationName { get; set; }
        public string TenantId { get; set; }
    }
}
