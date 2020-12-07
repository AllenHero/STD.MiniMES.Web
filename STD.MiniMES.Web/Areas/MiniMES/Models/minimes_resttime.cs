using STD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_resttimeService : ServiceBase<minimes_resttime>
    {

    }
    public class minimes_resttime : ModelBase
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string WorkShopId { get; set; }
        public string RestTime { get; set; }
        public DateTime? CreateDate { get; set; }
        public string TenantId { get; set; }
    }
}