using STD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_order_record_stateService : ServiceBase<minimes_order_record_state>
    {

    }

    public class minimes_order_record_state : ModelBase
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string OrderNo { get; set; }
        public string LastState { get; set; }
        public string NowState { get; set; }
        public string LineId { get; set; }
        public string TenantID { get; set; }
        public string UserID { get; set; }
        public DateTime CreateDate { get; set; }
      
    }

}