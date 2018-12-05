using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.ResponseObjects
{
    public class PropertyRespBase
    {
        public string MPRN { get; set; }
        public string BuildingName { get; set; }
        public string SubBuildingName { get; set; }
        public string BuildingNumber { get; set; }
        public string MCBuildingName { get; set; }
        public string MCSubBuildingName { get; set; }
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
        public string IncidentName { get; set; }

        public string CellManagerId { get; set; }
        public string ZoneManagerId { get; set; }

        public Nullable<int> Status { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public string LatestStatus { get; set; }
        public string LatestStatusId { get; set; }
        public string LatestSubStatus { get; set; }
        public string LatestSubStatusId { get; set; }
        public string Notes { get; set; }
        public string Id { get; set; }
        public string PropertyId { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Nullable<bool> IsIsolated { get; set; }
    }
}