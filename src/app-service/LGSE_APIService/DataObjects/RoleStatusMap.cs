using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LGSE_APIService.DataObjects
{
    public class RoleStatusMap:EntityData
    {
        public string RoleId { get; set; }
        public string StatusId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        [JsonIgnore]
        [ForeignKey("StatusId")]
        public virtual PropertyStatusMstr PropertyStatusMstr { get; set; }
        [JsonIgnore]
        public virtual Role Role { get; set; }
    }
}
