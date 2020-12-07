using System;
using System.Collections.Generic;
using System.Text;
using STD.Framework.Core;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_commandlogService : ServiceBase<minimes_commandlog>
    {

    }

    public class minimes_commandlog : ModelBase
    {
        public int AID { get; set; }
        public int ID { get; set; }
        public string LineId { get; set; }
        public string LineName { get; set; }
        public string UserCode { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int CommandState { get; set; }
        public string CommandModule { get; set; }
        public string CommandInfo { get; set; }
        public string Remark { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LogDate { get; set; }
        public DateTime PlanDate { get; set; }
        public string TenantID { get; set; }

        public string GId { get; set; }
    }
}