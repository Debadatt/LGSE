using System;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.Services.Models
{
   public class ChangePasswordModel: TokenModel
    {
        
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
     
    }
}
