using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;
using LGSE_APIService.Common;
using LGSE_APIService.ResponseObjects;
using LGSE_APIService.RequestObjects;
using System;
using LGSE_APIService.Utilities;
using LGSE_APIService.Authorization;
using LGSE_APIService.Common.Utilities;

namespace LGSE_APIService.Controllers
{
    public class IncidentOverviewMstrController : TableController<IncidentOverviewMstr>
    {
        LGSE_APIContext context = null;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = LGSE_APIContext.GetIntance();
            DbUtilities.dbContext = context;
            DomainManager = new EntityDomainManager<IncidentOverviewMstr>(context, Request, enableSoftDelete: true);
        }

        // GET tables/IncidentOverviewMstr
        [EnableQuery]
        public SingleResult<IncidentOverviewMstrResponse> GetAllIncidentOverviewMstr()
        {

            var data = from iom in context.IncidentOverviewMstrs

                       select new IncidentOverviewMstrResponse
                       {
                           Id = iom.Id,
                           Description = (iom.IsActive ? iom.DefaultText : iom.Description),
                           IsActive = iom.IsActive
                       };

            SingleResult<IncidentOverviewMstrResponse> result = new SingleResult<IncidentOverviewMstrResponse>(data);
            return result;

        }

        // GET tables/IncidentOverviewMstr/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<IncidentOverviewMstrResponse> GetIncidentOverviewMstr(string id)
        {
            var data = from iom in context.IncidentOverviewMstrs
                       select new IncidentOverviewMstrResponse
                       {
                           Id = iom.Id,
                           Description = iom.Description,
                           IsActive = iom.IsActive
                       };

            SingleResult<IncidentOverviewMstrResponse> result = new SingleResult<IncidentOverviewMstrResponse>(data);
            return result;
        }

        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.UPDATE)]
        // PATCH tables/IncidentOverviewMstr/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<IncidentOverviewMstr> PatchIncidentOverviewMstr(string id, Delta<IncidentOverviewMstrRequest> patch)
        {
            try
            {
                string requesterEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                Delta<IncidentOverviewMstr> deltaDest = new Delta<IncidentOverviewMstr>();
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

        // POST tables/IncidentOverviewMstr
        public async Task<IHttpActionResult> PostIncidentOverviewMstr(IncidentOverviewMstr item)
        {
            IncidentOverviewMstr current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/IncidentOverviewMstr/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteIncidentOverviewMstr(string id)
        {
            return DeleteAsync(id);
        }
    }
}
