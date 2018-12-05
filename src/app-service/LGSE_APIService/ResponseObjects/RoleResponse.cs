using LGSE_APIService.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.ResponseObjects
{
    public class RoleResponse
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        //public List<RoleUsersResponse> Users { get; set; }
    }
    public class RoleUsersResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}