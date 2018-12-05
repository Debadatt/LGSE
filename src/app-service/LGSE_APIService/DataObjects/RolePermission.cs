using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;

namespace LGSE_APIService.DataObjects
{
    public class RolePermission : EntityData
    {

        public string CreatePermission { get; set; }
        public string ReadPermission { get; set; }
        public string UpdatePermission { get; set; }
        public string DeletePermission { get; set; }
        public string RoleId { get; set; }
        public string FeatureId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public virtual Feature Feature { get; set; }
        public virtual Role Role { get; set; }
    }
}
