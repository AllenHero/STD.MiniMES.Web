using STD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_product_updateService : ServiceBase<minimes_product_update>
    {

    }

    public class minimes_product_update : ModelBase
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public int? TallyRatio { get; set; }
        public string UserCode { get; set; }
        public string TenantID { get; set; }
        public string UpdateUser { get; set; }
        public string UpdateTime { get; set; }
    }
}