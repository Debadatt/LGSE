using System;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.Services.Models
{
  
    public class ForgotPasswordModel
    {
        public string EmailId { get; set; }
        public string OtpCode { get; set; }
        public string Password { get; set; }
    }
}
