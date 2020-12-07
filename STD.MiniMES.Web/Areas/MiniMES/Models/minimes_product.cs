using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_productService : ServiceBase<minimes_product>
    {
       
    }

    public class minimes_product : ModelBase
    {
        [PrimaryKey]
        public string InventoryId { get; set; }
        public string TenantID { get; set; }
        public int? TallyRatio { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryName { get; set; }
        public string InventoryClassId { get; set; }
        public int? StandardUPH { get; set; }
        public int? StandardPersonNum { get; set; }
        public int? StandardProcessNum { get; set; }
        public int? StandardTime { get; set; }
        public string Spec { get; set; }
        public string Unit { get; set; }
        public int? Seq { get; set; }
        public DateTime? CreateTime { get; set; }
        public string IsEnable { get; set; }
    }
}
