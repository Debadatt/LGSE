
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using LGSE_APIService.Common.Utilities;
using System.Web.Http;

namespace LGSE_APIService.Utilities
{
    /// <summary>
    /// This class holds all the required HTTP header related methods.
    /// </summary>
    public static class HttpUtilities
    {
        ///// <summary>
        ///// Creates the http response based on user inputs
        ///// </summary>
        ///// <param name="statusCode"></param>
        ///// <param name="message"></param>
        ///// <returns></returns>
        //public static HttpResponseMessage FrameHTTPResp(HttpStatusCode statusCode,string message)
        //{
        //    var response = new HttpResponseMessage()
        //    {
        //        Content = new StringContent("{\"code\":\"INFO\", \"message\":\""+message+"\"}", Encoding.Unicode, "application/json"),
        //        StatusCode = statusCode
        //    };
        //    return response;
        //}

        /// <summary>
        /// Creates the http response based on user inputs
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static HttpResponseMessage FrameHTTPResp(HttpStatusCode statusCode, ErrorCodes errorCode,string message)
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent("{\"message\":\""+ errorCode + "\", \"description\":\"" + message + "\"}", Encoding.Unicode, "application/json"),
                StatusCode = statusCode
            };
            return response;
        }
        /// <summary>
        /// Creates the http response based on user inputs
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static HttpResponseMessage FrameHTTPResp(HttpStatusCode statusCode, ErrorCodes errorCode)
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent("{\"message\":\"" + errorCode + "\"}", Encoding.Unicode, "application/json"),
                StatusCode = statusCode
            };
            return response;
        }
        /// <summary>
        /// Creates the http response based on user inputs
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static HttpResponseMessage FrameHTTPResp(HttpStatusCode statusCode, string responseBody)
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent(responseBody, Encoding.Unicode, "application/json"),
                StatusCode = statusCode
            };
            return response;
        }

        /// <summary>
        /// Creates the CustomResponse based on user inputs
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static CustomResponse CustomResp(string message)
        {
            var response = new CustomResponse()
            {
                message = message
            };

            return response;
        }

        /// <summary>
        /// Gets the userName from Token
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRolesFromToken(HttpRequestMessage context)
        {
            string token = string.Empty;
            try
            {
                 token = context.Headers.GetValues("X-ZUMO-AUTH").FirstOrDefault();
                if (token != null)
                {
                    JwtSecurityToken jwtToken = new JwtSecurityToken(token);
                    List<System.Security.Claims.Claim> roleClaims = new List<System.Security.Claims.Claim>();
                    roleClaims = jwtToken.Claims.Where(item => item.Type == "role").ToList();
                    return roleClaims.Select(i=>i.Value.Split('|')[0].ToString()).ToList();
                }
                else
                {
                    return new List<string>();
                }
            }
            catch (Exception ex)
            {
                return new List<string>();
                //log
            }
        }

        /// <summary>
        /// Gets the userName from Token
        /// </summary>
        /// <returns></returns>
        public static string GetUserNameFromToken(HttpRequestMessage context)
        {
            string token = string.Empty;
            try
            {
                token = context.Headers.GetValues("X-ZUMO-AUTH").FirstOrDefault();
                System.Diagnostics.Trace.TraceInformation("token:"+token);
                if (token != null)
                {
                    JwtSecurityToken jwtToken = new JwtSecurityToken(token);
                    System.Security.Claims.Claim uidClaim = jwtToken.Claims.FirstOrDefault(item => item.Type == "email");
                    System.Diagnostics.Trace.TraceInformation("Email:" + uidClaim.Value);
                    return uidClaim.Value;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("error in token :" + ex.Message);
                return "TOKEN NOT GIVEN";
                //log
            }
        }
        /// <summary>
        /// Gets the userName from Token
        /// </summary>
        /// <returns></returns>
        public static int GetUserAccessApi(HttpRequestMessage context)
        {
            int access = 0;
            try
            {
                if(context.Headers.Contains("X-ACCESS-FLAG"))
                    return access = Convert.ToInt32(context.Headers.GetValues("X-ACCESS-FLAG").FirstOrDefault());
                else
                return access;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("error in token :" + ex.Message);
                return access;
                //log
            }
        }
        /// <summary>
        /// Gets the User Role from Token
        /// </summary>
        /// <returns></returns>
        public static string GetUserRoleAccessApi(HttpRequestMessage context)
        {
            string access = string.Empty;
            try
            {
                if (context.Headers.Contains("X-ACCESS-ROLE"))
                    return access = Convert.ToString(context.Headers.GetValues("X-ACCESS-ROLE").FirstOrDefault());
                else
                    return access;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("error in Role :" + ex.Message);
                return access;
                //log
            }
        }
        /// <summary>
        /// Gets the Token from Action Context
        /// </summary>
        /// <returns></returns>
        public static string GetRequestToken(HttpRequestMessage context)
        {
            try
            {
                string token = context.Headers.GetValues("X-ZUMO-AUTH").FirstOrDefault();
                System.Diagnostics.Trace.TraceInformation("token in  GetRequestToken:" + token);
                return token;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceInformation("Error in GetRequestToken" + ex.Message+ex.StackTrace.ToString());
                throw new Exception(ErrorCodes.TOKEN_DOES_NOT_EXISTS.ToString());
            }
        } 

        /// <summary>
        /// Reads the Device ID from the Request Header
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string GetDeviceIDFromHeader(HttpRequestMessage context, string username)
        {
            var listHeaders = (new List<String>()).AsEnumerable();
            var isHeaderExists = context.Headers.TryGetValues("X-DSS-DEVICEID", out listHeaders);
            if (isHeaderExists)
                return listHeaders.FirstOrDefault();
            return string.Format("DEFAULT_DEVICE_{0}", username);
        }
        /// <summary>
        /// Return internal server exceptions for table controllers.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="request"></param>
        public static void ServerError(Exception ex,HttpRequestMessage request)
        {
            LGSELogger.Error(ex);
            throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.InternalServerError,"System exception has occurred. Please contact administrator." ,ex));
        }
    }

    public class CustomResponse
    {
      
        /// <summary>
        /// message
        /// </summary>
        public string message { get; set; }

    }
}