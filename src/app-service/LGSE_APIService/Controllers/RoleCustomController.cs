using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using AutoMapper;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;
using LGSE_APIService.RequestObjects;
using LGSE_APIService.Utilities;
using LGSE_APIService.Validators;
using Microsoft.Azure.Mobile.Server.Config;
using LGSE_APIService.ResponseObjects;
using LGSE_APIService.Authorization;
using LGSE_APIService.Common.Utilities;

namespace LGSE_APIService.Controllers
{
    [MobileAppController]
    public class RoleCustomController : ApiController
    {
        LGSE_APIContext context;
        public RoleCustomController()
        {
            context = LGSE_APIContext.GetIntance();
            ValidationUtilities.dbContext = context;
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT}, OperationType = OperationType.CREATE)]
        [HttpPost]
        [Route("api/RoleCustom/AssignUnAssignUsersToRole")]
        [ValidateModel]
        public IHttpActionResult AssignUnAssignUsersToRole(UsersToRoleRequest requestList)
        {
            try
            {
                string currentUserEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                ValidationUtilities.ValidateAssignUnAssignUsersToRole(requestList);
                //Get Existing users of the role
                List<UserRoleMap> dbexistingUsers = context.UserRoleMaps.Where(i => i.RoleId == requestList.RoleId && i.Deleted == false).ToList();
                foreach (var item in requestList.UserIds)
                {
                    if (dbexistingUsers.FirstOrDefault(i => i.UserId == item && i.RoleId == requestList.RoleId) == null)
                    {
                        // if does not exists then only add it.
                        context.UserRoleMaps.Add(new UserRoleMap()
                        {
                            Id = Guid.NewGuid().ToString(),
                            RoleId = requestList.RoleId,
                            UserId = item,
                            CreatedBy = currentUserEmail
                        });
                    }
                }
                //remove existing mapping if they dont exists in the input
                foreach (var itemUser in dbexistingUsers)
                {
                    if (requestList != null && requestList.UserIds != null)
                    {
                        if (!requestList.UserIds.Contains(itemUser.UserId))
                        {
                            itemUser.Deleted = true;
                            context.Entry(itemUser).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                }
                context.SaveChanges();
                return Ok();
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
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT}, OperationType = OperationType.CREATE)]
        [HttpPost]
        [Route("api/RoleCustom/AssignUnAssignRolesToUser")]
        [ValidateModel]
        public IHttpActionResult AssignUnAssignRolesToUser(RolesToUserRequest requestList)
        {
            try
            {
                string currentUserEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                ValidationUtilities.ValidateAssignUnAssignRolesToUser(requestList);
                //Get Existing users of the role
                List<UserRoleMap> dbexistingRoles = context.UserRoleMaps.Where(i => i.UserId == requestList.UserId && i.Deleted == false).ToList();
                foreach (var itemRole in requestList.RoleIds)
                {
                    if (dbexistingRoles.FirstOrDefault(i => i.RoleId == itemRole && i.UserId == requestList.UserId) == null)
                    {
                        // if does not exists then only add it.
                        context.UserRoleMaps.Add(new UserRoleMap()
                        {
                            Id = Guid.NewGuid().ToString(),
                            RoleId = itemRole,
                            UserId = requestList.UserId,
                            CreatedBy = currentUserEmail
                        });
                    }
                }
                //remove existing mapping if they dont exists in the input
                foreach (var itemRole in dbexistingRoles)
                {
                    if (requestList != null && requestList.RoleIds != null)
                    {
                        if (!requestList.RoleIds.Contains(itemRole.RoleId))
                        {
                            itemRole.Deleted = true;
                            context.Entry(itemRole).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                }
                context.SaveChanges();
                return Ok();
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
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT}, OperationType = OperationType.READ)]
        [HttpGet]
        [Route("api/RoleCustom/GetRoles")]
        [ValidateModel]
        public IHttpActionResult GetRolesWithUsers(string userId)
        {
            try
            {
                var roles = from rol in context.Roles.Where(r => r.Deleted == false)
                            join urm in context.UserRoleMaps.Where(u => !u.Deleted)
                            on rol.Id equals urm.RoleId
                            where urm.UserId == userId
                            select new RoleResponse
                            {
                                Id = rol.Id,
                                RoleName = rol.RoleName,
                                Description = rol.Description,
                                //Users = (from urm in rol.UserRoleMap.Where(i => i.RoleId == rol.Id && i.Deleted == false)
                                //         join usr in context.Users.Where(i => i.Deleted == false)
                                //         on urm.UserId equals usr.Id
                                //         select new RoleUsersResponse
                                //         {
                                //             Id = usr.Id,
                                //             Email = usr.Email,
                                //             FirstName = usr.FirstName,
                                //             LastName = usr.LastName
                                //         }).ToList()
                            };
                return Ok(roles);
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
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT}, OperationType = OperationType.READ)]
        [HttpGet]
        [Route("api/RoleCustom/GetUsers")]
        [ValidateModel]
        public IHttpActionResult GetUsersWithRoles(string roleId)
        {
            try
            {
                var user = from usr in context.Users.Where(i => i.Deleted == false)
                           join urm in context.UserRoleMaps.Where(u => !u.Deleted)
                            on usr.Id equals urm.UserId
                           where urm.RoleId == roleId
                           select new UserResponseSub
                           {
                               Id = usr.Id,
                               Email = usr.Email,
                               FirstName = usr.FirstName,
                               LastName = usr.LastName,
                               //Roles = (from urm in usr.UserRoleMap.Where(i => i.UserId == usr.Id && i.Deleted == false)
                               //         join role in context.Roles.Where(i => i.Deleted == false)
                               //         on urm.RoleId equals role.Id
                               //         select new RoleResponseSub
                               //         {
                               //             Id = role.Id,
                               //             Description = role.Description,
                               //             RoleName = role.RoleName
                               //         }).ToList()
                           };
                return Ok(user);
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
        //  [CustomAuthorize(Module = Features.PORTALMANAGEMENT, OperationType = OperationType.READ)]
        [HttpGet]
        [Route("api/RoleCustom/GetRolePermissions")]
        [ValidateModel]
        public IHttpActionResult GetRolePermissions(string roleId)
        {
            try
            {
                var result = from feat in context.Features
                             join rolePerm in context.RolePermissions.Where(r => r.RoleId == roleId)
                             on feat.Id equals rolePerm.FeatureId into rp
                             from rpjoin in rp.DefaultIfEmpty()
                                 //where (roleId=="NULL" || rpjoin == null || rpjoin.RoleId==null|| rpjoin.RoleId==roleId)
                             select new RoleFeaturePermResp
                             {
                                 FeatureId = feat.Id.Trim(),
                                 FeatureText = feat.FeatureText,
                                 FeatureName = feat.FeatureName,
                                 RoleId = rpjoin.RoleId,
                                 RoleName = rpjoin.Role.RoleName,
                                 CreatePermission = rpjoin.CreatePermission.Contains("A") ? true : false,
                                 ReadPermission = rpjoin.ReadPermission.Contains("A") ? true : false,
                                 UpdatePermission = rpjoin.UpdatePermission.Contains("A") ? true : false
                             };
                return Ok(result.AsQueryable());
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT}, OperationType = OperationType.CREATE)]
        [HttpPost]
        [Route("api/RoleCustom/SetRolePermissions")]
        public IHttpActionResult SetRolePermissions(List<RolePermReq> requestList)
        {
            try
            {
                string currentUserEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                foreach (var item in requestList)
                {
                    var existingPerm = context.RolePermissions.
                        FirstOrDefault(i => i.FeatureId == item.FeatureId && i.RoleId == item.RoleId);
                    if (existingPerm != null)
                    {

                        existingPerm.CreatePermission = item.CreatePermission == true ? "A" : "N";
                        existingPerm.UpdatePermission = item.UpdatePermission == true ? "A" : "N";
                        existingPerm.ReadPermission = item.ReadPermission == true ? "A" : "N";
                        existingPerm.UpdatedAt = DateTime.UtcNow;
                        existingPerm.ModifiedBy = currentUserEmail;
                        context.Entry(existingPerm).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        context.RolePermissions.Add(
                            new RolePermission()
                            {
                                FeatureId = item.FeatureId,
                                RoleId = item.RoleId,
                                CreatePermission = item.CreatePermission == true ? "A" : "N",
                                ReadPermission = item.ReadPermission == true ? "A" : "N",
                                UpdatePermission = item.UpdatePermission == true ? "A" : "N",
                                DeletePermission = "N",
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = currentUserEmail,
                                Id = Guid.NewGuid().ToString()
                            }
                            );
                    }
                    context.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }
    }
}
