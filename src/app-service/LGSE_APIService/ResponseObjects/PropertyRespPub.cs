using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.ResponseObjects
{
    public class PropertyRespPub:PropertyRespBase
    {
        public Nullable<System.DateTime> IncidentStartTime { get; set; }
        public Nullable<System.DateTime> IncidentEndTime { get; set; }
        public int NoOfPropsAffected { get; set; }
        public int NoOfPropsCompleted { get; set; }
    }
}