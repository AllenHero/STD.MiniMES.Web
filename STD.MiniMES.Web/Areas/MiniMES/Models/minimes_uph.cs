using STD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STD.MiniMES.Web.Areas.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_uphService : ServiceBase<minimes_uph>
    {

    }

    public class minimes_uph : ModelBase
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string WorkShopId { get; set; }
        public string WorkShopName { get; set; }
        public string ProductCode { get; set; }
        public int? StandardUPH { get; set; }
        public int? Ratio { get; set; }
        public string PackCount { get; set; }
        public string BoxCount { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreatePerson { get; set; }
        public string TenantID { get; set; }

        public string SingleWeight { get; set; }
    }
}