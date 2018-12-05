
using LGSE_APIService.Common;
using LGSE_APIService.Common.Utilities;
using LGSE_APIService.Controllers;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;
using LGSE_APIService.RequestObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace LGSE_APIService.Utilities
{
    /// <summary>
    /// Contains the generic validation utility methods
    /// </summary>
    public class ValidationUtilities
    {
        /// <summary>
        /// Mobile Services context 
        /// </summary>
        public static LGSE_APIContext dbContext { get; set; }

        /// <summary>
        /// Does the validations on the user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string ValidateUserDetails(RegisterRequest user)
        {
            string errorMessage = string.Empty;
            string userValidationErrors = ValidateModel(user);

            if (!userValidationErrors.Equals(string.Empty))
                return userValidationErrors;

            var account = dbContext.Users.SingleOrDefault(a => a.Email == user.Email);
            var role = dbContext.Roles.SingleOrDefault(a => a.Id == user.RoleId);
            if (account != null && !account.IsActivated)
                return ErrorCodes.USER_EXISTS_NOT_ACTIVATED.ToString();
            else if (account != null)
                return ErrorCodes.USER_EXISTS.ToString();
            else if (!DbUtilities.ValidateUserDomain(user.Email))
                return ErrorCodes.DOMAIN_IS_INACTIVE.ToString();
            else if (string.IsNullOrWhiteSpace(user.Email))
                return ErrorCodes.EMAIL_ADD_REQ.ToString();
            else if (!string.IsNullOrEmpty(user.RoleId) && role == null)
            {
                return ErrorCodes.INVALID_ROLE.ToString();
            }
            else if (user.RoleId == "8FE7DBCB-DCC3-4AC1-803A-5336621C8359" && string.IsNullOrEmpty(user.EUSR))
            {
                return ErrorCodes.EUSR_REQ.ToString();
            }
            else
                return string.Empty;
        }
        /// <summary>
        /// Validates the Role Request
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string ValidateRole(RoleRequest request)
        {
            string errorMessage = string.Empty;
            if (dbContext.Roles.FirstOrDefault(item => item.RoleName == request.RoleName) != null)
            {
                return ErrorCodes.ROLE_EXISTS_ALREADY.ToString();
            }
            return errorMessage;
        }

        /// <summary>
        /// Validates the Given OTP
        /// </summary>
        /// <returns></returns>
        public static string ValidateForActivation(ActivationRequest inputuser, User dbUser, bool isResetPwd)
        {
            String statusMsg = string.Empty;
            if (dbUser.IsActivated && !isResetPwd)
                statusMsg = ErrorCodes.USER_ALREADY_ACTIVATED.ToString();
            else if (HttpUtility.HtmlDecode(dbUser.OTPCode) != inputuser.OTPCode)
                statusMsg = ErrorCodes.INVALID_OTP_CODE.ToString();
            else if (DateTimeOffset.UtcNow > dbUser.OTPGeneratedAt.Value.AddMinutes(Convert.ToInt64(ConfigurationManager.AppSettings[Constants.OTP_TIME_CONN_STRING])))
                statusMsg = ErrorCodes.OTP_EXPIRED.ToString();
            else if (!dbUser.IsActivated && isResetPwd)
                statusMsg = ErrorCodes.USER_NOT_ACVTD.ToString();
            else
                statusMsg = "Success";
            return statusMsg;
        }


        ///// <summary>
        ///// Validates the User Profile
        ///// </summary>
        ///// <param name="profile"></param>
        ///// <param name="email"></param>
        ///// <returns></returns>
        //public static string ValidateProfileDetails(ProfileRequest profile, string email)
        //{
        //    string errorMessage = string.Empty;
        //    string userValidationErrors = ValidateModel(profile);
        //    if (!userValidationErrors.Equals(string.Empty))
        //        return userValidationErrors;

        //    var account = dbContext.Account.SingleOrDefault(a => a.Email == email);
        //    if (account == null)
        //        return Constants.INVALID_USER;

        //    if (!AuthorizationUtilities.IsUserExists(account.Email))
        //        return Constants.INVALID_USER;
        //    else
        //        return string.Empty;
        //}
        public static string ValidateIncident(IncidentEditRequest request)
        {
            String statusMsg = string.Empty;
            //Modify Incident
            if (string.IsNullOrEmpty(request.Id))
            {
                statusMsg = ErrorCodes.INCIDENT_ID_REQD.ToString();
                return statusMsg;
            }
            // check for duplicate MPRNs.
            CheckDuplicateMPRNS(request.MPRNs);
            return string.Empty;
        }
        /// <summary>
        /// Validates Incident before creates
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static void ValidateIncidentCreate(IncidentRequest request)
        {
            var result = dbContext.Incidents.Any(i => (i.Status == 0 || i.Status == null) && i.Deleted == false);
            if (result)
            {
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.INPROGRESS_INCIDENT_EXISTS);
                throw new HttpResponseException(response);
            }
            //check for duplicate MPRNs:
            CheckDuplicateMPRNS(request.MPRNs);
        }
        private static void CheckDuplicateMPRNS(List<PropertyRequest> MPRNList)
        {
            if (MPRNList != null)
            {
                var duplicateMPRNs = MPRNList.GroupBy(x => x.MPRN).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
                if (duplicateMPRNs != null && duplicateMPRNs.Count > 0)
                {
                    dynamic response = new ExpandoObject();
                    response.message = ErrorCodes.DUPLICATE_MPRNS_FOUND_IN_REQ.ToString();
                    response.MPRNS = string.Join(",", duplicateMPRNs);
                    string output = JsonConvert.SerializeObject(response);
                    var httpResponse = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, output.ToString());
                    throw new HttpResponseException(httpResponse);
                }
            }

        }

        /// <summary>
        /// Validates USers Existance 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="dbCellManagers"></param>
        /// <param name="dbZoneManagers"></param>
        /// <returns></returns>
        public static void ValidateIncidentManagers(List<PropertyRequest> MPRNs, List<User> dbCellManagers, List<User> dbZoneManagers, List<ResolveUser> resolveUsers)
        {
            List<string> notFoundUsers = new List<string>();
            List<User> duplicateUsers = new List<User>();


            string status = string.Empty;
            if (MPRNs != null)
            {
                foreach (var itemMPRN in MPRNs)
                {
                    if (!string.IsNullOrEmpty(itemMPRN.CellManagerName))
                    {
                        var cellManager = dbCellManagers.Where(item => (item.FirstName + " " + item.LastName).ToUpper() == itemMPRN.CellManagerName.ToUpper()).ToList();
                        if (cellManager == null || cellManager.Count == 0)
                        {
                            notFoundUsers.Add(itemMPRN.CellManagerName);
                        }
                        else if (cellManager.Count > 1)
                        {
                            ResolveUser resolvedUser = null;
                            if (resolveUsers != null)
                                resolvedUser = resolveUsers.FirstOrDefault(i => i.Name == cellManager.First().FirstName + " " + cellManager.First().LastName);
                            if (resolvedUser == null)
                            {
                                duplicateUsers.AddRange(cellManager);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(itemMPRN.ZoneManagerName))
                    {
                        var zoneManager = dbZoneManagers.Where(item => (item.FirstName + " " + item.LastName).ToUpper() == itemMPRN.ZoneManagerName.ToUpper()).ToList();
                        if (zoneManager == null || zoneManager.Count == 0)
                        {
                            notFoundUsers.Add(itemMPRN.ZoneManagerName);
                        }
                        else if (zoneManager.Count > 1)
                        {
                            ResolveUser resolvedUser = null;
                            if (resolveUsers != null && resolveUsers.Count > 0)
                                resolvedUser = resolveUsers.FirstOrDefault(i => i.Name == zoneManager.First().FirstName + " " + zoneManager.First().LastName);
                            if (resolvedUser == null)
                            {
                                duplicateUsers.AddRange(zoneManager);
                            }
                        }
                    }
                }
            }
            if (notFoundUsers.Count > 0)
            {
                dynamic result = new ExpandoObject();
                result.message = ErrorCodes.USER_DOES_NOT_EXISTS.ToString();
                result.users = notFoundUsers.Distinct();
                string output = JsonConvert.SerializeObject(result);

                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, output);
                throw new HttpResponseException(response);
            }
            if (duplicateUsers.Count > 0)
            {
                dynamic result = new ExpandoObject();
                result.message = ErrorCodes.DUPLICATE_USERS_FOUND.ToString();
                result.users = duplicateUsers.Select(i => new { Name = i.FirstName + " " + i.LastName, i.Email }).Distinct();
                string output = JsonConvert.SerializeObject(result);
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, output.ToString());
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// Validates the Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal static string ValidateModel(Object model)
        {
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, context, results);
            StringBuilder modelValidationErrors = new StringBuilder();

            if (!isValid)
            {
                modelValidationErrors.Append(Constants.STR_INVALID_USER);
                foreach (var validationResult in results)
                {
                    modelValidationErrors.Append(validationResult.ErrorMessage);
                    modelValidationErrors.AppendLine();
                }
                return modelValidationErrors.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Validation for the User Role Mapping
        /// </summary>
        public static void ValidateDomain(DomainRequest request)
        {
            var dbDomain = dbContext.Domains.FirstOrDefault(item => (item.DomainName == request.DomainName || item.OrgName == request.OrgName) && !item.Deleted);
            if (dbDomain != null)
            {
                if (dbDomain.DomainName == request.DomainName)
                {
                    var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.DOMAIN_EXISTS_ALREADY);
                    throw new HttpResponseException(response);
                }
                else
                {
                    var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.ORG_EXISTS_ALREADY);
                    throw new HttpResponseException(response);
                }
            }

        }
        /// <summary>
        /// Validation for the User Role Mapping
        /// </summary>
        public static void ValidateForUserRoleMap(UserRoleMapRequest request)
        {
            var dbRole = dbContext.Roles.FirstOrDefault(item => item.Id == request.RoleId);
            if (dbRole == null)
            {
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.ROLE_DOES_NOT_EXISTS);
                throw new HttpResponseException(response);
            }
            var dbUser = dbContext.Users.FirstOrDefault(item => item.Id == request.UserId);
            if (dbUser == null)
            {
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.USER_DOES_NOT_EXISTS);
                throw new HttpResponseException(response);
            }
            var dbUserRoleMap = dbContext.UserRoleMaps.FirstOrDefault(item => item.RoleId == request.RoleId && item.UserId == request.UserId && !item.Deleted);
            if (dbUserRoleMap != null)
            {
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.USER_ROLE_MAP_EXISTS);
                throw new HttpResponseException(response);
            }
        }
        /// <summary>
        /// Validation for the User Role Mapping
        /// </summary>
        public static void ValidateAssignUnAssignUsersToRole(UsersToRoleRequest request)
        {
            var dbRole = dbContext.Roles.FirstOrDefault(item => item.Id == request.RoleId);
            if (dbRole == null)
            {
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.ROLE_DOES_NOT_EXISTS);
                throw new HttpResponseException(response);
            }
            //if (request.UserIds == null || request.UserIds.Count()==0)
            //{
            //    var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.USER_ID_RQD);
            //    throw new HttpResponseException(response);
            //}
            if (dbRole.RoleName.ToUpper() == "ENGINEER")
            {
                foreach (var item in request.UserIds)
                {
                    //  dbContext.Users.Where(item => request.UserIds.Contains(item.Id));
                    var dbUser = dbContext.Users.FirstOrDefault(i => i.Id == item);
                    if (dbUser.EUSR == null)
                    {
                        var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.EUSR_DOES_NOT_EXISTS);
                        throw new HttpResponseException(response);
                    }
                }
            }
        }
        /// <summary>
        /// Validation for the User Role Mapping
        /// </summary>
        public static void ValidateAssignUnAssignRolesToUser(RolesToUserRequest request)
        {
            var dbUser = dbContext.Users.FirstOrDefault(item => item.Id == request.UserId);
            if (dbUser == null)
            {
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.USER_DOES_NOT_EXISTS);
                throw new HttpResponseException(response);
            }
            foreach (var item in request.RoleIds)
            {
                
                if (dbUser.EUSR == null && dbContext.Roles.Where(i=>i.Id == item).FirstOrDefault().RoleName.ToUpper()=="ENGINEER")
                {
                    var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.EUSR_DOES_NOT_EXISTS);
                    throw new HttpResponseException(response);
                }
            }
        }
        /// <summary>
        /// Validate the Edit USer details
        /// </summary>
        /// <param name="request"></param>
        public static void ValidateEditUserRequest(string userId,UserEditRequest request, List<string> ChangedProps)
        {
            User user = dbContext.Users.FirstOrDefault(i => i.Id == userId);
            if (user == null)
            {
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.USER_DOES_NOT_EXISTS);
                throw new HttpResponseException(response);
            }
            if (string.IsNullOrEmpty(request.FirstName) && ChangedProps.Contains("FirstName"))
            {
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.USER_FIRSTNAME_REQ);
                throw new HttpResponseException(response);
            }
            if (string.IsNullOrEmpty(request.LastName) && ChangedProps.Contains("LastName"))
            {
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.USER_LASTNAME_REQ);
                throw new HttpResponseException(response);
            }
            //check for inprogress assignments.
            if (request.IsActiveUser == false)
            {
                //Abhijeet 06-11-2018
                //Remove PropertyUserMap in Query

                //var assignmentCount = (from pum in user.PropertyUserMaps.Where(i => i.Deleted == false).ToList()
                //                       join pus in user.PropertyUserStatus.Where(i => i.Deleted == false).ToList()
                //                       on pum.Id equals pus.PropertyUserMapsId into temp
                //                       from lj in temp.DefaultIfEmpty()
                //                       where (lj == null || lj.StatusId == null) && pum.Property.Incident.Status == 0
                //                       select new { pum.PropertyId, pum.UserId }).Distinct().Count();

                var assignmentCount = (from pus in user.PropertyUserStatus.Where(i => i.Deleted == false && i.StatusId == null).ToList()
                       
                    where pus.Property.Incident.Status == 0
                    select new { pus.PropertyId, pus.UserId }).Distinct().Count();
                if (assignmentCount > 0)
                {
                    var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.UNASSIGN_MPRNs_BEFORE_DEACTIVATE);
                    throw new HttpResponseException(response);
                }
            }
        }
        /// <summary>
        /// Validate the Edit USer details
        /// </summary>
        /// <param name="request"></param>
        public static void ValidateEditDomainRequest(DomainRequest request, List<string> ChangedProps)
        {
            var dbDomain = dbContext.Domains.FirstOrDefault(item => item.Id == request.DomainName);
            if (dbDomain == null && ChangedProps.Contains("DomainName"))
            {
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.DOMAIN_MOD_NOT_ALLOWED);
                throw new HttpResponseException(response);
            }
        }
        /// <summary>
        /// Validate User Details while login
        /// </summary>
        /// <param name="request"></param>
        public static void ValidateUserForLogin(LoginRequest request, User account)
        {
            if (account == null)
            {
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.USER_DOES_NOT_EXISTS);
                throw new HttpResponseException(response);
            }
            var dbDomain = (from dom in dbContext.Domains
                            join usr in dbContext.Users on dom.Id equals usr.DomainId
                            where usr.Email == request.Email 
                            select dom).FirstOrDefault();
            if (dbDomain == null || dbDomain.IsActive == false)
            {
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.DOMAIN_IS_INACTIVE);
                throw new HttpResponseException(response);
            }
        }
        /// <summary>
        /// Validate the request before sending the activation email.
        /// </summary>
        /// <param name="userId"></param>
        public static void ValidateUserforActivationLink(string userId, User user)
        {
            if (user == null)
            {
                // user does not exists.
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.USER_DOES_NOT_EXISTS);
                throw new HttpResponseException(response);
            }
            if (user != null && user.IsActivated)
            {
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.USER_ALREADY_ACTIVATED);
                throw new HttpResponseException(response);
            }
        }
        /// <summary>
        /// Validate the request before sending the activation email.
        /// </summary>
        /// <param name="userId"></param>
        public static void ValidateUserforNewPassword(string userId, User user)
        {
            if (user == null)
            {
                // user does not exists.
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.USER_DOES_NOT_EXISTS);
                throw new HttpResponseException(response);
            }
           else if (user != null && user.IsActivated == false)
            {
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.USER_NOT_ACVTD);
                throw new HttpResponseException(response);
            }
            else if (user != null && user.IsActiveUser == false)
            {
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.USER_DEACTIVATED_BY_ADMIN);
                throw new HttpResponseException(response);
            }
        }

        ///// <summary>
        ///// Validates the request before MPRN Assignments
        ///// </summary>
        //public static void ValidateMPRNAssignment(List<AssignMPRNsRequest> reqList)
        //{
        //    //check for existing mapping..Uncomment once UI passes the RoleId
        //    var invalidUsrRoleMapLst = from req in reqList.Where(i => i.IsUnAssign == false)
        //                               join urm in dbContext.UserRoleMaps.Where(i => i.Deleted == false)
        //                                     on new { req.UserId, req.RoleId } equals new { urm.UserId, urm.RoleId }
        //                               into tempurm
        //                               from urmleftjoin in tempurm.DefaultIfEmpty()
        //                               where urmleftjoin == null || urmleftjoin.Id == null
        //                               select new
        //                               {
        //                                   UserName = dbContext.Users.Where(i => i.Id == req.UserId).Select(i => i.FirstName + " " + i.LastName).FirstOrDefault(),
        //                                   RolName = dbContext.Roles.Where(i => i.Id == req.RoleId).Select(i => i.RoleName).FirstOrDefault()
        //                               };
        //    if (invalidUsrRoleMapLst != null && invalidUsrRoleMapLst.Count() > 0)
        //    {
        //        dynamic result = new ExpandoObject();
        //        result.message = ErrorCodes.USER_ROLE_MAP_DOES_NOT_EXISTS.ToString();
        //        result.UserRoles = invalidUsrRoleMapLst.Select(i => new { UserName = i.UserName, i.RolName }).Distinct();
        //        string output = JsonConvert.SerializeObject(result);
        //        var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, output.ToString());
        //        throw new HttpResponseException(response);
        //    }

        //    //check for already assignment and which does not have status..
        //    //If no mapping found then assign can happen.
        //    var existingMapping = from req in reqList.Where(i => i.IsUnAssign == false)
        //                          join pum in dbContext.PropertyUserMap.Where(pum => pum.Deleted == false)
        //                                on new { req.PropertyId, req.UserId, req.RoleId } equals new { pum.PropertyId, pum.UserId, pum.RoleId }
        //                          join pus in dbContext.PropertyUserStatus.Where(pus => pus.Deleted == false)
        //                                 on pum.Id equals pus.PropertyUserMapsId
        //                          into temppus
        //                          from pusleftjoin in temppus.DefaultIfEmpty()
        //                          where pusleftjoin == null || pusleftjoin.StatusId == null
        //                          select new
        //                          {
        //                              pum.PropertyId,
        //                              pum.Property.MPRN,
        //                              pum.User.FirstName,
        //                              pum.User.LastName,
        //                              pum.User.Email,
        //                              pum.Role.RoleName
        //                          };
        //    //find any existing MPRN assignments
        //    if (existingMapping != null && existingMapping.Count() > 0)
        //    {
        //        dynamic result = new ExpandoObject();
        //        result.message = ErrorCodes.USER_MPRN_MAPPING_EXISTS.ToString();
        //        result.UserMPRNs = existingMapping.Select(i => new { i.PropertyId, i.MPRN, UserName = i.FirstName + " " + i.LastName, i.Email, i.RoleName }).Distinct();
        //        string output = JsonConvert.SerializeObject(result);
        //        var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, output.ToString());
        //        throw new HttpResponseException(response);
        //    }
        //    // unassignement can happen only when inprogress assignment found.
        //    var inprogressMappings = from req in reqList.Where(i => i.IsUnAssign == true)
        //                             join pum in dbContext.PropertyUserMap.Where(pum => pum.Deleted == false)
        //                                   on new { req.PropertyId, req.UserId, req.RoleId } equals new { pum.PropertyId, pum.UserId, pum.RoleId }
        //                             join pus in dbContext.PropertyUserStatus.Where(pus => pus.Deleted == false)
        //                                    on pum.Id equals pus.PropertyUserMapsId
        //                             into temppus
        //                             from pusleftjoin in temppus.DefaultIfEmpty()
        //                             where pusleftjoin == null || pusleftjoin.StatusId == null
        //                             select new
        //                             {
        //                                 pum.PropertyId,
        //                                 pum.Property.MPRN,
        //                                 pum.User.FirstName,
        //                                 pum.User.LastName,
        //                                 pum.User.Email,
        //                                 pum.Role.RoleName
        //                             };
        //    if (inprogressMappings != null && inprogressMappings.Count() != reqList.Where(i => i.IsUnAssign == true).Count())
        //    {
        //        dynamic result = new ExpandoObject();
        //        result.message = ErrorCodes.NO_INPROGRESS_USER_MPRN_MAPPING.ToString();
        //        string output = JsonConvert.SerializeObject(result);
        //        var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, output.ToString());
        //        throw new HttpResponseException(response);
        //    }
        //}

        /// <summary>
        /// Validates the request before MPRN Assignments
        /// </summary>
        public static void ValidateMPRNAssignment(List<AssignMPRNsRequest> reqList)
        {
            //check for existing mapping..Uncomment once UI passes the RoleId
            var invalidUsrRoleMapLst = from req in reqList.Where(i => i.IsUnAssign == false)
                                       join urm in dbContext.UserRoleMaps.Where(i => i.Deleted == false)
                                             on new { req.UserId, req.RoleId } equals new { urm.UserId, urm.RoleId }
                                       into tempurm
                                       from urmleftjoin in tempurm.DefaultIfEmpty()
                                       where urmleftjoin == null || urmleftjoin.Id == null
                                       select new
                                       {
                                           UserName = dbContext.Users.Where(i => i.Id == req.UserId).Select(i => i.FirstName + " " + i.LastName).FirstOrDefault(),
                                           RolName = dbContext.Roles.Where(i => i.Id == req.RoleId).Select(i => i.RoleName).FirstOrDefault()
                                       };
            if (invalidUsrRoleMapLst != null && invalidUsrRoleMapLst.Count() > 0)
            {
                dynamic result = new ExpandoObject();
                result.message = ErrorCodes.USER_ROLE_MAP_DOES_NOT_EXISTS.ToString();
                result.UserRoles = invalidUsrRoleMapLst.Select(i => new { UserName = i.UserName, i.RolName }).Distinct();
                string output = JsonConvert.SerializeObject(result);
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, output.ToString());
                throw new HttpResponseException(response);
            }

            //check for already assignment and which does not have status..
            //If no mapping found then assign can happen.
            var existingMapping = from req in reqList.Where(i => i.IsUnAssign == false)
                                  join pus in dbContext.PropertyUserStatus.Where(pus => pus.Deleted == false)
                                        on new { req.PropertyId, req.UserId, req.RoleId } equals new { pus.PropertyId, pus.UserId, pus.RoleId }


                                  where pus == null || pus.StatusId == null
                                  select new
                                  {
                                      pus.PropertyId,
                                      pus.Property.MPRN,
                                      pus.User.FirstName,
                                      pus.User.LastName,
                                      pus.User.Email,
                                      pus.Role.RoleName
                                  };
            //find any existing MPRN assignments
            if (existingMapping != null && existingMapping.Count() > 0)
            {
                dynamic result = new ExpandoObject();
                result.message = ErrorCodes.USER_MPRN_MAPPING_EXISTS.ToString();
                result.UserMPRNs = existingMapping.Select(i => new { i.PropertyId, i.MPRN, UserName = i.FirstName + " " + i.LastName, i.Email, i.RoleName }).Distinct();
                string output = JsonConvert.SerializeObject(result);
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, output.ToString());
                throw new HttpResponseException(response);
            }
            // unassignement can happen only when inprogress assignment found.
            var inprogressMappings = from req in reqList.Where(i => i.IsUnAssign == true)
                                     join pus in dbContext.PropertyUserStatus.Where(pum => pum.Deleted == false)
                                           on new { req.PropertyId, req.UserId, req.RoleId } equals new { pus.PropertyId, pus.UserId, pus.RoleId }

                                     where pus == null || pus.StatusId == null
                                     select new
                                     {
                                         pus.PropertyId,
                                         pus.Property.MPRN,
                                         pus.User.FirstName,
                                         pus.User.LastName,
                                         pus.User.Email,
                                         pus.Role.RoleName
                                     };
            if (inprogressMappings != null && inprogressMappings.Count() != reqList.Where(i => i.IsUnAssign == true).Count())
            {
                dynamic result = new ExpandoObject();
                result.message = ErrorCodes.NO_INPROGRESS_USER_MPRN_MAPPING.ToString();
                string output = JsonConvert.SerializeObject(result);
                var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, output.ToString());
                throw new HttpResponseException(response);
            }
        }
    }
}
