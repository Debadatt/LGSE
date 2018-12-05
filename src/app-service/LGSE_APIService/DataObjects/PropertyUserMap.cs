using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LGSE_APIService.DataObjects
{
    public  class PropertyUserMap:EntityData
    {
        public string UserId { get; set; }
        public string PropertyId { get; set; }
        public string RoleId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        //[NotMapped]
        //public new DateTimeOffset? UpdatedAt { get; set; }
        public virtual Property Property { get; set; }
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual ICollection<PropertyUserStatus> PropertyUserStatus { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
