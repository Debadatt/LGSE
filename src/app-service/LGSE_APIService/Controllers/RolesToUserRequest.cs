using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LGSE_APIService.Controllers
{
    public class RolesToUserRequest
    {
            public List<string> RoleIds { get; set; }
            [Required]
            public string UserId { get; set; }
    }
}