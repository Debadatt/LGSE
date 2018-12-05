using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class ActivationRequest:BaseRequest
    {
        //[RegularExpression("^[!-/a-zA-Z0-9]{8,8}$",ErrorMessage ="Invalid OTP Code")]
        [Required]
        public string OTPCode { get; set; }

        [Required]
        public string Password { get; set; }
    }
}