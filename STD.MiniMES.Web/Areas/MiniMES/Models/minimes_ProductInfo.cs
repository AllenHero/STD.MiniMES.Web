using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_ProductInfoService : ServiceBase<minimes_ProductInfo>
    {
       
    }

    public class minimes_ProductInfo : ModelBase
    {
        [PrimaryKey]   
        public string ID { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public int? UPH { get; set; }

        public string TenantID { get; set; }

    }
}
