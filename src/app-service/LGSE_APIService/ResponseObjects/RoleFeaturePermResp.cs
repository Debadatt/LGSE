using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.ResponseObjects
{
    public class RoleFeaturePermResp
    {
        public string FeatureText { get; set; }
        public string FeatureName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string FeatureId { get; set; }
        public bool CreatePermission { get; set; }
        public bool ReadPermission { get; set; }
        public bool UpdatePermission { get; set; }
        //public string DeletePermission { get; set; }
    }
  
}