using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_recordService : ServiceBase<minimes_record>
    {
       
    }

    public class minimes_record : ModelBase
    {
        [PrimaryKey]   
        public string ID { get; set; }
        public int? PlcQty { get; set; }
        public int? PlcUpperQty { get; set; }
        public int? Qty { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ShiftCode { get; set; }
        public string LineCode { get; set; }
        public string Station { get; set; }

        public string ProductCode { get; set; }

        public string ProductModel { get; set; }

        public string ConfigureID { get; set; }
        public string PKID { get; set; }
        public int? NGCurQty { get; set; }
        public int? NGUpperQty { get; set; }
        public int? NGQty { get; set; }
        public string OrderNO { get; set; }
        public DateTime? PreTime { get; set; }
        public int? IntervalTime { get; set; }
        public string TenantID { get; set; }
    }
}
