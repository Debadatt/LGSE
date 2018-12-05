using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class PreferredRoleRequest
    {
        [Required]
        public string RoleId { get; set; }
    }
}