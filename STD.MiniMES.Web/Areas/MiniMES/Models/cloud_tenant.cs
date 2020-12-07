using STD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STD.MiniMES.Models
{
    [Module("CMP")]
    public class cloud_tenantService : ServiceBase<cloud_tenant>
    {
    }

    public class cloud_tenant : ModelBase
    {
        [PrimaryKey]
        public string TenantId { get; set; }
        public string TenantCode { get; set; }
        public string TenantName { get; set; }
        public string PhoneNumber { get; set; }
        public string Contacts { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string FaxNumber { get; set; }
        public string Memo { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string IsEnable { get; set; }
        public string TenantPsd { get; set; }
        public string StartEnableTime { get; set; }
        public string EndEnableTime { get; set; }
        public string PayMode { get; set; }

    }
}