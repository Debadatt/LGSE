
namespace LGSE_APIService.Utilities
{
    /// <summary>
    /// Global constants 
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// SendGrid From Address config key 
        /// </summary>
        public const string SENDGRID_FROMADD = "SENDGRID_FROMADD";

        /// <summary>
        /// Admin error message
        /// </summary>
        public const string CONACT_ADMIN_MSG = "Please contact administrator";

        /// <summary>
        /// Azure storage connection string configuration entry name
        /// </summary>
        public const string STG_CONN_STRING = "STG_CONN_STRING";


        /// <summary>
        /// Azure storage connection string configuration entry name
        /// </summary>
        public const string OTP_TIME_CONN_STRING = "OTP_TIMESPAN";
        
        
        /// <summary>
        /// Azure storage connection string configuration entry name
        /// </summary>
        public const string OTP_REQTIME_CONN_STRING = "OTP_REQTIMESPAN";
        
        /// <summary>
        /// Token life time. 90days by defualt
        /// </summary>
        public const int TOKEN_LIFE_TIME = 90;

        /// <summary>
        /// ASCII_VALUE_FOR_A
        /// </summary>
        public const int ASCII_VALUE_FOR_A = 65;

        /// <summary>
        /// ASCII_VALUE_FOR_Z
        /// </summary>
        public const int ASCII_VALUE_FOR_Z = 90;

        /// <summary>
        /// ASCII_VALUE_FOR_SPECIALCHAR_START
        /// </summary>
        public const int ASCII_VALUE_FOR_SPECIALCHAR_START = 35;

        /// <summary>
        /// ASCII_VALUE_FOR_SPECIALCHAR_END
        /// </summary>
        public const int ASCII_VALUE_FOR_SPECIALCHAR_END = 38;

        /// <summary>
    
        /// <summary>
        /// User registered
        /// </summary>
        public const string USER_REGISTERED = "User has been registered successfully";

        /// <summary>
        /// User activated
        /// </summary>
        public const string USER_ACTIVATED = "User has been activated";

        /// <summary>
        /// User already activated
        /// </summary>
        public const string USER_ALREADY_ACTIVATED = "User has been already activated";
        /// <summary>
        /// User not activated
        /// </summary>
        public const string USER_NOT_ACVTD = "User not yet activated";

        /// <summary>
        /// User not activated
        /// </summary>
        public const string SUCCESS_MSG = "Success";

        /// <summary>
        /// Invalide user name
        /// </summary>
        public const string INVALID_USER_PWD = "Invalid username or password";
        /// <summary>
        /// Unauthorized access
        /// </summary>
        public const string UNAUTHORIZED_ACCESS = "Unauthorized Access";
        /// <summary>
        /// Unauthorized access
        /// </summary>
        public const string UNAUTHORIZED_REQUEST = "Unauthorized Request";
        /// <summary>
        /// Password Changed
        /// </summary>
        public const string PWD_CHANGED = "Password changed successfully. Please login again.";
        /// <summary>
        /// Password does not match
        /// </summary>
        public const string PASSWORD_NOTMATCHED = "Password does not match";

        /// <summary>
        /// Password used earlier
        /// </summary>
        public const string PASSWORD_ALREADY_USED = "You have used this password previously, please choose another";

        /// <summary>
        /// Invalid Access Token
        /// </summary>
        public const string INVALID_TOKEN = "Invalid Access Token";
        /// <summary>
        /// Invalid User
        /// </summary>
        public const string INVALID_USER = "User does not exist";

        /// <summary>
        /// Invalid ROLE
        /// </summary>
        public const string INVALID_ROLE = "Role does not exist";

        /// <summary>
        /// Invalid User
        /// </summary>
        public const string INVALID_REQUEST_TYPE = "Invalid Request Type";

        /// <summary>
        /// Invalid UserId
        /// </summary>
        public const string INVALID_USER_ID = "Invalid User Id";

        /// <summary>
        /// Unknown error
        /// </summary>
        public const string UNKNOWN_ERROR = "UnKnown Error..";
        /// <summary>
        /// System error
        /// </summary>
        public const string SYSTEM_ERROR = "System Error..";
        /// <summary>
        /// Error occured while saving user
        /// </summary>
        public const string ERROR_ON_SAVING = "Error occured while saving user -";

        /// <summary>
        /// Unable to save profile details
        /// </summary>
        public const string UNABLE_TO_SAVE = "Unable to save profile details";
      
        /// <summary>
        /// Otp Generation
        /// </summary>
        public const string OTP_GENERATED = "New OTP has been sent to your registered email";

        /// <summary>
        /// Otp Generation
        /// </summary>
        public const string OTP_GENERATED_ALREADY = "OTP was already sent to your email inbox. You have to wait 10 minutes before requesting a new OTP";

        /// <summary>
        /// Empty User ID
        /// </summary>
        public const string EMPTY_USER_ID = "User ID cannot be empty";
        /// <summary>
        /// Password reset success message.
        /// </summary>
        public const string PWD_UPDATED = "Password reset is successful";
        /// <summary>
        /// User already exists
        /// </summary>
        public const string USER_EXISTS = "User already exists";
        /// <summary>
        /// User is not in allowed list of domains
        /// </summary>
        public const string USER_NOT_ALLOWED = "User is not in allowed list of domains";
        /// <summary>
        /// Email Address is mandatory
        /// </summary>
        public const string EMAIL_ADD_REQ = "Email Address is mandatory";

        /// <summary>
        /// FirstName
        /// </summary>
        public const string STR_FIRSTNAME = "FirstName";
        /// <summary>
        /// LastName
        /// </summary>
        public const string STR_LASTNAME = "LastName";

        /// <summary>
        /// Invalid User
        /// </summary>
        public const string STR_INVALID_USER = "Invalid User";
        /// <summary>
        /// OTP Expired
        /// </summary>
        public const string OTP_EXPIRED = "OTP code expired";
        /// <summary>
        /// Invalid OTP Code
        /// </summary>
        public const string INVALID_OTP_CODE = "Invalid OTP code";
       
        /// <summary>
        /// SUCCESS
        /// </summary>
        public const string SUCCESS = "SUCCESS";

        /// <summary>
        /// FAILURE
        /// </summary>
        public const string FAILURE = "FAILED";

        /// <summary>
        /// LOGIN_OPERATION
        /// </summary>
        public const string LOGIN_OPERATION = "LOGIN";

        /// <summary>
        /// LOGOUT_OPERATION
        /// </summary>
        public const string LOGOUT_OPERATION = "LOGOUT";

        /// <summary>
        /// ACCOUNT_LOCKED
        /// </summary>
        public const string ACCOUNT_LOCKED = "Your account is locked. Please reset your password.";

        /// <summary>
        /// Account locking time span.
        /// </summary>
        public const string ACCOUNTLOCK_TIMESPAN = "ACCOUNTLOCK_TIMESPAN";
        
    }
    public static class DBConstants
    {
        /// <summary>
        /// ISOLATED.
        /// </summary>
        public const string ISOLATED_STATUS = "Isolated";
        public const string RESTORED_STATUS = "Restored";
    }
  }
