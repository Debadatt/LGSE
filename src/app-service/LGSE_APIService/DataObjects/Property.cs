using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LGSE_APIService.DataObjects
{
    public class Property:EntityData
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
        public string StatusId { get; set; }
        public string SubStatusId { get; set; }
        public string CellManagerId { get; set; }
        public string ZoneManagerId { get; set; }

        public Nullable<int> Status { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public Nullable<bool> IsIsolated { get; set; }
        public virtual Incident Incident { get; set; }
        [ForeignKey("StatusId")]
        public virtual PropertyStatusMstr PropertyStatusMstr { get; set; }
        [ForeignKey("SubStatusId")]
        public virtual PropertySubStatusMstr PropertySubStatusMstr { get; set; }
        //[ForeignKey("CellManagerId")]
        //public virtual User CellManager { get; set; }

        //[ForeignKey("ZoneManagerId")]
        //public virtual User ZoneManager { get; set; }

        //public virtual ICollection<PropertyStatusMstr> PropertyStatusMstrs { get; set; }
        //public virtual ICollection<PropertySubStatusMstr> PropertySubStatusMstrs { get; set; }
        public virtual ICollection<PropertyUserStatus> PropertyUserStatus { get; set; }
        public virtual ICollection<PropertyUserMap> PropertyUserMaps { get; set; }
        public virtual ICollection<PropertyUserNote> PropertyUserNotes { get; set; }
    }
}
