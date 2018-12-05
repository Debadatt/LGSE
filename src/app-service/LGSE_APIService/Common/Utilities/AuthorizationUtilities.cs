
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

using System.Net.Http;
using System.IO;
using System.Drawing;
using System.Web;
using System.Reflection;
using System.Net;
using System.Web.Http;
using LGSE_APIService.Utilities;
using LGSE_APIService;
using LGSE_APIService.Models;
using LGSE_APIService.RequestObjects;
using LGSE_APIService.DataObjects;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Azure.Mobile.Server.Login;
namespace LGSE_APIService.Utilities
{
    /// <summary>
    /// Utility class for the authentication process
    /// </summary>
    public static class AuthorizationUtilities
    {
        /// <summary>
        /// Mobile Services context 
        /// </summary>
        public static LGSE_APIContext dbContext { get; set; }

        /// <summary>
        /// Generates the hash for given text and salt
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static byte[] hash(string plaintext, byte[] salt)
        {
            SHA512Cng hashFunc = new SHA512Cng();
            byte[] plainBytes = System.Text.Encoding.ASCII.GetBytes(plaintext);
            byte[] toHash = new byte[plainBytes.Length + salt.Length];
            plainBytes.CopyTo(toHash, 0);
            salt.CopyTo(toHash, plainBytes.Length);
            return hashFunc.ComputeHash(toHash);
        }

        /// <summary>
        /// Generates a random salt based on the requested size
        /// </summary>
        /// <returns></returns>
        public static byte[] generateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[256];
            rng.GetBytes(salt);
            return salt;
        }

        /// <summary>
        /// Generates the OTP code
        /// </summary>
        /// <returns></returns>
        public static string GenerateOTPCode()
        {
            Random rand = new Random();
            var code = rand.Next(100000, 1000000).ToString();
            //string otpgeneration = string.Empty;
            //int randomNumber;
            //randomNumber = GetRandomCode(Constants.ASCII_VALUE_FOR_A, Constants.ASCII_VALUE_FOR_Z);
            //otpgeneration += ((char)randomNumber).ToString();
            //randomNumber = GetRandomCode(Constants.ASCII_VALUE_FOR_SPECIALCHAR_START, Constants.ASCII_VALUE_FOR_SPECIALCHAR_END);
            ////removing the % special character.
            //while (randomNumber == 37)
            //{
            //    randomNumber = GetRandomCode(Constants.ASCII_VALUE_FOR_SPECIALCHAR_START, Constants.ASCII_VALUE_FOR_SPECIALCHAR_END);
            //    if (randomNumber != 37)
            //        break;
            //}
            //otpgeneration += ((char)randomNumber).ToString();
            //otpgeneration += code;
            return code;
           // return new string(otpgeneration.OrderBy(rel => rand.Next()).ToArray());
        }

