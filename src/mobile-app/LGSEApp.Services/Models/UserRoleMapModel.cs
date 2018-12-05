using System;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.Services.Models
{
   public class UserRoleMapModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; }
    }
}
