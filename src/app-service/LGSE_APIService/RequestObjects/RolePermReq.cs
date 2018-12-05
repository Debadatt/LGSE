using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class RolePermReq
    {
        public string RoleId { get; set; }
        public string FeatureId { get; set; }
        public bool CreatePermission { get; set; }
        public bool ReadPermission { get; set; }
        public bool UpdatePermission { get; set; }
    }
}