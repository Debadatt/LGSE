using LGSEApp.Services.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LGSEApp.Services.Services
{
    public class Client
    {
        public static string  token { get; set; }
        public static string roleId { get; set; }
        public static string roleName { get; set; }
        public static string username { get; set; }
        public static string userId { get; set; }
        public static string email { get; set; }
        public static string firstName { get; set; }
        public static string lastName { get; set; }
        public static bool IsCleared { get; set; }
        public static bool IsUserMatched { get; set; }
        public static bool IsRoleMatched { get; set; }
        public static bool IsLogged { get; set; }
        public static bool IsPermission { get; set; }
        public static int PendingCount { get; set; }
        public static async Task<HttpResponseMessage> PostAsAsync(string _strUri, dynamic dynamicJson)
        {
            HttpResponseMessage httpRespon = null;
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
                if (!string.IsNullOrEmpty(Client.token))
                {
                    httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", Client.token);
                    httpClient.DefaultRequestHeaders.Add("X-ACCESS-ROLE", Client.roleId);
                }
                var _strJson = JsonConvert.SerializeObject(dynamicJson);
                var httpContent = new StringContent(_strJson, Encoding.UTF8, "application/json");
                Debug.Write(Constants.app_api + _strUri);
                httpRespon = await httpClient.PostAsync(Constants.app_api + _strUri, httpContent);
                return httpRespon;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            return httpRespon;
        }
        public static async Task<HttpResponseMessage> GetAsAsync(string _strUri)
        {
            HttpResponseMessage httpRespon = null;
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
                if (!string.IsNullOrEmpty(Client.token))
                {
                    httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", Client.token);
                    httpClient.DefaultRequestHeaders.Add("X-ACCESS-ROLE", Client.roleId);
                }

                Debug.Write(Constants.app_api + _strUri);
                httpRespon = await httpClient.GetAsync(Constants.app_api + _strUri);
                return httpRespon;
            }
            catch(Exception ex)
            {
                Debug.Write(ex.Message);
              
            }
            return httpRespon;
        }
    
    }
}
