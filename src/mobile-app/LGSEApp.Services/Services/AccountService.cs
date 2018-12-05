/*
 * To add Offline Sync Support:
 *  1) Add the NuGet package Microsoft.Azure.Mobile.Client.SQLiteStore (and dependencies) to all client projects
 *  2) Uncomment the #define OFFLINE_SYNC_ENABLED
 *
 * For more information, see: http://go.microsoft.com/fwlink/?LinkId=620342
 */
#define OFFLINE_SYNC_ENABLED

using System;
using System.Threading.Tasks;
using LGSEApp.Services.Models;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http;
using System.Diagnostics;

namespace LGSEApp.Services.Services
{
    public partial class AccountService
    {
       
        static string LoginUrl = "api/Account/Login";
        static string ChangePasswordUrl = "api/Account/ChangePassword";
        static string OTPurl = "api/Account/OTP";
        static string ForgotPasswordurl = "api/Account/ForgotPassword";
        static string ActivateUserurl = "api/Account/ActivateUser";
        static string Signupurl = "api/Account/Signup";
        static string UpdatePreferredRoleurl = "api/Account/UpdatePreferredRole";
        static string LogoutUrl = "api/Account/Logout";
        static string SendActivationLinkUrl = "api/Account/SendActivationLink";
        static string IncidentOverviewUrl = "api/IncidentOverviewMstr";
        public async Task<TokenModel> AuthorizeUser(LoginModel loginModel)
        {
            TokenModel tokenModel = new TokenModel();
            try
            {

                dynamic dynamicJson = new ExpandoObject();
                dynamicJson.Email = loginModel.Email;
                dynamicJson.Password = loginModel.Password;
                HttpResponseMessage httpRespon = await Client.PostAsAsync(LoginUrl, dynamicJson);
                var responJsonText = await httpRespon.Content.ReadAsStringAsync();
                int statusCode = Convert.ToInt32(httpRespon.StatusCode);
                if (httpRespon == null)
                {
                    tokenModel.message = ResponceCode.customErrorFunction(651, null); ;
                }                
                else if (httpRespon.IsSuccessStatusCode)
                {                   
                    return tokenModel = JsonConvert.DeserializeObject<TokenModel>(responJsonText);
                }                
                else
                {
                var   message= JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                    tokenModel.message = ResponceCode.customErrorFunction(Convert.ToInt32(statusCode),(message==null ? null: message.message));
                }
            }
            catch (Exception Ex)
            {
                Debug.Write(Ex.Message);
                tokenModel.message = ResponceCode.customErrorFunction(651, null);
            }
            return tokenModel;
        }

        public async Task<CustomeMessage> ChangePassword(ChangePasswordModel changePassword)
        {
            CustomeMessage message = new CustomeMessage();
            try
            {
                Client client = new Client();               
                dynamic dynamicJson = new ExpandoObject();
                dynamicJson.OldPassword = changePassword.OldPassword;
                dynamicJson.NewPassword = changePassword.NewPassword;

                HttpResponseMessage httpRespon = await Client.PostAsAsync(ChangePasswordUrl, dynamicJson);
                var responJsonText = await httpRespon.Content.ReadAsStringAsync();
                int statusCode = Convert.ToInt32(httpRespon.StatusCode);
                if (Convert.ToInt32(httpRespon.StatusCode) == 401)
                {
                    message.message = Convert.ToString(statusCode);
                }
                else if (httpRespon.IsSuccessStatusCode)
                {
                   
                    return message = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                }
                else
                {
                    message = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                    message.message = ResponceCode.customErrorFunction(Convert.ToInt32(statusCode), message.message);
                }

            }
            catch (Exception Ex)
            {

                Debug.Write(Ex.Message);
            }
            return message;
        }
        public async Task<CustomeMessage> ForgotPassword(ForgotPasswordModel forgotPassword)
        {
            CustomeMessage message = new CustomeMessage();
            try
            {
               
                dynamic dynamicJson = new ExpandoObject();
                dynamicJson.Email = forgotPassword.EmailId;
                dynamicJson.OTPCode = forgotPassword.OtpCode;
                dynamicJson.Password = forgotPassword.Password;

                HttpResponseMessage httpRespon = await Client.PostAsAsync(ForgotPasswordurl, dynamicJson);
                var responJsonText = await httpRespon.Content.ReadAsStringAsync();
              if (httpRespon.IsSuccessStatusCode)
                {
                   
                    return message = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                }
                else
                {
                    message = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                    message.message = ResponceCode.customErrorFunction(Convert.ToInt32(httpRespon.StatusCode), message.message);
                }

            }
            catch (Exception Ex)
            {
                Debug.Write(Ex.Message);

            }
            return message;
        }

