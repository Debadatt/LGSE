using System;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.Services.Models
{
    public class PropertyHistoryModel
    {
        public string Notes { get; set; }
        public string FirstRow { get; set; }
        public string SecoundRow { get; set; }
        public DateTime? StatusChangedOn { get; set; }
    }
    public class PropertyStatusHistoryModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string id { get; set; }
        public string status { get; set; }
        public string statusId { get; set; }
        public string subStatus { get; set; }
        public string propertySubStatusMstrsId { get; set; }
        public DateTime? statusChangedOn { get; set; }
        public string notes { get; set; }
        public string roleId { get; set; }
        public string roleName { get; set; }
    }
}
