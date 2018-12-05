using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class PropertyCountsRequest
    {
        public string MPRN { get; set; }
        public string Zone { get; set; }
        public string Cell { get; set; }
    }
}