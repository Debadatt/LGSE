using LGSE_APIService.Common.Utilities;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;
using LGSE_APIService.RequestObjects;
using LGSE_APIService.ResponseObjects;
using LGSE_APIService.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace LGSE_APIService.Common
{
    public static class DbUtilities
    {
        /// <summary>
        /// Mobile Services context 
        /// </summary>
        public static LGSE_APIContext dbContext { get; set; }

        /// <summary>
        /// Checks User existance
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsUserExists(string email)
        {
            var user = dbContext.Users.SingleOrDefault(i => i.Email == email);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Gets user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static User GetUserByEmail(string email)
        {
            var user = dbContext.Users.SingleOrDefault(i => i.Email == email);
            return user;
        }

        /// <summary>
        /// Load user about with details of current user who sent http request
        /// TODO: If "account" is null, return empty object instead of null
        /// </summary>
        /// <returns>User object</returns>
        public static User GetLoggedInUser(HttpRequestMessage request)
        {
            string userEmail = HttpUtilities.GetUserNameFromToken(request);
            //TODO: What happens if account is null?
            User account = dbContext.Users.Where(a => a.Email == userEmail).SingleOrDefault();
            return account;
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
            else if (dbUser != null && dbUser.Domain.IsActive == false)
                statusMsg = ErrorCodes.DOMAIN_IS_INACTIVE.ToString();
            else
                statusMsg = Constants.SUCCESS_MSG;
            return statusMsg;
        }
        /// <summary>
        /// Validates user domain against whitelisted domain
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidateUserDomain(string email)
        {
            var dbDomain = GetDomainDetails(email);
            if (dbDomain != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Gets Domain Details of user
        /// </summary>
        /// <returns></returns>
        public static Domain GetDomainDetails(string email)
        {
            string domain = email.Split('@')[1];
            var dbDomain = dbContext.Domains.SingleOrDefault(i => i.DomainName == domain && i.Deleted == false && i.IsActive);
            return dbDomain;
        }
        /// <summary>
        /// Get USer objects by usernames
        /// </summary>
        /// <param name="inputUsers"></param>
        /// <returns></returns>
        public static IQueryable<User> GetUsersByNames(List<string> inputUsers)
        {
            var users = dbContext.Users.Where(item => inputUsers.Contains(item.FirstName+" "+item.LastName));
            return users;

        }
        
        /// <summary>
        /// Get Incident Detials
        /// </summary>
        /// <param name="inputUsers"></param>
        /// <returns></returns>
        public static Incident GetIncidentDetails(string incidentId)
        {
            var dbIncident = dbContext.Incidents.FirstOrDefault(item=>item.Id==incidentId);
            return dbIncident;
        }

        /// <summary>
        /// Get existing MPRNS.
        /// </summary>
        /// <param name="InputMPRNS"></param>
        /// <param name="incidentId"></param>
        /// <returns></returns>
        public static IQueryable<Property> GetExistingMPRNs(List<string> InputMPRNS,string incidentId)
        {
            var MPRNs = dbContext.Properties.Where(item => InputMPRNS.Contains(item.MPRN) && item.IncidentId== incidentId && item.Deleted==false);
            return MPRNs;
        }

        /// <summary>
        /// Saves the user into the db
        /// </summary>
        /// <param name="user"></param>
        /// <param name="OTPCode"></param>
        public static void SaveTheUser(RegisterRequest user, string OTPCode, Domain domainObj,string createdBy,bool IsActiveUser)
        {
            try
            {
                User newAccount = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CreatedBy = createdBy,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ContactNo = user.ContactNo,
                    EmployeeId = user.EmployeeId,
                    OTPGeneratedAt = DateTimeOffset.UtcNow,
                    EUSR = user.EUSR,
                    PwdStartDate = DateTimeOffset.UtcNow,
                    DomainId = domainObj.Id,
                    IsActiveUser= IsActiveUser,
                    //Salt = salt,
                    OTPCode = OTPCode,
                    // Password = AuthorizationUtilities.hash(user.Password, salt),
                };
                var savedUser = dbContext.Users.Add(newAccount);
                if (!string.IsNullOrEmpty(user.RoleId))
                {
                    dbContext.UserRoleMaps.Add(new UserRoleMap()
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoleId = user.RoleId,
                        UserId = savedUser.Id,
                        CreatedBy = user.Email,
                        CreatedAt = DateTimeOffset.UtcNow
                    });
                }
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Get Incident Detials
        /// </summary>
        /// <param name="inputUsers"></param>
        /// <returns></returns>
        public static Role GetUserPreferredRole(string email)
        {
                       //if single Role is present then return that.
            var allRoles = (from usr in dbContext.Users.Where(i => i.Email == email && i.Deleted==false).ToList()
                         join urm in dbContext.UserRoleMaps.Where(i=>i.Deleted==false).ToList() on usr.Id equals urm.UserId
                         select new 
                         {
                             Id = urm.Role.Id,
                             RoleName = urm.Role.RoleName,
                             urm.IsPreferredRole
                         }).ToList();

            if (allRoles != null && allRoles.Count > 1)
            {
                var result= allRoles.Where(i => i.IsPreferredRole == true).Select(i => new Role { Id = i.Id, RoleName = i.RoleName }).FirstOrDefault();
                if (result == null)
                {
                    //PReferred Role is not set by the user.
                    var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.USER_PREFERED_ROLE_NOTSET);
                    throw new HttpResponseException(response);
                }
                else
                {
                    return result;
                }
            }
            else
                return allRoles.Select(i => new Role { Id = i.Id, RoleName = i.RoleName }).FirstOrDefault();
        }
        /// <summary>
        /// Addes entry to the Audit trial
        /// </summary>
        /// <param name="account"></param>
        /// <param name="Status"></param>
        /// <param name="operationType"></param>
        /// <param name="RequestContext"></param>
        public static void AuditTrialEntry(User account, AuditTrialStatus Status, AuditTrialOpType operationType, HttpRequestMessage requestMessage, string roleId = null,string tokenId=null)
        {
            
            var context = requestMessage.Properties["MS_OwinContext"] as Microsoft.Owin.OwinContext;
            AuditTrial operationLogObj = new AuditTrial()
            {
                Id = (Guid.NewGuid()).ToString(),
                UserId = account.Id,
                RoleId= roleId,
                TokenId= tokenId,
                //DeviceId = HttpUtilities.GetDeviceIDFromHeader(Request, account.Email),
                TypeofOperation = operationType.ToString(),
                Status = Status.ToString(),
                OperationTimeStamp = DateTimeOffset.UtcNow,
                CreatedBy = account.Email,
                //ModifiedBy = account.Id,
                Deleted = false,
                IPAddress = context.Request.RemoteIpAddress
            };
            dbContext.AuditTrials.Add(operationLogObj);
            dbContext.SaveChanges();
            //Enable this when account locking mechanism required.
            //var operationsobj = dbContext.AuditTrials.Where(item => item.UserId == account.Id && item.TypeofOperation == Constants.LOGIN_OPERATION
            //                                                && item.OperationTimeStamp >= account.UpdatedAt && account.ModifiedBy == account.Email
            //                                                && item.IsOffline == false)
            //                                    .OrderByDescending(item => item.OperationTimeStamp)
            //                                    .Take(3);

            //if (operationsobj.Count() > 0)
            //{
            //    if (operationsobj.Where(item => item.Status == Constants.FAILURE).Count() == 5)
            //    {
            //        account.IsLocked = true;
            //        _context.Entry(account).State = System.Data.Entity.EntityState.Modified;
            //        _context.SaveChanges();

            //    }
            //}
        }
        public static void PropertyStatusCount(string id)
        {
      
            List<string> dummyList = new List<string>();
            dummyList.Add("Temp for union");
            List<PropsStatusCountsResp> count = ((from t in dummyList
                                                  select new PropsStatusCountsResp
                                                  {
                                                      Status = "No Status",
                                                      // statusId = "NoStatus", //Get the count which does not have status
                                                      Count = dbContext.Properties.Count(i => i.PropertyUserStatus.Where(j => j.Deleted == false).Count() == 0 && i.IncidentId == id && i.Deleted == false),
                                                      ShortText = "NS",
                                                      DisplayOrder = -1
                                                  }).Union
                                                        (from psm in dbContext.PropertyStatusMstr.Where(i => i.Deleted == false).OrderBy(i => i.DisplayOrder)
                                                         select new PropsStatusCountsResp
                                                         {
                                                             Status = psm.Status,
                                                             //statusId = psm.Id,
                                                             Count = psm.PropertyUserStatus.Where(i => i.Property.IncidentId == id
                                                                              && i.Deleted == false
                                                                              && i.StatusId == (dbContext.PropertyUserStatus.
                                                                              Where(pus => pus.Property.IncidentId == id && i.Deleted == false)
                                                                                        .OrderByDescending(k => k.StatusChangedOn)
                                                                                         .FirstOrDefault(k => k.PropertyId == i.PropertyId
                                                                                         ).StatusId
                                                                                        ) // if multiple status exists for the property then lake the latest count by oredering.
                                                                         )
                                                                        .Select(i => new
                                                                        {
                                                                            i.PropertyId,
                                                                            i.StatusId
                                                                        }).Distinct().Count(),
                                                             ShortText = psm.ShortText,
                                                             DisplayOrder = psm.DisplayOrder
                                                         }).OrderBy(i => i.DisplayOrder)
                                                         ).ToList<PropsStatusCountsResp>();

            var incidentdata = dbContext.IncidentPropsStatusCounts.FirstOrDefault(i => i.IncidentId == id);
            if (incidentdata != null)
            {
                //Mark them as deleted:
                foreach (var item in count)
                {

                    if (item.ShortText == "NS")
                        incidentdata.NS = item.Count;
                    else if (item.ShortText == "NC")
                        incidentdata.NC = item.Count;
                    else if (item.ShortText == "IS")
                        incidentdata.IS = item.Count;
                    else if (item.ShortText == "RS")
                        incidentdata.RS = item.Count;
                    else if (item.ShortText == "NA")
                        incidentdata.NA = item.Count;
                }

            }
            dbContext.Entry(incidentdata).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChanges();



        }
    }
}