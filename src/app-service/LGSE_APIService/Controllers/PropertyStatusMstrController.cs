using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;
using System.Collections.Generic;
using LGSE_APIService.Utilities;
using LGSE_APIService.Common;
using LGSE_APIService.Authorization;
using LGSE_APIService.Common.Utilities;
using System;

namespace LGSE_APIService.Controllers
{
    public class PropertyStatusMstrController : TableController<PropertyStatusMstr>
    {
        LGSE_APIContext context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = LGSE_APIContext.GetIntance();
            DbUtilities.dbContext = context;
            DomainManager = new EntityDomainManager<PropertyStatusMstr>(context, Request);
        }

        // GET tables/PropertyStatusMstr
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT, Features.RESOURCEMGT, Features.ASSIGNEDMPRN,Features.PORTALMANAGEMENT }, OperationType = OperationType.READ)]
        public IQueryable<PropertyStatusMstr> GetAllPropertyStatusMstr()
        {
            try
            {
                string userEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                // List<string> roles = HttpUtilities.GetRolesFromToken(this.Request);
                string userRole = HttpUtilities.GetUserRoleAccessApi(this.Request);// Added 24-10-2018
                var preferredRole = context.Roles.FirstOrDefault(i => i.Id == userRole);// Added 24-10-2018
                //Filter the status based on mapping
                if (preferredRole.RoleName.ToUpper() == "ENGINEER" || preferredRole.RoleName.ToUpper() == "ISOLATOR")
                {
                    //Role preferredRole = DbUtilities.GetUserPreferredRole(userEmail);
                    //if (preferredRole != null)
                    //{
                    var result = from rsm in context.RoleStatusMaps
                                 join psm in context.PropertyStatusMstr on rsm.StatusId equals psm.Id
                                 where rsm.RoleId == preferredRole.Id
                                 select psm;
                    return result.AsQueryable();
                    //}
                    //else
                    //{
                    //    //Preferred role doe snot exists so dont return any data.
                    //    return new List<PropertyStatusMstr>().AsQueryable();
                    //}
                }
                else
                {
                    //return all status for the users.
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
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.READ)]
        // GET tables/PropertyStatusMstr/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PropertyStatusMstr> GetPropertyStatusMstr(string id)
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
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.UPDATE)]
        // PATCH tables/PropertyStatusMstr/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<PropertyStatusMstr> PatchPropertyStatusMstr(string id, Delta<PropertyStatusMstr> patch)
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
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.CREATE)]
        // POST tables/PropertyStatusMstr
        public async Task<IHttpActionResult> PostPropertyStatusMstr(PropertyStatusMstr item)
        {
            try
            {
                PropertyStatusMstr current = await InsertAsync(item);
                return CreatedAtRoute("Tables", new { id = current.Id }, current);
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.DELETE)]
        // DELETE tables/PropertyStatusMstr/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePropertyStatusMstr(string id)
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
