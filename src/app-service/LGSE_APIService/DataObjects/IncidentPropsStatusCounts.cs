using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.DataObjects
{
    public class IncidentPropsStatusCounts : EntityData
    {
      
        public string IncidentId { get; set; }
        public int NS { get; set; }
        public int NA { get; set; }
        public int NC { get; set; }
        public int IS { get; set; }
        public int RS { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public virtual Incident Incident { get; set; }
       

    }
}