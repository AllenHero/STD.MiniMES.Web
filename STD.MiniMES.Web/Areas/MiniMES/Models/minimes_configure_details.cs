using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;
using STD.MiniMES.Web;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_configure_detailsService : ServiceBase<minimes_configure_details>
    {
        
    }

    public class minimes_configure_details : ModelBase
    {
        [PrimaryKey]   
        public string ID { get; set; }
        public string Date { get; set; }
        public string LineCode { get; set; }
        public int? UPH { get; set; }
        public string Station { get; set; }

        public string ProductCode { get; set; }
        public string ProductModel { get; set; }
        public int? Ratio { get; set; }
        public string OrderNO { get; set; }
        public int? Time { get; set; }
        public int? Qty { get; set; }
        public int? State { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? CreateDate { get; set; }
        public string TenantID { get; set; }
    }
}
