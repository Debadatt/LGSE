using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGSE_FunctionsHost
{
    public class IncidentRequest
    {
        public string Id { get; set; }
        public string IncidentId { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public List<PropertyRequest> MPRNs { get; set; }
    }
    public class PropertyRequest
    {
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
    }
}
