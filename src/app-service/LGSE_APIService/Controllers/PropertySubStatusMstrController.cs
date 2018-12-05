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
    public class PropertySubStatusMstrController : TableController<PropertySubStatusMstr>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            LGSE_APIContext context = LGSE_APIContext.GetIntance();
            DomainManager = new EntityDomainManager<PropertySubStatusMstr>(context, Request);
        }

        // GET tables/PropertySubStatusMstr
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT, Features.RESOURCEMGT, Features.ASSIGNEDMPRN, Features.PORTALMANAGEMENT }, OperationType = OperationType.READ)]
        public IQueryable<PropertySubStatusMstr> GetAllPropertySubStatusMstr()
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
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT }, OperationType = OperationType.READ)]
        // GET tables/PropertySubStatusMstr/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PropertySubStatusMstr> GetPropertySubStatusMstr(string id)
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
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT }, OperationType = OperationType.UPDATE)]
        // PATCH tables/PropertySubStatusMstr/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<PropertySubStatusMstr> PatchPropertySubStatusMstr(string id, Delta<PropertySubStatusMstr> patch)
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
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT }, OperationType = OperationType.CREATE)]
        // POST tables/PropertySubStatusMstr
        public async Task<IHttpActionResult> PostPropertySubStatusMstr(PropertySubStatusMstr item)
        {
            try
            {
                PropertySubStatusMstr current = await InsertAsync(item);
                return CreatedAtRoute("Tables", new { id = current.Id }, current);
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT }, OperationType = OperationType.DELETE)]
        // DELETE tables/PropertySubStatusMstr/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePropertySubStatusMstr(string id)
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
