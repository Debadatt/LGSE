using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;
using LGSE_APIService.RequestObjects;
using AutoMapper;
using LGSE_APIService.Utilities;
using System;
using LGSE_APIService.Validators;
using System.Dynamic;
using LGSE_APIService.Authorization;
using LGSE_APIService.Common.Utilities;
namespace LGSE_APIService.Controllers
{
    public class DomainController : TableController<Domain>
    {
        LGSE_APIContext context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = LGSE_APIContext.GetIntance();
            ValidationUtilities.dbContext = context;
            DomainManager = new EntityDomainManager<Domain>(context, Request, enableSoftDelete: true);
        }
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.READ)]
        // GET tables/Domain
        public IQueryable<Domain> GetAllDomain()
        {
            return Query();
        }
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.READ)]
        // GET tables/Domain/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Domain> GetDomain(string id)
        {
            return Lookup(id);
        }
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.UPDATE)]
        // PATCH tables/Domain/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Domain> PatchDomain(string id, Delta<DomainRequest> patch)
        {
            try
            {
                // patch Mappers does not work for Patch.Itws a limitation.
                ValidationUtilities.ValidateEditDomainRequest(patch.GetEntity(), patch.GetChangedPropertyNames().ToList());
                string currentUserEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                Delta<Domain> deltaDest = new Delta<Domain>();
                //Domain dbObject = context.Domains.FirstOrDefault(a => a.Id == id);
                //Mapper.Initialize(cfg => cfg.CreateMap<DomainRequest, Domain>()
                // .ForMember(i => i.ModifiedBy, j => j.UseValue(currentUserEmail))
                // .ForMember(i => i.UpdatedAt, j => j.UseValue(DateTimeOffset.UtcNow))
                //);
                //var domainMap = Mapper.Map<DomainRequest, Domain>(patch.GetEntity(), dbObject);
                //deltaDest.Put(domainMap);
                //patch.
                foreach (var item in patch.GetChangedPropertyNames())
                {
                    object result;
                    patch.TryGetPropertyValue(item, out result);
                    bool bResult;
                    if (bool.TryParse(result.ToString(), out bResult))
                    {
                        deltaDest.TrySetPropertyValue(item, bResult);
                    }
                    else
                    {
                        deltaDest.TrySetPropertyValue(item, result);
                    }
                }
                deltaDest.TrySetPropertyValue("ModifiedBy", currentUserEmail);
                deltaDest.TrySetPropertyValue("UpdatedAt", DateTimeOffset.UtcNow);
                return UpdateAsync(id, deltaDest);
            }
            catch (HttpResponseException ex)
            {
                LGSELogger.Error(ex);
                throw ex;
            }
        }

        // POST tables/Domain
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.CREATE)]
        [ValidateModel]
        public async Task<IHttpActionResult> PostDomain(DomainRequest item)
        {
            // Mapper.Reset();
            try
            {
                ValidationUtilities.ValidateDomain(item);
                string currentUserEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                Mapper.Initialize(cfg => cfg.CreateMap<DomainRequest, Domain>().ForMember(i => i.CreatedBy,
                    j => j.UseValue(currentUserEmail)));
                var domain = Mapper.Map<DomainRequest, Domain>(item);
                domain.Id = Guid.NewGuid().ToString();
                Domain current = await InsertAsync(domain);
                return CreatedAtRoute("Tables", new { id = current.Id }, current);
            }
            catch (HttpResponseException ex)
            {
                LGSELogger.Error(ex);
                throw ex;
            }
        }
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.DELETE)]
        // DELETE tables/Domain/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteDomain(string id)
        {
            return DeleteAsync(id);
        }
    }
}
