using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class DomainRequest
    {
        [Required]
        [StringLength(250, ErrorMessage = "The {0} must be 1 to 250 characters long", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string OrgName { get; set; }
        //[Required]
        //[StringLength(250, ErrorMessage = "The {0} must be 1 to 250 characters long", MinimumLength = 1)]
        //[DataType(DataType.Text)]
        public string DomainName { get; set; }
        public string IsActive { get; set; }
    }
}