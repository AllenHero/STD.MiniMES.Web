using STD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STD.MiniMES.Web.Areas.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_restdateService : ServiceBase<minimes_restdate>
    {

    }

    public class minimes_restdate : ModelBase
    {
        [PrimaryKey]
        public string ID { get; set; }
        public DateTime? RestDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatePerson { get; set; }
        public string TenantID { get; set; }
    }
}