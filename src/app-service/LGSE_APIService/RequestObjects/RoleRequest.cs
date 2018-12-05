using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class RoleRequest
    {
        [Required]
        [StringLength(250, ErrorMessage = "The {0} must be 1 to 250 characters long", MinimumLength = 1)]
        public string RoleName { get; set; }
        [StringLength(2000, ErrorMessage = "The {0} must be below 2000 characters")]
        public string Description { get; set; }
    }
}