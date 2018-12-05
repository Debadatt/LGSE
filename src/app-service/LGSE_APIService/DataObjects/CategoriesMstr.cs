using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;

namespace LGSE_APIService.DataObjects
{
    public class CategoriesMstr : EntityData
    {
        public string Category { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public virtual ICollection<Incident> Incidents { get; set; }
    }
}
