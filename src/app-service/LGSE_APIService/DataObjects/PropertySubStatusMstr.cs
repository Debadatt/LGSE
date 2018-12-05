using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LGSE_APIService.DataObjects
{
    public class PropertySubStatusMstr : EntityData
    {
        public string SubStatus { get; set; }
        public string PropertyStatusMstrsId { get; set;}
        public int DisplayOrder { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        [JsonIgnore]
        public virtual PropertyStatusMstr PropertyStatusMstr { get; set; }
    }
}
