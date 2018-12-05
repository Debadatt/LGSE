using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.Services.Models
{
   public class TokenModel: CustomeMessage
    {
        public string token { get; set; }
       
        public string username { get; set; }      
      
        public string userId { get; set; }

        public string RoleId { get; set; }
    }
}
