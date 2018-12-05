using System;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.Models
{
   public class PropertyModel
    {
        public string Id { get; set; }
        public string MPRN { get; set; }
        public string BuildingName { get; set; }
        public string SubBuildingName { get; set; }
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
        public int Status { get; set; }
    }
}
