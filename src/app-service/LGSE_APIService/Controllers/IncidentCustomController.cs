using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using AutoMapper;
using LGSE_APIService.Authorization;
using LGSE_APIService.Common;
using LGSE_APIService.Common.Utilities;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;
using LGSE_APIService.RequestObjects;
using LGSE_APIService.ResponseObjects;
using LGSE_APIService.Utilities;
using LGSE_APIService.Validators;
using Microsoft.Azure.Mobile.Server.Config;
using Newtonsoft.Json;


using Microsoft.Azure.Mobile.Server;
using System.Web.Http.OData.Query;

namespace LGSE_APIService.Controllers
{
    [MobileAppController]
    //Custom controller for Incident Management
    public class IncidentCustomController : ApiController
    {
        public LGSE_APIContext _context = null;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            _context = LGSE_APIContext.GetIntance();
            DbUtilities.dbContext = _context;
            ValidationUtilities.dbContext = _context;
        }
        //public IncidentCustomController()
        //{

        //}
        [HttpPost]
        [Route("api/IncidentCustom/Create")]
        [ValidateModel]
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT }, OperationType = OperationType.CREATE)]
        public async Task<IHttpActionResult> CreateIncident(IncidentRequest request)
        {
            try
            {
                LGSELogger.Information("Incident Creation started.");
                ValidationUtilities.ValidateIncidentCreate(request);
                string currentUserEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                //Collect Cell managers and zone managers
                List<string> cellManagers = request.MPRNs.Where(item => string.IsNullOrEmpty(item.CellManagerName) == false).Select(item => item.CellManagerName).ToList();
                List<string> zoneManagers = request.MPRNs.Where(item => string.IsNullOrEmpty(item.ZoneManagerName) == false).Select(item => item.ZoneManagerName).ToList();

                List<User> dbCellManagers = DbUtilities.GetUsersByNames(cellManagers).ToList();
                List<User> dbzoneManager = DbUtilities.GetUsersByNames(zoneManagers).ToList();

                ValidationUtilities.ValidateIncidentManagers(request.MPRNs, dbCellManagers, dbzoneManager, request.ResolveUsers);
                //Call bing map api to resolv Address

                _context.Configuration.AutoDetectChangesEnabled = false;
                _context.Configuration.ValidateOnSaveEnabled = false;
                Mapper.Initialize(cfg => cfg.CreateMap<IncidentRequest, Incident>().ForMember(i => i.CreatedBy,
          j => j.UseValue(currentUserEmail)));

                var IncidentDbMap = Mapper.Map<IncidentRequest, Incident>(request);
                IncidentDbMap.Id = Guid.NewGuid().ToString();
                //Need to change this logic
                int incId = _context.Incidents.Count() + 1;
                IncidentDbMap.IncidentId = "INC" + $"{incId:D6}";
                LGSELogger.Information("Incident id generated as:" + IncidentDbMap.IncidentId);

                //calculate zones,cells and props and store
                IncidentDbMap.NoOfPropsAffected = request.MPRNs.Count;
                IncidentDbMap.NoOfZones = request.MPRNs.Select(i => i.Zone).Distinct().Count();
                IncidentDbMap.NoOfCells = request.MPRNs.Select(i => i.Cell).Distinct().Count();
                IncidentDbMap.StartTime = DateTime.UtcNow;
                IncidentDbMap.Status = 0;
                //0-Inprogress,1-Completed,2 cancelled.
                Incident incidentDb = _context.Incidents.Add(IncidentDbMap);
                List<Property> PMRNSList = await GetMappedMPRNs(request.MPRNs, IncidentDbMap.Id, dbCellManagers, dbzoneManager, request.ResolveUsers);
                _context.Properties.AddRange(PMRNSList);
                _context.SaveChanges();

                //_context.IncidentPropsStatusCounts.Add(new IncidentPropsStatusCounts()
                //{
                //    Id = Guid.NewGuid().ToString(),
                //    IncidentId = IncidentDbMap.Id,
                //    NS = request.MPRNs.Count,
                //    NA = 0,
                //    NC = 0,
                //    IS = 0,
                //    RS = 0,
                //    CreatedBy = currentUserEmail,
                //    CreatedAt = DateTimeOffset.UtcNow,
                //    UpdatedAt = DateTimeOffset.UtcNow

                //});
                //_context.SaveChanges();
                //Send the MPRNS to QUEUE for Backgound processing to Resolv MPRNS.
                // request.Id = IncidentDbMap.Id;
                // SendNewMPRNStoQueue(request);
                LGSELogger.Information("Incident Creation successful.");
                return Ok();
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/IncidentCustom/Edit")]
        [ValidateModel]
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT }, OperationType = OperationType.UPDATE)]
        public async Task<IHttpActionResult> EditIncident(IncidentEditRequest request)
        {
            try
            {
                LGSELogger.Information("Incident Edit started");
                string errorMessage = ValidationUtilities.ValidateIncident(request);
                if (errorMessage.Equals(string.Empty))
                {

                    Incident dbIncidentDetails = DbUtilities.GetIncidentDetails(request.Id);
                    if (dbIncidentDetails != null)
                    {
                        _context.Configuration.AutoDetectChangesEnabled = false;
                        _context.Configuration.ValidateOnSaveEnabled = false;
                        //Collect Cell managers and zone managers
                        List<User> dbCellManagers = new List<DataObjects.User>();
                        List<User> dbzoneManager = new List<DataObjects.User>();
                        // Do not consider MPRNs if they are not passed in request
                        if (request.MPRNs != null && request.MPRNs.Count > 0)
                        {
                            List<string> cellManagers = request.MPRNs.Where(item => string.IsNullOrEmpty(item.CellManagerName) == false).Select(item => item.CellManagerName).ToList();
                            List<string> zoneManagers = request.MPRNs.Where(item => string.IsNullOrEmpty(item.ZoneManagerName) == false).Select(item => item.ZoneManagerName).ToList();
                            dbCellManagers = DbUtilities.GetUsersByNames(cellManagers).ToList();
                            dbzoneManager = DbUtilities.GetUsersByNames(zoneManagers).ToList();
                            ValidationUtilities.ValidateIncidentManagers(request.MPRNs, dbCellManagers, dbzoneManager, request.ResolveUsers);
                        }
                        string currentUserEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                        Mapper.Initialize(cfg => cfg.CreateMap<IncidentEditRequest, Incident>().ForMember(i => i.ModifiedBy,
                        j => j.UseValue(currentUserEmail)).ForMember(i => i.UpdatedAt, i => i.UseValue(DateTimeOffset.UtcNow)));

                        var IncidentDbMap = Mapper.Map<IncidentEditRequest, Incident>(request, dbIncidentDetails);
                        if (request.MPRNs != null && request.MPRNs.Count > 0)
                        {
                            //dbIncidentDetails.NoOfPropsAffected = request.MPRNs.Count;
                            dbIncidentDetails.NoOfZones = request.MPRNs.Select(i => i.Zone).Distinct().Count();
                            dbIncidentDetails.NoOfCells = request.MPRNs.Select(i => i.Cell).Distinct().Count();
                        }
                        if (request.Status == 1 || request.Status == 2)
                        {
                            //cancelled or completed;
                            dbIncidentDetails.EndTime = DateTime.UtcNow;
                            //UnassignMPRNsForCancelledOrCompletedIncidents(request.Id);
                        }
                        dbIncidentDetails.UpdatedAt = DateTimeOffset.UtcNow;
                        _context.Entry(dbIncidentDetails).State = System.Data.Entity.EntityState.Modified;
                        if (request.MPRNs != null && request.MPRNs.Count > 0)
                        {
                            // Consider MPRNs only when they supplied
                            List<string> mprnids = request.MPRNs.Select(item => item.MPRN).ToList();
                            List<Property> existingMPRNS = DbUtilities.GetExistingMPRNs(mprnids, dbIncidentDetails.Id).ToList();
                            List<Property> PMRNSList = await GetMappedMPRNsforEdit(request, dbIncidentDetails.Id, dbCellManagers, dbzoneManager, existingMPRNS, request.ResolveUsers);

                        }

                        _context.SaveChanges();
                        if (request.MPRNs != null && request.MPRNs.Count > 0)
                        {
                            UpdateNoOfAffectedPropertiesCount(request.Id);
                        }
                       //    DbUtilities.PropertyStatusCount(IncidentDbMap.Id);

                            // invoke job 
                            //pass the incidentid 
                            //     request.Id = IncidentDbMap.Id;
                            //  SendUpdatedMPRNStoQueue(request);
                            LGSELogger.Information("Incident updation successful.");
                        return Ok();
                    }
                    else
                    {
                        return BadRequest(ErrorCodes.INVALID_INCIDENT_ID.ToString());
                    }
                }
                else
                {
                    return BadRequest(errorMessage);
                }
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                // System.Diagnostics.Trace.TraceError("Error in Edit Incident" + ex.Message + "\n" + ex.StackTrace.ToString() + "\n" + ex.InnerException.ToString());
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }

      

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT, Features.RESOURCEMGT }, OperationType = OperationType.READ)]
        [HttpGet]
        [Route("api/IncidentCustom/GetAssignedMPRNS")]
        public IHttpActionResult GetAssignedMPRNS()
        {
            string userEmail = HttpUtilities.GetUserNameFromToken(this.Request);
            try
            {
                dynamic useDetails;
                //dynamic user = new ExpandoObject();

                //Abhijeet 06-11-2018
                //Remove PropertyUserMap in Query

                //useDetails = (from user in _context.Users
                //              join pumap in _context.PropertyUserMap.Where(i => i.Deleted == false) on user.Id equals pumap.UserId
                //              join prop in _context.Properties.Where(i => i.Deleted == false) on pumap.PropertyId equals prop.Id
                //              where user.Email == userEmail
                //              select new
                //              {
                //                  user.Email,
                //                  prop.Id,
                //                  prop.MPRN,
                //                  prop.BuildingName,
                //                  prop.SubBuildingName,
                //                  prop.MCBuildingName,
                //                  prop.MCSubBuildingName,
                //                  prop.BuildingNumber,
                //                  prop.PrincipalStreet,
                //                  prop.DependentStreet,
                //                  prop.PostTown,
                //                  prop.LocalityName,
                //                  prop.DependentLocality,
                //                  prop.Country,
                //                  prop.Postcode,
                //                  prop.PriorityCustomer,
                //                  prop.Zone,
                //                  prop.Cell,
                //                  prop.IncidentId,
                //                  CellManager = _context.Users.Where(item => item.Id == prop.CellManagerId).Select(item => item.FirstName + " " + item.LastName).FirstOrDefault(),
                //                  ZoneManager = _context.Users.Where(item => item.Id == prop.ZoneManagerId).Select(item => item.FirstName + " " + item.LastName).FirstOrDefault(),
                //                  prop.Status,
                //              }).ToList();

                useDetails = (from user in _context.Users.Where(i => i.Email == userEmail)
                              join pumap in _context.PropertyUserStatus.Where(i => i.Deleted == false && i.StatusId == null) on user.Id equals pumap.UserId
                    join prop in _context.Properties.Where(i => i.Deleted == false) on pumap.PropertyId equals prop.Id
                    where user.Email == userEmail
                    select new
                    {
                        user.Email,
                        prop.Id,
                        prop.MPRN,
                        prop.BuildingName,
                        prop.SubBuildingName,
                        prop.MCBuildingName,
                        prop.MCSubBuildingName,
                        prop.BuildingNumber,
                        prop.PrincipalStreet,
                        prop.DependentStreet,
                        prop.PostTown,
                        prop.LocalityName,
                        prop.DependentLocality,
                        prop.Country,
                        prop.Postcode,
                        prop.PriorityCustomer,
                        prop.Zone,
                        prop.Cell,
                        prop.IncidentId,
                        CellManager = _context.Users.Where(item => item.Id == prop.CellManagerId).Select(item => item.FirstName + " " + item.LastName).FirstOrDefault(),
                        ZoneManager = _context.Users.Where(item => item.Id == prop.ZoneManagerId).Select(item => item.FirstName + " " + item.LastName).FirstOrDefault(),
                        prop.Status,
                    }).ToList();
                return Ok(useDetails);
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }


        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT, Features.RESOURCEMGT }, OperationType = OperationType.READ)]
        [HttpGet]
        [Route("api/IncidentCustom/GetUserMPRNS")]
        public IHttpActionResult GetUserMPRNS(string userEmail, string roleId)
        {
            try
            {
                //  Role preferredRole = DbUtilities.GetUserPreferredRole(userEmail); //Abhijeet 24-10-2018
                //   string userRole = HttpUtilities.GetUserRoleAccessApi(this.Request); // Added 24-10-2018
                var preferredRole = _context.Roles.FirstOrDefault(i => i.Id == roleId); // Added 24-10-2018
                dynamic useDetails;
                //dynamic user = new ExpandoObject();
                //Do not send already updated status

                //Abhijeet 06-11-2018
                //Remove PropertyUserMap in Query

                //useDetails = (from user in _context.Users
                //              join pumap in _context.PropertyUserMap.Where(i => i.Deleted == false) on user.Id equals pumap.UserId
                //              join prop in _context.Properties.Where(i => i.Deleted == false && i.Incident.Status == 0) on pumap.PropertyId equals prop.Id
                //              where user.Email == userEmail && pumap.RoleId == preferredRole.Id &&
                //              prop.PropertyUserStatus.Count(i => i.Deleted == false && i.PropertyUserMapsId == pumap.Id) == 0
                //              select new
                //              {
                //                  user.Email,
                //                  prop.Id,
                //                  prop.MPRN,
                //                  prop.BuildingName,
                //                  prop.SubBuildingName,
                //                  prop.MCBuildingName,
                //                  prop.MCSubBuildingName,
                //                  prop.BuildingNumber,
                //                  prop.PrincipalStreet,
                //                  prop.DependentStreet,
                //                  prop.PostTown,
                //                  prop.LocalityName,
                //                  prop.DependentLocality,
                //                  prop.Country,
                //                  prop.Postcode,
                //                  prop.PriorityCustomer,
                //                  prop.Zone,
                //                  prop.Cell,
                //                  prop.IncidentId,
                //                  CellManager = _context.Users.Where(item => item.Id == prop.CellManagerId).Select(item => item.FirstName + " " + item.LastName).FirstOrDefault(),
                //                  ZoneManager = _context.Users.Where(item => item.Id == prop.ZoneManagerId).Select(item => item.FirstName + " " + item.LastName).FirstOrDefault(),
                //                  prop.Status,
                //                  Latitude = "51.52219" + prop.CreatedAt.Value.Second,
                //                  Longitude = "-0.11505" + prop.CreatedAt.Value.Second,
                //                  IncidentName = prop.Incident.IncidentId
                //              }).ToList();

                useDetails = (from user in _context.Users.Where(i=>i.Email==userEmail)
                              join pumap in _context.PropertyUserStatus.Where(i => i.Deleted == false && i.StatusId == null && i.RoleId==preferredRole.Id) on user.Id equals pumap.UserId
                              join prop in _context.Properties.Where(i => i.Deleted == false && i.Incident.Status == 0) on pumap.PropertyId equals prop.Id
                             where user.Email == userEmail && pumap.RoleId == preferredRole.Id 
                              select new
                              {
                                  user.Email,
                                  prop.Id,
                                  prop.MPRN,
                                  prop.BuildingName,
                                  prop.SubBuildingName,
                                  prop.MCBuildingName,
                                  prop.MCSubBuildingName,
                                  prop.BuildingNumber,
                                  prop.PrincipalStreet,
                                  prop.DependentStreet,
                                  prop.PostTown,
                                  prop.LocalityName,
                                  prop.DependentLocality,
                                  prop.Country,
                                  prop.Postcode,
                                  prop.PriorityCustomer,
                                  prop.Zone,
                                  prop.Cell,
                                  prop.IncidentId,
                                  LatestStatus= prop.PropertyStatusMstr.Status,
                                  LatestSubStatus = prop.PropertySubStatusMstr.SubStatus,
                                  CellManager = _context.Users.Where(item => item.Id == prop.CellManagerId).Select(item => item.FirstName + " " + item.LastName).FirstOrDefault(),
                                  ZoneManager = _context.Users.Where(item => item.Id == prop.ZoneManagerId).Select(item => item.FirstName + " " + item.LastName).FirstOrDefault(),
                                  prop.Status,
                                  AssignedResourceCount = (from pus in prop.PropertyUserStatus.Where(i => i.Deleted == false && i.StatusId == null && i.Notes == null)

                                          select new { pus.PropertyId, pus.UserId, pus.RoleId }//Abhijeet added  pum.RoleId 25-10-2018
                                      ).Distinct().Count(),
                                  Latitude = "51.52219" + prop.CreatedAt.Value.Second,
                                  Longitude = "-0.11505" + prop.CreatedAt.Value.Second,
                                  IncidentName = prop.Incident.IncidentId
                              }).ToList();
                return Ok(useDetails);
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT, Features.RESOURCEMGT }, OperationType = OperationType.READ)]
        [Route("api/IncidentCustom/GetMPRNResources")]
        public IHttpActionResult GetMPRNResources(string PropertyId)
        {
            try
            {
                dynamic useDetails;
                //Abhijeet 06-11-2018
                //Remove PropertyUserMap in Query
                // Return only inprogress detials..
                //useDetails = (from pum in _context.PropertyUserMap.Where(i => i.PropertyId == PropertyId && i.Deleted == false)  // on user.Id equals pumap.UserId
                //              join pus in _context.PropertyUserStatus.Where(i => i.Deleted == false)
                //                          on pum.Id equals pus.PropertyUserMapsId into temp
                //              from lj in temp.DefaultIfEmpty()
                //              where lj == null || lj.StatusId == null
                //              select new
                //              {
                //                  pum.User.Email,
                //                  pum.User.Id,
                //                  pum.User.FirstName,
                //                  pum.User.LastName,
                //                  RoleId = pum.RoleId,
                //                  RoleName = pum.Role.RoleName,//Abhijeet added  RoleName 25-10-2018
                //                  Roles = pum.User.UserRoleMap.Select(i => i.Role.RoleName).ToList(),
                //                  pum.PropertyId,
                //                  pum.Property.MPRN,
                //              }).ToList();

                useDetails = (from pus in _context.PropertyUserStatus.Where(i => i.PropertyId == PropertyId && i.Deleted == false && i.StatusId==null && i.Notes==null)
                       
                    select new
                    {
                        pus.User.Email,
                        pus.User.Id,
                        pus.User.FirstName,
                        pus.User.LastName,
                        RoleId = pus.RoleId,
                        RoleName = pus.Role.RoleName,//Abhijeet added  RoleName 25-10-2018
                        Roles = pus.User.UserRoleMap.Select(i => i.Role.RoleName).ToList(),
                        Zones = pus.User.PropertyUserStatus.Where(i => i.Deleted == false && i.Property.Incident.Status == 0 && i.RoleId == pus.RoleId).Select(i => i.Property.Zone).Distinct().ToList(),
                        Cells = pus.User.PropertyUserStatus.Where(i => i.Deleted == false && i.Property.Incident.Status == 0 && i.RoleId == pus.RoleId).Select(i => i.Property.Cell).Distinct().ToList(),
                        AssignedMPRNCount = //(user.PropertyUserMaps.Count(i => i.Property.Incident.Status == 0 && i.RoleId == urm.RoleId && i.Deleted == false) - user.PropertyUserStatus.Count(i => i.Property.Incident.Status == 0 && i.RoleId == urm.RoleId && i.Deleted == false)),
                            (from pus1 in pus.User.PropertyUserStatus.Where(i => i.Deleted == false && i.RoleId == pus.RoleId && i.StatusId == null)
                                where pus1.Property.Incident.Status == 0
                                select new { pus1.PropertyId, pus1.UserId, pus1.RoleId }
                            ).Distinct().Count(),
                        Completed = //user.PropertyUserStatus.Count(i => i.Property.Incident.Status == 0 && i.RoleId == urm.RoleId && i.Deleted == false),
                            //Abhijeet 06-11-2018
                            //Remove PropertyUserMap in Query
                            (from pus1 in pus.User.PropertyUserStatus.Where(i => i.Deleted == false && i.RoleId == pus.RoleId && i.StatusId != null)
                                where pus1.Property.Incident.Status == 0
                                select new { pus1.PropertyId, pus1.UserId, pus1.RoleId }
                            ).Distinct().Count(),
                        pus.PropertyId,
                        pus.Property.MPRN,
                    }).ToList();
                return Ok(useDetails);
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.RESOURCEMGT }, OperationType = OperationType.CREATE)]
        [HttpPost]
        [Route("api/IncidentCustom/MPRNsAssignment")]
        [ValidateModel]

        //Abhijeet 06-11-2018
        //Remove PropertyUserMap in Query
        //public IHttpActionResult AssignOrUnassignMPRNs(List<AssignMPRNsRequest> request)
        //{
        //    try
        //    {

        //        ValidationUtilities.ValidateMPRNAssignment(request);
        //        string currentUserEmail = HttpUtilities.GetUserNameFromToken(this.Request);
        //        if (request != null && request.Count > 0)
        //        {
        //            foreach (var item in request.Where(i => i.IsUnAssign == false).ToList())
        //            {

        //                //Assigns MPRNs
        //                _context.PropertyUserMap.Add(new PropertyUserMap
        //                {
        //                    Id = Guid.NewGuid().ToString(),
        //                    PropertyId = item.PropertyId,
        //                    RoleId = item.RoleId,
        //                    UserId = item.UserId,
        //                    CreatedBy = currentUserEmail,
        //                    CreatedAt = DateTimeOffset.UtcNow,
        //                    //   UpdatedAt = DateTimeOffset.UtcNow

        //                });
        //            }
        //            //Unassigns MPRN
        //            var dbItemsToRemove = (from pum in _context.PropertyUserMap.Where(i => i.Deleted == false).ToList()
        //                                   join req in request on new { pum.PropertyId, pum.UserId, pum.RoleId } //, pum.RoleId //Abhijeet Added RoleId filter
        //                                            equals new { req.PropertyId, req.UserId, req.RoleId } //, req.RoleId
        //                                   join pus in _context.PropertyUserStatus.Where(i => i.Deleted == false)
        //                                            on pum.Id equals pus.PropertyUserMapsId into temp
        //                                   from lj in temp.DefaultIfEmpty()
        //                                   where (lj == null || lj.StatusId == null) && req.IsUnAssign == true
        //                                   select pum).ToList();
        //            if (dbItemsToRemove != null && dbItemsToRemove.Count > 0)
        //            {
        //                //Mark them as deleted:
        //                foreach (var item in dbItemsToRemove)
        //                {
        //                    item.Deleted = true;
        //                    //    item.UpdatedAt = DateTimeOffset.UtcNow;
        //                }
        //                // _context.PropertyUserMap.RemoveRange(dbItemsToRemove);
        //            }
        //            _context.SaveChanges();
        //            return Ok();
        //        }
        //        else
        //        {
        //            return BadRequest(ErrorCodes.INPUT_DOES_NOT_HAVE_DATA.ToString());
        //        }
        //    }
        //    catch (HttpResponseException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        LGSELogger.Error(ex);
        //        return InternalServerError(ex);
        //    }
        //}
        public async Task<IHttpActionResult> AssignOrUnassignMPRNs(List<AssignMPRNsRequest> request)
        {
            try
            {

                ValidationUtilities.ValidateMPRNAssignment(request);
                string currentUserEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                string incidentId = _context.Incidents.FirstOrDefault(i => i.Status == 0).Id;
                if (request != null && request.Count > 0)
                {
                    List<PropertyUserStatus> dbList = new List<PropertyUserStatus>();
                    foreach (var item in request.Where(i => i.IsUnAssign == false).ToList())
                    {

                        //Assigns MPRNs
                        dbList.Add(new PropertyUserStatus
                        {
                            Id = Guid.NewGuid().ToString(),
                            PropertyId = item.PropertyId,
                            RoleId = item.RoleId,
                            UserId = item.UserId,
                            StatusChangedOn = DateTimeOffset.UtcNow,
                            IncidentId = incidentId,
                            CreatedBy = currentUserEmail,
                            CreatedAt = DateTimeOffset.UtcNow,
                            //   UpdatedAt = DateTimeOffset.UtcNow

                        });
                    }

                    _context.PropertyUserStatus.AddRange(dbList);
                    //Unassigns MPRN
                    if (request.Where(i => i.IsUnAssign == true).Count() > 0)
                    {
                        var dbItemsToRemove =
                            (from pus in _context.PropertyUserStatus.Where(i => i.Deleted == false).ToList()
                                join req in request on
                                    new
                                    {
                                        pus.PropertyId, pus.UserId, pus.RoleId
                                    } //, pum.RoleId //Abhijeet Added RoleId filter
                                    equals new {req.PropertyId, req.UserId, req.RoleId} //, req.RoleId
                                where (pus == null || pus.StatusId == null) && req.IsUnAssign == true
                                select pus).ToList();
                        if (dbItemsToRemove != null && dbItemsToRemove.Count > 0)
                        {
                            //Mark them as deleted:
                            foreach (var item in dbItemsToRemove)
                            {
                                item.Deleted = true;
                                //    item.UpdatedAt = DateTimeOffset.UtcNow;
                            }

                            // _context.PropertyUserMap.RemoveRange(dbItemsToRemove);
                        }
                    }

                    _context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest(ErrorCodes.INPUT_DOES_NOT_HAVE_DATA.ToString());
                }
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT }, OperationType = OperationType.READ)]
        [HttpPost]
        [Route("api/IncidentCustom/GetPropZoneCellCounts")]
        public IHttpActionResult GetPropZoneCellCounts(List<PropertyCountsRequest> request)
        {
            dynamic result = new ExpandoObject();
            result.NoOfPropsAffected = 0;
            result.NoOfZones = 0;
            result.NoOfCells = 0;
            if (request != null)
            {
                result.NoOfPropsAffected = request.Count;
                result.NoOfZones = request.Select(i => i.Zone).Distinct().Count();
                result.NoOfCells = request.Select(i => i.Cell).Distinct().Count();
            }
            return Ok(result);
        }
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT }, OperationType = OperationType.READ)]
        [HttpGet]
        [Route("api/IncidentCustom/GetPostCodes")]
        public IHttpActionResult GetPostCode(string incidentId)
        {
            dynamic result = _context.Properties.Where(i => i.IncidentId == incidentId).Select(i => i.Postcode.Substring(0, i.Postcode.IndexOf(" "))).Distinct();
            return Ok(result);
        }
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT,Features.RESOURCEMGT,Features.ASSIGNEDMPRN }, OperationType = OperationType.UPDATE)]
        [HttpPost]
        [Route("api/IncidentCustom/PropertyUserStatus")]
        public IHttpActionResult PostPropertyUserStatus(PropertyUserStatusRequest item)
        {
            User account = DbUtilities.GetLoggedInUser(this.Request);
            //   Role preferredRole = DbUtilities.GetUserPreferredRole(account.Email); //Abhijeet 24-10-2018
            string userRole = HttpUtilities.GetUserRoleAccessApi(this.Request); // Added 24-10-2018
            var preferredRole = _context.Roles.FirstOrDefault(i => i.Id == userRole); // Added 24-10-2018

            string incidentId = _context.Incidents.FirstOrDefault(i => i.Status == 0).Id;// Added 09-11-2018
            if (_context.PropertyUserStatus.Where(i =>
                    i.Deleted == false && i.UserId == account.Id && i.RoleId == userRole &&
                    i.PropertyId == item.PropertyId && i.StatusId==null && i.Notes == null && i.Deleted == false).Count() == 0)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<PropertyUserStatusRequest, PropertyUserStatus>()
                    .ForMember(i => i.CreatedBy, j => j.UseValue(account.Email))
                    .ForMember(i => i.StatusChangedOn, j => j.UseValue(DateTime.Now))
                    .ForMember(i => i.UserId, j => j.UseValue(account.Id))
                );
                if (string.IsNullOrEmpty(item.StatusId))
                    item.StatusId = null;
                //item.StatusId = "7A279257-8CFA-488C-A807-FDBD05A81DD0";
                var PropertyUserStatusMap = Mapper.Map<PropertyUserStatusRequest, PropertyUserStatus>(item);
                PropertyUserStatusMap.Id = Guid.NewGuid().ToString();
                PropertyUserStatusMap.RoleId = preferredRole.Id;
                PropertyUserStatusMap.StatusChangedOn = DateTime.UtcNow;
                PropertyUserStatusMap.IncidentId = incidentId;
                var result = _context.PropertyUserStatus.Add(PropertyUserStatusMap);
            }
            else
            {
                var pusResult = _context.PropertyUserStatus.Where(i => i.PropertyId == item.PropertyId && i.UserId == account.Id && i.RoleId == userRole && i.StatusId == null && i.Notes == null && i.Deleted == false).ToList();
                if (pusResult != null && pusResult.Count > 0)
                {

                    foreach (var propItem in pusResult)
                    {
                        propItem.StatusId = item.StatusId;
                        propItem.PropertySubStatusMstrsId = item.PropertySubStatusMstrsId;
                        propItem.Notes = item.Notes;
                        propItem.StatusChangedOn = DateTime.UtcNow;
                        propItem.ModifiedBy = account.Email;
                    }

                }
            }
            //Abhijeet 06-11-2018
            //Remove PropertyUserMap in Query

            //var propUserMap = _context.PropertyUserMap.OrderByDescending(i => i.CreatedAt).FirstOrDefault(i => i.UserId == account.Id && i.PropertyId == item.PropertyId && i.RoleId == preferredRole.Id);
            //if (propUserMap != null)
            //{
            //    PropertyUserStatusMap.PropertyUserMapsId = propUserMap.Id;
            //}
          
            var IsIsaolated = _context.PropertyStatusMstr.Where(i => i.Id == item.StatusId).Select(i => i.Status).FirstOrDefault();
            if (IsIsaolated == "Isolated")
            {
                var prop = _context.Properties.Where(i => i.Id == item.PropertyId).ToList();
                if (prop != null && prop.Count > 0)
                {

                    foreach (var propItem in prop)
                    {
                        propItem.StatusId = item.StatusId;
                        propItem.SubStatusId = item.PropertySubStatusMstrsId;
                        propItem.IsIsolated = true;
                        //   propItem.UpdatedAt = DateTimeOffset.UtcNow;
                        propItem.ModifiedBy = account.Email;
                    }

                }


            }
            else
            {
                var prop = _context.Properties.Where(i => i.Id == item.PropertyId).ToList();
                if (prop != null && prop.Count > 0)
                {

                    foreach (var propItem in prop)
                    {
                        propItem.StatusId = item.StatusId;
                        propItem.SubStatusId = item.PropertySubStatusMstrsId;
                        //  propItem.UpdatedAt = DateTimeOffset.UtcNow;
                        propItem.ModifiedBy = account.Email;
                    }

                }
            }
            _context.SaveChanges();
            //var IncidentId = _context.Properties.FirstOrDefault(i => i.Id == item.PropertyId).IncidentId;
            //DbUtilities.PropertyStatusCount(IncidentId);
            return Ok();
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT }, OperationType = OperationType.READ)]
        [HttpGet]
        [Route("api/IncidentCustom/IsAnyIncidentInprogress")]
        public IHttpActionResult IsAnyIncidentInprogress()
        {
            var result = _context.Incidents.Any(i => (i.Status == 0 || i.Status == null) && i.Deleted == false);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT,Features.ASSIGNEDMPRN,Features.RESOURCEMGT,Features.DASHBOARD }, OperationType = OperationType.READ)]
        [Route("api/IncidentCustom/GetInprogressIncident")]
        public IHttpActionResult GetInprogressIncident()
        {
            Incident result = _context.Incidents.FirstOrDefault(i => (i.Status == 0 || i.Status == null) && i.Deleted == false);
            if (result == null)
            {
                result = _context.Incidents.OrderByDescending(i => i.EndTime).FirstOrDefault(i => (i.Status == 1 || i.Status == 2) && i.Deleted == false);
            }
            return Ok(result);
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT, Features.RESOURCEMGT, Features.ASSIGNEDMPRN }, OperationType = OperationType.READ)]
        [HttpGet]
        [Route("api/IncidentCustom/MPRNStatusHistory")]
        public IHttpActionResult GetMPRNStatusHistory(string propertyId)
        {
            try
            {
                //Abhijeet 06-11-2018
                //Remove PropertyUserMap in Query
                //Remove PropertyUserMap in Query
                //var result = (from pum in _context.PropertyUserMap
                //              join usr in _context.Users on pum.UserId equals usr.Id
                //              where pum.PropertyId == propertyId
                //              select new PropertyHistoryResponse
                //              {
                //                  FirstName = usr.FirstName,
                //                  LastName = usr.LastName,
                //                  Email = usr.Email,
                //                  Id = usr.Id,
                //                  Status = "Assigned",
                //                  StatusId = "",
                //                  SubStatus = "",
                //                  PropertySubStatusMstrsId = "",
                //                  StatusChangedOn = pum.CreatedAt,
                //                  Notes = "",
                //                  roleId = pum.RoleId,
                //                  roleName = pum.Role.RoleName,
                //              }).Union(from pum in _context.PropertyUserMap
                //                       join usr in _context.Users on pum.UserId equals usr.Id
                //                       where pum.PropertyId == propertyId && pum.Deleted
                //                       select new PropertyHistoryResponse
                //                       {
                //                           FirstName = usr.FirstName,
                //                           LastName = usr.LastName,
                //                           Email = usr.Email,
                //                           Id = usr.Id,
                //                           Status = "UnAssigned",
                //                           StatusId = "",
                //                           SubStatus = "",
                //                           PropertySubStatusMstrsId = "",
                //                           StatusChangedOn = pum.UpdatedAt,
                //                           Notes = "",
                //                           roleId = pum.RoleId,
                //                           roleName = pum.Role.RoleName,

                //                       }).Union(from pus in _context.PropertyUserStatus
                //                                join psm in _context.PropertyStatusMstr on pus.StatusId equals psm.Id
                //                                join usr in _context.Users on pus.UserId equals usr.Id
                //                                where pus.PropertyId == propertyId
                //                                select new PropertyHistoryResponse
                //                                {
                //                                    FirstName = usr.FirstName,
                //                                    LastName = usr.LastName,
                //                                    Email = usr.Email,
                //                                    Id = usr.Id,
                //                                    Status = psm.Status,
                //                                    StatusId = pus.StatusId,
                //                                    SubStatus = pus.PropertySubStatusMstr.SubStatus,
                //                                    PropertySubStatusMstrsId = pus.PropertySubStatusMstrsId,
                //                                    StatusChangedOn = pus.StatusChangedOn,
                //                                    Notes = pus.Notes,
                //                                    roleId = pus.Role.Id,    //.FirstOrDefault(i => i.StatusId == pus.StatusId).RoleId,
                //                                    roleName = pus.Role.RoleName,
                //                                    //psm.RoleStatusMaps.FirstOrDefault(i => i.StatusId == pus.StatusId).Role.RoleName,

                //                                }).OrderByDescending(i => i.StatusChangedOn).ToList();

                //Abhijeet 06-11-2018
                //Remove PropertyUserMap in Query
                var result = (from pus in _context.PropertyUserStatus.Where(i=>i.Role.RoleName.ToUpper()=="ENGINEER" || i.Role.RoleName.ToUpper() == "ISOLATOR")
                              join usr in _context.Users on pus.UserId equals usr.Id
                              where pus.PropertyId == propertyId
                              select new PropertyHistoryResponse
                              {
                                  FirstName = usr.FirstName,
                                  LastName = usr.LastName,
                                  Email = usr.Email,
                                  Id = usr.Id,
                                  Status = "Assigned",
                                  StatusId = "",
                                  SubStatus = "",
                                  PropertySubStatusMstrsId = "",
                                  StatusChangedOn = pus.CreatedAt,
                                  Notes = "",
                                  roleId = pus.RoleId,
                                  roleName = pus.Role.RoleName,
                              }).Union(from pus in _context.PropertyUserStatus.Where(i => i.Role.RoleName.ToUpper() == "ENGINEER" || i.Role.RoleName.ToUpper() == "ISOLATOR")
                                       join usr in _context.Users on pus.UserId equals usr.Id
                                       where pus.PropertyId == propertyId && pus.Deleted
                                       select new PropertyHistoryResponse
                                       {
                                           FirstName = usr.FirstName,
                                           LastName = usr.LastName,
                                           Email = usr.Email,
                                           Id = usr.Id,
                                           Status = "UnAssigned",
                                           StatusId = "",
                                           SubStatus = "",
                                           PropertySubStatusMstrsId = "",
                                           StatusChangedOn = pus.UpdatedAt,
                                           Notes = "",
                                           roleId = pus.RoleId,
                                           roleName = pus.Role.RoleName,

                                       }).Union(from pus in _context.PropertyUserStatus.Where(i => i.StatusId==null && i.Notes!=null)
                    join usr in _context.Users on pus.UserId equals usr.Id
                    where pus.PropertyId == propertyId
                    select new PropertyHistoryResponse
                    {
                        FirstName = usr.FirstName,
                        LastName = usr.LastName,
                        Email = usr.Email,
                        Id = usr.Id,
                        Status = "",
                        StatusId = "",
                        SubStatus = "",
                        PropertySubStatusMstrsId = "",
                        StatusChangedOn = pus.CreatedAt,
                        Notes = pus.Notes,
                        roleId = pus.RoleId,
                        roleName = pus.Role.RoleName,
                    }).Union(from pus in _context.PropertyUserStatus
                                                join psm in _context.PropertyStatusMstr on pus.StatusId equals psm.Id
                                                join usr in _context.Users on pus.UserId equals usr.Id
                                                where pus.PropertyId == propertyId
                                                select new PropertyHistoryResponse
                                                {
                                                    FirstName = usr.FirstName,
                                                    LastName = usr.LastName,
                                                    Email = usr.Email,
                                                    Id = usr.Id,
                                                    Status = psm.Status,
                                                    StatusId = pus.StatusId,
                                                    SubStatus = pus.PropertySubStatusMstr.SubStatus,
                                                    PropertySubStatusMstrsId = pus.PropertySubStatusMstrsId,
                                                    StatusChangedOn = pus.StatusChangedOn,
                                                    Notes = pus.Notes,
                                                    roleId = pus.Role.Id,    //.FirstOrDefault(i => i.StatusId == pus.StatusId).RoleId,
                                                    roleName = pus.Role.RoleName,
                                                    //psm.RoleStatusMaps.FirstOrDefault(i => i.StatusId == pus.StatusId).Role.RoleName,

                                                }).OrderByDescending(i => i.StatusChangedOn).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                return InternalServerError(ex);
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT }, OperationType = OperationType.READ)]
        [HttpGet]
        [Route("api/IncidentCustom/DownloadMPRN")]
        public HttpResponseMessage DownloadMPRN(string incidentId)
        {
            try
            {
                StringBuilder sbResult = new StringBuilder();
                var result = from prop in _context.Properties.Where(p => p.IncidentId == incidentId
                             && p.Deleted == false)
                             select new
                             {
                                 prop,
                                 ZoneManager = _context.Users.Where(u => u.Id == prop.ZoneManagerId).Select(i => i.FirstName + " " + i.LastName).FirstOrDefault(),
                                 CellManager = _context.Users.Where(u => u.Id == prop.CellManagerId).Select(i => i.FirstName + " " + i.LastName).FirstOrDefault(),
                                 StatusList = (from psm in _context.PropertyStatusMstr
                                               join pus in _context.PropertyUserStatus.Where(ps => ps.PropertyId == prop.Id && !ps.Deleted)
                                               on psm.Id equals pus.StatusId into temppus
                                               from pusLeftJ in temppus.DefaultIfEmpty()// Find the latest Status
                                               where !psm.Deleted && (pusLeftJ.Id == null || pusLeftJ.Id == (from innerPus in _context.PropertyUserStatus
                                                                                        .Where(i => i.PropertyId == pusLeftJ.PropertyId && !i.Deleted
                                                                                          && i.StatusId == pusLeftJ.StatusId).OrderByDescending(i => i.StatusChangedOn
                                                                                          )
                                                                                                                 //group innerPus by innerPus.StatusId into grp
                                                                                                             select innerPus.Id).FirstOrDefault())
                                               select new
                                               {
                                                   pusLeftJ.PropertyId,
                                                   pusLeftJ.PropertyStatusMstr.Status,
                                                   pusLeftJ.PropertySubStatusMstr.SubStatus,
                                                   StatusChangedOn = (DateTimeOffset?)pusLeftJ.StatusChangedOn,
                                                   ChangedBy = _context.Users.Where(u => u.Id == pusLeftJ.UserId).Select(i => i.FirstName + " " + i.LastName).FirstOrDefault(),
                                                   psm.DisplayOrder
                                               }).OrderByDescending(i => i.DisplayOrder).ToList()
                             };
                //Get masterlist of statuses
                var statusList = _context.PropertyStatusMstr.Where(i => i.Deleted == false).OrderByDescending(i => i.DisplayOrder).Select(i => i.Status).ToList<string>();
                //write the header for the CSV
                WriteCSVHeader(sbResult, statusList);
                foreach (var item in result)
                {
                    WritetoCSV(item, sbResult);
                }
                var resultResp = GetHttpRespforDownloadMPRN(sbResult);
                return resultResp;
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }

        /// <summary>
        /// Returns the MPRNS of the current incidents if not found then returns the recent completed 
        /// Incident MPRNs
        /// </summary>
        /// <returns></returns>
        [Route("api/IncidentCustom/MPRNS")]
        public IHttpActionResult GetInprogressMPRNs()
        {
            try
            {
                //THis API is for public access no need to any authentication.
                //INprogress incident.
                Incident incident = _context.Incidents.FirstOrDefault(i => (i.Status == 0 || i.Status == null) && i.Deleted == false);
                if (incident == null)
                {   // recent completed incident
                    incident = _context.Incidents.OrderByDescending(i => i.EndTime).FirstOrDefault(i => (i.Status == 1 || i.Status == 2) && i.Deleted == false);
                }
                var resultProps = from prop in _context.Properties
                                  where prop.IncidentId == incident.Id && !prop.Deleted
                                  select new PropertyRespPub
                                  {
                                      BuildingName = prop.BuildingName,
                                      BuildingNumber = prop.BuildingNumber,
                                      Cell = prop.Cell,
                                      CellManagerId = prop.CellManagerId,
                                      Country = prop.Country,
                                      CreatedBy = prop.CreatedBy,
                                      DependentLocality = prop.DependentLocality,
                                      DependentStreet = prop.DependentStreet,
                                      LocalityName = prop.LocalityName,
                                      MCBuildingName = prop.MCBuildingName,
                                      MCSubBuildingName = prop.MCSubBuildingName,
                                      ModifiedBy = prop.ModifiedBy,
                                      MPRN = prop.MPRN,
                                      Postcode = prop.Postcode,
                                      PostTown = prop.PostTown,
                                      PrincipalStreet = prop.PrincipalStreet,
                                      PriorityCustomer = prop.PriorityCustomer,
                                      Status = prop.Status,
                                      SubBuildingName = prop.SubBuildingName,
                                      Zone = prop.Zone,
                                      ZoneManagerId = prop.ZoneManagerId,
                                      CreatedAt = prop.CreatedAt,
                                      Deleted = prop.Deleted,
                                      Id = prop.Id,
                                      UpdatedAt = prop.UpdatedAt,
                                      Latitude = prop.Latitude,
                                      Longitude = prop.Longitude,
                                  };
                IncidentRespPub incidentResult = new IncidentRespPub();
                incidentResult.IncidentId = incident.IncidentId;
                incidentResult.Id = incident.Id;
                incidentResult.NoOfPropsAffected = resultProps.ToList().Count();
                incidentResult.NoOfPropsCompleted = (_context.PropertyUserStatus.Where(i => i.Property.IncidentId == incident.Id
                                                          && i.Deleted == false
                                                          && i.PropertyStatusMstr.Status == "Restored"
                                                          && i.StatusId == (_context.PropertyUserStatus.
                                                          Where(pus => pus.Property.IncidentId == incident.Id && i.Deleted == false)
                                                                    .OrderByDescending(k => k.StatusChangedOn)
                                                                     .Where(k => k.PropertyId == i.PropertyId
                                                                     ).Select(k => k.StatusId).FirstOrDefault()) // if multiple status exists for the property then lake the latest count by oredering.
                                                         )
                                                        .Select(i => new
                                                        {
                                                            i.PropertyId,
                                                            i.StatusId
                                                        }).Distinct().Count());
                incidentResult.StartTime = incident.StartTime;
                incidentResult.EndTime = incident.EndTime;
                incidentResult.Status = incident.Status;
                incidentResult.MPRNS = resultProps.ToList();
                incidentResult.Notes = incident.Notes;
                incidentResult.ClosingNotes = incident.ClosingNotes;
                incidentResult.CategoryName = incident.CategoriesMstr.Category;
                incidentResult.CreatedBy = incident.CreatedBy;
                return Ok(incidentResult);
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }
        #region PrivateMethods

        /// <summary>
        /// This method is to get List of MPRNs
        /// </summary>
        /// <param name="request"></param>
        /// <param name="incidentId"></param>
        /// <param name="dbCellManagers"></param>
        /// <param name="dbzoneManagers"></param>
        /// <returns></returns>
        private async Task<List<Property>> GetMappedMPRNs(List<PropertyRequest> MPRNs, string incidentId, List<User> dbCellManagers, List<User> dbzoneManagers, List<ResolveUser> resolvUsers)
        {
            string currentUserEmail = HttpUtilities.GetUserNameFromToken(this.Request);
            //Need to remove from Loop
            List<Property> dbProperties = new List<Property>();
            Mapper.Initialize(cfg => cfg.CreateMap<PropertyRequest, Property>().ForMember(i => i.CreatedBy,
            j => j.UseValue(currentUserEmail)).ForMember(i => i.IncidentId, j => j.UseValue(incidentId)));

            foreach (var itemMPRN in MPRNs)
            {
                var PropertyDb = Mapper.Map<PropertyRequest, Property>(itemMPRN);
                AssignZoneAndCellMgrs(itemMPRN, dbCellManagers, dbzoneManagers, PropertyDb, resolvUsers);
                // Get Lat and Lang from BingMaps
                //string query = BingMapHelper.GetQueryforMPRN(itemMPRN);
                //string latLang = await BingMapHelper.GetLatLang(query);
                //if (!string.IsNullOrEmpty(latLang) && latLang.Split('|').Length > 1)
                //{
                //    PropertyDb.Latitude = latLang.Split('|')[0];
                //    PropertyDb.Longitude = latLang.Split('|')[1];
                //}
                PropertyDb.Id = Guid.NewGuid().ToString();
                dbProperties.Add(PropertyDb);
            }
            return dbProperties;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<List<Property>> GetMappedMPRNsforEdit(IncidentEditRequest request, string incidentId, List<User> dbCellManagers, List<User> dbzoneManagers, List<Property> existingMPRNs, List<ResolveUser> resolvUsers)
        {
            List<Property> result = new List<Property>();
            var newMPRNsRequests = request.MPRNs.Where(item => !existingMPRNs.Exists(k => k.MPRN == item.MPRN)).ToList();
            if (newMPRNsRequests != null && newMPRNsRequests.Count > 0)
            {
                List<Property> newMPRNs = await GetMappedMPRNs(newMPRNsRequests, incidentId, dbCellManagers, dbzoneManagers, request.ResolveUsers);
                //add new MPRNs to the result
                _context.Properties.AddRange(newMPRNs);
            }
            //Modify Existing MPRNS...
            foreach (var itemMPRN in request.MPRNs)
            {
                var dbMPRN = existingMPRNs.FirstOrDefault(item => item.MPRN == itemMPRN.MPRN);
                if (dbMPRN != null)
                {
                    // Update the new properties
                    bool res = await UpdateMPRNProperties(itemMPRN, dbMPRN, dbCellManagers, dbzoneManagers, request.ResolveUsers);
                    dbMPRN.IncidentId = request.Id;
                    _context.Entry(dbMPRN).State = System.Data.Entity.EntityState.Modified;
                    result.Add(dbMPRN);
                }
            }

            //Abhijeet
            // 24-10-2018 - New Requirement MPRN do not delete.
            // Delete the existing MPRNS which does not exists in the input request
            //Make it soft delete.
            //var allDBMPRNs = _context.Properties.Where(item => item.IncidentId == incidentId);
            //if (allDBMPRNs != null && request.MPRNs != null)
            //{
            //    foreach (var itemExistingMPRN in allDBMPRNs)
            //    {
            //        var reqMPRN = request.MPRNs.FirstOrDefault(item => item.MPRN == itemExistingMPRN.MPRN);
            //        if (reqMPRN == null)
            //        {
            //            // update the mprn to deleted.
            //            itemExistingMPRN.Deleted = true;
            //            _context.Entry(itemExistingMPRN).State = System.Data.Entity.EntityState.Modified;
            //            //Remove the existing Mapping and status...
            //            UnassignMPRNSWhenPropertyDeleted(itemExistingMPRN);
            //        }
            //    }
            //}
            return result;
        }

        /// <summary>
        /// Gets and assigns the Zone manager and cell mansger ids.
        /// </summary>
        /// <param name="itemMPRN"></param>
        /// <param name="dbCellManagers"></param>
        /// <param name="dbzoneManagers"></param>
        /// <param name="PropertyDb"></param>
        private void AssignZoneAndCellMgrs(PropertyRequest itemMPRN, List<User> dbCellManagers, List<User> dbzoneManagers, Property PropertyDb, List<ResolveUser> resolvUsers)
        {
            if (!string.IsNullOrEmpty(itemMPRN.CellManagerName))
            {
                var dbCellManager = dbCellManagers.Where(item => (item.FirstName + " " + item.LastName).ToLower() == itemMPRN.CellManagerName.ToLower());
                if (dbCellManager != null)
                {
                    if (dbCellManager.Count() > 1)
                    {
                        string userEmail = resolvUsers.FirstOrDefault(item => item.Name == itemMPRN.CellManagerName).Email;
                        // Duplicate users found .Resolve the users.

                        PropertyDb.CellManagerId = dbCellManager.FirstOrDefault(item => item.Email == userEmail).Id;
                    }
                    else
                    {
                        PropertyDb.CellManagerId = dbCellManager.FirstOrDefault().Id;
                    }
                }
            }
            else
            {
                PropertyDb.CellManagerId = null;
            }
            if (!string.IsNullOrEmpty(itemMPRN.ZoneManagerName))
            {
                var dbZoneMgr = dbzoneManagers.Where(item => (item.FirstName + " " + item.LastName).ToLower() == itemMPRN.ZoneManagerName.ToLower());
                if (dbZoneMgr != null)
                {
                    if (dbZoneMgr.Count() > 1)
                    {
                        string userEmail = resolvUsers.FirstOrDefault(item => item.Name == itemMPRN.ZoneManagerName).Email;
                        // Duplicate users found .Resolve the users.
                        PropertyDb.ZoneManagerId = dbZoneMgr.FirstOrDefault(item => item.Email == userEmail).Id;
                    }
                    else
                    {
                        PropertyDb.ZoneManagerId = dbZoneMgr.FirstOrDefault().Id;
                    }
                }
            }
            else
            {
                PropertyDb.ZoneManagerId = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private async Task<bool> UpdateMPRNProperties(PropertyRequest request, Property dbProperty, List<User> dbCellManagers, List<User> dbzoneManagers, List<ResolveUser> resolvedUsers)
        {
            string currentUserEmail = HttpUtilities.GetUserNameFromToken(this.Request);
            Mapper.Initialize(cfg => cfg.CreateMap<PropertyRequest, Property>().ForMember(i => i.ModifiedBy,
            j => j.UseValue(currentUserEmail)).ForMember(i => i.UpdatedAt, i => i.UseValue(DateTimeOffset.UtcNow)));
            var PropertyDb = Mapper.Map<PropertyRequest, Property>(request, dbProperty);
            // Get Lat and Lang from BingMaps
            //string query = BingMapHelper.GetQueryforMPRN(request);
            //string latLang = await BingMapHelper.GetLatLang(query);
            //if (!string.IsNullOrEmpty(latLang) && latLang.Split('|').Length > 1)
            //{
            //    dbProperty.Latitude = latLang.Split('|')[0];
            //    dbProperty.Longitude = latLang.Split('|')[1];
            //}
            //if ((dbCellManagers != null && dbCellManagers.Count > 0) || (dbzoneManagers != null && dbzoneManagers.Count > 0))
            //{
            AssignZoneAndCellMgrs(request, dbCellManagers, dbzoneManagers, dbProperty, resolvedUsers);
            //}
            return true;
        }

        /// <summary>
        /// Un assign all the MPRNs when incident is cancelled or completed.
        /// </summary>
        /// <param name="incidentId"></param>
        private void UnassignMPRNsForCancelledOrCompletedIncidents(string incidentId)
        {
            //Abhijeet 06-11-2018
            //Remove PropertyUserMap in Query

            //List<PropertyUserMap> dbItemsToRemove = (from pum in _context.PropertyUserMap.Where(i => i.Deleted == false).ToList()
            //                                         where pum.Property.IncidentId == incidentId
            //                                         select pum).ToList<PropertyUserMap>();

            List<PropertyUserStatus> dbItemsToRemove = (from pum in _context.PropertyUserStatus.Where(i => i.Deleted == false && i.StatusId == null && i.IncidentId==incidentId).ToList()
                                                      
                                                        select pum).ToList<PropertyUserStatus>();
            if (dbItemsToRemove != null && dbItemsToRemove.Count > 0)
            {
                foreach (var item in dbItemsToRemove)
                {
                    item.Deleted = true;
                    // item.UpdatedAt = DateTime.UtcNow;
                    _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }
            }


        }

        //Update Property Count by Incident in Edit
        private void UpdateNoOfAffectedPropertiesCount(string incidentId)
        {
            var propCount = _context.Properties.Where(i => i.Deleted == false && i.IncidentId == incidentId).Count();
            var incidentdata = _context.Incidents.FirstOrDefault(i => i.Id == incidentId);
            if (incidentdata != null)
            {
                incidentdata.NoOfPropsAffected = propCount;

            }
            _context.Entry(incidentdata).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
        //un assign mapping and status when property deleted.
        private void UnassignMPRNSWhenPropertyDeleted(Property itemExistingMPRN)
        {
            // un assign mappings
            foreach (var item in itemExistingMPRN.PropertyUserMaps)
            {
                item.Deleted = true;
                _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }
            // un assign status
            foreach (var item in itemExistingMPRN.PropertyUserStatus)
            {
                item.Deleted = true;
                _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }
        }
        /// <summary>
        /// Constructs and writes the CSV header
        /// </summary>
        private void WriteCSVHeader(StringBuilder sb, List<string> statusLists)
        {
            CSVHelper.AppendValue(sb, "MPRN", true, false);
            CSVHelper.AppendValue(sb, "BuildingName", false, false);
            CSVHelper.AppendValue(sb, "SubBuildingName", false, false);
            CSVHelper.AppendValue(sb, "MCBuildingName", false, false);
            CSVHelper.AppendValue(sb, "MCSubBuildingName", false, false);
            CSVHelper.AppendValue(sb, "BuildingNumber", false, false);
            CSVHelper.AppendValue(sb, "PrincipalStreet", false, false);
            CSVHelper.AppendValue(sb, "DependentStreet", false, false);
            CSVHelper.AppendValue(sb, "DependentLocality", false, false);
            CSVHelper.AppendValue(sb, "LocalityName", false, false);
            CSVHelper.AppendValue(sb, "PostTown", false, false);
            CSVHelper.AppendValue(sb, "Postcode", false, false);
            CSVHelper.AppendValue(sb, "PriorityCustomer", false, false);
            CSVHelper.AppendValue(sb, "Zone", false, false);
            CSVHelper.AppendValue(sb, "ZoneController", false, false);
            CSVHelper.AppendValue(sb, "Cell", false, false);
            CSVHelper.AppendValue(sb, "CellManager", false, false);
            foreach (var item in statusLists)
            {
                CSVHelper.AppendValue(sb, item, false, false);
                CSVHelper.AppendValue(sb, item + "TimeStamp", false, false);
                if (statusLists.IndexOf(item) == statusLists.Count - 1)
                {
                    //pass true for last lines
                    CSVHelper.AppendValue(sb, item + "By", false, true);
                }
                else
                {
                    CSVHelper.AppendValue(sb, item + "By", false, false);
                }
            }


        }
        /// <summary>
        /// Send Property item for csv
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="item"></param>
        public static void WritetoCSV(dynamic inputItem, StringBuilder sb)
        {
            CSVHelper.AppendValue(sb, inputItem.prop.MPRN, true, false);
            CSVHelper.AppendValue(sb, inputItem.prop.BuildingName, false, false);
            CSVHelper.AppendValue(sb, inputItem.prop.SubBuildingName, false, false);
            CSVHelper.AppendValue(sb, inputItem.prop.MCBuildingName, false, false);
            CSVHelper.AppendValue(sb, inputItem.prop.MCSubBuildingName, false, false);
            CSVHelper.AppendValue(sb, inputItem.prop.BuildingNumber, false, false);
            CSVHelper.AppendValue(sb, inputItem.prop.PrincipalStreet, false, false);
            CSVHelper.AppendValue(sb, inputItem.prop.DependentStreet, false, false);
            CSVHelper.AppendValue(sb, inputItem.prop.DependentLocality, false, false);
            CSVHelper.AppendValue(sb, inputItem.prop.LocalityName, false, false);
            CSVHelper.AppendValue(sb, inputItem.prop.PostTown, false, false);
            CSVHelper.AppendValue(sb, inputItem.prop.Postcode, false, false);
            CSVHelper.AppendValue(sb, Convert.ToString(inputItem.prop.PriorityCustomer), false, false);
            CSVHelper.AppendValue(sb, inputItem.prop.Zone, false, false);
            CSVHelper.AppendValue(sb, inputItem.ZoneManager, false, false);
            CSVHelper.AppendValue(sb, inputItem.prop.Cell, false, false);
            CSVHelper.AppendValue(sb, inputItem.CellManager, false, false);
            foreach (var itemStatus in inputItem.StatusList)
            {
                if (string.IsNullOrEmpty(itemStatus.Status))
                {
                    CSVHelper.AppendValue(sb, " ", false, false);
                }
                else if (!string.IsNullOrEmpty(itemStatus.SubStatus))
                {
                    CSVHelper.AppendValue(sb, itemStatus.Status + " | " + itemStatus.SubStatus, false, false);
                }
                else
                {
                    CSVHelper.AppendValue(sb, itemStatus.Status, false, false);
                }
                CSVHelper.AppendValue(sb, itemStatus.StatusChangedOn != null ? Convert.ToString(itemStatus.StatusChangedOn.ToString("dd/MM/yyyy hh:mm:ss tt")) : string.Empty, false, false);
                if (inputItem.StatusList.IndexOf(itemStatus) == inputItem.StatusList.Count - 1)
                {
                    //pass true for last lines
                    CSVHelper.AppendValue(sb, itemStatus.ChangedBy, false, true);
                }
                else
                {
                    CSVHelper.AppendValue(sb, itemStatus.ChangedBy, false, false);
                }
            }
        }

        /// <summary>
        /// Send Property item for csv
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="item"></param>
        public static void WriteItem(StringBuilder sb, Property item)
        {
            CSVHelper.AppendValue(sb, item.Id.ToString(), true, false);
            CSVHelper.AppendValue(sb, item.MPRN, false, false);
            //CSVHelper.writeProperty(sb, item.CreatedAt.ToString(), false, false);
            CSVHelper.AppendValue(sb, item.Postcode, false, false);
            CSVHelper.AppendValue(sb, item.PostTown, false, false);
            CSVHelper.AppendValue(sb, item.BuildingName, false, false);
            CSVHelper.AppendValue(sb, item.MCBuildingName, false, false);
            CSVHelper.AppendValue(sb, item.SubBuildingName, false, true);
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
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "INCMPRNs.csv" };
            return response;
        }

        /// <summary>
        ///  Sends New MPRNs for Lat Lang Resolution.
        /// </summary>
        private void SendNewMPRNStoQueue(IncidentRequest request)
        {
            try
            {
                if (request.MPRNs != null && request.MPRNs.Count > 0)
                {
                    for (int i = 0; i < request.MPRNs.Count; i += 50)
                    {
                        List<PropertyRequest> MPRNstoSend = new List<PropertyRequest>();
                        MPRNstoSend = request.MPRNs.Skip(i).Take(50).ToList();
                        IncidentRequest incReqToSend = new IncidentRequest();
                        incReqToSend.Id = request.Id;
                        incReqToSend.MPRNs = MPRNstoSend;
                        string message = JsonConvert.SerializeObject(incReqToSend);
                        AzureStorageHelper.SendMessageToQueue(message);
                        LGSELogger.Information("MPRNS sent to the queue {0}", MPRNstoSend.Count);
                        LGSELogger.Information("MPRNS Seriealized {0}", message);
                    }
                }
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
            }
        }
        /// <summary>
        ///  Sends Modified MPRNs for Lat Lang Resolution.
        /// </summary>
        private void SendUpdatedMPRNStoQueue(IncidentEditRequest request)
        {
            try
            {
                if (request.MPRNs != null && request.MPRNs.Count > 0)
                {
                    for (int i = 0; i < request.MPRNs.Count; i += 50)
                    {
                        List<PropertyRequest> MPRNstoSend = new List<PropertyRequest>();
                        MPRNstoSend = request.MPRNs.Skip(i).Take(50).ToList();
                        IncidentRequest incReqToSend = new IncidentRequest();
                        incReqToSend.Id = request.Id;
                        incReqToSend.MPRNs = MPRNstoSend;
                        string message = JsonConvert.SerializeObject(incReqToSend);
                        AzureStorageHelper.SendMessageToQueue(message);
                        LGSELogger.Information("Updated MPRNS sent to the queue {0}", MPRNstoSend.Count);
                        LGSELogger.Information("Updated MPRNS Seriealized {0}", message);
                    }
                }
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
            }
        }
        #endregion
    }
}
