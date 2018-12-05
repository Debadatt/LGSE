using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;

namespace LGSE_APIService.DataObjects
{
    public class IncidentHistory:EntityData
    {
        public string UserId { get; set; }
        public string Incidentid { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public virtual Incident Incident { get; set; }
        public virtual User User { get; set; }
    }
}
