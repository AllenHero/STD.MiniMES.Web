using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_personService : ServiceBase<minimes_person>
    {
       
    }

    public class minimes_person : ModelBase
    {
        [PrimaryKey]   
        public string ID { get; set; }
        public string LineCode { get; set; }
        public string Date { get; set; }
        public string Station { get; set; }
        public decimal? Person { get; set; }
        public DateTime? CreateDate { get; set; }
        public string TenantID { get; set; }
    }
}
