using System;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.Services.Models
{
    public class UserModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrgId { get; set; }
        public string EmployeeId { get; set; }
        public string RoleId { get; set; }
        public List<RoleModel> roles { get; set; }
        public string EUSR { get; set; }
        public string ContactNo { get; set; }

    }
    public class UserProfileModel
    {
        public string Email { get; set; }
        public string FirtName { get; set; }
        public string LastName { get; set; }
        public string OrgId { get; set; }
        public string EmployeeId { get; set; }
        public string RoleId { get; set; }
        public List<RoleModel> roles { get; set; }
        public string EUSR { get; set; }
        public string ContactNo { get; set; }

    }
}
