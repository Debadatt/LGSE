using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class UserEditRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeId { get; set; }
        public string EUSR { get; set; }
        public string ContactNo { get; set; }
        public bool IsActiveUser { get; set; }
    }
}