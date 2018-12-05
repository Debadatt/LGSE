using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace LGSE_APIService.RequestObjects
{
    public class RegisterRequest: BaseRequest
    {
        [Required]
        [StringLength(250,ErrorMessage ="The {0} must be 1 to 250 characters long",MinimumLength =1)]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "The {0} must be 1 to 250 characters long", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        //public string OrgId { get; set; }

        [DataType(DataType.Text)]
        public string EmployeeId { get; set; }

        public string RoleId { get; set; }
       
        [DataType(DataType.Text)]
        public string EUSR { get; set; }

        [StringLength(15, ErrorMessage = "The {0} must be 1 to 15 characters long", MinimumLength = 1)]
        [DataType(DataType.PhoneNumber)]
        public string ContactNo { get; set; }

        public bool IsActiveUser { get; set; }
    }
}