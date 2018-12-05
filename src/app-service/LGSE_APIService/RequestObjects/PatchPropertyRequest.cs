using LGSE_APIService.ResponseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class PatchPropertyRequest : PropertyRespBase
    {

        public bool isMPRNAssigned { get; set; }
        public int AssignedResourceCount { get; set; }
        public bool? IsStatusUpdated { get; set; }
        public bool IsLastStatusUpdate { get; set; }
        // For Engineer and Isolator this flags tells that its Unassigned...
        public bool IsUnassigned { get; set; }
        public DateTimeOffset? StatusChangedOn { get; set; }
        public int NotesCount { get; set; }
    }
}