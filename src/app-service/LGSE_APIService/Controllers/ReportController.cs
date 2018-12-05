using System;
using System.Web.Http;
using LGSE_APIService.Models;
using LGSE_APIService.RequestObjects;
using Microsoft.Azure.Mobile.Server.Config;
using System.Linq;
using LGSE_APIService.DataObjects;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using LGSE_APIService.Authorization;
using LGSE_APIService.Common.Utilities;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Net.Http.Headers;

namespace LGSE_APIService.Controllers
{
    [MobileAppController]
    public class ReportController : ApiController
    {
        readonly LGSE_APIContext _context = null;
        public ReportController()
        {
            _context = LGSE_APIContext.GetIntance();
        }

        /// <summary>
        /// Gets the status of incident 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.DASHBOARD}, OperationType = OperationType.READ)]
        [HttpPost]
        [Route("api/Report/IncidentStatus")]
        public IHttpActionResult IncidentStatusReport(IncidentReportRequest request)
        {
            List<string> dummyList = new List<string>();
            dummyList.Add("Temp for union");
            // if Incident id is not passed then return inprogress incident details
            /*
             if incident has 10 MPRNS then result of total count of over all status should be 10
             Ex: NS-2
                 IS-3
                 RS-5
             */
            try
            {
                string incidentId = string.Empty;
                if (string.IsNullOrEmpty(request.IncidentId))
                {
                    incidentId = _context.Incidents.FirstOrDefault(i => i.Status == null || i.Status == 0).Id;
                }
                else
                {
                    incidentId = request.IncidentId;
                }
                var propStatus =
                    (from t in dummyList
                     select new
                     {
                         name = "No Status",
                         statusId = "NoStatus", //Get the count which does not have status
                         count = _context.Properties.Count(i => i.IncidentId == incidentId &&
                       i.StatusId == null 
                         //  i.PropertyUserStatus.Where(j => j.Deleted == false).Count() == 0
                         && i.Deleted == false),
                         displayOrder = -1
                     }).Union
                    (from psm in _context.PropertyStatusMstr.Where(i => i.Deleted == false)
                                 .OrderBy(i => i.DisplayOrder)
                     select new
                     {
                         name = psm.Status,
                         statusId = psm.Id,
                         //Get the latest status counts only
                         count = psm.Properties.Where(i => i.IncidentId == incidentId
                                                                               && i.Deleted == false)

                         //psm.PropertyUserStatus.Where(i => i.Property.IncidentId == incidentId
                         //        && i.Deleted == false
                         //        && i.StatusId == (_context.PropertyUserStatus.
                         //        Where(pus => pus.Property.IncidentId == incidentId && i.Deleted == false)
                         //                  .OrderByDescending(k => k.StatusChangedOn)
                         //                   .Where(k => k.PropertyId == i.PropertyId
                         //                   ).Select(k => k.StatusId).FirstOrDefault()) // if multiple status exists for the property then lake the latest count by oredering.
                         //        )
                                .Select(i => new
                                {
                                    i.Id,
                                    //i.UserId,
                                    i.StatusId
                                }).Distinct().Count(),
                         displayOrder = psm.DisplayOrder
                     }).OrderBy(i => i.displayOrder);
                return Ok(propStatus);
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Gets the MPRN status by zone
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.DASHBOARD}, OperationType = OperationType.READ)]
        [HttpPost]
        [Route("api/Report/IncidentStatusByZone")]
        public IHttpActionResult IncidentStatusByZone(IncidentReportRequest request)
        {
            // if Incident id is not passed then return inprogress incident details
            /*
            if incident has 10 MPRNS then result of total count of over all status should be 10
           Ex:
            Zone1 has 5 MPRNS and  Zone2 has 5 MPRNS
           Result will be
            Zone1    
                NS-1
                IS-2
                RS-2
            Zone2    
                NS-2
                IS-2
                RS-1
            */
            try
            {
                string incidentId = string.Empty;
                if (string.IsNullOrEmpty(request.IncidentId))
                {
                    incidentId = _context.Incidents.FirstOrDefault(i => i.Status == null || i.Status == 0).Id;
                }
                else
                {
                    incidentId = request.IncidentId;
                }
                // This collection is for generating the No Status.
                List<string> dummyList = new List<string>();
                dummyList.Add("Temp for union");
                // gets the status in order...
                var result = (from prop in _context.Properties.Where(i => i.IncidentId == incidentId && i.Deleted == false)
                              group prop by prop.Zone into gZone
                              select new
                              {
                                  name = gZone.Key,
                                  series = (from t in dummyList
                                            select new
                                            {
                                                name = "No Status",
                                                statusId = "NoStatus", //Get the count which does not have status
                                                count = _context.Properties.Count(i => i.IncidentId == incidentId && i.Zone==gZone.Key &&
                                                                                       i.StatusId == null
                                                                                       //  i.PropertyUserStatus.Where(j => j.Deleted == false).Count() == 0
                                                                                       && i.Deleted == false),
                                                    //gZone.Count(i => i.PropertyUserStatus.Where(j => j.Deleted == false).Count() == 0),
                                                displayOrder = -1
                                            })
                                                .Union
                                                 (from psm in _context.PropertyStatusMstr.Where(i => i.Deleted == false)
                                                            .OrderBy(i => i.DisplayOrder)
                                                  select new
                                                  {
                                                      name = psm.Status,
                                                      statusId = psm.Id,
                                                      count = psm.Properties.Where(i => i.IncidentId == incidentId
                                                                              && i.Deleted == false && i.Zone == gZone.Key)
                                                      //psm.PropertyUserStatus.Where(i => i.Property.IncidentId == incidentId
                                                      //                      && i.Property.Zone == gZone.Key && i.Deleted == false
                                                      //                        && i.StatusId ==
                                                      //                        (_context.PropertyUserStatus.
                                                      //                           Where(pus => pus.Property.IncidentId == incidentId && pus.Property.Zone == gZone.Key && i.Deleted == false)
                                                      //                                     .OrderByDescending(k => k.StatusChangedOn)
                                                      //                                      .Where(k => k.PropertyId == i.PropertyId).Select(k => k.StatusId).FirstOrDefault()
                                                      //                        ) // if multiple status exists for the property then lake the latest count by oredering.
                                                      //                      )
                                                     .Select(i => new
                                                     {
                                                         i.Id,
                                                         // i.UserId,
                                                         i.StatusId
                                                     }).Distinct().Count(),
                                                      displayOrder = psm.DisplayOrder
                                                  }).OrderBy(i => i.displayOrder)
                              }).OrderBy(i => i.name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Gets the MRPNS status by cells
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.DASHBOARD}, OperationType = OperationType.READ)]
        [HttpPost]
        [Route("api/Report/IncidentStatusByCell")]
        public IHttpActionResult IncidentStatusByCell(IncidentReportRequest request)
        {
            // if Incident id is not passed then return inprogress incident details
            try
            {
                // This collection is for generating the No Status.
                List<string> dummyList = new List<string>();
                dummyList.Add("Temp for union");

                string incidentId = string.Empty;
                if (string.IsNullOrEmpty(request.IncidentId))
                {
                    incidentId = _context.Incidents.FirstOrDefault(i => i.Status == null || i.Status == 0).Id;
                }
                else
                {
                    incidentId = request.IncidentId;
                }
                var result = (from prop in _context.Properties.Where(i => i.IncidentId == incidentId && i.Deleted == false)
                             group prop by prop.Cell into gCell
                             select new
                             {
                                 name = gCell.Key,
                                 series =
                                 ((from t in dummyList
                                   select new
                                   {
                                       name = "No Status",
                                       statusId = "NoStatus", //Get the count which does not have status
                                       count = _context.Properties.Count(i => i.IncidentId == incidentId && i.Cell==gCell.Key &&
                                                                              i.StatusId == null
                                                                              //  i.PropertyUserStatus.Where(j => j.Deleted == false).Count() == 0
                                                                              && i.Deleted == false),
                                           //gCell.Count(i => i.PropertyUserStatus.Where(j => j.Deleted == false).Count() == 0),
                                       displayOrder = -1
                                   })
                                .Union
                                 (from psm in _context.PropertyStatusMstr.Where(i => i.Deleted == false).OrderBy(i => i.DisplayOrder)
                                  select new
                                  {
                                      name = psm.Status,
                                      statusId = psm.Id,
                                      count = psm.Properties.Where(i => i.IncidentId == incidentId
                                                                              && i.Deleted == false && i.Cell == gCell.Key)

                                              //Abhijeet 26-10-2018
                                              //psm.PropertyUserStatus.Where(i => i.Property.IncidentId == incidentId
                                              //&& i.Property.Cell == gCell.Key && i.Deleted == false
                                              //  && i.StatusId ==
                                              //  (_context.PropertyUserStatus.
                                              //          Where(pus => pus.Property.IncidentId == incidentId && pus.Property.Cell == gCell.Key && i.Deleted == false)
                                              //                    .OrderByDescending(k => k.StatusChangedOn)
                                              //                     .Where(k => k.PropertyId == i.PropertyId).Select(k => k.StatusId).FirstOrDefault()) // if multiple status exists for the property then lake the latest count by oredering.
                                              //                                                                                                         // Consider only the properties at the cell level.
                                              //         )
                                              .Select(i => new
                                              {
                                                  i.Id,
                                                  //  i.UserId,
                                                  i.StatusId
                                              }).Distinct().Count(),
                                      displayOrder = psm.DisplayOrder
                                  }
                                     ).ToList()).OrderBy(i => i.displayOrder)
                             }).OrderBy(i => i.name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Userwise status report
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.DASHBOARD}, OperationType = OperationType.READ)]
        [HttpPost]
        [Route("api/Report/UserWiseStatus")]
        public IHttpActionResult UserWiseStatus(UserWiseStatusRequest request)
        {
            // if Incident id is not passed then return inprogress incident details
            try
            {
                // This collection is for generating the No Status.
                List<string> dummyList = new List<string>();
                dummyList.Add("Temp for union");

                var result = from pum in _context.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId && i.Property.Cell == request.CellName && i.Property.Deleted == false && !i.Deleted)
                             group pum by pum.UserId into gpum
                             select new
                             {
                                 id = gpum.Key,
                                 name = _context.Users.Where(i => i.Id == gpum.Key).Select(i => new { Name = i.FirstName + " " + i.LastName }).FirstOrDefault().Name,
                                 series =
                                 (
                                 // (from t in dummyList
                                 //   select new
                                 //   {
                                 //       name = "No Status",
                                 //       ShortText="NA",
                                 //       statusId = "NoStatus", //Get the count which does not have status
                                 //       value = gpum.Count(i => i.PropertyUserStatus.Where(j => j.Deleted == false).Count() == 0),
                                 //       displayOrder = -1
                                 //   })
                                 //.Union
                                 (from psm in _context.PropertyStatusMstr.Where(i => i.Deleted == false ).OrderBy(i => i.DisplayOrder)
                                  select new
                                  {
                                      name = psm.Status,
                                      psm.ShortText,
                                      statusId = psm.Id,
                                      value = psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId && i.UserId== gpum.Key
                                      && i.Property.Cell == request.CellName && i.Deleted == false
                                      && i.StatusId ==
                                        (_context.PropertyUserStatus.
                                                Where(pus => pus.Property.IncidentId == request.IncidentId &&  pus.Property.Cell == request.CellName && i.Deleted == false)
                                                          .OrderByDescending(k => k.StatusChangedOn)
                                                           .Where(k => k.PropertyId == i.PropertyId).Select(k => k.StatusId).FirstOrDefault()
                                  ) // if multiple status exists for the property then lake the latest count by oredering.
                                    // Consider only the properties at the cell level.
                                               )
                                              .Select(i => new
                                              {
                                                  i.PropertyId,
                                                    i.UserId,
                                                  i.StatusId
                                              }).Distinct().Count(),
                                      displayOrder = psm.DisplayOrder
                                  }
                                     ).ToList()).OrderBy(i => i.displayOrder)
                             };
                return Ok(result);
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Report for Average time taken at status level for Incident
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.DASHBOARD}, OperationType = OperationType.READ)]
        [HttpPost]
        [Route("api/Report/AvgStatusTimeAtIncident")]
        public IHttpActionResult AvgStatusTimeAtIncident(IncidentReportRequest request)
        {
            try
            {
                //for e.g.Isolated. (endtime - starttime) / totalnoofisolatedproperrties = X seconds
                //starttime is the time when the status of any property was updated as isolated in the incident
                //endtime is the time when the status of last property was updated as isolated in the incident
                //if there is only one property isolated, the starttime and endtime will be same, 
                //hence the average time will be 0.This is known issue.
                List<string> dummyList = new List<string>();
                dummyList.Add("Temp for union");
                DateTimeOffset dtcurrent = DateTimeOffset.Now;
                var propStatus =
                    //No need No Status Column  -Abhijeet-08-10-2018
                    //(from t in dummyList
                    // select new
                    // {
                    //     name = "No Status",
                    //     statusId = "NoStatus", //Get the count which does not have status
                    //     ShortText = "NA",
                    //     value = 0.0,
                    //     displayOrder = -1
                    // }).ToList().Union
                    (from psm in _context.PropertyStatusMstr.Where(i => i.Deleted == false && (i.Status.ToLower()=="isolated" || i.Status.ToLower() == "restored"))
                                 .OrderBy(i => i.DisplayOrder).ToList()
                     select new
                     {
                         name = psm.Status,
                         statusId = psm.Id,
                         propCount= psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId
                                                                      && !i.Deleted).Count(),
                         endTime = (psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId
                                                                       && !i.Deleted).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault() != null ?
                             psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId
                                                               && !i.Deleted).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().StatusChangedOn
                             : dtcurrent),
                         startTime = (psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId && !i.Deleted)
                                       .OrderByDescending(i => i.StatusChangedOn).LastOrDefault() != null ?
                                 psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId && !i.Deleted)
                                     .OrderByDescending(i => i.StatusChangedOn).LastOrDefault().StatusChangedOn : dtcurrent
                             ),
                         psm.ShortText,
                         value =(((psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId
                                                                     && !i.Deleted).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault() != null ?
                                      psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId
                                                                        && !i.Deleted).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().StatusChangedOn
                                      : dtcurrent)
                                  - // minus
                                  (psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId && !i.Deleted)
                                       .OrderByDescending(i => i.StatusChangedOn).LastOrDefault() != null ?
                                      psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId && !i.Deleted)
                                          .OrderByDescending(i => i.StatusChangedOn).LastOrDefault().StatusChangedOn : dtcurrent
                                  )).TotalSeconds > 0 ? ((psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId
                                 && !i.Deleted).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault() != null ?
                                          psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId
                                 && !i.Deleted).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().StatusChangedOn
                                 : dtcurrent)
                                 - // minus
                                 (psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId && !i.Deleted)
                                 .OrderByDescending(i => i.StatusChangedOn).LastOrDefault() != null ?
                                 psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId && !i.Deleted)
                                 .OrderByDescending(i => i.StatusChangedOn).LastOrDefault().StatusChangedOn : dtcurrent
                                 )).TotalSeconds 
                                
                                 / // Division by no of props of its status..
                                psm.PropertyUserStatus.Where(i => !i.Deleted && i.IncidentId == request.IncidentId && i.StatusId == psm.Id).Select(i => i.PropertyId).Distinct().Count() : 0)
                                 ,
                         displayOrder = psm.DisplayOrder
                     }).OrderBy(i => i.displayOrder);
                return Ok(propStatus);
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }


        /// <summary>
        /// Report for Average time taken at status level for Incident at zone level
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.DASHBOARD}, OperationType = OperationType.READ)]
        [HttpPost]
        [Route("api/Report/AvgStatusTimeByZone")]
        public IHttpActionResult AvgStatusTimeByZone(IncidentReportRequest request)
        {
            try
            {
                //for e.g.Isolated. (endtime - starttime) / totalnoofisolatedproperrties = X seconds
                //starttime is the time when the status of any property was updated as isolated in the incident
                //endtime is the time when the status of last property was updated as isolated in the incident
                //if there is only one property isolated, the starttime and endtime will be same, 
                //hence the average time will be 0.This is known issue.
                List<string> dummyList = new List<string>();
                dummyList.Add("Temp for union");
                DateTimeOffset dtcurrent = DateTimeOffset.Now;
                var propStatus = (from prop in _context.Properties.Where(i => i.IncidentId == request.IncidentId && i.Deleted == false).ToList()
                                 group prop by prop.Zone into gZone
                                 select new
                                 {
                                     name = gZone.Key,
                                     series =
                                        //No need No Status Column  -Abhijeet-08-10-2018
                                        //(from t in dummyList
                                        // select new
                                        // {
                                        //     name = "No Status",
                                        //     statusId = "NoStatus", //Get the count which does not have status
                                        //     ShortText = "NA",
                                        //     value = 0.0,
                                        //     displayOrder = -1
                                        // }).ToList().Union
                                        (from psm in _context.PropertyStatusMstr.Where(i => i.Deleted == false && (i.Status == "Isolated" || i.Status == "Restored"))
                                                     .OrderBy(i => i.DisplayOrder).ToList()
                                         select new
                                         {
                                             name = psm.Status,
                                             statusId = psm.Id,
                                             psm.ShortText,
                                             value = (((psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId
                                                     && !i.Deleted && i.Property.Zone == gZone.Key).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault() != null ?
                                                              psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId
                                                     && !i.Deleted && i.Property.Zone == gZone.Key).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().StatusChangedOn
                                                     : dtcurrent)
                                                     - // minus
                                                     (psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId && !i.Deleted && i.Property.Zone == gZone.Key)
                                                     .OrderByDescending(i => i.StatusChangedOn).LastOrDefault() != null ?
                                                     psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId && !i.Deleted && i.Property.Zone == gZone.Key)
                                                     .OrderByDescending(i => i.StatusChangedOn).LastOrDefault().StatusChangedOn : dtcurrent
                                                     )).TotalSeconds > 0 ? ((psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId
                                                                                                               && !i.Deleted && i.Property.Zone == gZone.Key).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault() != null ?
                                                                                psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId
                                                                                                                  && !i.Deleted && i.Property.Zone == gZone.Key).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().StatusChangedOn
                                                                                : dtcurrent)
                                                                            - // minus
                                                                            (psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId && !i.Deleted && i.Property.Zone == gZone.Key)
                                                                                 .OrderByDescending(i => i.StatusChangedOn).LastOrDefault() != null ?
                                                                                psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId && !i.Deleted && i.Property.Zone == gZone.Key)
                                                                                    .OrderByDescending(i => i.StatusChangedOn).LastOrDefault().StatusChangedOn : dtcurrent
                                                                            )).TotalSeconds
                                                     / // Division by no of props
                                                       //_context.Properties.Count(i => !i.Deleted && i.IncidentId == request.IncidentId && i.Zone == gZone.Key)
                                                      psm.PropertyUserStatus.Where(i => !i.Deleted && i.Property.IncidentId == request.IncidentId && i.StatusId == psm.Id && i.Property.Zone == gZone.Key).Select(i => i.PropertyId).Distinct().Count() : 0)
                                                     ,
                                             displayOrder = psm.DisplayOrder
                                         }).OrderBy(i => i.displayOrder)
                                 }).OrderBy(i=>i.name);
                return Ok(propStatus);
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Report for Average time taken at status level for Incident at Cell level
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[Authorize]
        [CustomAuthorize(Module = new Features[] { Features.DASHBOARD}, OperationType = OperationType.READ)]
        [HttpPost]
        [Route("api/Report/AvgStatusTimeByCell")]
        public IHttpActionResult AvgStatusTimeByCell(IncidentReportRequest request)
        {
            try
            {
                //for e.g.Isolated. (endtime - starttime) / totalnoofisolatedproperrties = X seconds
                //starttime is the time when the status of any property was updated as isolated in the incident
                //endtime is the time when the status of last property was updated as isolated in the incident
                //if there is only one property isolated, the starttime and endtime will be same, 
                //hence the average time will be 0.This is known issue.
                List<string> dummyList = new List<string>();
                dummyList.Add("Temp for union");
                DateTimeOffset dtcurrent = DateTimeOffset.Now;
                var propStatus = (from prop in _context.Properties.Where(i => i.IncidentId == request.IncidentId && i.Deleted == false).ToList()
                                 group prop by prop.Cell into gCell
                                 select new
                                 {
                                     name = gCell.Key,
                                     series =
                                        //No need No Status Column -Abhijeet-08-10-2018
                                        //(from t in dummyList
                                        // select new
                                        // {
                                        //     name = "No Status",
                                        //     statusId = "NoStatus", //Get the count which does not have status
                                        //             ShortText = "NA",
                                        //     value = 0.0,
                                        //     displayOrder = -1
                                        // }).ToList().Union
                                        (from psm in _context.PropertyStatusMstr.Where(i => i.Deleted == false && (i.Status == "Isolated" || i.Status == "Restored"))
                                                     .OrderBy(i => i.DisplayOrder).ToList()
                                         select new
                                         {
                                             name = psm.Status,
                                             statusId = psm.Id,
                                             psm.ShortText,
                                             value = (((psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId
                                                     && !i.Deleted && i.Property.Cell == gCell.Key).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault() != null ?
                                                              psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId
                                                     && !i.Deleted && i.Property.Cell == gCell.Key).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().StatusChangedOn
                                                     : dtcurrent)
                                                     - // minus
                                                     (psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId && !i.Deleted && i.Property.Cell == gCell.Key)
                                                     .OrderByDescending(i => i.StatusChangedOn).LastOrDefault() != null ?
                                                     psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId && !i.Deleted && i.Property.Cell == gCell.Key)
                                                     .OrderByDescending(i => i.StatusChangedOn).LastOrDefault().StatusChangedOn : dtcurrent
                                                     )).TotalSeconds > 0 ? ((psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId
                                                                                                               && !i.Deleted && i.Property.Cell == gCell.Key).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault() != null ?
                                                                             psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId
                                                                                                               && !i.Deleted && i.Property.Cell == gCell.Key).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().StatusChangedOn
                                                                             : dtcurrent)
                                                                         - // minus
                                                                         (psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId && !i.Deleted && i.Property.Cell == gCell.Key)
                                                                              .OrderByDescending(i => i.StatusChangedOn).LastOrDefault() != null ?
                                                                             psm.PropertyUserStatus.Where(i => i.Property.IncidentId == request.IncidentId && !i.Deleted && i.Property.Cell == gCell.Key)
                                                                                 .OrderByDescending(i => i.StatusChangedOn).LastOrDefault().StatusChangedOn : dtcurrent
                                                                         )).TotalSeconds
                                                     / // Division by no of props
                                                       //_context.Properties.Count(i => !i.Deleted && i.IncidentId == request.IncidentId && i.Cell == gCell.Key)
                                                     psm.PropertyUserStatus.Where(i => !i.Deleted && i.Property.IncidentId == request.IncidentId && i.StatusId == psm.Id && i.Property.Cell == gCell.Key).Select(i => i.PropertyId).Distinct().Count() : 0),
                                             displayOrder = psm.DisplayOrder
                                         }).OrderBy(i => i.displayOrder).OrderBy(i=>i.name)
                                 }).OrderBy(i => i.name);
                return Ok(propStatus);
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Report for Average time taken at status level for Incident at Cell level
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[Authorize]
        [CustomAuthorize(Module = new Features[] { Features.DASHBOARD}, OperationType = OperationType.READ)]
        [HttpPost]
        [Route("api/Report/AvgStatusTimeTakenByUserLevel")]
        public IHttpActionResult AvgStatusTimeTakenByUserLevel(UserWiseStatusRequest request)
        {
            try
            {
                //for e.g.Isolated. (endtime - starttime) / totalnoofisolatedproperrties = X seconds
                //starttime is the time when the status of any property was updated as isolated in the incident
                //endtime is the time when the status of last property was updated as isolated in the incident
                //if there is only one property isolated, the starttime and endtime will be same, 
                //hence the average time will be 0.This is known issue.
                List<string> dummyList = new List<string>();
                dummyList.Add("Temp for union");
                DateTimeOffset dtcurrent = DateTimeOffset.Now;
                var propStatus = //from prop in _context.Properties//.Where(i => i.IncidentId == request.IncidentId && i.Cell == request.CellName && i.Deleted == false && !i.Deleted)
                                 from pus in _context.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId && i.Property.Cell == request.CellName  && i.Deleted == false && !i.Deleted).ToList()
                                     // on prop.Id equals pus.PropertyId
                                 group pus by pus.UserId into gCell
                                 select new
                                 {

                                     name = _context.Users.Where(i => i.Id == gCell.Key).Select(i => new { Name = i.FirstName + " " + i.LastName }).FirstOrDefault().Name,

                                     series =

                                        (from psm in _context.PropertyStatusMstr.Where(i => i.Deleted == false && (i.Status == "Isolated" || i.Status == "Restored"))
                                                     .OrderBy(i => i.DisplayOrder).ToList()
                                         select new
                                         {
                                             name = psm.Status,
                                             statusId = psm.Id,
                                             psm.ShortText,
                                             value = (((psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId
                                                           && !i.Deleted && i.Property.Cell == request.CellName && i.UserId == gCell.Key).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault() != null ?
                                                                      psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId
                                                             && !i.Deleted && i.Property.Cell == request.CellName && i.UserId == gCell.Key).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().StatusChangedOn
                                                             : dtcurrent)
                                                     - // minus
                                                     (psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId && !i.Deleted && i.Property.Cell == request.CellName && i.UserId == gCell.Key)
                                                     .OrderByDescending(i => i.StatusChangedOn).LastOrDefault() != null ?
                                                     psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId && !i.Deleted && i.Property.Cell == request.CellName && i.UserId == gCell.Key)
                                                     .OrderByDescending(i => i.StatusChangedOn).LastOrDefault().StatusChangedOn : dtcurrent))
                                                     .TotalSeconds > 0 ? ((psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId
                                                                                                             && !i.Deleted && i.Property.Cell == request.CellName && i.UserId == gCell.Key).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault() != null ?
                                                                             psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId
                                                                                                               && !i.Deleted && i.Property.Cell == request.CellName && i.UserId == gCell.Key).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().StatusChangedOn
                                                                             : dtcurrent)
                                                                         - // minus
                                                                         (psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId && !i.Deleted && i.Property.Cell == request.CellName && i.UserId == gCell.Key)
                                                                              .OrderByDescending(i => i.StatusChangedOn).LastOrDefault() != null ?
                                                                             psm.PropertyUserStatus.Where(i => i.IncidentId == request.IncidentId && !i.Deleted && i.Property.Cell == request.CellName && i.UserId == gCell.Key)
                                                                                 .OrderByDescending(i => i.StatusChangedOn).LastOrDefault().StatusChangedOn : dtcurrent))
                                                                        .TotalSeconds
                                                     / // Division by no of props
                                                     psm.PropertyUserStatus.Count(i => !i.Deleted && i.IncidentId == request.IncidentId && i.Property.Cell == request.CellName && i.UserId==gCell.Key) : 0)
                                                     ,
                                             displayOrder = psm.DisplayOrder
                                         }).OrderBy(i => i.displayOrder)
                                 };
                return Ok(propStatus);
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Report for Average time taken at status level for Incident at Cell level
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[Authorize]
        [CustomAuthorize(Module = new Features[] { Features.DASHBOARD}, OperationType = OperationType.READ)]
        [HttpPost]
        [Route("api/Report/EngineeringCapacity")]
        public IHttpActionResult EngineeringCapacity(EngineeringCapacityRequest request)
        {
            try
            {
                //for e.g.Isolated. (endtime - starttime) / totalnoofisolatedproperrties = X seconds
                //starttime is the time when the status of any property was updated as isolated in the incident
                //endtime is the time when the status of last property was updated as isolated in the incident
                //if there is only one property isolated, the starttime and endtime will be same, 
                //hence the average time will be 0.This is known issue.
                List<string> dummyList = new List<string>();
                dummyList.Add("Temp for union");
                DateTime dtcurrent = DateTime.Now;
                var incident = _context.Incidents.Where(i => i.Id == request.IncidentId).FirstOrDefault();
                // incident.EndTime=(incident.EndTime == null ? dtcurrent : incident.EndTime);


                //if (request.Type.ToUpper() == "ZONE")
                //{

                //    var temp = (from pum in _context.PropertyUserMap.Where(i =>
                //            i.Property.IncidentId == request.IncidentId && i.Property.Zone == request.Value &&
                //            i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                //        select new
                //        {
                //            pum.UserId
                //        }).Distinct().ToList();
                //    var zoneOrCell= (from pum in _context.PropertyUserMap.Where(i =>
                //            i.Property.IncidentId == request.IncidentId && i.Property.Zone == request.Value &&
                //            i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                //        select new
                //        {
                //            pum.UserId,
                //          Zone=  pum.Property.Zone,
                //          Cell=  pum.Property.Cell
                //        }).ToList();
                //    var propStatus = from pum in  temp 
                //                     join atu in _context.AuditTrials.Where(i=>i.Role.RoleName.ToLower()=="engineer"  && i.TypeofOperation == "LOGIN" && i.Status == "SUCCESS" && i.OperationTimeStamp >= incident.StartTime && i.OperationTimeStamp <= (incident.EndTime == null ? dtcurrent : incident.EndTime)) on pum.UserId equals atu.UserId

                //                     select new
                //                     {
                //                         Firstname = atu.User.FirstName,
                //                         Lastname = atu.User.LastName,
                //                         Zone = zoneOrCell.Where(i=>i.UserId==atu.UserId).Select(i=>i.Zone).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Zone).Distinct().ToList(),
                //                         Cell = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Cell).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Cell).Distinct().ToList(),
                //                         LoggedInTime = atu.OperationTimeStamp,
                //                         LoggedOutTime = (_context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.OperationTimeStamp > atu.OperationTimeStamp).Count()==0 ?( incident.EndTime !=null ? incident.EndTime:null) : _context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.OperationTimeStamp > atu.OperationTimeStamp).OrderByDescending(i => i.OperationTimeStamp).Select(i => new { Name = i.OperationTimeStamp }).FirstOrDefault().Name)
                //                     };


                //    return Ok(propStatus);
                //}
                //else if (request.Type.ToUpper() == "CELL")
                //{
                //    var temp = (from pum in _context.PropertyUserMap.Where(i =>
                //            i.Property.IncidentId == request.IncidentId && i.Property.Cell == request.Value &&
                //            i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                //        select new
                //        {
                //            pum.UserId
                //        }).Distinct().ToList();
                //    var zoneOrCell = (from pum in _context.PropertyUserMap.Where(i =>
                //            i.Property.IncidentId == request.IncidentId && i.Property.Cell == request.Value &&
                //            i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                //        select new
                //        {
                //            pum.UserId,
                //            Zone = pum.Property.Zone,
                //            Cell = pum.Property.Cell
                //        }).ToList();
                //    var propStatus = from pum in temp
                //                     join atu in _context.AuditTrials.Where(i => i.Role.RoleName.ToLower() == "engineer"  && i.TypeofOperation == "LOGIN" && i.Status == "SUCCESS" && i.OperationTimeStamp >= incident.StartTime && i.OperationTimeStamp <= (incident.EndTime == null ? dtcurrent : incident.EndTime)) on pum.UserId equals atu.UserId

                //        select new
                //        {
                //            Firstname = atu.User.FirstName,
                //            Lastname = atu.User.LastName,
                //            Zone = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Zone).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Zone).Distinct().ToList(),
                //            Cell = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Cell).Distinct().ToList(),
                //            LoggedInTime = atu.OperationTimeStamp,
                //            LoggedOutTime = (_context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.OperationTimeStamp > atu.OperationTimeStamp).Count() == 0 ? null : (incident.EndTime != null ? incident.EndTime : _context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.OperationTimeStamp > atu.OperationTimeStamp).OrderByDescending(i => i.OperationTimeStamp).Select(i => new { Name = i.OperationTimeStamp }).FirstOrDefault().Name)),

                //        };
                //    return Ok(propStatus);
                //}
                //else
                //{
                //    var temp = (from pum in _context.PropertyUserMap.Where(i =>
                //            i.Property.IncidentId == request.IncidentId   && i.Role.RoleName.ToLower() == "engineer")
                //        select new
                //        {
                //            pum.UserId
                //        }).Distinct().ToList();
                //    var zoneOrCell = (from pum in _context.PropertyUserMap.Where(i =>
                //            i.Property.IncidentId == request.IncidentId &&
                //            i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                //        select new
                //        {
                //            pum.UserId,
                //            Zone = pum.Property.Zone,
                //            Cell = pum.Property.Cell
                //        }).ToList();
                //    var propStatus = from pum in temp
                //                     join atu in _context.AuditTrials.Where(i => i.Role.RoleName.ToLower() == "engineer"  && i.TypeofOperation == "LOGIN" && i.Status == "SUCCESS" && i.OperationTimeStamp >= incident.StartTime && i.OperationTimeStamp <= (incident.EndTime == null ? dtcurrent : incident.EndTime)) on pum.UserId equals atu.UserId

                //        select new
                //        {
                //            Firstname = atu.User.FirstName,
                //            Lastname = atu.User.LastName,
                //            Zone = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Zone).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Zone).Distinct().ToList(),
                //            Cell = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Cell).Distinct().ToList(),
                //            LoggedInTime = atu.OperationTimeStamp,
                //            LoggedOutTime = (_context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.OperationTimeStamp > atu.OperationTimeStamp).Count() == 0  ? null: ( incident.EndTime != null  ? incident.EndTime : _context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.OperationTimeStamp > atu.OperationTimeStamp).OrderByDescending(i => i.OperationTimeStamp).Select(i => new { Name = i.OperationTimeStamp }).FirstOrDefault().Name)),
                //           // Count= _context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.OperationTimeStamp > atu.OperationTimeStamp).Count()
                //        };
                //    return Ok(propStatus);
                //}

                if (request.Type.ToUpper() == "ZONE")
                {

                    var temp = (from pum in _context.PropertyUserStatus.Where(i =>
                            i.Property.IncidentId == request.IncidentId && i.Property.Zone == request.Value &&
                            i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                                select new
                                {
                                    pum.UserId
                                }).Distinct().ToList();
                    var zoneOrCell = (from pum in _context.PropertyUserStatus.Where(i =>
                             i.Property.IncidentId == request.IncidentId && i.Property.Zone == request.Value &&
                             i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                                      select new
                                      {
                                          pum.UserId,
                                          Zone = pum.Property.Zone,
                                          Cell = pum.Property.Cell
                                      }).ToList();
                    var propStatus = (from pum in temp
                                     join atu in _context.AuditTrials.Where(i => i.Role.RoleName.ToLower() == "engineer" && i.TypeofOperation == "LOGIN" && i.Status == "SUCCESS" && i.OperationTimeStamp >= incident.StartTime && i.OperationTimeStamp <= (incident.EndTime == null ? dtcurrent : incident.EndTime)) on pum.UserId equals atu.UserId

                                     select new
                                     {
                                         Firstname = atu.User.FirstName,
                                         Lastname = atu.User.LastName,
                                         Zone = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Zone).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Zone).Distinct().ToList(),
                                         Cell = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Cell).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Cell).Distinct().ToList(),
                                         LoggedInTime = atu.OperationTimeStamp,
                                         LoggedOutTime = (_context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.TokenId == atu.TokenId).Count() == 0 ? (incident.EndTime != null ? incident.EndTime : null) : _context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.TokenId == atu.TokenId).OrderByDescending(i => i.OperationTimeStamp).Select(i => new { Name = i.OperationTimeStamp }).FirstOrDefault().Name),

                                     }).OrderByDescending(i => i.LoggedInTime);


                    return Ok(propStatus);
                }
                else if (request.Type.ToUpper() == "CELL")
                {
                    var temp = (from pum in _context.PropertyUserStatus.Where(i =>
                            i.Property.IncidentId == request.IncidentId && i.Property.Cell == request.Value &&
                            i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                                select new
                                {
                                    pum.UserId
                                }).Distinct().ToList();
                    var zoneOrCell = (from pum in _context.PropertyUserStatus.Where(i =>
                            i.Property.IncidentId == request.IncidentId && i.Property.Cell == request.Value &&
                            i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                                      select new
                                      {
                                          pum.UserId,
                                          Zone = pum.Property.Zone,
                                          Cell = pum.Property.Cell
                                      }).ToList();
                    var propStatus = (from pum in temp
                                     join atu in _context.AuditTrials.Where(i => i.Role.RoleName.ToLower() == "engineer" && i.TypeofOperation == "LOGIN" && i.Status == "SUCCESS" && i.OperationTimeStamp >= incident.StartTime && i.OperationTimeStamp <= (incident.EndTime == null ? dtcurrent : incident.EndTime)) on pum.UserId equals atu.UserId

                                     select new
                                     {
                                         Firstname = atu.User.FirstName,
                                         Lastname = atu.User.LastName,
                                         Zone = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Zone).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Zone).Distinct().ToList(),
                                         Cell = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Cell).Distinct().ToList(),
                                         LoggedInTime = atu.OperationTimeStamp,
                                         LoggedOutTime = (_context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.TokenId == atu.TokenId).Count() == 0 ? (incident.EndTime != null ? incident.EndTime : null) : _context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.TokenId == atu.TokenId).OrderByDescending(i => i.OperationTimeStamp).Select(i => new { Name = i.OperationTimeStamp }).FirstOrDefault().Name),

                                     }).OrderByDescending(i => i.LoggedInTime);
                    return Ok(propStatus);
                }
                else
                {
                    var temp = (from pum in _context.PropertyUserStatus.Where(i =>
                            i.Property.IncidentId == request.IncidentId && i.Role.RoleName.ToLower() == "engineer")
                                select new
                                {
                                    pum.UserId
                                }).Distinct().ToList();
                    var zoneOrCell = (from pum in _context.PropertyUserStatus.Where(i =>
                            i.Property.IncidentId == request.IncidentId &&
                            i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                                      select new
                                      {
                                          pum.UserId,
                                          Zone = pum.Property.Zone,
                                          Cell = pum.Property.Cell
                                      }).ToList();
                    var propStatus = (from pum in temp
                        join atu in _context.AuditTrials.Where(i => i.Role.RoleName.ToLower() == "engineer" && i.TypeofOperation == "LOGIN" && i.Status == "SUCCESS" && i.OperationTimeStamp >= incident.StartTime && i.OperationTimeStamp <= (incident.EndTime == null ? dtcurrent : incident.EndTime)) on pum.UserId equals atu.UserId

                                     select new
                                     {
                                         Firstname = atu.User.FirstName,
                                         Lastname = atu.User.LastName,
                                         Zone = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Zone).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Zone).Distinct().ToList(),
                                         Cell = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Cell).Distinct().ToList(),
                                         LoggedInTime = atu.OperationTimeStamp,
                                         LoggedOutTime = (_context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.TokenId == atu.TokenId).Count() == 0 ? (incident.EndTime != null ? incident.EndTime: null) :  _context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.TokenId == atu.TokenId).OrderByDescending(i => i.OperationTimeStamp).Select(i => new { Name = i.OperationTimeStamp }).FirstOrDefault().Name),
                                     }).OrderByDescending(i=>i.LoggedInTime);
                    return Ok(propStatus);
                }
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Report for Average time taken at status level for Incident at Cell level
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[Authorize]
        [CustomAuthorize(Module = new Features[] { Features.DASHBOARD}, OperationType = OperationType.READ)]
        [HttpGet]
        [Route("api/Report/DownLoadEngineeringCapacity")]
        public HttpResponseMessage DownLoadEngineeringCapacity(string IncidentId, string Type, string Value)
        {
            try
            {
                StringBuilder sbResult = new StringBuilder();
                //for e.g.Isolated. (endtime - starttime) / totalnoofisolatedproperrties = X seconds
                //starttime is the time when the status of any property was updated as isolated in the incident
                //endtime is the time when the status of last property was updated as isolated in the incident
                //if there is only one property isolated, the starttime and endtime will be same, 
                //hence the average time will be 0.This is known issue.
                List<string> dummyList = new List<string>();
                dummyList.Add("Temp for union");
                //    DateTimeOffset dtcurrent = DateTimeOffset.Now;
                DateTime dtcurrent = DateTime.Now;
                var incident = _context.Incidents.Where(i => i.Id == IncidentId).FirstOrDefault();
                //   incident.EndTime = (incident.EndTime == null ? dtcurrent : incident.EndTime);
                //if (Type.ToUpper() == "ZONE")
                //{

                //    var temp = (from pum in _context.PropertyUserMap.Where(i =>
                //                               i.Property.IncidentId == IncidentId && i.Property.Zone == Value &&
                //                               i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                //                select new
                //                {
                //                    pum.UserId
                //                }).Distinct().ToList();
                //    var zoneOrCell = (from pum in _context.PropertyUserMap.Where(i =>
                //             i.Property.IncidentId == IncidentId && i.Property.Zone == Value &&
                //             i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                //                      select new
                //                      {
                //                          pum.UserId,
                //                          Zone = pum.Property.Zone,
                //                          Cell = pum.Property.Cell
                //                      }).ToList();
                //    var propStatus = from pum in temp
                //                     join atu in _context.AuditTrials.Where(i => i.Role.RoleName.ToLower() == "engineer" && i.TypeofOperation == "LOGIN" && i.Status == "SUCCESS" && i.OperationTimeStamp >= incident.StartTime && i.OperationTimeStamp <= (incident.EndTime == null ? dtcurrent : incident.EndTime)) on pum.UserId equals atu.UserId

                //                     select new
                //                     {
                //                         Firstname = atu.User.FirstName,
                //                         Lastname = atu.User.LastName,
                //                         Zone = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Zone).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Zone).Distinct().ToList(),
                //                         Cell = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Cell).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Cell).Distinct().ToList(),
                //                         LoggedInTime = atu.OperationTimeStamp,
                //                         LoggedOutTime = (_context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.OperationTimeStamp > atu.OperationTimeStamp).Count() == 0 ? (incident.EndTime != null ? incident.EndTime : null) : _context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.OperationTimeStamp > atu.OperationTimeStamp).OrderByDescending(i => i.OperationTimeStamp).Select(i => new { Name = i.OperationTimeStamp }).FirstOrDefault().Name)
                //                     };
                //    //propStatus = propStatus.Where(i =>
                //    //    i.LoggedInTime >= incident.StartTime && i.LoggedOutTime <= incident.EndTime);
                //    WriteCSVHeader(sbResult);
                //    foreach (var item in propStatus)
                //    {
                //        WritetoCSV(item, sbResult);
                //    }

                //}
                //else if (Type.ToUpper() == "CELL")
                //{



                //    var temp = (from pum in _context.PropertyUserMap.Where(i =>
                //                               i.Property.IncidentId == IncidentId && i.Property.Cell == Value &&
                //                               i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                //                select new
                //                {
                //                    pum.UserId
                //                }).Distinct().ToList();
                //    var zoneOrCell = (from pum in _context.PropertyUserMap.Where(i =>
                //             i.Property.IncidentId == IncidentId && i.Property.Cell == Value &&
                //             i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                //                      select new
                //                      {
                //                          pum.UserId,
                //                          Zone = pum.Property.Zone,
                //                          Cell = pum.Property.Cell
                //                      }).ToList();
                //    var propStatus = from pum in temp
                //                     join atu in _context.AuditTrials.Where(i => i.Role.RoleName.ToLower() == "engineer" && i.TypeofOperation == "LOGIN" && i.Status == "SUCCESS" && i.OperationTimeStamp >= incident.StartTime && i.OperationTimeStamp <= (incident.EndTime == null ? dtcurrent : incident.EndTime)) on pum.UserId equals atu.UserId

                //                     select new
                //                     {
                //                         Firstname = atu.User.FirstName,
                //                         Lastname = atu.User.LastName,
                //                         Zone = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Zone).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Zone).Distinct().ToList(),
                //                         Cell = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Cell).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Cell).Distinct().ToList(),
                //                         LoggedInTime = atu.OperationTimeStamp,
                //                         LoggedOutTime = (_context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.OperationTimeStamp > atu.OperationTimeStamp).Count() == 0 ? (incident.EndTime != null ? incident.EndTime : null) : _context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.OperationTimeStamp > atu.OperationTimeStamp).OrderByDescending(i => i.OperationTimeStamp).Select(i => new { Name = i.OperationTimeStamp }).FirstOrDefault().Name)
                //                     };
                //    //propStatus = propStatus.Where(i =>
                //    //    i.LoggedInTime >= incident.StartTime && i.LoggedOutTime <= incident.EndTime);
                //    WriteCSVHeader(sbResult);
                //    foreach (var item in propStatus)
                //    {
                //        WritetoCSV(item, sbResult);
                //    }

                //}
                //else
                //{
                //    var temp = (from pum in _context.PropertyUserMap.Where(i =>
                //                              i.Property.IncidentId == IncidentId  &&
                //                              i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                //                select new
                //                {
                //                    pum.UserId
                //                }).Distinct().ToList();
                //    var zoneOrCell = (from pum in _context.PropertyUserMap.Where(i =>
                //             i.Property.IncidentId == IncidentId  &&
                //             i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                //                      select new
                //                      {
                //                          pum.UserId,
                //                          Zone = pum.Property.Zone,
                //                          Cell = pum.Property.Cell
                //                      }).ToList();
                //    var propStatus = from pum in temp
                //                     join atu in _context.AuditTrials.Where(i => i.Role.RoleName.ToLower() == "engineer" && i.TypeofOperation == "LOGIN" && i.Status == "SUCCESS" && i.OperationTimeStamp >= incident.StartTime && i.OperationTimeStamp <= (incident.EndTime == null ? dtcurrent : incident.EndTime)) on pum.UserId equals atu.UserId

                //                     select new
                //                     {
                //                         Firstname = atu.User.FirstName,
                //                         Lastname = atu.User.LastName,
                //                         Zone = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Zone).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Zone).Distinct().ToList(),
                //                         Cell = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Cell).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Cell).Distinct().ToList(),
                //                         LoggedInTime = atu.OperationTimeStamp,
                //                         LoggedOutTime = (_context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.OperationTimeStamp > atu.OperationTimeStamp).Count() == 0 ? (incident.EndTime != null ? incident.EndTime : null) : _context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.OperationTimeStamp > atu.OperationTimeStamp).OrderByDescending(i => i.OperationTimeStamp).Select(i => new { Name = i.OperationTimeStamp }).FirstOrDefault().Name)
                //                     };
                //    //propStatus = propStatus.Where(i =>
                //    //    i.LoggedInTime >= incident.StartTime && i.LoggedOutTime <= incident.EndTime);
                //    WriteCSVHeader(sbResult);
                //    foreach (var item in propStatus)
                //    {
                //        WritetoCSV(item, sbResult);
                //    }
                //}
                if (Type.ToUpper() == "ZONE")
                {

                    var temp = (from pum in _context.PropertyUserStatus.Where(i =>
                                               i.Property.IncidentId == IncidentId && i.Property.Zone == Value &&
                                               i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                                select new
                                {
                                    pum.UserId
                                }).Distinct().ToList();
                    var zoneOrCell = (from pum in _context.PropertyUserStatus.Where(i =>
                             i.Property.IncidentId == IncidentId && i.Property.Zone == Value &&
                             i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                                      select new
                                      {
                                          pum.UserId,
                                          Zone = pum.Property.Zone,
                                          Cell = pum.Property.Cell
                                      }).ToList();
                    var propStatus = (from pum in temp
                                     join atu in _context.AuditTrials.Where(i => i.Role.RoleName.ToLower() == "engineer" && i.TypeofOperation == "LOGIN" && i.Status == "SUCCESS" && i.OperationTimeStamp >= incident.StartTime && i.OperationTimeStamp <= (incident.EndTime == null ? dtcurrent : incident.EndTime)) on pum.UserId equals atu.UserId

                                     select new
                                     {
                                         Firstname = atu.User.FirstName,
                                         Lastname = atu.User.LastName,
                                         Zone = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Zone).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Zone).Distinct().ToList(),
                                         Cell = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Cell).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Cell).Distinct().ToList(),
                                         LoggedInTime = atu.OperationTimeStamp,
                                         LoggedOutTime = (_context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.TokenId == atu.TokenId).Count() == 0 ? (incident.EndTime != null ? incident.EndTime : null) : _context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.TokenId == atu.TokenId).OrderByDescending(i => i.OperationTimeStamp).Select(i => new { Name = i.OperationTimeStamp }).FirstOrDefault().Name),

                                     }).OrderByDescending(i => i.LoggedInTime);
                    //propStatus = propStatus.Where(i =>
                    //    i.LoggedInTime >= incident.StartTime && i.LoggedOutTime <= incident.EndTime);
                    WriteCSVHeader(sbResult);
                    foreach (var item in propStatus)
                    {
                        WritetoCSV(item, sbResult);
                    }

                }
                else if (Type.ToUpper() == "CELL")
                {



                    var temp = (from pum in _context.PropertyUserStatus.Where(i =>
                                               i.Property.IncidentId == IncidentId && i.Property.Cell == Value &&
                                               i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                                select new
                                {
                                    pum.UserId
                                }).Distinct().ToList();
                    var zoneOrCell = (from pum in _context.PropertyUserStatus.Where(i =>
                             i.Property.IncidentId == IncidentId && i.Property.Cell == Value &&
                             i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                                      select new
                                      {
                                          pum.UserId,
                                          Zone = pum.Property.Zone,
                                          Cell = pum.Property.Cell
                                      }).ToList();
                    var propStatus = (from pum in temp
                                     join atu in _context.AuditTrials.Where(i => i.Role.RoleName.ToLower() == "engineer" && i.TypeofOperation == "LOGIN" && i.Status == "SUCCESS" && i.OperationTimeStamp >= incident.StartTime && i.OperationTimeStamp <= (incident.EndTime == null ? dtcurrent : incident.EndTime)) on pum.UserId equals atu.UserId

                                     select new
                                     {
                                         Firstname = atu.User.FirstName,
                                         Lastname = atu.User.LastName,
                                         Zone = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Zone).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Zone).Distinct().ToList(),
                                         Cell = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Cell).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Cell).Distinct().ToList(),
                                         LoggedInTime = atu.OperationTimeStamp,
                                         LoggedOutTime = (_context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.TokenId == atu.TokenId).Count() == 0 ? (incident.EndTime != null ? incident.EndTime : null) : _context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.TokenId == atu.TokenId).OrderByDescending(i => i.OperationTimeStamp).Select(i => new { Name = i.OperationTimeStamp }).FirstOrDefault().Name),

                                     }).OrderByDescending(i => i.LoggedInTime);
                    //propStatus = propStatus.Where(i =>
                    //    i.LoggedInTime >= incident.StartTime && i.LoggedOutTime <= incident.EndTime);
                    WriteCSVHeader(sbResult);
                    foreach (var item in propStatus)
                    {
                        WritetoCSV(item, sbResult);
                    }

                }
                else
                {
                    var temp = (from pum in _context.PropertyUserStatus.Where(i =>
                                              i.Property.IncidentId == IncidentId &&
                                              i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                                select new
                                {
                                    pum.UserId
                                }).Distinct().ToList();
                    var zoneOrCell = (from pum in _context.PropertyUserStatus.Where(i =>
                             i.Property.IncidentId == IncidentId &&
                             i.Property.Deleted == false && i.Role.RoleName.ToLower() == "engineer")
                                      select new
                                      {
                                          pum.UserId,
                                          Zone = pum.Property.Zone,
                                          Cell = pum.Property.Cell
                                      }).ToList();
                    var propStatus = (from pum in temp
                                     join atu in _context.AuditTrials.Where(i => i.Role.RoleName.ToLower() == "engineer" && i.TypeofOperation == "LOGIN" && i.Status == "SUCCESS" && i.OperationTimeStamp >= incident.StartTime && i.OperationTimeStamp <= (incident.EndTime == null ? dtcurrent : incident.EndTime)) on pum.UserId equals atu.UserId

                                     select new
                                     {
                                         Firstname = atu.User.FirstName,
                                         Lastname = atu.User.LastName,
                                         Zone = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Zone).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Zone).Distinct().ToList(),
                                         Cell = zoneOrCell.Where(i => i.UserId == atu.UserId).Select(i => i.Cell).Distinct().ToList(), // _context.Properties.Where(i => i.IncidentId == request.IncidentId  && i.Deleted == false).Select(i => i.Cell).Distinct().ToList(),
                                         LoggedInTime = atu.OperationTimeStamp,
                                         LoggedOutTime = (_context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.TokenId == atu.TokenId).Count() == 0 ? (incident.EndTime != null ? incident.EndTime : null) : _context.AuditTrials.Where(i => i.UserId == atu.UserId && i.Role.RoleName.ToLower() == "engineer" && i.Status == "SUCCESS" && i.TypeofOperation == "LOGOUT" && i.TokenId == atu.TokenId).OrderByDescending(i => i.OperationTimeStamp).Select(i => new { Name = i.OperationTimeStamp }).FirstOrDefault().Name),

                                     }).OrderByDescending(i => i.LoggedInTime);
                    //propStatus = propStatus.Where(i =>
                    //    i.LoggedInTime >= incident.StartTime && i.LoggedOutTime <= incident.EndTime);
                    WriteCSVHeader(sbResult);
                    foreach (var item in propStatus)
                    {
                        WritetoCSV(item, sbResult);
                    }
                }

                var resultResp = GetHttpRespforDownloadMPRN(sbResult);
                return resultResp;
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                throw ex;
            }
        }
        /// <summary>
        /// Constructs and writes the CSV header
        /// </summary>
        private void WriteCSVHeader(StringBuilder sb)
        {
            CSVHelper.AppendValue(sb, "Firstname", true, false);
            CSVHelper.AppendValue(sb, "Lastname", false, false);
            CSVHelper.AppendValue(sb, "Zone", false, false);
            CSVHelper.AppendValue(sb, "Cell", false, false);
            CSVHelper.AppendValue(sb, "LoggedInTime", false, false);
            CSVHelper.AppendValue(sb, "LoggedOutTime", false, true);



        }
        /// <summary>
        /// Send Property item for csv
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="item"></param>
        public static void WritetoCSV(dynamic inputItem, StringBuilder sb)
        {
            CSVHelper.AppendValue(sb, inputItem.Firstname, true, false);
            CSVHelper.AppendValue(sb, inputItem.Lastname, false, false);
            string zone="";
            string cell = "";
            foreach (var itemStatus in inputItem.Zone)
            {

                if (!string.IsNullOrEmpty(itemStatus))
                {
                    zone +=  itemStatus + "|";
                }
            }
            CSVHelper.AppendValue(sb, zone.Substring(0, zone.Length - 1), false, false);

            foreach (var itemStatus in inputItem.Cell)
            {
                if (!string.IsNullOrEmpty(itemStatus))
                {
                    cell += itemStatus + "|";
                }

            }
            CSVHelper.AppendValue(sb, cell.Substring(0, cell.Length - 1), false, false);

            //CSVHelper.AppendValue(sb, inputItem.Zone, false, false);
            // CSVHelper.AppendValue(sb, inputItem.Cell, false, false);
            CSVHelper.AppendValue(sb, inputItem.LoggedInTime != null ? Convert.ToString(inputItem.LoggedInTime.ToString("dd/MM/yyyy hh:mm tt")) : string.Empty, false, false);
            CSVHelper.AppendValue(sb, inputItem.LoggedOutTime != null ? Convert.ToString(inputItem.LoggedOutTime.ToString("dd/MM/yyyy hh:mm tt")) : string.Empty, false, true);



        }
        /// <summary>
        /// Gets the response object to send back to client
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <returns></returns>
        private HttpResponseMessage GetHttpRespforDownloadMPRN(StringBuilder sbResult)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);

            writer.Write(sbResult);
            writer.Flush();
            stream.Position = 0;
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "ENgineerIncidentDetails.csv" };
            return response;
        }
    }
}