        /// <summary>
        /// Generates the Random code
        /// </summary>
        /// <returns></returns>
        public static int GetRandomCode(int min, int max)
        {
            Random r = new Random();
            return r.Next(min, max);
        }
        public static JwtSecurityToken GetAuthenticationTokenForUser(string username, Claim[] claims)
        {

        #if DEBUG
           var signingKey = ConfigurationManager.AppSettings["SigningKey"];
        #else
           var signingKey = Environment.GetEnvironmentVariable("WEBSITE_AUTH_SIGNING_KEY");
        #endif

            var audience = ConfigurationManager.AppSettings["ValidAudience"];
            var issuer = ConfigurationManager.AppSettings["ValidIssuer"]; // audience must match the url of the site
            var timeSpan = TimeSpan.FromHours(Convert.ToInt16(ConfigurationManager.AppSettings["TOKEN_TIMESPAN"]));
            JwtSecurityToken token = AppServiceLoginHandler.CreateToken(
                claims,
                signingKey,
                audience,
                issuer,
                timeSpan
                );

            return token;
        }
        /// <summary>
        /// Sends the OTP code to User
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="OTPCode"></param>
        public static void SendOTPtoUser(string emailAddress, string OTPCode)
        {
            List<string> recipients = new List<string>();
            recipients.Add(emailAddress);
            string ENOTPCode = HttpUtility.UrlEncode(OTPCode);
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string Uripath = Uri.UnescapeDataString(uri.Path);
            string path = Path.GetDirectoryName(Uripath);

            string ActivateUrl = ConfigurationManager.AppSettings["WEBAPP_URL"].ToString() + "#/auth/activate?code=" + ENOTPCode + "&workemail=" + emailAddress;
            string EmailTemplatePath = @"\Common\EmailTemplate\HTML\UserActivationEmail.html";

            string htmlTemplateData = File.ReadAllText(path.Remove(path.Length - 3) + EmailTemplatePath);

            htmlTemplateData = htmlTemplateData.Replace("[OTPCode]", OTPCode)
                                              .Replace("[emailAddress]", emailAddress)
                                              .Replace("[ENOTPCode]", ENOTPCode)
                                              .Replace("[URL]", ActivateUrl)
                                              .Replace("[OTPSpan]", ConfigurationManager.AppSettings["OTP_TIMESPAN"]);

            string subject = "LGSE Activation Code";
            SendGridUtilities.SendEmail(recipients, subject, htmlTemplateData);
        }
        /// <summary>
        /// Sends the OTP code for password reset
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="OTPCode"></param>
        public static void SendOTPForPwdReset(string emailAddress, string OTPCode)
        {
            List<string> recipients = new List<string>();
            recipients.Add(emailAddress);
            
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string Uripath = Uri.UnescapeDataString(uri.Path);
            string ENOTPCode = HttpUtility.UrlEncode(OTPCode);
            string path = Path.GetDirectoryName(Uripath);
            string EmailTemplatePath = @"\Common\EmailTemplate\HTML\GetOTPEmail.html";
            string ActivateUrl = ConfigurationManager.AppSettings["WEBAPP_URL"].ToString()+"#/auth/reset-password?code=" + ENOTPCode + "&email=" + emailAddress;

            string htmlTemplateData = File.ReadAllText(path.Remove(path.Length - 3) + EmailTemplatePath);

            htmlTemplateData = htmlTemplateData.Replace("[OTPCode]", OTPCode)
                                              .Replace("[emailAddress]", emailAddress)
                                              .Replace("[ENOTPCode]", ENOTPCode)
                                              .Replace("[URL]", ActivateUrl)
                                              .Replace("[OTPSpan]", ConfigurationManager.AppSettings["OTP_TIMESPAN"]);

            string subject = "LGSE OTP";
            //string body = "Please use the OTP code " + OTPCode + " to reset your password <br>";
            //body += "csp://forgetpassword?code=" + OTPCode + "&email=" + emailAddress;
            SendGridUtilities.SendEmail(recipients, subject, htmlTemplateData);
        }
        /// <summary>
        /// Generates the new Password
        /// </summary>
        /// <returns></returns>
       
        public static string GeneratePassword()
        {
            Random rand = new Random();
            var code = rand.Next(100000, 1000000).ToString();
            string otpgeneration = string.Empty;
            int randomNumber;
            randomNumber = GetRandomCode(Constants.ASCII_VALUE_FOR_A, Constants.ASCII_VALUE_FOR_Z);
            otpgeneration += ((char)randomNumber).ToString();
            randomNumber = GetRandomCode(Constants.ASCII_VALUE_FOR_SPECIALCHAR_START, Constants.ASCII_VALUE_FOR_SPECIALCHAR_END);
            //removing the % special character.
            while (randomNumber == 37)
            {
                randomNumber = GetRandomCode(Constants.ASCII_VALUE_FOR_SPECIALCHAR_START, Constants.ASCII_VALUE_FOR_SPECIALCHAR_END);
                if (randomNumber != 37)
                    break;
            }
            otpgeneration += ((char)randomNumber).ToString();
           otpgeneration += code;
            return otpgeneration;
        }
        /// <summary>
        /// Send the new Password
        /// </summary>
        /// <returns></returns>
        /// <param name="emailAddress"></param>
        /// <param name="Password"></param>
        internal static void SendPasswordtoUser(string emailAddress, string Password)
        {
            List<string> recipients = new List<string>();
            recipients.Add(emailAddress);

            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string Uripath = Uri.UnescapeDataString(uri.Path);
            //string ENOTPCode = HttpUtility.UrlEncode(OTPCode);
            string path = Path.GetDirectoryName(Uripath);
            string EmailTemplatePath = @"\Common\EmailTemplate\HTML\GetPasswordEmail.html";
            //string ActivateUrl = ConfigurationManager.AppSettings["WEBAPP_URL"].ToString() + "#/auth/reset-password?code=" + ENOTPCode + "&email=" + emailAddress;

            string htmlTemplateData = File.ReadAllText(path.Remove(path.Length - 3) + EmailTemplatePath);

            htmlTemplateData = htmlTemplateData.Replace("[Password]", Password);

            string subject = "New Password send by Admin";
            //string body = "Please use the OTP code " + OTPCode + " to reset your password <br>";
            //body += "csp://forgetpassword?code=" + OTPCode + "&email=" + emailAddress;
            SendGridUtilities.SendEmail(recipients, subject, htmlTemplateData);
        }
    }
}