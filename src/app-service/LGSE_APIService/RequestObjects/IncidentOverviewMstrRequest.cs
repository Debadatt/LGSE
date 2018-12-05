using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class IncidentOverviewMstrRequest
    {
       
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}