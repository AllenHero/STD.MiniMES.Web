using STD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STD.MiniMES.Models
{
    [Module("MiniMES")]
    public class minimes_parameterService : ServiceBase<minimes_parameter>
    {

    }

    public class minimes_parameter : ModelBase
    {
        [PrimaryKey]
        public string ID { get; set; }
        public string ParameterCode { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }

        public string TenantID { get; set; }
    }

}