using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.ResponseObjects
{
    public class UserResponseSub
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
       // public List<RoleResponseSub> Roles { get; set; }
    }
    public class RoleResponseSub
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    }
}