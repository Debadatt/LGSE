using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class IncidentRequest
    {
        public string Id { get; set; }
        public string IncidentId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [StringLength(128, ErrorMessage = "The {0} must be 1 to 250 characters long", MinimumLength = 1)]
        public string CategoriesMstrId { get; set; }

        public string Description { get; set; }
        public string Notes { get; set; }
        //public Nullable<int> NoOfPropsAffected { get; set; }
        //public Nullable<int> NoOfZones { get; set; }
        //public Nullable<int> NoOfCells { get; set; }

        //public Nullable<System.DateTime> StartTime { get; set; }
        //public Nullable<System.DateTime> EndTime { get; set; }

        //public Nullable<int> Status { get; set; }
        //public string ClosingNotes { get; set; }
        
        //public Nullable<int> NoOfPropsIsolated { get; set; }
        //public Nullable<int> NoOfPropsRestored { get; set; }
        public List<PropertyRequest> MPRNs { get; set; }
        public List<ResolveUser> ResolveUsers { get; set; }
    }
    public class ResolveUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}