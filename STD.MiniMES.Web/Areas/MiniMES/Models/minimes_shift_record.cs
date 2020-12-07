using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_shift_recordService : ServiceBase<minimes_shift_record>
    {
        
    }

    public class minimes_shift_record : ModelBase
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string LineId { get; set; }
        public string LineName { get; set; }
        public DateTime? Date { get; set; }
        public string Shift { get; set; }
        public int Qty { get; set; }
        public string TenantID { get; set; }
    }
}
