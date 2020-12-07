using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_losttimesetService : ServiceBase<minimes_losttimeset>
    {
       
    }

    public class minimes_losttimeset : ModelBase
    {
        [PrimaryKey]   
        public string ID { get; set; }
        public string LineCode { get; set; }
        public string Station { get; set; }
        public int? WhenLong { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
