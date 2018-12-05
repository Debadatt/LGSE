using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LGSE_APIService.DataObjects
{
    public  class PropertyUserStatus:EntityData
    {
      
        public string PropertyId { get; set; }
        public string StatusId { get; set; }
        public string PropertySubStatusMstrsId { get; set; }
        public DateTimeOffset StatusChangedOn { get; set; }
        public string UserId { get; set; }

        public string RoleId { get; set; }
        public string PropertyUserMapsId { get; set; }
        public string Notes { get; set; }
        public string IncidentId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
   

        [JsonIgnore]
        [ForeignKey("PropertySubStatusMstrsId")]
        public virtual PropertySubStatusMstr PropertySubStatusMstr { get; set; }

        [JsonIgnore]
        [ForeignKey("PropertyId")]
        public virtual Property Property { get; set; }
        [JsonIgnore]
        [ForeignKey("StatusId")]
        public virtual PropertyStatusMstr PropertyStatusMstr { get; set; }
        [JsonIgnore]
        [ForeignKey("IncidentId")]
        public virtual Incident Incidents { get; set; }
        [JsonIgnore]
        [ForeignKey("PropertyUserMapsId")]
        public virtual PropertyUserMap PropertyUserMap { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
