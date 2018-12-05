using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.ResponseObjects
{
    public class PropertyResponse: PropertyRespBase
    {
      
        public bool isMPRNAssigned { get; set; }
        public int AssignedResourceCount { get; set; }
        public bool? IsStatusUpdated { get; set; }
        public bool IsLastStatusUpdate { get; set; }
        // For Engineer and Isolator this flags tells that its Unassigned...
        public bool IsUnassigned { get; set; }
        public DateTimeOffset? StatusChangedOn { get; set; }
        public int NotesCount { get; set; }

        public string ZoneManagerName { get; set; }
        public string CellManagerName { get; set; }
    }
}