using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;

namespace LGSE_APIService.DataObjects
{

    public  class Domain:EntityData
    {
        public string OrgName { get; set; }
        public string DomainName { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public string ModifiedBy { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
