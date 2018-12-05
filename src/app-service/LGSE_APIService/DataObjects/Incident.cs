using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LGSE_APIService.DataObjects
{
    public  class Incident:EntityData
    {
        public string IncidentId { get; set; }
        public string CategoriesMstrId { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public Nullable<int> NoOfPropsAffected { get; set; }
        public Nullable<int> NoOfZones { get; set; }
        public Nullable<int> NoOfCells { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<int> Status { get; set; }
        public string ClosingNotes { get; set; }
        public Nullable<int> NoOfPropsIsolated { get; set; }
        public Nullable<int> NoOfPropsRestored { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        [ForeignKey("CategoriesMstrId")]
        [JsonIgnore]
        public virtual CategoriesMstr CategoriesMstr { get; set; }
        [JsonIgnore]
        public virtual ICollection<IncidentHistory> IncidentHistories { get; set; }
        [JsonIgnore]
        public virtual ICollection<Property> Properties { get; set; }
        [JsonIgnore]
        public virtual ICollection<IncidentPropsStatusCounts> IncidentPropsStatusCounts { get; set; }
    }
}
