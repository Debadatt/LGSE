using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class EngineeringCapacityRequest
    {
        public string IncidentId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}