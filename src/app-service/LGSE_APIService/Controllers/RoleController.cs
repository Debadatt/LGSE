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
using LGSE_APIService.Validators;
using LGSE_APIService.Utilities;
using System;
using LGSE_APIService.Common;
using LGSE_APIService.Authorization;
using LGSE_APIService.Common.Utilities;

namespace LGSE_APIService.Controllers
{
    public class RoleController : TableController<Role>
    {
        LGSE_APIContext context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = LGSE_APIContext.GetIntance();
            ValidationUtilities.dbContext = context;
            DomainManager = new EntityDomainManager<Role>(context, Request, enableSoftDelete: true);
        }

        // [Authorize]
        // [CustomAuthorize(Module = Features.PORTALMANAGEMENT, OperationType = OperationType.READ)]
        // GET tables/Role
        public IQueryable<Role> GetAllRole()
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
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.READ)]
        // GET tables/Role/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Role> GetRole(string id)
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
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.UPDATE)]
        // PATCH tables/Role/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Role> PatchRole(string id, Delta<RoleRequest> patch)
        {
            try
            {
                string currentUserEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                // string message = ValidationUtilities.ValidateRole(patch);
                //string message = string.Empty;
                //if (string.IsNullOrEmpty(message))
                //{
                Delta<Role> deltaDest = new Delta<Role>();
                Role dbObject = context.Roles.FirstOrDefault(a => a.Id == id);
                Mapper.Initialize(cfg => cfg.CreateMap<RoleRequest, Role>()
                 .ForMember(i => i.ModifiedBy, j => j.UseValue(currentUserEmail))
                 .ForMember(i => i.UpdatedAt, j => j.UseValue(DateTimeOffset.UtcNow))
                );
                var RoleMap = Mapper.Map<RoleRequest, Role>(patch.GetEntity(), dbObject);
                deltaDest.Patch(RoleMap);
                return UpdateAsync(id, deltaDest);
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.CREATE)]
        // POST tables/Role
        [ValidateModel]
        public async Task<IHttpActionResult> PostRole(RoleRequest item)
        {
            try
            {
                string currentUserEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                string message = ValidationUtilities.ValidateRole(item);
                if (string.IsNullOrEmpty(message))
                {
                    Mapper.Initialize(cfg => cfg.CreateMap<RoleRequest, Role>()
                   .ForMember(i => i.CreatedBy, j => j.UseValue(currentUserEmail))
                   );
                    var RoleMap = Mapper.Map<RoleRequest, Role>(item);
                    RoleMap.Id = Guid.NewGuid().ToString();
                    Role current = await InsertAsync(RoleMap);
                    return CreatedAtRoute("Tables", new { id = current.Id }, current);
                }
                else
                {
                    return BadRequest(message);
                }
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.PORTALMANAGEMENT }, OperationType = OperationType.DELETE)]
        // DELETE tables/Role/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteRole(string id)
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
