
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class UsersToRoleRequest
    {
        [Required]
        public List<string> UserIds { get; set; }
        [Required]
        public string RoleId { get; set; }
    }
}