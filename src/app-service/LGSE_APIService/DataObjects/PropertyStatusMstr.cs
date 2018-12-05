using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LGSE_APIService.DataObjects
{
    public class PropertyStatusMstr : EntityData
    {
        
        public string Status { get; set; }
        public int DisplayOrder { get; set; }
        public string ShortText { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        [JsonIgnore]
        public virtual ICollection<Property> Properties { get; set; }
     
        [JsonIgnore]
        public virtual ICollection<PropertyUserStatus> PropertyUserStatus { get; set; }
        [JsonIgnore]
        public virtual ICollection<RoleStatusMap> RoleStatusMaps { get; set; }
        [JsonIgnore]
        public virtual ICollection<PropertySubStatusMstr> PropertySubStatusMstrs { get; set; }
    }
}
