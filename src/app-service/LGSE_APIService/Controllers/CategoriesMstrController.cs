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
    public class CategoriesMstrController : TableController<CategoriesMstr>
    {
        LGSE_APIContext context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = LGSE_APIContext.GetIntance();
            DomainManager = new EntityDomainManager<CategoriesMstr>(context, Request);
        }

        // GET tables/CategoriesMstr
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT }, OperationType = OperationType.READ)]
        public IQueryable<CategoriesMstr> GetAllCategoriesMstr()
        {
            return Query();
        }
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT }, OperationType = OperationType.READ)]
        // GET tables/CategoriesMstr/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<CategoriesMstr> GetCategoriesMstr(string id)
        {
            return Lookup(id);
        }
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT }, OperationType = OperationType.UPDATE)]
        // PATCH tables/CategoriesMstr/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<CategoriesMstr> PatchCategoriesMstr(string id, Delta<CategoriesMstr> patch)
        {
            return UpdateAsync(id, patch);
        }
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT }, OperationType = OperationType.CREATE)]
        // POST tables/CategoriesMstr
        public async Task<IHttpActionResult> PostCategoriesMstr(CategoriesMstr item)
        {
            CategoriesMstr current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT }, OperationType = OperationType.DELETE)]
        // DELETE tables/CategoriesMstr/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteCategoriesMstr(string id)
        {
            return DeleteAsync(id);
        }
    }
}
