using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.DataObjects
{
    public class IncidentOverviewMstr:EntityData
    {
        public string Description { get; set; }
        public string DefaultText { get; set; }
        public bool IsActive { get; set; }
    }
}