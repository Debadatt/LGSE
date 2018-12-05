using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LGSE_APIService.RequestObjects
{
    public class PropertyUserStatusRequest
    {

       
        [Required]
        public string PropertyId { get; set; }
        [Required]
        public string StatusId { get; set; }
        public string PropertySubStatusMstrsId { get; set; }
        //public string UserId { get; set; }
        public string Notes { get; set; }
    }
}