using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;

namespace LGSE_APIService.Controllers
{
    public class AuditTrialController : TableController<AuditTrial>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            LGSE_APIContext context = new LGSE_APIContext();
            DomainManager = new EntityDomainManager<AuditTrial>(context, Request);
        }

        // GET tables/AuditTrial
        [Authorize]
        public IQueryable<AuditTrial> GetAllAuditTrial()
        {
            return Query(); 
        }
        [Authorize]
        // GET tables/AuditTrial/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<AuditTrial> GetAuditTrial(string id)
        {
            return Lookup(id);
        }
        //[Authorize]
        //// PATCH tables/AuditTrial/48D68C86-6EA6-4C25-AA33-223FC9A27959
        //public Task<AuditTrial> PatchAuditTrial(string id, Delta<AuditTrial> patch)
        //{
        //     return UpdateAsync(id, patch);
        //}
        [Authorize]
        // POST tables/AuditTrial
        public async Task<IHttpActionResult> PostAuditTrial(AuditTrial item)
        {
            AuditTrial current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }
        //[Authorize]
        //// DELETE tables/AuditTrial/48D68C86-6EA6-4C25-AA33-223FC9A27959
        //public Task DeleteAuditTrial(string id)
        //{
        //     return DeleteAsync(id);
        //}
    }
}
