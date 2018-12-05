using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class PropertyRequest
    {
        [Required]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "The {0} must be 1 to 50 characters long", MinimumLength = 1)]
        public string MPRN { get; set; }
        public string BuildingName { get; set; }
        public string SubBuildingName { get; set; }
        public string MCBuildingName { get; set; }
        public string MCSubBuildingName { get; set; }
        public string BuildingNumber { get; set; }
        public string PrincipalStreet { get; set; }
        public string DependentStreet { get; set; }
        public string PostTown { get; set; }
        public string LocalityName { get; set; }
        public string DependentLocality { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }
        public bool PriorityCustomer { get; set; }
        public string Zone { get; set; }
        public string Cell { get; set; }
        public string IncidentId { get; set; }
        public string ZoneManagerName { get; set; }
        public string CellManagerName { get; set; }
        //public Nullable<int> Status { get; set; }
    }
}