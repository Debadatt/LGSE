using System;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.Services.Services
{
    public static class Constants
    {
        
        static string lgse_api = "https://lgse-api.azurewebsites.net/";
        static string lgse_api_dev = "https://lgse-api-dev.azurewebsites.net/";
        static string lgse_api_qa = "https://lgse-api-qa.azurewebsites.net/";
        static string lgse_api_prod = "https://app-lgseapiprod.azurewebsites.net/";
        static string lgse_table_api = @"https://lgse-api.azurewebsites.net";
        static string lgse_table_api_dev = @"https://lgse-api-dev.azurewebsites.net";
        static string lgse_table_api_qa = @"https://lgse-api-qa.azurewebsites.net";

        private static string lgse_table_api_prod = @"https://app-lgseapiprod.azurewebsites.net/";
       // public static string M_IPAddress = lgse_api;
        public static string P_lngUserId = "";
        // Replace strings with your Azure Mobile App endpoint.
        public static string app_api = lgse_api_prod;
        public static string app_table_api = lgse_table_api_prod;
       
    }
}
