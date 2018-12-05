using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.ResponseObjects
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Organization { get; set; }
        public List<string> Roles { get; set; }
        public string PreferredRole { get; set; }
        public string PreferredRoleId { get; set; }
        public string EmployeeId { get; set; }
        public string EUSR { get; set; }
        public string ContactNo { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsAssigned { get; set; }
        public int Inprogress { get; set; }
        public int TotalAssigned { get; set; }
        public int Completed { get; set; }
        public int AssignedMPRNCount { get; set; }
        public bool IsActive { get; set; }
        public bool IsLoggedIn { get; set; }
        public List<string> Zones { get; set; }
        public List<string> Cells { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActivated { get; set; }
        public string ActivationOTP { get; set; }
            
    }
}