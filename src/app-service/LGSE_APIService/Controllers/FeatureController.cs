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

namespace LGSE_APIService.Controllers
{
    public class FeatureController : TableController<Feature>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            LGSE_APIContext context = LGSE_APIContext.GetIntance();
            DomainManager = new EntityDomainManager<Feature>(context, Request);
        }

        // GET tables/Feature
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.READ)]
        public IQueryable<Feature> GetAllFeature()
        {
            return Query();
        }

        // GET tables/Feature/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.READ)]
        public SingleResult<Feature> GetFeature(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Feature/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.UPDATE)]
        public Task<Feature> PatchFeature(string id, Delta<Feature> patch)
        {
            return UpdateAsync(id, patch);
        }
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.CREATE)]
        // POST tables/Feature
        public async Task<IHttpActionResult> PostFeature(Feature item)
        {
            Feature current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Feature/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.DELETE)]
        public Task DeleteFeature(string id)
        {
            return DeleteAsync(id);
        }
    }
}
