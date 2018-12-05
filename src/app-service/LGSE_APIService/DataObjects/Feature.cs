using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;

namespace LGSE_APIService.DataObjects
{
    public class Feature : EntityData
    {
        public string FeatureName { get; set; }
        public string FeatureText { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
