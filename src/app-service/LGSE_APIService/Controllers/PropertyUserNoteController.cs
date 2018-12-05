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
    public class PropertyUserNoteController : TableController<PropertyUserNote>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            LGSE_APIContext context = LGSE_APIContext.GetIntance();
            DomainManager = new EntityDomainManager<PropertyUserNote>(context, Request);
        }

        // GET tables/PropertyUserNote
        public IQueryable<PropertyUserNote> GetAllPropertyUserNote()
        {
            return Query(); 
        }

        // GET tables/PropertyUserNote/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PropertyUserNote> GetPropertyUserNote(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/PropertyUserNote/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<PropertyUserNote> PatchPropertyUserNote(string id, Delta<PropertyUserNote> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/PropertyUserNote
        public async Task<IHttpActionResult> PostPropertyUserNote(PropertyUserNote item)
        {
            PropertyUserNote current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/PropertyUserNote/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePropertyUserNote(string id)
        {
             return DeleteAsync(id);
        }
    }
}
