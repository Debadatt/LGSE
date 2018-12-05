using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;
using LGSE_APIService.Authorization;
using LGSE_APIService.Common.Utilities;
using System;
using LGSE_APIService.Utilities;

namespace LGSE_APIService.Controllers
{
    public class RoleStatusMapController : TableController<RoleStatusMap>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            LGSE_APIContext context = LGSE_APIContext.GetIntance();
            DomainManager = new EntityDomainManager<RoleStatusMap>(context, Request);
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.READ)]
        // GET tables/RoleStatusMap
        public IQueryable<RoleStatusMap> GetAllRoleStatusMap()
        {
            try
            {
                return Query();
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.READ)]
        // GET tables/RoleStatusMap/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<RoleStatusMap> GetRoleStatusMap(string id)
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
        // PATCH tables/RoleStatusMap/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<RoleStatusMap> PatchRoleStatusMap(string id, Delta<RoleStatusMap> patch)
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
        // POST tables/RoleStatusMap
        public async Task<IHttpActionResult> PostRoleStatusMap(RoleStatusMap item)
        {
            try
            {
                RoleStatusMap current = await InsertAsync(item);
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
        // DELETE tables/RoleStatusMap/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteRoleStatusMap(string id)
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
