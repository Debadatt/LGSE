using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class LoginRequest:BaseRequest
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}