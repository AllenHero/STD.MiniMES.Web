using STD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_order_record_qtyService : ServiceBase<minimes_order_record_qty>
    {

    }

    public class minimes_order_record_qty : ModelBase
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string OrderNo { get; set; }
        public int LastNGQty { get; set; }
        public int ChangeNGQty { get; set; }
        public int NowNGQty { get; set; }

        public int LastTotalQty { get; set; }
        public int ChangeTotalQty { get; set; }
        public int NowTotalQty { get; set; }
        public string LineId { get; set; }
        public string TenantID { get; set; }

        public string UserID { get; set; }
        public DateTime CreateDate { get; set; }
      
    }

}