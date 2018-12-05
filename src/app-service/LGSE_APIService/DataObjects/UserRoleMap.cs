using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;

namespace LGSE_APIService.DataObjects
{
    public class UserRoleMap:EntityData
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }       
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<bool> IsPreferredRole { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
