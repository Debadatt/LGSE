using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;
using LGSE_APIService.RequestObjects;
using AutoMapper;
using System;
using LGSE_APIService.Utilities;
using System.Collections.Generic;
using LGSE_APIService.Common;
using LGSE_APIService.Common.Utilities;
using LGSE_APIService.Authorization;

namespace LGSE_APIService.Controllers
{
    public class PropertyUserStatusController : TableController<PropertyUserStatus>
    {
        LGSE_APIContext context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = LGSE_APIContext.GetIntance();
            DbUtilities.dbContext = context;
            DomainManager = new EntityDomainManager<PropertyUserStatus>(context, Request);
        }

        // GET tables/PropertyUserStatus
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.ASSIGNEDMPRN }, OperationType = OperationType.READ)]
        public IQueryable<PropertyUserStatus> GetAllPropertyUserStatus()
        {
            try
            {
                string userEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                List<string> roles = HttpUtilities.GetRolesFromToken(this.Request);
                User usr = DbUtilities.GetUserByEmail(userEmail);
                //Need to optimise the query
                if (roles.Contains("Engineer") || roles.Contains("Isolator") || roles.Contains("engineer") || roles.Contains("isolator"))
                {
                    var result = context.PropertyUserStatus.Where(i => i.UserId == usr.Id && i.Property.Incident.Status == 0);
                    return result.AsQueryable();
                }
                else
                {
                    return Query();
                }
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.ASSIGNEDMPRN }, OperationType = OperationType.READ)]
        // GET tables/PropertyUserStatus/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PropertyUserStatus> GetPropertyUserStatus(string id)
        {
            try
            {
                return Lookup(id);
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.ASSIGNEDMPRN }, OperationType = OperationType.UPDATE)]
        // PATCH tables/PropertyUserStatus/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<PropertyUserStatus> PatchPropertyUserStatus(string id, Delta<PropertyUserStatus> patch)
        {
            try
            {
                return UpdateAsync(id, patch);
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.ASSIGNEDMPRN }, OperationType = OperationType.CREATE)]
        // POST tables/PropertyUserStatus
        //public async Task<IHttpActionResult> PostPropertyUserStatus(PropertyUserStatus item)
        //{
        //    try
        //    {
        //        User account = GetLoggedInUser();
        //        //  item.RoleId = DbUtilities.GetUserPreferredRole(account.Email).Id;
        //        string userRole = HttpUtilities.GetUserRoleAccessApi(this.Request); // Added 24-10-2018
        //        var preferredRole = context.Roles.FirstOrDefault(i => i.Id == userRole); // Added 24-10-2018
        //        item.RoleId = preferredRole.Id;
        //        //Mapper.Initialize(cfg => cfg.CreateMap<PropertyUserStatusRequest, PropertyUserStatus>()
        //        //.ForMember(i => i.CreatedBy,j => j.UseValue(account.Email)).ForMember(i=>i.StatusChangedOn,j=>j.UseValue(DateTime.Now))
        //        //.ForMember(i => i.UserId, j => j.UseValue(account.Id))
        //        //);
        //        //var PropertyUserStatusMap = Mapper.Map<PropertyUserStatusRequest, PropertyUserStatus>(item);
        //        item.StatusChangedOn = DateTime.UtcNow;
        //        var propUserMap = context.PropertyUserMap.OrderByDescending(i => i.CreatedAt).FirstOrDefault(i => i.UserId == account.Id && i.PropertyId == item.PropertyId && i.RoleId == item.RoleId);
        //        if (propUserMap != null)
        //        {
        //            item.PropertyUserMapsId = propUserMap.Id;
        //        }
        //        item.CreatedBy = account.Email;
        //        PropertyUserStatus current = await InsertAsync(item);
        //        CreatedAtRoute("Tables", new { id = current.Id }, current);
        //        var IsIsaolated = context.PropertyStatusMstr.Where(i => i.Id == item.StatusId).Select(i => i.Status).FirstOrDefault();
        //        if (IsIsaolated == "Isolated")
        //        {
        //            var prop = context.Properties.Where(i => i.Id == item.PropertyId).ToList();
        //            if (prop != null && prop.Count > 0)
        //            {

        //                foreach (var propItem in prop)
        //                {
        //                    propItem.StatusId = item.StatusId;
        //                    propItem.SubStatusId = item.PropertySubStatusMstrsId;
        //                    propItem.IsIsolated = true;
        //                    propItem.UpdatedAt = DateTimeOffset.UtcNow;
        //                    propItem.ModifiedBy = account.Email;
        //                }

        //            }
        //            //context.SaveChanges();
        //        }
        //        else
        //        {
        //            var prop = context.Properties.Where(i => i.Id == item.PropertyId).ToList();
        //            if (prop != null && prop.Count > 0)
        //            {

        //                foreach (var propItem in prop)
        //                {
        //                    propItem.StatusId = item.StatusId;
        //                    propItem.SubStatusId = item.PropertySubStatusMstrsId;
        //                    propItem.UpdatedAt = DateTimeOffset.UtcNow;
        //                    propItem.ModifiedBy = account.Email;
        //                }

        //            }

        //        }
        //        context.SaveChanges();
        //        //var IncidentId = context.Properties.FirstOrDefault(i => i.Id == item.PropertyId).IncidentId;
        //        //DbUtilities.PropertyStatusCount(IncidentId);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        HttpUtilities.ServerError(ex, Request);
        //        return null;
        //    }

        //}
        public async Task<IHttpActionResult> PostPropertyUserStatus(PropertyUserStatus item)
        {
            try
            {
                User account = GetLoggedInUser();
                //  item.RoleId = DbUtilities.GetUserPreferredRole(account.Email).Id;
                string userRole = HttpUtilities.GetUserRoleAccessApi(this.Request); // Added 24-10-2018
                var preferredRole = context.Roles.FirstOrDefault(i => i.Id == userRole); // Added 24-10-2018
                item.RoleId = preferredRole.Id;

                //var pusResult = context.PropertyUserStatus.Where(i => i.PropertyId == item.PropertyId && i.UserId == item.UserId && i.RoleId == item.RoleId && i.StatusId == null && i.Notes == null && i.Deleted == false).ToList();
                var pusResult = context.PropertyUserStatus.Where(i => i.Id==item.PropertyUserMapsId).ToList();

                if (pusResult != null && pusResult.Count > 0)
                {

                    foreach (var pusItem in pusResult)
                    {
                        pusItem.StatusId = item.StatusId;
                        pusItem.PropertySubStatusMstrsId = item.PropertySubStatusMstrsId;
                        pusItem.Notes = item.Notes;
                        pusItem.StatusChangedOn = item.StatusChangedOn;
                        pusItem.ModifiedBy = account.Email;
                    }

                
                var prop = context.Properties.Where(i => i.Id == item.PropertyId).ToList();
                if (prop != null && prop.Count > 0)
                {
                  
                    var IsIsaolated = context.PropertyStatusMstr.Where(i => i.Id == item.StatusId).Select(i => i.Status).FirstOrDefault();
                    if (IsIsaolated == "Isolated")
                    {
                        foreach (var propItem in prop)
                        {
                            propItem.StatusId = item.StatusId;
                            propItem.SubStatusId = item.PropertySubStatusMstrsId;
                            propItem.IsIsolated = true;
                            propItem.UpdatedAt = DateTimeOffset.UtcNow;
                            propItem.ModifiedBy = account.Email;
                        }
                    }
                    else
                    {
                        foreach (var propItem in prop)
                        {
                            propItem.StatusId = item.StatusId;
                            propItem.SubStatusId = item.PropertySubStatusMstrsId;
                            propItem.UpdatedAt = DateTimeOffset.UtcNow;
                            propItem.ModifiedBy = account.Email;
                        }
                    }
                }
                    //context.SaveChanges();



                    //    var IsIsaolated = context.PropertyStatusMstr.Where(i => i.Id == item.StatusId).Select(i => i.Status).FirstOrDefault();
                    //if (IsIsaolated == "Isolated")
                    //{

                    //    }
                    //    //context.SaveChanges();
                    //}
                    //else
                    //{
                    //    var prop = context.Properties.Where(i => i.Id == item.PropertyId).ToList();
                    //    if (prop != null && prop.Count > 0)
                    //    {

                    //        foreach (var propItem in prop)
                    //        {
                    //            propItem.StatusId = item.StatusId;
                    //            propItem.SubStatusId = item.PropertySubStatusMstrsId;
                    //            propItem.UpdatedAt = DateTimeOffset.UtcNow;
                    //            propItem.ModifiedBy = account.Email;
                    //        }

                    //    }

                }
                context.SaveChanges();
                //var IncidentId = context.Properties.FirstOrDefault(i => i.Id == item.PropertyId).IncidentId;
                //DbUtilities.PropertyStatusCount(IncidentId);
                return Ok();
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }

        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.ASSIGNEDMPRN }, OperationType = OperationType.DELETE)]
        // DELETE tables/PropertyUserStatus/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePropertyUserStatus(string id)
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

        /// <summary>
        /// Load user about with details of current user who sent http request
        /// TODO: If "account" is null, return empty object instead of null
        /// </summary>
        /// <returns>User object</returns>
        private User GetLoggedInUser()
        {
            string userEmail = HttpUtilities.GetUserNameFromToken(this.Request);
            //TODO: What happens if account is null?
            User account = context.Users.Where(a => a.Email == userEmail).SingleOrDefault();
            return account;
        }
    }
}
