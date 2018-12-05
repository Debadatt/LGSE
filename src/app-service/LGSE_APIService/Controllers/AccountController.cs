using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using LGSE_APIService.Common;
using LGSE_APIService.Common.Utilities;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;
using LGSE_APIService.RequestObjects;
using LGSE_APIService.Utilities;
using LGSE_APIService.Validators;
using Microsoft.Azure.Mobile.Server.Config;

namespace LGSE_APIService.Controllers
{
    [MobileAppController]
    public class AccountController : ApiController
    {
        readonly LGSE_APIContext _context = null;
        public AccountController()
        {
            _context = LGSE_APIContext.GetIntance();
            ValidationUtilities.dbContext = _context;
            AuthorizationUtilities.dbContext = _context;
            DbUtilities.dbContext = _context;
        }

        [HttpPost]
        [Route("api/Account/Signup")]
        [ValidateModel]
        public IHttpActionResult SignUp(RegisterRequest request)
        {
            try
            {
                string errorMessage = ValidationUtilities.ValidateUserDetails(request);
                var domainObj = DbUtilities.GetDomainDetails(request.Email);
                if (errorMessage.Equals(string.Empty))
                {
                    string otpCode = AuthorizationUtilities.GenerateOTPCode();
                    DbUtilities.SaveTheUser(request, otpCode, domainObj, request.Email, true);
                    AuthorizationUtilities.SendOTPtoUser(request.Email, otpCode);
                    return Ok(HttpUtilities.CustomResp(ErrorCodes.USER_REGISTERED.ToString()));
                }
                else
                {
                    return BadRequest(errorMessage);
                }
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/Account/ChangePassword")]
        [ValidateModel]
        public IHttpActionResult ChangePassword(ChangePasswordRequest request)
        {
            try
            {
                //get user details from request _context
                User account = GetLoggedInUser();
                if (account != null)
                {
                    //compare old password with the one saved in Db
                    if (IsPasswordCorrect(request.OldPassword))
                    {
                        if (IsPasswordCorrect(request.NewPassword))
                            return BadRequest(ErrorCodes.PASSWORD_ALREADY_USED.ToString());
                        byte[] pwdhash = AuthorizationUtilities.hash(request.NewPassword, account.Salt);
                   //     account.UpdatedAt = DateTime.UtcNow;
                        account.ModifiedBy = account.Email;
                        account.Password = pwdhash;
                        account.PwdStartDate = DateTimeOffset.UtcNow;
                        // _context.Entry(account).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                        return Ok(HttpUtilities.CustomResp(ErrorCodes.PWD_CHANGED.ToString()));

                    }
                    else
                    {
                        return BadRequest(ErrorCodes.OLD_PWD_NOTMATCHED.ToString());
                    }
                }
                else
                {
                    return BadRequest(ErrorCodes.INVALID_USER.ToString());
                }
                //return Ok();
            }
            catch (Exception ex)
            {
                //Services.Log.Error(ex);
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Compare user entered password with the password saved in Db
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <returns>
        ///     true:  if passwords are same
        ///     false: if password are not same
        /// </returns>
        private bool IsPasswordCorrect(string oldPassword,User account = null)
        {
            if(account==null)
             account = GetLoggedInUser();
            byte[] exisingPassword = account.Password;
            byte[] oldPasswordSalted = AuthorizationUtilities.hash(oldPassword, account.Salt);

            //compare old password 
            return Utilities.Utilities.slowEquals(oldPasswordSalted, exisingPassword);
        }

        [HttpPost]
        // Forgot Password or Reset Password
        [Route("api/Account/ForgotPassword")]
        [ValidateModel]
        public IHttpActionResult ForgotPassword(ActivationRequest request)
        {
            try
            {
                var user = DbUtilities.GetUserByEmail(request.Email);
                if (user != null)
                {
                    string validStatus = ValidationUtilities.ValidateForActivation(request, user, true);
                    if (validStatus == Constants.SUCCESS_MSG)
                    {
                        User account = _context.Users.Where(a => a.Email == request.Email).SingleOrDefault();
                        //if (IsPasswordCorrect(request.Password, account))
                        //    return BadRequest(ErrorCodes.PASSWORD_ALREADY_USED.ToString());
                        byte[] pwdhash = AuthorizationUtilities.hash(request.Password, account.Salt);

                      //  account.UpdatedAt = DateTime.UtcNow;
                        account.ModifiedBy = request.Email;
                        account.IsActivated = true;
                        account.OTPCode = "";

                        //add the new password to the database 
                        account.Password = pwdhash;
                        account.PwdStartDate = DateTimeOffset.UtcNow;
                        account.IsLocked = false;
                        _context.SaveChanges();
                        return Ok(HttpUtilities.CustomResp(ErrorCodes.PWD_UPDATED.ToString()));
                    }
                    else
                    {
                        return BadRequest(validStatus);
                    }
                }
                else
                {
                    //user doesn't exists
                    return BadRequest(ErrorCodes.INVALID_USER.ToString());
                }
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Route("api/Account/OTP")]
        [ValidateModel]
        public IHttpActionResult OTP(BaseRequest request)
        {
            try
            {
                if (!request.Email.Equals(string.Empty))
                {
                    //Check the user exists with that Email 
                    User user = _context.Users.SingleOrDefault(a => a.Email == request.Email  );
                    //&& a.IsActiveUser == true && a.IsActivated == true && a.Domain.IsActive == true
                    if (user != null && user.IsActiveUser == false)
                    {
                        return BadRequest(ErrorCodes.USER_DEACTIVATED_BY_ADMIN.ToString());//BadRequest(Constants.INVALID_USER);
                    }
                    else if (user != null && user.IsActivated == false)
                    {
                        return BadRequest(ErrorCodes.USER_NOT_ACVTD.ToString());//BadRequest(Constants.INVALID_USER);
                    }
                    else if (user != null && user.Domain.IsActive == false)
                    {
                        return BadRequest(ErrorCodes.DOMAIN_IS_INACTIVE.ToString());//BadRequest(Constants.INVALID_USER);
                    }
                    else if (user != null)
                    {
                        if (DateTimeOffset.UtcNow < user.OTPGeneratedAt.Value.AddMinutes(Convert.ToInt64(ConfigurationManager.AppSettings[Constants.OTP_REQTIME_CONN_STRING])))
                        {
                            return BadRequest(ErrorCodes.OTP_GENERATED_ALREADY.ToString()); //HttpUtilities.FrameHTTPResp(HttpStatusCode.OK, Constants.OTP_GENERATED_ALREADY);//Ok(Constants.OTP_GENERATED_ALREADY);
                        }
                        else
                        {
                            string otpCode = AuthorizationUtilities.GenerateOTPCode();
                            user.OTPGeneratedAt = DateTimeOffset.UtcNow;
                            AuthorizationUtilities.SendOTPForPwdReset(user.Email, otpCode);
                            user.OTPCode = otpCode;
                            user.ModifiedBy = request.Email;
                        //    user.UpdatedAt = DateTimeOffset.UtcNow;
                            _context.SaveChanges();
                            return Ok(HttpUtilities.CustomResp(ErrorCodes.OTP_GENERATED.ToString()));
                        }

                    }
                    //user doesn't exists
                    return BadRequest(ErrorCodes.INVALID_USER.ToString());//BadRequest(Constants.INVALID_USER);
                }
                else
                {
                    //Empty Email ID
                    return BadRequest(ErrorCodes.EMPTY_USER_ID.ToString());//HttpUtilities.FrameHTTPResp(HttpStatusCode.BadRequest, Constants.EMPTY_USER_ID);//BadRequest(Constants.EMPTY_USER_ID);
                }
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);//HttpUtilities.FrameHTTPResp(HttpStatusCode.BadRequest,ex.Message);   //InternalServerError(ex);
            }
        }
        [HttpPost]
        [Route("api/Account/ActivateUser")]
        [ValidateModel]
        public IHttpActionResult ActivateUser(ActivationRequest request)
        {

            try
            {
                var dbUser = DbUtilities.GetUserByEmail(request.Email);
                if (dbUser != null)
                {
                    string validStatus = DbUtilities.ValidateForActivation(request, dbUser, false);
                    if (validStatus == Constants.SUCCESS_MSG)
                    {
                        //Need to get Oorg id based on domain
                        byte[] salt = AuthorizationUtilities.generateSalt();

                        User account = _context.Users.SingleOrDefault(a => a.Email == request.Email);
                    //    account.UpdatedAt = DateTime.UtcNow;
                        account.ModifiedBy = request.Email;
                        account.Salt = salt;
                        account.Password = AuthorizationUtilities.hash(request.Password, salt);
                        account.IsActivated = true;
                        account.OTPCode = "";
                        _context.SaveChanges();
                        return Ok(HttpUtilities.CustomResp(ErrorCodes.USER_ACTIVATED.ToString()));
                    }
                    else
                    {
                        return BadRequest(validStatus);
                    }
                }
                else
                {
                    return BadRequest(ErrorCodes.INVALID_USER.ToString());
                }
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Route("api/Account/Logout")]
        public IHttpActionResult Logout()
        {
            try
            {
                string userRole = HttpUtilities.GetUserRoleAccessApi(this.Request);
                string token = HttpUtilities.GetRequestToken(this.Request);
                var user = GetLoggedInUser();

                UserRoleMap userRoleMap = _context.UserRoleMaps.FirstOrDefault(i => i.RoleId == userRole && i.UserId == user.Id);
                if (userRoleMap == null)
                {
                    return BadRequest(ErrorCodes.USER_ROLE_MAP_DOES_NOT_EXISTS.ToString());
                }
                else
                {
                    userRoleMap.IsPreferredRole = false;
                    //UserRoleMap existingRoles = _context.UserRoleMaps.FirstOrDefault(i => i.UserId == user.Id && i.Id != userRoleMap.Id);
                    //if (existingRoles != null)
                    //{
                    //    existingRoles.IsPreferredRole = false;
                    //}
                    //remove existing preference.
                    _context.SaveChanges();
                    //return Ok();
                }
                UserRoleMap userRoleMapIsAvailable = _context.UserRoleMaps.FirstOrDefault(i => i.UserId == user.Id && i.IsPreferredRole == true);
                if (userRoleMapIsAvailable == null)
                {
                    user.IsLoggedIn = false;
                 //   user.UpdatedAt = DateTimeOffset.UtcNow;
                    _context.SaveChanges();
                }
                DbUtilities.AuditTrialEntry(user, AuditTrialStatus.SUCCESS, AuditTrialOpType.LOGOUT, this.Request, userRole, token);
                return Ok();
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError();
            }
        }
        [HttpPost]
        [Route("api/Account/Login")]
        [ValidateModel]
        public IHttpActionResult Login(LoginRequest request)
        {

            try
            {
                User account = _context.Users.Where(a => a.Email == request.Email).SingleOrDefault();
                ValidationUtilities.ValidateUserForLogin(request, account);
                if (account != null)
                {
                    if (account.IsActiveUser == false)
                    {
                        return BadRequest(ErrorCodes.USER_DEACTIVATED_BY_ADMIN.ToString());
                        //var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.USER_DEACTIVATED_BY_ADMIN);
                        // throw new HttpResponseException(response);
                    }
                    if (account.IsActivated==false)
                    {
                        return BadRequest(ErrorCodes.USER_NOT_ACVTD.ToString());
                    }
                    if (!account.IsLocked)
                    {
                        byte[] incoming = AuthorizationUtilities.hash(request.Password, account.Salt);
                        if (Utilities.Utilities.slowEquals(incoming, account.Password))
                        {
                            if (account.IsActivated)
                            {
                                //Audit Trial Entry.
                                //DbUtilities.AuditTrialEntry(account, AuditTrialStatus.SUCCESS, AuditTrialOpType.LOGIN, this.Request);
                                //Creating a Token
                                ClaimsIdentity claimsIdentity = new ClaimsIdentity();
                                claimsIdentity.AddClaim(new Claim(Constants.STR_FIRSTNAME, account.FirstName));
                                claimsIdentity.AddClaim(new Claim(Constants.STR_LASTNAME, account.LastName));
                                claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, request.Email));
                                claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, request.Email));
                                claimsIdentity.AddClaim(new Claim("UserId", account.Id));
                                List<Role> roles = (from userMaps in _context.UserRoleMaps.ToList()
                                                    join rol in _context.Roles.ToList() on userMaps.RoleId equals rol.Id
                                                    where userMaps.UserId == account.Id && userMaps.Deleted == false
                                                    select rol).ToList();
                                if (roles != null && roles.Count > 0)
                                {
                                    foreach (var item in roles)
                                    {
                                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, item.RoleName));
                                        //claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, item.RoleName + "|" + item.Id));
                                    }
                                }
                                // Update user details:
                                UpdateUserDetailsInLogin(account);
                                JwtSecurityToken token = AuthorizationUtilities.GetAuthenticationTokenForUser(request.Email, claimsIdentity.Claims.ToArray());
                                return Ok(new
                                {
                                    Token = token.RawData,
                                    Username = request.Email,
                                    UserId = account.Id
                                });
                            }
                            else
                            {
                                return BadRequest(ErrorCodes.USER_NOT_ACVTD.ToString());
                            }
                        }
                        else
                        {

                            //Audit Trial Entry.
                            DbUtilities.AuditTrialEntry(account, AuditTrialStatus.FAILURE, AuditTrialOpType.LOGIN, this.Request);
                            // return Unauthorized();
                            return BadRequest(ErrorCodes.PASSWORD_NOTMATCHED.ToString());
                        }
                    }
                    else
                    {
                        return BadRequest(ErrorCodes.ACCOUNT_LOCKED.ToString());
                    }
                }
                return Unauthorized();
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Load user about with details of current user who sent http request
        /// TODO: If "account" is null, return empty object instead of null
        /// </summary>
        /// <returns>User object</returns>

        private User GetLoggedInUser()
        {
            string userEmail = HttpUtilities.GetUserNameFromToken(this.Request);
            //TODO: What happens if account is null?
            User account = _context.Users.Where(a => a.Email == userEmail).SingleOrDefault();
            return account;
        }
        /// <summary>
        /// Update use 
        /// </summary>
        private void UpdateUserDetailsInLogin(User user)
        {
            user.IsLoggedIn = true;
          //  user.UpdatedAt = DateTimeOffset.UtcNow;
            _context.SaveChanges();
        }
        //GET api/Account
        [HttpGet]
        [Route("api/Account/UserProfile")]
        public IHttpActionResult GetUserDetails()
        {
            try
            {
                string userEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                dynamic useDetails;
                //dynamic user = new ExpandoObject();
                useDetails = (from user in _context.Users
                              where user.Email == userEmail
                              select new
                              {
                                  userId = user.Id,
                                  firtName = user.FirstName,
                                  lastName = user.LastName,
                                  email = user.Email,
                                  domainId = user.DomainId,
                                  employeeId = user.EmployeeId,
                                  eusr = user.EUSR,
                                  contactNo = user.ContactNo,
                                  isAvailable = user.IsAvailable,
                                  roles = user.UserRoleMap.Where(i => i.UserId == user.Id && i.Deleted == false)
                                  .Select(r => new { r.Role.Id, r.Role.RoleName }).ToList(),
                                  PreferredRole = user.UserRoleMap.Where(i => i.IsPreferredRole == true && i.Deleted == false).Select(i => i.Role.RoleName).FirstOrDefault()
                              }).FirstOrDefault();
                return Ok(useDetails);
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/Account/UpdatePreferredRole")]
        public IHttpActionResult UpdatePreferredRole(PreferredRoleRequest req)
        {
            try
            {
                User user = GetLoggedInUser();
                string token = HttpUtilities.GetRequestToken(this.Request);
                if (user == null)
                {
                    return BadRequest(ErrorCodes.USER_DOES_NOT_EXISTS.ToString());
                }
                Role role = _context.Roles.FirstOrDefault(i => i.Id == req.RoleId);
                if (role == null)
                {
                    return BadRequest(ErrorCodes.ROLE_DOES_NOT_EXISTS.ToString());
                }
                UserRoleMap userRoleMap = _context.UserRoleMaps.FirstOrDefault(i => i.RoleId == req.RoleId && i.UserId == user.Id);
                if (userRoleMap == null)
                {
                    return BadRequest(ErrorCodes.USER_ROLE_MAP_DOES_NOT_EXISTS.ToString());
                }
                else
                {
                    userRoleMap.IsPreferredRole = true;
                    //UserRoleMap existingRoles = _context.UserRoleMaps.FirstOrDefault(i => i.UserId == user.Id && i.Id != userRoleMap.Id);
                    //if (existingRoles != null)
                    //{
                    //    existingRoles.IsPreferredRole = false;
                    //}
                    //remove existing preference.
                    _context.SaveChanges();
                    DbUtilities.AuditTrialEntry(user, AuditTrialStatus.SUCCESS, AuditTrialOpType.LOGIN, this.Request, req.RoleId,token);

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Admin can send activation link to the users if user request for it.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Account/SendActivationLink")]
        public IHttpActionResult SendActivationLink(SendActivationReq req)
        {
            try
            {
                string currentUsrEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                var user = _context.Users.FirstOrDefault(i => i.Id == req.UserId);
                if(user.IsActiveUser==false)
                    return BadRequest(ErrorCodes.USER_DEACTIVATED_BY_ADMIN.ToString());
                else if (user.Domain.IsActive == false)
                    return BadRequest(ErrorCodes.DOMAIN_IS_INACTIVE.ToString());
                ValidationUtilities.ValidateUserforActivationLink(req.UserId, user);
                string otpCode = AuthorizationUtilities.GenerateOTPCode();
                AuthorizationUtilities.SendOTPtoUser(user.Email, otpCode);
                user.OTPCode = otpCode;
                user.OTPGeneratedAt = DateTimeOffset.UtcNow;
                user.ModifiedBy = currentUsrEmail;
               // user.UpdatedAt = DateTimeOffset.UtcNow;
                _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                return Ok();
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Admin can send New Password to the users if user request for it.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Account/ResetPassword")]
        public IHttpActionResult ResetPassword(SendActivationReq req)
        {
            try
            {
                string currentUsrEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                byte[] salt = AuthorizationUtilities.generateSalt();
                User user = _context.Users.FirstOrDefault(i => i.Id == req.UserId);
              //  if (user.IsActiveUser == false)
                    ValidationUtilities.ValidateUserforNewPassword(req.UserId, user);

                string generatedPassword = AuthorizationUtilities.GeneratePassword();
                AuthorizationUtilities.SendPasswordtoUser(user.Email, generatedPassword);
                byte[] pwdhash = AuthorizationUtilities.hash(generatedPassword, user.Salt);
                //  user.Salt = salt;
                user.Password = pwdhash;//AuthorizationUtilities.hash(generatedPassword, salt);
                user.ModifiedBy = currentUsrEmail;
               // user.UpdatedAt = DateTimeOffset.UtcNow;
                _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                return Ok();
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }
    }
}
