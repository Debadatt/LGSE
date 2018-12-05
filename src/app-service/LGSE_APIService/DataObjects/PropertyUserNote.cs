using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;

namespace LGSE_APIService.DataObjects
{
    public  class PropertyUserNote:EntityData
    {
       
        public string Notes { get; set; }
        public string UserId { get; set; }
        public string PropertyId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public virtual Property Property { get; set; }
        public virtual User User { get; set; }
    }
}
