using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.Services.Tables
{
    public class PropertyUserStatus : BaseParameters
    {
        private string notes;
        [JsonProperty(PropertyName = "notes")]
        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }
        private string userId;
        [JsonProperty(PropertyName = "userId")]
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        private DateTime statusChangedOn;
        [JsonProperty(PropertyName = "statusChangedOn")]
        public DateTime StatusChangedOn
        {
            get { return statusChangedOn; }
            set { statusChangedOn = value; }
        }
        private string propertySubStatusMstrsId;
        [JsonProperty(PropertyName = "propertySubStatusMstrsId")]
        public string PropertySubStatusMstrsId
        {
            get { return propertySubStatusMstrsId; }
            set { propertySubStatusMstrsId = value; }
        }
        private string statusId;
        [JsonProperty(PropertyName = "statusId")]
        public string StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        private string propertyId;
        [JsonProperty(PropertyName = "propertyId")]
        public string PropertyId
        {
            get { return propertyId; }
            set { propertyId = value; }
        }
        private string roleId;
        [JsonProperty(PropertyName = "roleId")]
        public string RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }
        private string propertyUserMapsId;
        [JsonProperty(PropertyName = "propertyUserMapsId")]
        public string PropertyUserMapsId
        {
            get { return propertyUserMapsId; }
            set { propertyUserMapsId = value; }
        }
        
    }
}
