using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.ResponseObjects
{
    public class PropertyHistoryResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public string Status { get; set; }
        public string StatusId { get; set; }
        public string SubStatus { get; set; }
        public string PropertySubStatusMstrsId { get; set; }
        public DateTimeOffset? StatusChangedOn { get; set; }
        public string Notes { get; set; }
        public string roleId { get; set; }
        public string roleName { get; set; }
    }
}