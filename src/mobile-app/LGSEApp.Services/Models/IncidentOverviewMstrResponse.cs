using System;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.Services.Models
{
    public class IncidentOverviewMstrResponse
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