        public async Task<CustomeMessage> SendOTP(ForgotPasswordModel forgotPassword)
        {
            CustomeMessage message = new CustomeMessage();
            try
            {
             
                dynamic dynamicJson = new ExpandoObject();
                dynamicJson.Email = forgotPassword.EmailId;
                HttpResponseMessage httpRespon = await Client.PostAsAsync(OTPurl, dynamicJson);
                var responJsonText = await httpRespon.Content.ReadAsStringAsync();
               
                if (httpRespon.IsSuccessStatusCode)
                {                   
                    return message = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                }
                else
                {
                    message = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                    message.message = ResponceCode.customErrorFunction(Convert.ToInt32(httpRespon.StatusCode), message.message);
                }

            }
            catch (Exception Ex)
            {
                Debug.Write(Ex.Message);
            }
            return message;
        }
        public static async Task<CustomeMessage> SendActivationLink(UserModel userModel)
        {
            CustomeMessage message = new CustomeMessage();
            try
            {

                dynamic dynamicJson = new ExpandoObject();
                dynamicJson.UserId = userModel.Email;
                HttpResponseMessage httpRespon = await Client.PostAsAsync(SendActivationLinkUrl, dynamicJson);
                var responJsonText = await httpRespon.Content.ReadAsStringAsync();
                int statusCode = Convert.ToInt32(httpRespon.StatusCode);
                if (Convert.ToInt32(statusCode) == 401)
                {
                    message.message = Convert.ToString(statusCode);
                }
                else if (httpRespon.IsSuccessStatusCode)
                {
                    return message = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                }
                else
                {
                    message = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                    message.message = ResponceCode.customErrorFunction(Convert.ToInt32(statusCode), message.message);
                }

            }
            catch (Exception Ex)
            {
                Debug.Write(Ex.Message);
            }
            return message;
        }
        public static async Task<CustomeMessage> Logout()
        {
            CustomeMessage message = new CustomeMessage();
            try
            {

               
                HttpResponseMessage httpRespon = await Client.PostAsAsync(LogoutUrl,null);
                var responJsonText = await httpRespon.Content.ReadAsStringAsync();
                if (httpRespon.IsSuccessStatusCode)
                {
                    return message = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                }
                else
                {
                    message = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                    message.message = ResponceCode.customErrorFunction(Convert.ToInt32(httpRespon.StatusCode), message.message);
                }

            }
            catch (Exception Ex)
            {
                Debug.Write(Ex.Message);
            }
            return message;
        }
        public async Task<CustomeMessage> ActivateUser(UserActiveModel userActiveModel)
        {
            CustomeMessage message = new CustomeMessage();
            try
            {
               
                dynamic dynamicJson = new ExpandoObject();
                dynamicJson.OTPCode = userActiveModel.OtpCode;
                dynamicJson.Password = userActiveModel.Password;             
                dynamicJson.Email = userActiveModel.EmailId;
                HttpResponseMessage httpRespon = await Client.PostAsAsync(ActivateUserurl, dynamicJson);
                var responJsonText = await httpRespon.Content.ReadAsStringAsync();
               if (httpRespon.IsSuccessStatusCode)
                {
                   
                    return message = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                }
                else
                {
                    message = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                    message.message = ResponceCode.customErrorFunction(Convert.ToInt32(httpRespon.StatusCode), message.message);
                }


            }
            catch (Exception Ex)
            {
                Debug.Write(Ex.Message);

            }
            return message;
        }
        public static async Task<CustomeMessage> UpdatePreferredRole(string role)
        {
            CustomeMessage message = new CustomeMessage();
            try
            {

                dynamic dynamicJson = new ExpandoObject();
              
                dynamicJson.RoleId = role;
                HttpResponseMessage httpRespon = await Client.PostAsAsync(UpdatePreferredRoleurl, dynamicJson);
                var responJsonText = await httpRespon.Content.ReadAsStringAsync();
                int statusCode = Convert.ToInt32(httpRespon.StatusCode);
                if (Convert.ToInt32(statusCode) == 401)
                {
                    message.message = Convert.ToString(statusCode);
                }
                else if (httpRespon.IsSuccessStatusCode)
                {

                    return message = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                }
                else
                {
                    message = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                    message.message = ResponceCode.customErrorFunction(Convert.ToInt32(statusCode), message.message);
                }


            }
            catch (Exception Ex)
            {

                Debug.Write(Ex.Message);
            }
            return message;
        }
        public async Task<CustomeMessage> PostUser(UserModel userModel)
        {
            CustomeMessage message = new CustomeMessage();
            try
            {
               
                dynamic dynamicJson = new ExpandoObject();
                dynamicJson.FirstName = userModel.FirstName;
                dynamicJson.LastName = userModel.LastName;
               // dynamicJson.OrgId = userModel.OrgId;
                dynamicJson.EmployeeId = userModel.EmployeeId;
                dynamicJson.RoleId = userModel.RoleId;
                dynamicJson.EUSR = userModel.EUSR;
                dynamicJson.ContactNo = userModel.ContactNo;
                dynamicJson.Email = userModel.Email;
              //  dynamicJson.Password = userModel.Password;
                HttpResponseMessage httpRespon = await Client.PostAsAsync(Signupurl, dynamicJson);
                var responJsonText = await httpRespon.Content.ReadAsStringAsync();
                message = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                string statusCode = Convert.ToString(httpRespon.StatusCode);
              if (httpRespon.IsSuccessStatusCode)
                {                   
                    return message = JsonConvert.DeserializeObject<CustomeMessage>(responJsonText);
                }
                else
                {
                    //if (message.message == "USER_EXISTS_NOT_ACTIVATED")
                    //   return message;
                    message.message = ResponceCode.customErrorFunction(Convert.ToInt32(httpRespon.StatusCode), message.message);
                }

            }
            catch (Exception Ex)
            {
                Debug.Write(Ex.Message);

            }
            return message;
        }
        public static async Task<IncidentOverviewMstrResponse> GetIncidentOverview()
        {
            IncidentOverviewMstrResponse incident = new IncidentOverviewMstrResponse();
        
            try
            {
                //Client client = new Client();
                HttpResponseMessage httpRespon = await Client.GetAsAsync(IncidentOverviewUrl);

                if (httpRespon.IsSuccessStatusCode)
                {
                    var responJsonText = await httpRespon.Content.ReadAsStringAsync();
                    incident = JsonConvert.DeserializeObject<IncidentOverviewMstrResponse>(responJsonText);
                    return incident;
                }

            }


            catch (Exception Ex)
            {
                Debug.Write(Ex.Message);

            }
            return incident;
        }
    }
}
