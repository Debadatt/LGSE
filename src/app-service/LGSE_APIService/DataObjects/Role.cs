using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LGSE_APIService.DataObjects
{
    public class Role:EntityData
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        [JsonIgnore]
        public virtual ICollection<RoleStatusMap> RoleStatusMaps { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserRoleMap> UserRoleMap { get; set; }
      
    }
}
