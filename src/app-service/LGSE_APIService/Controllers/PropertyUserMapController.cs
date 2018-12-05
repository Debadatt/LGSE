using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;
using LGSE_APIService.Common;
using LGSE_APIService.Authorization;
using LGSE_APIService.Common.Utilities;
using LGSE_APIService.Utilities;
using System;

namespace LGSE_APIService.Controllers
{
    public class PropertyUserMapController : TableController<PropertyUserMap>
    {
        LGSE_APIContext context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = LGSE_APIContext.GetIntance();
            DbUtilities.dbContext = context;
            DomainManager = new EntityDomainManager<PropertyUserMap>(context, Request);
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.RESOURCEMGT}, OperationType = OperationType.READ)]
        // GET tables/PropertyUserMap
        public IQueryable<PropertyUserMap> GetAllPropertyUserMap()
        {
            try
            {
                //User usr = DbUtilities.GetLoggedInUser(this.Request);
                //string userId = string.Empty;
                //if (usr != null)
                //{
                //    userId = usr.Id;
                //}
                //return context.PropertyUserMap.Where(i => i.UserId == userId).AsQueryable();
                return Query();
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.RESOURCEMGT}, OperationType = OperationType.READ)]
        // GET tables/PropertyUserMap/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PropertyUserMap> GetPropertyUserMap(string id)
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
        [CustomAuthorize(Module = new Features[] { Features.RESOURCEMGT}, OperationType = OperationType.UPDATE)]
        // PATCH tables/PropertyUserMap/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<PropertyUserMap> PatchPropertyUserMap(string id, Delta<PropertyUserMap> patch)
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
        [CustomAuthorize(Module = new Features[] { Features.RESOURCEMGT}, OperationType = OperationType.CREATE)]
        // POST tables/PropertyUserMap
        public async Task<IHttpActionResult> PostPropertyUserMap(PropertyUserMap item)
        {
            try
            {
                PropertyUserMap current = await InsertAsync(item);
                return CreatedAtRoute("Tables", new { id = current.Id }, current);
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.RESOURCEMGT}, OperationType = OperationType.DELETE)]
        // DELETE tables/PropertyUserMap/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePropertyUserMap(string id)
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
