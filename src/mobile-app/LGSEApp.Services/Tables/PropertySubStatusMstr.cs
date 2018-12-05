using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.Services.Tables
{
    public class PropertySubStatusMstr : BaseParameters
    {
        private int displayOrder;
        [JsonProperty(PropertyName = "displayOrder")]
        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }
        private string propertyStatusMstrsId;
        [JsonProperty(PropertyName = "propertyStatusMstrsId")]
        public string PropertyStatusMstrsId
        {
            get { return propertyStatusMstrsId; }
            set { propertyStatusMstrsId = value; }
        }
        private string subStatus;
        [JsonProperty(PropertyName = "subStatus")]
        public string SubStatus
        {
            get { return subStatus; }
            set { subStatus = value; }
        }
    }
}
