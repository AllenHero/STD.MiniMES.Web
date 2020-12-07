using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_plc_record_ngService : ServiceBase<minimes_plc_record_ng>
    {
    }

    public class minimes_plc_record_ng : ModelBase
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string OrderNO { get; set; }
        public string LineId { get; set; }
        public string PlanDate { get; set; }
        public string LineName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int PlcQty { get; set; }
        public int PlcUpperQty { get; set; }
        public int QtyNG { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
        public string PreTime { get; set; }
        public int IntervalTime { get; set; }
        public string SignID { get; set; }
        public string TenantID { get; set; }
        public string Barcode { get; set; }
    }
}