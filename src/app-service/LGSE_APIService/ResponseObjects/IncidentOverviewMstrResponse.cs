using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.ResponseObjects
{
    public class IncidentOverviewMstrResponse
    {
        public string Id { get; set; }
        public string Description { get; set; }
      
        public bool IsActive { get; set; }
    }
}