using STD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_barcodeService : ServiceBase<minimes_barcode>
    {

    }

    public class minimes_barcode : ModelBase
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string Barcode { get; set; }
        public DateTime? PlanDate { get; set; }
        public string WorkShopCode { get; set; }
        public string LineCode { get; set; }
        public int? Qty { get; set; }
        public string WorkSheetNo { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? CreateDate { get; set; }
        public string TenantID { get; set; }
    }


}