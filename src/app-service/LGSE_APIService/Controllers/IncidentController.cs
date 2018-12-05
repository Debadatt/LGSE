using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;
using LGSE_APIService.ResponseObjects;
using System.Collections.Generic;
using AutoMapper;
using LGSE_APIService.Utilities;
using LGSE_APIService.Authorization;
using LGSE_APIService.Common.Utilities;
using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace LGSE_APIService.Controllers
{
    public class IncidentController : TableController<Incident>
    {
        LGSE_APIContext context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = LGSE_APIContext.GetIntance();
            DomainManager = new EntityDomainManager<Incident>(context, Request, enableSoftDelete: true);
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT, Features.RESOURCEMGT }, OperationType = OperationType.READ)]
        // GET tables/Incident
        [EnableQuery]
        public IQueryable<IncidentResponse> GetAllIncident()
        {
            try
            {
                List<string> dummyList = new List<string>();
                dummyList.Add("Temp for union");
                //var propertiesCount = context.Properties
                //    .GroupBy(x => new { x.IncidentId, x.StatusId })
                //    .Select(g => new { g.Key.IncidentId, g.Key.StatusId,  Count = g.Count() });

                var resIncResList = from inc in context.Incidents
                                    select new IncidentResponse
                                    {
                                        CategoriesMstrId = inc.CategoriesMstrId,
                                        CategoryName = inc.CategoriesMstr.Category,
                                        ClosingNotes = inc.ClosingNotes,
                                        CreatedBy = inc.CreatedBy,
                                        Description = inc.Description,
                                        EndTime = inc.EndTime,
                                        Id = inc.Id,
                                        IncidentId = inc.IncidentId,
                                        ModifiedBy = inc.ModifiedBy,
                                        NoOfCells = inc.NoOfCells,
                                        NoOfPropsAffected = inc.NoOfPropsAffected,
                                        NoOfZones = inc.NoOfZones,
                                        //NoOfPropsIsolated=inc.Properties.Where(i=>i.Deleted==false && 
                                        //                  i.PropertyUserStatus.Count(pus=>pus.PropertyStatusMstr.Status==DBConstants.ISOLATED_STATUS)==1).Count(),
                                        //NoOfPropsRestored= inc.Properties.Where(i => i.Deleted == false &&
                                        //                   i.PropertyUserStatus.Count(pus => pus.PropertyStatusMstr.Status == DBConstants.RESTORED_STATUS) == 1).Count(),
                                        //PropsStatusCounts = propertiesCount.Where(i=>i.IncidentId==inc.Id).Select(i => new PropsStatusCountsResp1
                                        //{
                                        //    StatusId= i.StatusId,
                                        //    Count= i.Count
                                        //}).ToList<PropsStatusCountsResp1>(),

                                        //PropsStatusCounts = inc.Properties.Where(i=>i.PropertyStatusMstr.Status!=" ")
                                        //    .GroupBy(x => x.StatusId )
                                        //    .Select(g => new PropsStatusCountsResp1 {  StatusId = g.Key, Count = g.Count() }).ToList(),

                                        PropsStatusCounts = inc.Properties.Where(i => i.Deleted == false)
                                            .GroupBy(x => x.StatusId)
                                            .Select(g => new PropsStatusCountsResp1 { StatusId = g.Key, Count = g.Count() }).ToList(),

                                        //((from t in dummyList
                                        //                  select new PropsStatusCountsResp
                                        //                  {
                                        //                      Status = "No Status",
                                        //                      // statusId = "NoStatus", //Get the count which does not have status
                                        //                      Count = inc.Properties.Count(i => i.StatusId == null && i.Deleted == false),
                                        //                      ShortText = "NS",
                                        //                      DisplayOrder = -1
                                        //                  }).Union
                                        //            (from psm in context.PropertyStatusMstr.Where(i => i.Deleted == false).OrderBy(i => i.DisplayOrder)
                                        //             select new PropsStatusCountsResp
                                        //             {
                                        //                 Status = psm.Status,
                                        //                 //statusId = psm.Id,
                                        //                 Count = psm.Properties.Where(i => i.IncidentId == inc.Id
                                        //                                  && i.Deleted == false // && i.StatusId==psm.Id
                                        //                                                        //&& i.StatusId == (context.PropertyUserStatus.
                                        //                                                        //Where(pus => pus.Property.IncidentId == inc.Id && i.Deleted == false)
                                        //                                                        //          .OrderByDescending(k => k.StatusChangedOn)
                                        //                                                        //           .FirstOrDefault(k => k.PropertyId == i.PropertyId
                                        //                                                        //).StatusId
                                        //                                                        //  ) // if multiple status exists for the property then lake the latest count by oredering.
                                        //                             )
                                        //                            .Select(i => new
                                        //                            {
                                        //                                i.Id,
                                        //                                i.StatusId
                                        //                            }).Distinct().Count(),
                                        //                 ShortText = psm.ShortText,
                                        //                 DisplayOrder = psm.DisplayOrder
                                        //             }).OrderBy(i => i.DisplayOrder)
                                        //             ).ToList<PropsStatusCountsResp>(),
                                        //  PropsStatusCounts=inc.IncidentPropsStatusCounts.Select(i=> new IncidentPropsStatusCountsResp { NS=i.NS, NA=i.NA, NC=i.NC, IS=i.IS, RS=i.RS }).ToList<IncidentPropsStatusCountsResp>(),
                                        Notes = inc.Notes,
                                        StartTime = inc.StartTime,
                                        Status = inc.Status
                                    };
                return resIncResList.AsQueryable();
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }
        // GET tables/Incident/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT, Features.DASHBOARD}, OperationType = OperationType.READ)]
        public SingleResult<IncidentResponse> GetIncident(string id)
        {
            try
            {
                //return Lookup(id);
                var resIncRes = from inc in context.Incidents.Where(i => i.Id == id)
                                select new IncidentResponse
                                {
                                    CategoriesMstrId = inc.CategoriesMstrId,
                                    CategoryName = inc.CategoriesMstr.Category,
                                    ClosingNotes = inc.ClosingNotes,
                                    CreatedBy = inc.CreatedBy,
                                    Description = inc.Description,
                                    EndTime = inc.EndTime,
                                    Id = inc.Id,
                                    IncidentId = inc.IncidentId,
                                    ModifiedBy = inc.ModifiedBy,
                                    NoOfCells = inc.NoOfCells,
                                    NoOfPropsAffected = inc.NoOfPropsAffected,
                                    NoOfZones = inc.NoOfZones,
                                    //We may need to remove the following two property as its handled differently in GetIncidents API.
                                    NoOfPropsIsolated = inc.Properties.Where(i => i.Deleted == false &&
                                                                                  i.PropertyStatusMstr.Status == DBConstants.RESTORED_STATUS).Count(),
                                    NoOfPropsRestored = inc.Properties.Where(i => i.Deleted == false &&
                                                      i.PropertyStatusMstr.Status == DBConstants.RESTORED_STATUS).Count(),
                                    Notes = inc.Notes,
                                    StartTime = inc.StartTime,
                                    Status = inc.Status,
                                    Cells = inc.Properties.Where(i => !i.Deleted).Select(i => i.Cell).Distinct().ToList(),
                                    Zones = inc.Properties.Where(i => !i.Deleted).Select(i => i.Zone).Distinct().ToList()
                                };
                SingleResult<IncidentResponse> result = new SingleResult<IncidentResponse>(resIncRes);
                return result;
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }

        /* This has been customised in Custom Incident Controller*/
        // PATCH tables/Incident/48D68C86-6EA6-4C25-AA33-223FC9A27959
        //public Task<Incident> PatchIncident(string id, Delta<Incident> patch)
        //{
        //     return UpdateAsync(id, patch);
        //}

        /* This has been customised in Custom Incident Controller*/
        //// POST tables/Incident
        //public async Task<IHttpActionResult> PostIncident(Incident item)
        //{
        //    Incident current = await InsertAsync(item);
        //    return CreatedAtRoute("Tables", new { id = current.Id }, current);
        //}

        // no need of this API..
        //// DELETE tables/Incident/48D68C86-6EA6-4C25-AA33-223FC9A27959
        //public Task DeleteIncident(string id)
        //{
        //     return DeleteAsync(id);
        //}
    }
}
