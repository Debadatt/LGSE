using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class UserWiseStatusRequest
    {
        public string IncidentId { get; set; }
        public string CellName { get; set; }
    }
}