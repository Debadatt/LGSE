using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class AssignMPRNsRequest
    {
        [Required]
        public string PropertyId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string RoleId { get; set; }
        public bool IsUnAssign { get; set; }
    }
}