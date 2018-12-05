using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.Services.Services
{
   
    public  class ResponceCode
    {

      static  Dictionary<string, string> desiredResults = new Dictionary<string, string>();
         static ResponceCode()
        {
            desiredResults.Add("HTTP_ERROR_201", "STATUS_UPDATED");
            desiredResults.Add("HTTP_ERROR_401", "Invalid username or password");
          desiredResults.Add("HTTP_ERROR_403", "You are not authorized to access this resource");
        desiredResults.Add("HTTP_ERROR_404", "The requested resource not found");
        desiredResults.Add("SERVER_ERROR_UNKNOWN", "Unknown Server Error");
        desiredResults.Add("USER_REGISTERED", "Registered Successfully! Activation link sent to your email, Please activate and Login.");
        desiredResults.Add("USER_ACTIVATED", "Activated Sucessfully!");
            desiredResults.Add("USER_DOES_NOT_EXISTS", "Invalid User!");
            desiredResults.Add("USER_DEACTIVATED_BY_ADMIN", "User is inactivated by admin");
            desiredResults.Add("USER_EXISTS_NOT_ACTIVATED", "User already exists,  please contact support to activate account");
        desiredResults.Add("USER_ALREADY_ACTIVATED", "You have already activated, Please Login!");
        desiredResults.Add("USER_NOT_ACVTD", "You have not activated your account, Activation link sent to your email, Please activate and Login.");
        desiredResults.Add("INVALID_USER_PWD", "You email id and Password, does not match");
        desiredResults.Add("UNAUTHORIZED_ACCESS", "You are not authorized to access");
        desiredResults.Add("UNAUTHORIZED_REQUEST", "You do not have access to this resource");
        desiredResults.Add("PWD_CHANGED", "Password changed successfully!");
        desiredResults.Add("PASSWORD_NOTMATCHED", "Invalid username or password");
        desiredResults.Add("PASSWORD_ALREADY_USED", "This password is already used");
        desiredResults.Add("INVALID_TOKEN", "Invalid Token");
        desiredResults.Add("INVALID_USER", "Invalid User");
        desiredResults.Add("INVALID_ROLE", "Invalid Role");
        desiredResults.Add("INVALID_REQUEST_TYPE", "Invalid request type");
        desiredResults.Add("INVALID_USER_ID", "Invalid User Id");
        desiredResults.Add("OTP_GENERATED", "OTP genetated successfully, please check your email");
        desiredResults.Add("OTP_GENERATED_ALREADY", "OTP is already generated, please check your email ");
        desiredResults.Add("EMPTY_USER_ID", "Empty User Id");
        desiredResults.Add("PWD_UPDATED", "Password updated successfully");
        desiredResults.Add("USER_EXISTS", "You are already registered, Please Login!");
        desiredResults.Add("USER_NOT_ALLOWED", "User not allowed");
        desiredResults.Add("EMAIL_ADD_REQ", "Email address request");
        desiredResults.Add("OTP_EXPIRED", "OTP Expired");
        desiredResults.Add("INVALID_OTP_CODE", "Invalid OTP code");
        desiredResults.Add("Unauthorized", "Email ID and Password did not match");
            desiredResults.Add("HTTP_ERROR_651", "Please check you internet connection!");
            desiredResults.Add("DOMAIN_IS_INACTIVE", "Domain is Inactive");
           

            //Mobile Field Validation
            desiredResults.Add("EMAIL_REQUIRED", "A work email is required.");
            desiredResults.Add("EMAIL_VALID", "Please enter a valid email.");
            desiredResults.Add("FIRSTNAME_REQUIRED", "A First Name is required.");
            desiredResults.Add("FIRSTNAME_VALID", "A First name should be alphabets & length less then 20 characters.");
            desiredResults.Add("LASTNAME_REQUIRED", "A Last Name is required.");
            desiredResults.Add("LASTNAME_VALID", "A Last name should be alphabets & length less then 20 characters.");
            desiredResults.Add("EUSR_REQUIRED", "A EUSR is required.");
            desiredResults.Add("EUSR_VALID", "Please enter a valid EUSR.");
            desiredResults.Add("CONTACT_VALID", "Please enter a valid Contact Number.");
            desiredResults.Add("ROLE_REQUIRED", "Please select role.");
            desiredResults.Add("ACEEPTSTERMS_REQUIRED", "Please Accept Terms and Conditions.");
            desiredResults.Add("PASSWORD_REQUIRED", "A password is required.");
            desiredResults.Add("PASSWORD_VALID", "Please enter a valid Password.");
            desiredResults.Add("OLDPASSWORD_REQUIRED", "A Old Password is required.");
            desiredResults.Add("OLD_PWD_NOTMATCHED", "A Old Password is not matched.");
            desiredResults.Add("NEWPASSWORD_REQUIRED", "A New Password is required.");
            desiredResults.Add("CONFIRMPASSWORD_REQUIRED", "Please enter a valid confirm Password.");
            desiredResults.Add("CONFIRMPASSWORD_VALID", "Confirm Password is not matched.");
            desiredResults.Add("OTP_REQUIRED", "A OTP Code is required.");
        }
    //  responceMessagesList.Add("HTTP_ERROR_401","Invalid username or password");
    //public static string HTTP_ERROR_403 = "You are not authorized to access this resource";
    //    public static string HTTP_ERROR_404 = "The requested resource not found";
    //    public static string SERVER_ERROR_UNKNOWN = "Unknown Server Error";
    //    public static string USER_REGISTERED = "Registered Successfully! Activation link sent to your email, Please activate and Login.";
    //    public static string USER_ACTIVATED = "Activated Sucessfully!";
    //    public static string USER_ALREADY_ACTIVATED = "You have already activated, Please Login!";
    //    public static string USER_NOT_ACVTD = "You have not activated your account, Activation link sent to your email, Please activate and Login.";
    //    public static string INVALID_USER_PWD = "You email id and Password, does not match";
    //    public static string UNAUTHORIZED_ACCESS = "You are not authorized to access";
    //    public static string UNAUTHORIZED_REQUEST = "You do not have access to this resource";
    //    public static string PWD_CHANGED = "Password changed successfully!";
    //    public static string PASSWORD_NOTMATCHED = "Current and New password did not match";
    //    public static string PASSWORD_ALREADY_USED = "This password is already used";
    //    public static string INVALID_TOKEN = "Invalid Token";
    //    public static string INVALID_USER = "Invalid User";
    //    public static string INVALID_ROLE = "Invalid Role";
    //    public static string INVALID_REQUEST_TYPE = "Invalid request type";
    //    public static string INVALID_USER_ID = "Invalid User Id";
    //    public static string OTP_GENERATED = "OTP genetated successfully, please check your email";
    //    public static string OTP_GENERATED_ALREADY = "OTP is already generated, please check your email ";
    //    public static string EMPTY_USER_ID = "Empty User Id";
    //    public static string PWD_UPDATED = "Password updated successfully";
    //    public static string USER_EXISTS = "You are already registered, Please Login!";
    //    public static string USER_NOT_ALLOWED = "User not allowed";
    //    public static string EMAIL_ADD_REQ = "Email address request";
    //    public static string OTP_EXPIRED = "OTP Expired";
    //    public static string INVALID_OTP_CODE = "Invalid OTP code";
    //    public static string Unauthorized = "Email ID and Password did not match";

        public static string customErrorFunction(int responceCOde,string error)
        {
            string custommessage = string.Empty;
            switch (responceCOde)
            {
                case 201:
                    custommessage = desiredResults["HTTP_ERROR_201"];
                    break;
                case 401:
                    custommessage= desiredResults["HTTP_ERROR_401"];  //   desiredResults.TryGetValue("HTTP_ERROR_401",out custommessage); // desiredResults["HTTP_ERROR_401"];
                    break;
                case 403:
                    custommessage = desiredResults["HTTP_ERROR_403"];
                    break;
                case 404:
                    custommessage = desiredResults["HTTP_ERROR_404"];
                    break;
                case 400:
                    custommessage= applicationErrorHandler(error);
                    break;
                case 651:
                    custommessage = desiredResults["HTTP_ERROR_651"];
                    break;
                default:
                    custommessage = desiredResults["SERVER_ERROR_UNKNOWN"];
                    break;
            }
            return custommessage;
        }
        public static string applicationErrorHandler(string errorMessage)
        {
            string message = string.Empty;


            if (errorMessage != "")
            {
                message = desiredResults[errorMessage];
            }
            else
            {
                message = desiredResults["SERVER_ERROR_UNKNOWN"];
            }


            return message;
        }

    }
}
