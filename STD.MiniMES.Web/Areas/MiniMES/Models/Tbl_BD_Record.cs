using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STD.MiniMES.Models
{
    public class Tbl_BD_Record
    {
        public string ID { get; set; }
        public string BarCode { get; set; }
        public string OrderNo { get; set; }
        public DateTime PlanDate { get; set; }
        public string LineId { get; set; }
        public int Ratio { get; set; }
        public string LineName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PlcQty { get; set; }
        public string PlcUpperQty { get; set; }
        public string Qty { get; set; }
        public string PlanQty { get; set; }
        public string CreateDate { get; set; }
        public string PreTime { get; set; }
        public string IntervalTime { get; set; }
        public string TenantId { get; set; }



    }

}
