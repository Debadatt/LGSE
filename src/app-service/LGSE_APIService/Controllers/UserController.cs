using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;
using LGSE_APIService.ResponseObjects;
using System.Collections.Generic;
using AutoMapper;
using LGSE_APIService.Common;
using LGSE_APIService.Utilities;
using LGSE_APIService.RequestObjects;
using System;
using LGSE_APIService.Common.Utilities;
using LGSE_APIService.Authorization;
using System.Configuration;

namespace LGSE_APIService.Controllers
{
    public class UserController : TableController<User>
    {
        LGSE_APIContext context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = LGSE_APIContext.GetIntance();
            DbUtilities.dbContext = context;
            ValidationUtilities.dbContext = context;
            DomainManager = new EntityDomainManager<User>(context, Request);
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT, Features.RESOURCEMGT }, OperationType = OperationType.READ)]
        // GET tables/User
        [EnableQuery]
        //public IQueryable<UserResponse> GetAllUser()
        //{
        //    try
        //    {
        //        int assignedFlag = HttpUtilities.GetUserAccessApi(this.Request);
        //        if (assignedFlag == 0)
        //        {
        //            var userResp = from user in context.Users
        //                           select new UserResponse
        //                           {
        //                               ContactNo = user.ContactNo,
        //                               Email = user.Email,
        //                               EmployeeId = user.EmployeeId,
        //                               EUSR = user.EUSR,
        //                               FirstName = user.FirstName,
        //                               LastName = user.LastName,
        //                               Id = user.Id,
        //                               IsAvailable = user.IsAvailable,
        //                               Organization = user.Domain.OrgName,
        //                               Roles = user.UserRoleMap.Where(i => i.Deleted == false).Select(i => i.Role.RoleName).ToList(),
        //                               // Consider the ressigned MPRNS as well if the status is not updated.
        //                               // Dont consider the reassignment.Count should be at MPRN level not at the incidnet level.
        //                               // Show the count only for inprogress incidents.
        //                               //Inprogress = (from pum in user.PropertyUserMaps.Where(i => i.Deleted == false)
        //                               //              join pus in user.PropertyUserStatus.Where(i => i.Deleted == false)
        //                               //              on pum.Id equals pus.PropertyUserMapsId into temp
        //                               //              from lj in temp.DefaultIfEmpty()
        //                               //              where (lj == null || lj.StatusId == null) && pum.Property.Incident.Status == 0
        //                               //              select new { pum.PropertyId, pum.UserId, pum.RoleId }
        //                               //            ).Distinct().Count(),

        //                               //Completed = (from pum in user.PropertyUserMaps.Where(i => i.Deleted == false)
        //                               //             join pus in user.PropertyUserStatus.Where(i => i.Deleted == false)
        //                               //             on pum.Id equals pus.PropertyUserMapsId
        //                               //             where pum.Property.Incident.Status == 0
        //                               //             select new { pus.PropertyId, pum.UserId, pum.RoleId }
        //                               //            ).Distinct().Count(),
        //                               ////we can remove this if UI can use Inprogress property itself.
        //                               //IsAssigned = (from pum in user.PropertyUserMaps.Where(i => i.Deleted == false)
        //                               //              join pus in user.PropertyUserStatus.Where(i => i.Deleted == false)
        //                               //              on pum.Id equals pus.PropertyUserMapsId into temp
        //                               //              from lj in temp.DefaultIfEmpty()
        //                               //              where lj.StatusId == null && pum.Property.Incident.Status == 0
        //                               //              select new { pum.PropertyId, pum.UserId, pum.RoleId }
        //                               //            ).Distinct().Count() > 0 ? true : false,
        //                               ////We can remove if UI uses the Inprogress count
        //                               //AssignedMPRNCount = (from pum in user.PropertyUserMaps.Where(i => i.Deleted == false)
        //                               //                     join pus in user.PropertyUserStatus.Where(i => i.Deleted == false)
        //                               //                     on pum.Id equals pus.PropertyUserMapsId into temp
        //                               //                     from lj in temp.DefaultIfEmpty()
        //                               //                     where (lj.StatusId == null) && pum.Property.Incident.Status == 0
        //                               //                     select new { pum.PropertyId, pum.UserId, pum.RoleId }
        //                               //            ).Distinct().Count(),


        //                               //Zones = user.PropertyUserMaps.Where(i => i.Deleted == false && i.Property.Incident.Status == 0).Select(i => i.Property.Zone).Distinct().ToList(),
        //                               //Cells = user.PropertyUserMaps.Where(i => i.Deleted == false && i.Property.Incident.Status == 0).Select(i => i.Property.Cell).Distinct().ToList(),

        //                               //PreferredRoleId = user.UserRoleMap.Count(i => i.Deleted == false) > 1 ?
        //                               //user.UserRoleMap.Where(i => i.IsPreferredRole == true).Select(i => i.Role.Id).FirstOrDefault() :
        //                               //user.UserRoleMap.Where(i => i.Deleted == false).Select(i => i.Role.Id).FirstOrDefault(),
        //                               //PreferredRole = user.UserRoleMap.Count(i => i.Deleted == false) > 1 ?
        //                               //user.UserRoleMap.Where(i => i.IsPreferredRole == true).Select(i => i.Role.RoleName).FirstOrDefault() :
        //                               //user.UserRoleMap.Where(i => i.Deleted == false).Select(i => i.Role.RoleName).FirstOrDefault()
        //                               //,


        //                               IsLoggedIn = user.IsLoggedIn,
        //                               IsActive = user.IsActiveUser,
        //                               CreatedAt = user.CreatedAt
        //                               ,
        //                               CreatedBy = user.CreatedBy
        //                               ,
        //                               ModifiedBy = user.ModifiedBy
        //                               ,
        //                               UpdatedAt = user.UpdatedAt,
        //                               IsActivated = user.IsActivated
        //                           };
        //            return userResp.AsQueryable();
        //        }
        //        else
        //        {
        //           // var incident = context.Incidents.Where(i => i.Status == 0).FirstOrDefault();
        //            //var incidentId = incident != null ? incident.Id : null;
        //            var users = from user in context.Users.Where(i => i.Deleted == false && i.IsLoggedIn)
        //                        join urm in context.UserRoleMaps.Where(u => !u.Deleted && u.IsPreferredRole == true)
        //                         on user.Id equals urm.UserId
        //                        select new UserResponse
        //                        {
        //                            ContactNo = user.ContactNo,
        //                            Email = user.Email,
        //                            EmployeeId = user.EmployeeId,
        //                            EUSR = user.EUSR,
        //                            FirstName = user.FirstName,
        //                            LastName = user.LastName,
        //                            Id = user.Id,
        //                            IsAvailable = user.IsAvailable,
        //                          //  Organization = user.Domain.OrgName,
        //                           // Roles = user.UserRoleMap.Where(i => i.Deleted == false).Select(i => i.Role.RoleName).ToList(),
        //                            // Consider the ressigned MPRNS as well if the status is not updated.
        //                            // Dont consider the reassignment.Count should be at MPRN level not at the incidnet level.
        //                            // Show the count only for inprogress incidents.
        //                            //Inprogress =  (user.PropertyUserMaps.Count(i =>  i.Property.Incident.Status == 0 && i.RoleId==urm.RoleId && i.Deleted == false)- user.PropertyUserStatus.Count(i => i.Property.Incident.Status == 0 && i.RoleId == urm.RoleId && i.Deleted == false)),

        //                                //(from pum in user.PropertyUserMaps.Where(i => i.Deleted == false && i.RoleId == urm.RoleId)
        //                                //          join pus in user.PropertyUserStatus.Where(i => i.Deleted == false && i.RoleId == urm.RoleId)
        //                                //          on pum.Id equals pus.PropertyUserMapsId
        //                                //         into temp
        //                                //          from lj in temp.DefaultIfEmpty()
        //                                //          where (lj == null || lj.StatusId == null) && pum.Property.Incident.Status == 0
        //                                //          select new { pum.PropertyId, pum.UserId, pum.RoleId }
        //                                //            ).Distinct().Count(),

        //                            Completed = //user.PropertyUserStatus.Count(i => i.Property.Incident.Status == 0 && i.RoleId == urm.RoleId && i.Deleted == false),
        //                                //Abhijeet 06-11-2018
        //                                //Remove PropertyUserMap in Query
        //                            (from pum in user.PropertyUserMaps.Where(i => i.Deleted == false && i.RoleId == urm.RoleId)
        //                             join pus in user.PropertyUserStatus.Where(i => i.Deleted == false && i.RoleId == urm.RoleId)
        //                             on pum.Id equals pus.PropertyUserMapsId
        //                             where pum.Property.Incident.Status == 0
        //                             select new { pus.PropertyId, pum.UserId, pum.RoleId }
        //                                        ).Distinct().Count(),
        //                            //we can remove this if UI can use Inprogress property itself.
        //                            //IsAssigned = (from pum in user.PropertyUserMaps.Where(i => i.Deleted == false && i.RoleId == urm.RoleId)
        //                            //              join pus in user.PropertyUserStatus.Where(i => i.Deleted == false && i.RoleId == urm.RoleId)
        //                            //              on pum.Id equals pus.PropertyUserMapsId into temp
        //                            //              from lj in temp.DefaultIfEmpty()
        //                            //              where lj.StatusId == null && pum.Property.Incident.Status == 0
        //                            //              select new { pum.PropertyId, pum.UserId, pum.RoleId }
        //                            //                ).Distinct().Count() > 0 ? true : false,
        //                            ////We can remove if UI uses the Inprogress count
        //                            AssignedMPRNCount = //(user.PropertyUserMaps.Count(i => i.Property.Incident.Status == 0 && i.RoleId == urm.RoleId && i.Deleted == false) - user.PropertyUserStatus.Count(i => i.Property.Incident.Status == 0 && i.RoleId == urm.RoleId && i.Deleted == false)),
        //                                (from pum in user.PropertyUserMaps.Where(i => i.Deleted == false && i.RoleId == urm.RoleId)
        //                                 join pus in user.PropertyUserStatus.Where(i => i.Deleted == false && i.RoleId == urm.RoleId)
        //                                 on pum.Id equals pus.PropertyUserMapsId
        //                                into temp
        //                                 from lj in temp.DefaultIfEmpty()
        //                                 where (lj == null || lj.StatusId == null) && pum.Property.Incident.Status == 0
        //                                 select new { pum.PropertyId, pum.UserId, pum.RoleId }
        //                                            ).Distinct().Count(),
        //                            //(from pum in user.PropertyUserMaps.Where(i => i.Deleted == false && i.RoleId == urm.RoleId)
        //                            //                 join pus in user.PropertyUserStatus.Where(i => i.Deleted == false && i.RoleId == urm.RoleId)
        //                            //                 on pum.Id equals pus.PropertyUserMapsId into temp
        //                            //                 from lj in temp.DefaultIfEmpty()
        //                            //                 where (lj.StatusId == null) && pum.Property.Incident.Status == 0
        //                            //                 select new { pum.PropertyId, pum.UserId, pum.RoleId }
        //                            //            ).Distinct().Count(),

        //                            IsActive = user.IsActiveUser,
        //                           Zones = user.PropertyUserMaps.Where(i => i.Deleted == false && i.Property.Incident.Status == 0 && i.RoleId == urm.RoleId).Select(i => i.Property.Zone).Distinct().ToList(),
        //                           Cells = user.PropertyUserMaps.Where(i => i.Deleted == false && i.Property.Incident.Status == 0 && i.RoleId == urm.RoleId).Select(i => i.Property.Cell).Distinct().ToList(),
        //                            IsLoggedIn = user.IsLoggedIn,
        //                            PreferredRoleId = urm.RoleId,
        //                            PreferredRole = urm.Role.RoleName,
        //                            CreatedAt = user.CreatedAt
        //                                ,
        //                            CreatedBy = user.CreatedBy
        //                                ,
        //                            ModifiedBy = user.ModifiedBy
        //                                ,
        //                            UpdatedAt = user.UpdatedAt,
        //                            IsActivated = user.IsActivated

        //                        };
        //            return users.AsQueryable();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HttpUtilities.ServerError(ex, Request);
        //        return null;
        //    }
        //}

        public IQueryable<UserResponse> GetAllUser()
        {
            try
            {
                int assignedFlag = HttpUtilities.GetUserAccessApi(this.Request);
                if (assignedFlag == 0)
                {
                    var userResp = from user in context.Users
                                   select new UserResponse
                                   {
                                       ContactNo = user.ContactNo,
                                       Email = user.Email,
                                       EmployeeId = user.EmployeeId,
                                       EUSR = user.EUSR,
                                       FirstName = user.FirstName,
                                       LastName = user.LastName,
                                       Id = user.Id,
                                       IsAvailable = user.IsAvailable,
                                       Organization = user.Domain.OrgName,
                                       Roles = user.UserRoleMap.Where(i => i.Deleted == false).Select(i => i.Role.RoleName).ToList(),
                                       // Consider the ressigned MPRNS as well if the status is not updated.
                                       // Dont consider the reassignment.Count should be at MPRN level not at the incidnet level.
                                       // Show the count only for inprogress incidents.
                                       //Inprogress = (from pum in user.PropertyUserMaps.Where(i => i.Deleted == false)
                                       //              join pus in user.PropertyUserStatus.Where(i => i.Deleted == false)
                                       //              on pum.Id equals pus.PropertyUserMapsId into temp
                                       //              from lj in temp.DefaultIfEmpty()
                                       //              where (lj == null || lj.StatusId == null) && pum.Property.Incident.Status == 0
                                       //              select new { pum.PropertyId, pum.UserId, pum.RoleId }
                                       //            ).Distinct().Count(),

                                       //Completed = (from pum in user.PropertyUserMaps.Where(i => i.Deleted == false)
                                       //             join pus in user.PropertyUserStatus.Where(i => i.Deleted == false)
                                       //             on pum.Id equals pus.PropertyUserMapsId
                                       //             where pum.Property.Incident.Status == 0
                                       //             select new { pus.PropertyId, pum.UserId, pum.RoleId }
                                       //            ).Distinct().Count(),
                                       ////we can remove this if UI can use Inprogress property itself.
                                       //IsAssigned = (from pum in user.PropertyUserMaps.Where(i => i.Deleted == false)
                                       //              join pus in user.PropertyUserStatus.Where(i => i.Deleted == false)
                                       //              on pum.Id equals pus.PropertyUserMapsId into temp
                                       //              from lj in temp.DefaultIfEmpty()
                                       //              where lj.StatusId == null && pum.Property.Incident.Status == 0
                                       //              select new { pum.PropertyId, pum.UserId, pum.RoleId }
                                       //            ).Distinct().Count() > 0 ? true : false,
                                       ////We can remove if UI uses the Inprogress count
                                       //AssignedMPRNCount = (from pum in user.PropertyUserMaps.Where(i => i.Deleted == false)
                                       //                     join pus in user.PropertyUserStatus.Where(i => i.Deleted == false)
                                       //                     on pum.Id equals pus.PropertyUserMapsId into temp
                                       //                     from lj in temp.DefaultIfEmpty()
                                       //                     where (lj.StatusId == null) && pum.Property.Incident.Status == 0
                                       //                     select new { pum.PropertyId, pum.UserId, pum.RoleId }
                                       //            ).Distinct().Count(),


                                       //Zones = user.PropertyUserMaps.Where(i => i.Deleted == false && i.Property.Incident.Status == 0).Select(i => i.Property.Zone).Distinct().ToList(),
                                       //Cells = user.PropertyUserMaps.Where(i => i.Deleted == false && i.Property.Incident.Status == 0).Select(i => i.Property.Cell).Distinct().ToList(),

                                       //PreferredRoleId = user.UserRoleMap.Count(i => i.Deleted == false) > 1 ?
                                       //user.UserRoleMap.Where(i => i.IsPreferredRole == true).Select(i => i.Role.Id).FirstOrDefault() :
                                       //user.UserRoleMap.Where(i => i.Deleted == false).Select(i => i.Role.Id).FirstOrDefault(),
                                       //PreferredRole = user.UserRoleMap.Count(i => i.Deleted == false) > 1 ?
                                       //user.UserRoleMap.Where(i => i.IsPreferredRole == true).Select(i => i.Role.RoleName).FirstOrDefault() :
                                       //user.UserRoleMap.Where(i => i.Deleted == false).Select(i => i.Role.RoleName).FirstOrDefault()
                                       //,


                                       IsLoggedIn = user.IsLoggedIn,
                                       IsActive = user.IsActiveUser,
                                       CreatedAt = user.CreatedAt
                                       ,
                                       CreatedBy = user.CreatedBy
                                       ,
                                       ModifiedBy = user.ModifiedBy
                                       ,
                                       UpdatedAt = user.UpdatedAt,
                                       IsActivated = user.IsActivated
                                   };
                    return userResp.AsQueryable();
                }
                else
                {
                    var incident = context.Incidents.Where(i => i.Status == 0).FirstOrDefault();
                    var incidentId = incident != null ? incident.Id : null;
                    var users = from user in context.Users.Where(i => i.Deleted == false && i.IsLoggedIn)
                                join urm in context.UserRoleMaps.Where(u => !u.Deleted && u.IsPreferredRole == true)
                                 on user.Id equals urm.UserId
                                select new UserResponse
                                {
                                    ContactNo = user.ContactNo,
                                    Email = user.Email,
                                    EmployeeId = user.EmployeeId,
                                    EUSR = user.EUSR,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    Id = user.Id,
                                    IsAvailable = user.IsAvailable,
                                    //  Organization = user.Domain.OrgName,
                                    // Roles = user.UserRoleMap.Where(i => i.Deleted == false).Select(i => i.Role.RoleName).ToList(),
                                    // Consider the ressigned MPRNS as well if the status is not updated.
                                    // Dont consider the reassignment.Count should be at MPRN level not at the incidnet level.
                                    // Show the count only for inprogress incidents.
                                    //Inprogress =  (user.PropertyUserMaps.Count(i =>  i.Property.Incident.Status == 0 && i.RoleId==urm.RoleId && i.Deleted == false)- user.PropertyUserStatus.Count(i => i.Property.Incident.Status == 0 && i.RoleId == urm.RoleId && i.Deleted == false)),

                                    //(from pum in user.PropertyUserMaps.Where(i => i.Deleted == false && i.RoleId == urm.RoleId)
                                    //          join pus in user.PropertyUserStatus.Where(i => i.Deleted == false && i.RoleId == urm.RoleId)
                                    //          on pum.Id equals pus.PropertyUserMapsId
                                    //         into temp
                                    //          from lj in temp.DefaultIfEmpty()
                                    //          where (lj == null || lj.StatusId == null) && pum.Property.Incident.Status == 0
                                    //          select new { pum.PropertyId, pum.UserId, pum.RoleId }
                                    //            ).Distinct().Count(),

                                    Completed = //user.PropertyUserStatus.Count(i => i.Property.Incident.Status == 0 && i.RoleId == urm.RoleId && i.Deleted == false),
                                                //Abhijeet 06-11-2018
                                                //Remove PropertyUserMap in Query
                                    (from pus in user.PropertyUserStatus.Where(i => i.Deleted == false && i.RoleId == urm.RoleId && i.StatusId != null)
                                     where pus.IncidentId == incidentId
                                     select new { pus.PropertyId, pus.UserId, pus.RoleId }
                                                ).Distinct().Count(),
                                    //we can remove this if UI can use Inprogress property itself.
                                    //IsAssigned = (from pum in user.PropertyUserMaps.Where(i => i.Deleted == false && i.RoleId == urm.RoleId)
                                    //              join pus in user.PropertyUserStatus.Where(i => i.Deleted == false && i.RoleId == urm.RoleId)
                                    //              on pum.Id equals pus.PropertyUserMapsId into temp
                                    //              from lj in temp.DefaultIfEmpty()
                                    //              where lj.StatusId == null && pum.Property.Incident.Status == 0
                                    //              select new { pum.PropertyId, pum.UserId, pum.RoleId }
                                    //                ).Distinct().Count() > 0 ? true : false,
                                    ////We can remove if UI uses the Inprogress count
                                    AssignedMPRNCount = //(user.PropertyUserMaps.Count(i => i.Property.Incident.Status == 0 && i.RoleId == urm.RoleId && i.Deleted == false) - user.PropertyUserStatus.Count(i => i.Property.Incident.Status == 0 && i.RoleId == urm.RoleId && i.Deleted == false)),
                                        (from pus in user.PropertyUserStatus.Where(i => i.Deleted == false && i.RoleId == urm.RoleId && i.StatusId == null)
                                         where pus.IncidentId == incidentId
                                         select new { pus.PropertyId, pus.UserId, pus.RoleId }
                                                    ).Distinct().Count(),
                                    //(from pum in user.PropertyUserMaps.Where(i => i.Deleted == false && i.RoleId == urm.RoleId)
                                    //                 join pus in user.PropertyUserStatus.Where(i => i.Deleted == false && i.RoleId == urm.RoleId)
                                    //                 on pum.Id equals pus.PropertyUserMapsId into temp
                                    //                 from lj in temp.DefaultIfEmpty()
                                    //                 where (lj.StatusId == null) && pum.Property.Incident.Status == 0
                                    //                 select new { pum.PropertyId, pum.UserId, pum.RoleId }
                                    //            ).Distinct().Count(),

                                    IsActive = user.IsActiveUser,

                                    Zones = user.PropertyUserStatus.Where(i => i.Deleted == false && i.IncidentId == incidentId  && i.RoleId == urm.RoleId).Select(i => i.Property.Zone).Distinct().ToList(),
                                    Cells = user.PropertyUserStatus.Where(i => i.Deleted == false && i.IncidentId == incidentId && i.RoleId == urm.RoleId).Select(i => i.Property.Cell).Distinct().ToList(),
                                    IsLoggedIn = user.IsLoggedIn,
                                    PreferredRoleId = urm.RoleId,
                                    PreferredRole = urm.Role.RoleName,
                                    CreatedAt = user.CreatedAt
                                        ,
                                    CreatedBy = user.CreatedBy
                                        ,
                                    ModifiedBy = user.ModifiedBy
                                        ,
                                    UpdatedAt = user.UpdatedAt,
                                    IsActivated = user.IsActivated

                                };
                    return users.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT, Features.RESOURCEMGT }, OperationType = OperationType.READ)]
        //public string GetRoles(List<string> roles)
        //{
        //   return string.Join(",", roles);
        //}
        // GET tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<UserResponse> GetUser(string id)
        {
            try
            {
                var resultQuery = context.Users.Where(i => i.Id == id).ToList()
                   .Select(i => new UserResponse()
                   {
                       FirstName = i.FirstName,
                       LastName = i.LastName,
                       EmployeeId = i.EmployeeId,
                       EUSR = i.EUSR,
                       Email = i.Email,
                       ContactNo = i.ContactNo,
                       IsActive = i.IsActiveUser,
                       IsActivated = i.IsActivated,
                       ActivationOTP = (string.IsNullOrEmpty(i.OTPCode) || i.OTPGeneratedAt.Value.AddMinutes(Convert.ToInt16(ConfigurationManager.AppSettings["OTP_TIMESPAN"]))
                       < DateTimeOffset.UtcNow) ? string.Empty : i.OTPCode
                   });

                SingleResult<UserResponse> result = new SingleResult<UserResponse>(resultQuery.AsQueryable());
                return result;
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.UPDATE)]
        // PATCH tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task PatchUser(string id, Delta<UserEditRequest> patch)
        {
            try
            {
                //Admins can also modify user.
                ValidationUtilities.ValidateEditUserRequest(id, patch.GetEntity(), patch.GetChangedPropertyNames().ToList());
                string requesterEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                var user = context.Users.FirstOrDefault(i => i.Email == requesterEmail);
                if (string.IsNullOrEmpty(requesterEmail) || user == null)
                {
                    var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, Common.Utilities.ErrorCodes.INVALID_TOKEN, string.Empty);
                    throw new HttpResponseException(response);
                }
                Delta<User> deltaDest = new Delta<User>();
                foreach (var item in patch.GetChangedPropertyNames())
                {
                    object result;
                    patch.TryGetPropertyValue(item, out result);
                    deltaDest.TrySetPropertyValue(item, result);
                }
                deltaDest.TrySetPropertyValue("ModifiedBy", requesterEmail);
                deltaDest.TrySetPropertyValue("UpdatedAt", DateTimeOffset.UtcNow);
                return UpdateAsync(id, deltaDest);
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }

        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.CREATE)]
        // POST tables/User
        public IHttpActionResult PostUser(RegisterRequest request)
        {
            try
            {
                string errorMessage = ValidationUtilities.ValidateUserDetails(request);
                var domainObj = DbUtilities.GetDomainDetails(request.Email);
                if (errorMessage.Equals(string.Empty))
                {
                    string otpCode = AuthorizationUtilities.GenerateOTPCode();
                    string userEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                    DbUtilities.SaveTheUser(request, otpCode, domainObj, userEmail, request.IsActiveUser);
                    AuthorizationUtilities.SendOTPtoUser(request.Email, otpCode);
                    return Ok(HttpUtilities.CustomResp(ErrorCodes.USER_CREATED.ToString()));
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
            //return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.DELETE)]
        // DELETE tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUser(string id)
        {
            try
            {
                return DeleteAsync(id);
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }
    }
}
