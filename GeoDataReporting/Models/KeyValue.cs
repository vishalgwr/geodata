using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoDataReporting.Models
{
    public class KeyValue
    {
        public string KeyName { get; set; }
        public string Value { get; set; }

        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
    }
}