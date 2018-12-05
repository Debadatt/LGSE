using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.Services.Tables
{
   public class PropertyStatusMstr : BaseParameters
    {
        private int displayOrder;
        [JsonProperty(PropertyName = "displayOrder")]
        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }
        private string status;
        [JsonProperty(PropertyName = "status")]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}
