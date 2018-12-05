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
    public class IncidentHistoryController : TableController<IncidentHistory>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            LGSE_APIContext context = LGSE_APIContext.GetIntance();
            DomainManager = new EntityDomainManager<IncidentHistory>(context, Request);
        }

        // GET tables/IncidentHistory
        public IQueryable<IncidentHistory> GetAllIncidentHistory()
        {
            return Query(); 
        }

        // GET tables/IncidentHistory/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<IncidentHistory> GetIncidentHistory(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/IncidentHistory/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<IncidentHistory> PatchIncidentHistory(string id, Delta<IncidentHistory> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/IncidentHistory
        public async Task<IHttpActionResult> PostIncidentHistory(IncidentHistory item)
        {
            IncidentHistory current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/IncidentHistory/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteIncidentHistory(string id)
        {
             return DeleteAsync(id);
        }
    }
}
