using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.DataObjects
{
    public class AuditTrial: EntityData
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string DeviceId { get; set; }
        public string TypeofOperation { get; set; }
        public string Status { get; set; }
        public string TokenId { get; set; }
        public DateTimeOffset? OperationTimeStamp { get; set; }
        public string IPAddress { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}