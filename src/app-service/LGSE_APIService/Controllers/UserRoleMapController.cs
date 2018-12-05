using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;
using LGSE_APIService.Validators;
using LGSE_APIService.RequestObjects;
using LGSE_APIService.Utilities;
using AutoMapper;
using System;
using LGSE_APIService.Authorization;
using LGSE_APIService.Common.Utilities;

namespace LGSE_APIService.Controllers
{
    public class UserRoleMapController : TableController<UserRoleMap>
    {
        LGSE_APIContext context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = LGSE_APIContext.GetIntance();
            ValidationUtilities.dbContext = context;
            DomainManager = new EntityDomainManager<UserRoleMap>(context, Request,enableSoftDelete:true);
        }
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT}, OperationType = OperationType.READ)]
        // GET tables/UserRoleMap
        public IQueryable<UserRoleMap> GetAllUserRoleMap()
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
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT}, OperationType = OperationType.READ)]
        // GET tables/UserRoleMap/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<UserRoleMap> GetUserRoleMap(string id)
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
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT}, OperationType = OperationType.UPDATE)]
        // PATCH tables/UserRoleMap/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<UserRoleMap> PatchUserRoleMap(string id, Delta<UserRoleMap> patch)
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
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT}, OperationType = OperationType.CREATE)]
        // POST tables/UserRoleMap
        [ValidateModel]
        public async Task<IHttpActionResult> PostUserRoleMap(UserRoleMapRequest item)
        {
            try
            {
                ValidationUtilities.ValidateForUserRoleMap(item);
                string currentUserEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                Mapper.Initialize(cfg => cfg.CreateMap<UserRoleMapRequest, UserRoleMap>().ForMember(i => i.CreatedBy,
                    j => j.UseValue(currentUserEmail)));
                var userRoleMap = Mapper.Map<UserRoleMapRequest, UserRoleMap>(item);
                userRoleMap.Id = Guid.NewGuid().ToString();
                UserRoleMap current = await InsertAsync(userRoleMap);
                return CreatedAtRoute("Tables", new { id = current.Id }, current);
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

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT}, OperationType = OperationType.DELETE)]
        // DELETE tables/UserRoleMap/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUserRoleMap(string id)
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
