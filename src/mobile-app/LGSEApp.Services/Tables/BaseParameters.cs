using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.Services.Tables
{
    public class BaseParameters
    {
        private string id;
        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string createdBy;
        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        private string modifiedBy;
        [JsonProperty(PropertyName = "modifiedBy")]
        public string ModifiedBy
        {
            get { return modifiedBy; }
            set { modifiedBy = value; }
        }
        private DateTime createdAt;
        [JsonProperty(PropertyName = "createdAt")]
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
        }
        private DateTime updatedAt;
        [JsonProperty(PropertyName = "updatedAt")]
        public DateTime UpdatedAt
        {
            get { return updatedAt; }
            set { updatedAt = value; }
        }
        private bool deleted;
        [JsonProperty(PropertyName = "deleted")]
        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }
        [Version]
        public string Version { get; set; }
    }
}
