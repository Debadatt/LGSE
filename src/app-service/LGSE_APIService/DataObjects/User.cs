using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LGSE_APIService.DataObjects
{
    public class User:EntityData
    {


        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DomainId { get; set; }
        public string EmployeeId { get; set; }
        public string EUSR { get; set; }
        public string ContactNo { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActivated { get; set; }
        public bool IsLocked { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public bool IsTermsAccepted { get; set; }
        public DateTimeOffset? PwdStartDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string OTPCode { get; set; }
        public DateTimeOffset? OTPGeneratedAt { get; set; }
        public bool IsActiveUser { get; set; }
        public bool IsLoggedIn { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        //[TableColumn(TableColumnType.UpdatedAt)]
        //public new DateTimeOffset? UpdatedAt { get; set; }

        [JsonIgnore]
        public virtual Domain Domain { get; set; }
        [JsonIgnore]
        public virtual ICollection<IncidentHistory> IncidentHistories { get; set; }
        [JsonIgnore]
        public virtual ICollection<PropertyUserMap> PropertyUserMaps { get; set; }
        [JsonIgnore]
        public virtual ICollection<PropertyUserNote> PropertyUserNotes { get; set; }
        [JsonIgnore]
        public virtual ICollection<PropertyUserStatus> PropertyUserStatus { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserRoleMap> UserRoleMap { get; set; }

        //public virtual ICollection<Property> Properties { get; set; }

    }
}
