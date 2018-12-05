using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;
using LGSE_APIService.ResponseObjects;
using AutoMapper;
using System;
using LGSE_APIService.Utilities;
using System.Collections.Generic;
using LGSE_APIService.Common;
using LGSE_APIService.Authorization;
using LGSE_APIService.Common.Utilities;
using LGSE_APIService.RequestObjects;

namespace LGSE_APIService.Controllers
{
    public class PropertyController : TableController<Property>
    {
        LGSE_APIContext context = null;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = LGSE_APIContext.GetIntance();
            DbUtilities.dbContext = context;
            DomainManager = new EntityDomainManager<Property>(context, Request, enableSoftDelete: true);
        }

        [Authorize] 
        [CustomAuthorize(Module= new Features[]{Features.INCIDENTMGT,Features.ASSIGNEDMPRN,Features.RESOURCEMGT}, OperationType = OperationType.READ)]
       // [CustomAuthorize(Module = Features.ASSIGNEDMPRN, OperationType = OperationType.READ)]
        // GET tables/Property
        [EnableQuery]
        //public IQueryable<PropertyResponse> GetAllProperty()
        //{
        //    try
        //    {
        //        string userEmail = HttpUtilities.GetUserNameFromToken(this.Request);
        //        int assignedFlag = HttpUtilities.GetUserAccessApi(this.Request);
        //        //List<string> roles=HttpUtilities.GetRolesFromToken(this.Request);
        //        //   Role preferredRole = DbUtilities.GetUserPreferredRole(userEmail); //Abhijeet 24-10-2018
        //        string userRole = HttpUtilities.GetUserRoleAccessApi(this.Request);// Added 24-10-2018
        //       var preferredRole = context.Roles.FirstOrDefault(i => i.Id == userRole);// Added 24-10-2018
        //        Random ran = new Random();

        //        //Abhijeet- Commented below Role wise code & created new method in "api/IncidentCustom/Properties" - 16-10-2018

        //        //Need to optimise the query
        //        if (assignedFlag == 0 && preferredRole != null && (preferredRole.RoleName.Contains("Engineer") || preferredRole.RoleName.Contains("Isolator") || preferredRole.RoleName.Contains("engineer") || preferredRole.RoleName.Contains("isolator")))
        //        {
        //            //if user logged in as Engineer and if user updated the property status 
        //            //then Dont return that Property as assigned.
        //            //Need to optimize the query
        //            //this sends only inprogress Incidents details.
        //            // Role preferredRole = DbUtilities.GetUserPreferredRole(userEmail);

        //            var propList = (from user in context.Users
        //                            join pumap in context.PropertyUserMap.Select(i => new { i.UserId, i.PropertyId, i.Deleted, i.CreatedAt, i.Id, i.RoleId }).Distinct()//, i.RoleId
        //                                 on user.Id equals pumap.UserId
        //                            join prop in context.Properties.Where(i => i.Incident.Status == 0 || i.Incident.Status == null) on pumap.PropertyId equals prop.Id
        //                            where user.Email == userEmail && pumap.RoleId ==  preferredRole.Id //Abhijeet Added RoleId filter
        //                            select new PropertyResponse
        //                            {
        //                                BuildingName = prop.BuildingName,
        //                                BuildingNumber = prop.BuildingNumber,
        //                                Cell = prop.Cell,
        //                                CellManagerId = prop.CellManagerId,
        //                                Country = prop.Country,
        //                                CreatedBy = prop.CreatedBy,
        //                                DependentLocality = prop.DependentLocality,
        //                                DependentStreet = prop.DependentStreet,
        //                                IncidentId = prop.IncidentId,
        //                                IncidentName = prop.Incident.IncidentId,
        //                                LocalityName = prop.LocalityName,
        //                                MCBuildingName = prop.MCBuildingName,
        //                                MCSubBuildingName = prop.MCSubBuildingName,
        //                                ModifiedBy = prop.ModifiedBy,
        //                                MPRN = prop.MPRN,
        //                                Postcode = prop.Postcode,
        //                                PostTown = prop.PostTown,
        //                                PrincipalStreet = prop.PrincipalStreet,
        //                                PriorityCustomer = prop.PriorityCustomer,
        //                                Status = prop.Incident.Status,
        //                                SubBuildingName = prop.SubBuildingName,
        //                                Zone = prop.Zone,
        //                                ZoneManagerId = prop.ZoneManagerId,
        //                                isMPRNAssigned = context.PropertyUserMap.Any(i => i.PropertyId == prop.Id && !i.Deleted),
        //                                AssignedResourceCount = context.PropertyUserMap.Count(i => i.PropertyId == prop.Id && !i.Deleted),
        //                                // Return the Latest status for the Enginerr or isolator if the Currest MPRN is inprogress(meansassigned but status not updated
        //                                // Return the his own status for the Engineerr or isolator if the current MPRN is completed by that user.
        //                                LatestStatus = (prop.PropertyUserStatus.Count(i => i.RoleId == preferredRole.Id && i.UserId==user.Id
        //                                                   && i.Deleted == false && i.PropertyUserMapsId == pumap.Id) > 0 //updated
        //                                                 ) ? prop.PropertyUserStatus.OrderByDescending(i => i.StatusChangedOn).FirstOrDefault(i => i.UserId == user.Id && i.RoleId== preferredRole.Id).PropertyStatusMstr.Status
        //                                                : prop.PropertyUserStatus.OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().PropertyStatusMstr.Status,

        //                                LatestSubStatus = (prop.PropertyUserStatus.Count(i => i.RoleId == preferredRole.Id
        //                                                    && i.Deleted == false && i.PropertyUserMapsId == pumap.Id) > 0
        //                                                   ) ? prop.PropertyUserStatus.OrderByDescending(i => i.StatusChangedOn).FirstOrDefault(i => i.UserId == user.Id && i.RoleId == preferredRole.Id).PropertySubStatusMstr.SubStatus
        //                                                  : prop.PropertyUserStatus.OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().PropertySubStatusMstr.SubStatus,

        //                                Notes = (prop.PropertyUserStatus.Count(i => i.RoleId == preferredRole.Id
        //                                                    && i.Deleted == false && i.PropertyUserMapsId == pumap.Id) > 0 //updated
        //                                                 ) ? prop.PropertyUserStatus.OrderByDescending(i => i.StatusChangedOn).FirstOrDefault(i => i.UserId == user.Id).Notes
        //                                                : prop.PropertyUserStatus.OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().Notes,
        //                                // Update flag as true if status is updated as logged in role already
        //                                // and status does not exists after the MPRN assignments.
        //                                IsStatusUpdated = prop.PropertyUserStatus
        //                                                    .Count(i => i.RoleId == preferredRole.Id
        //                                                    && i.Deleted == false
        //                                                    && i.PropertyUserMapsId == pumap.Id) > 0,//status updated true..

        //                                StatusChangedOn = (prop.PropertyUserStatus.Count(i => i.RoleId == preferredRole.Id
        //                                                    && i.Deleted == false && i.PropertyUserMapsId == pumap.Id) > 0 //updated
        //                                                 ) ? prop.PropertyUserStatus.OrderByDescending(i => i.StatusChangedOn).FirstOrDefault(i => i.UserId == user.Id).StatusChangedOn
        //                                                : prop.PropertyUserStatus.OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().StatusChangedOn,

        //                                //prop.PropertyUserStatus
        //                                //                    .FirstOrDefault(i => i.RoleId == preferredRole.Id
        //                                //                    && i.Deleted == false
        //                                //                    && i.PropertyUserMapsId == pumap.Id).StatusChangedOn,
        //                                CreatedAt = prop.CreatedAt,
        //                                Deleted = prop.Deleted,
        //                                Id = pumap.Id,//As suggested by Abhijith,Sending the PropertyUsermapping id instead of propertyID
        //                                PropertyId = prop.Id,
        //                                UpdatedAt = (prop.UpdatedAt >= prop.PropertyUserMaps.OrderByDescending(i => i.UpdatedAt).FirstOrDefault().UpdatedAt ? prop.UpdatedAt : prop.PropertyUserMaps.OrderByDescending(i => i.UpdatedAt).FirstOrDefault().UpdatedAt
        //                                               ),
        //                                //( prop.PropertyUserStatus // Abhijeet UpdatedAt null -12-10-2018
        //                                //                    .FirstOrDefault(i => i.RoleId == preferredRole.Id
        //                                //                    && i.Deleted == false
        //                                //                    && i.PropertyUserMapsId == pumap.Id).StatusChangedOn== null? prop.CreatedAt: prop.PropertyUserStatus // Abhijeet UpdatedAt null
        //                                //                    .FirstOrDefault(i => i.RoleId == preferredRole.Id
        //                                //                    && i.Deleted == false
        //                                //                    && i.PropertyUserMapsId == pumap.Id).StatusChangedOn) ,//prop.UpdatedAt,
        //                                IsUnassigned = pumap.Deleted,
        //                                Latitude = prop.Latitude,
        //                                Longitude = prop.Longitude,
        //                                IsIsolated = prop.IsIsolated,
        //                                NotesCount = prop.PropertyUserStatus.Count(i => i.Deleted == false && !string.IsNullOrEmpty(i.Notes)),
        //                                IsLastStatusUpdate = (pumap.Id == (prop.PropertyUserStatus.Where(i => i.RoleId == preferredRole.Id && i.UserId ==user.Id
        //                                                        && i.Deleted == false).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().PropertyUserMapsId)) ? true : false
        //                            });
        //            return propList.AsQueryable().OrderBy(i => i.StatusChangedOn);
        //        }
        //        else
        //        {
        //            if (assignedFlag == 1)
        //            {
        //                // for admin and other users
        //                var propList = from prop in context.Properties.Where(i => !i.Deleted)
        //                               select new PropertyResponse
        //                               {
        //                                   BuildingName = prop.BuildingName,
        //                                   BuildingNumber = prop.BuildingNumber,
        //                                   Cell = prop.Cell,
        //                                   CellManagerId = prop.CellManagerId,
        //                                   Country = (string.IsNullOrEmpty(prop.Country) ? "" : prop.Country), //Abhijeet-08-10-2018: this field does not show null value.
        //                                   CreatedBy = prop.CreatedBy,
        //                                   DependentLocality = prop.DependentLocality,
        //                                   DependentStreet = prop.DependentStreet,
        //                                   IncidentId = prop.IncidentId,
        //                                   IncidentName = prop.Incident.IncidentId,
        //                                   LocalityName = prop.LocalityName,
        //                                   MCBuildingName = prop.MCBuildingName,
        //                                   MCSubBuildingName = prop.MCSubBuildingName,
        //                                   ModifiedBy = prop.ModifiedBy,
        //                                   MPRN = prop.MPRN,
        //                                   Postcode = prop.Postcode,
        //                                   PostTown = prop.PostTown,
        //                                   PrincipalStreet = prop.PrincipalStreet,
        //                                   PriorityCustomer = prop.PriorityCustomer,
        //                                   Status = prop.Incident.Status,
        //                                   SubBuildingName = prop.SubBuildingName,
        //                                   Zone = prop.Zone,
        //                                   ZoneManagerId = prop.ZoneManagerId,
        //                                   //we can remove if UI can use AssignedResourcecount.
        //                                   //isMPRNAssigned = (from pum in prop.PropertyUserMaps.Where(i => i.Deleted == false)
        //                                   //                  join pus in prop.PropertyUserStatus.Where(i => i.Deleted == false)
        //                                   //                  on pum.Id equals pus.PropertyUserMapsId into temp
        //                                   //                  from lj in temp.DefaultIfEmpty()
        //                                   //                  where lj == null || lj.StatusId == null
        //                                   //                  select new { pum.PropertyId, pum.UserId, pum.RoleId } //Abhijeet added  pum.RoleId 25-10-2018
        //                                   //        ).Distinct().Count() > 0 ? true : false,
        //                                   // Consider only inprogress MPRNs dont consider completed count
        //                                   AssignedResourceCount = (from pum in prop.PropertyUserMaps.Where(i => i.Deleted == false)
        //                                                            join pus in prop.PropertyUserStatus.Where(i => i.Deleted == false)
        //                                                            on pum.Id equals pus.PropertyUserMapsId into temp
        //                                                            from lj in temp.DefaultIfEmpty()
        //                                                            where lj == null || lj.StatusId == null
        //                                                            select new { pum.PropertyId, pum.UserId, pum.RoleId }//Abhijeet added  pum.RoleId 25-10-2018
        //                                           ).Distinct().Count(),
        //                                   LatestStatus = prop.PropertyStatusMstr.Status, //prop.PropertyUserStatus.OrderByDescending(i => i.CreatedAt).Take(1).FirstOrDefault().PropertyStatusMstr.Status,
        //                                   LatestStatusId = prop.StatusId,// prop.PropertyUserStatus.OrderByDescending(i => i.CreatedAt).Take(1).FirstOrDefault().PropertyStatusMstr.Id,
        //                                   LatestSubStatus = prop.PropertySubStatusMstr.SubStatus, // prop.PropertyUserStatus.OrderByDescending(i => i.CreatedAt).Take(1).FirstOrDefault().PropertySubStatusMstr.SubStatus,
        //                                   LatestSubStatusId = prop.SubStatusId, //prop.PropertyUserStatus.OrderByDescending(i => i.CreatedAt).Take(1).FirstOrDefault().PropertySubStatusMstr.Id,
        //                                  // Notes = prop.PropertyUserStatus.OrderByDescending(i => i.CreatedAt).Take(1).FirstOrDefault().Notes,
        //                                   CreatedAt = prop.CreatedAt,
        //                                   Deleted = prop.Deleted,
        //                                   Id = prop.Id,
        //                                   UpdatedAt = prop.UpdatedAt,
        //                                   // Latitude= "18."+ran.Next(522100, 522193).ToString(),
        //                                   // Longitude = "-0." + ran.Next(115000, 115050).ToString()
        //                                   //IsUnassigned = (from pum in prop.PropertyUserMaps.Where(i => i.Deleted == false)
        //                                   //                join pus in prop.PropertyUserStatus.Where(i => i.Deleted == false)
        //                                   //                on pum.Id equals pus.PropertyUserMapsId into temp
        //                                   //                from lj in temp.DefaultIfEmpty()
        //                                   //                where lj == null || lj.StatusId == null
        //                                   //                select new { pum.PropertyId, pum.UserId, pum.RoleId }//Abhijeet added  pum.RoleId 25-10-2018
        //                                   //        ).Distinct().Count() > 0 ? false : true,
        //                                   Latitude = prop.Latitude,
        //                                   Longitude = prop.Longitude,
        //                                   IsIsolated = prop.IsIsolated,
        //                                   NotesCount = prop.PropertyUserStatus.Count(i => i.Deleted == false && !string.IsNullOrEmpty(i.Notes))
        //                               };

        //                return propList.AsQueryable();
        //            }
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HttpUtilities.ServerError(ex, Request);
        //        return null;
        //    }
        //}

        public IQueryable<PropertyResponse> GetAllProperty()
        {
            try
            {
                string userEmail = HttpUtilities.GetUserNameFromToken(this.Request);
                int assignedFlag = HttpUtilities.GetUserAccessApi(this.Request);
                //List<string> roles=HttpUtilities.GetRolesFromToken(this.Request);
                //   Role preferredRole = DbUtilities.GetUserPreferredRole(userEmail); //Abhijeet 24-10-2018
                string userRole = HttpUtilities.GetUserRoleAccessApi(this.Request);// Added 24-10-2018
                var preferredRole = context.Roles.FirstOrDefault(i => i.Id == userRole);// Added 24-10-2018
                Random ran = new Random();

                //Abhijeet- Commented below Role wise code & created new method in "api/IncidentCustom/Properties" - 16-10-2018

                //Need to optimise the query
                if (assignedFlag == 0 && preferredRole != null && (preferredRole.RoleName.Contains("Engineer") || preferredRole.RoleName.Contains("Isolator") || preferredRole.RoleName.Contains("engineer") || preferredRole.RoleName.Contains("isolator")))
                {
                    //if user logged in as Engineer and if user updated the property status 
                    //then Dont return that Property as assigned.
                    //Need to optimize the query
                    //this sends only inprogress Incidents details.
                    // Role preferredRole = DbUtilities.GetUserPreferredRole(userEmail);

                    var propList = (from user in context.Users
                                    join pumap in context.PropertyUserStatus.Select(i => new { i.UserId, i.PropertyId, i.Deleted, i.CreatedAt, i.Id, i.RoleId,i.StatusId, i.PropertySubStatusMstrsId,i.Notes,i.StatusChangedOn, i.UpdatedAt }).Distinct()//, i.RoleId
                                         on user.Id equals pumap.UserId
                                    join prop in context.Properties.Where(i => i.Incident.Status == 0 || i.Incident.Status == null) on pumap.PropertyId equals prop.Id
                                    where user.Email == userEmail && pumap.RoleId == preferredRole.Id //Abhijeet Added RoleId filter
                                    select new PropertyResponse
                                    {
                                        BuildingName = prop.BuildingName,
                                        BuildingNumber = prop.BuildingNumber,
                                        Cell = prop.Cell,
                                        CellManagerId = prop.CellManagerId,
                                        Country = prop.Country,
                                        CreatedBy = prop.CreatedBy,
                                        DependentLocality = prop.DependentLocality,
                                        DependentStreet = prop.DependentStreet,
                                        IncidentId = prop.IncidentId,
                                        IncidentName = prop.Incident.IncidentId,
                                        LocalityName = prop.LocalityName,
                                        MCBuildingName = prop.MCBuildingName,
                                        MCSubBuildingName = prop.MCSubBuildingName,
                                        ModifiedBy = prop.ModifiedBy,
                                        MPRN = prop.MPRN,
                                        Postcode = prop.Postcode,
                                        PostTown = prop.PostTown,
                                        PrincipalStreet = prop.PrincipalStreet,
                                        PriorityCustomer = prop.PriorityCustomer,
                                        Status = prop.Incident.Status,
                                        SubBuildingName = prop.SubBuildingName,
                                        Zone = prop.Zone,
                                        ZoneManagerId = prop.ZoneManagerId,
                                        isMPRNAssigned = context.PropertyUserStatus.Any(i => i.PropertyId == prop.Id && !i.Deleted),
                                        AssignedResourceCount = context.PropertyUserStatus.Count(i => i.PropertyId == prop.Id && !i.Deleted),
                                        // Return the Latest status for the Enginerr or isolator if the Currest MPRN is inprogress(meansassigned but status not updated
                                        // Return the his own status for the Engineerr or isolator if the current MPRN is completed by that user.
                                        LatestStatus =
                                            (pumap.StatusId==null 
                                                          ? prop.PropertyStatusMstr.Status
                                                        : context.PropertyStatusMstr.Where(i=>i.Id==pumap.StatusId).FirstOrDefault().Status),

                                        LatestSubStatus = (pumap.StatusId == null && pumap.PropertySubStatusMstrsId==null //updated
                                            ) ? prop.PropertySubStatusMstr.SubStatus
                                                : context.PropertySubStatusMstrs.Where(i => i.Id == pumap.PropertySubStatusMstrsId).FirstOrDefault().SubStatus,

                                        //(prop.PropertyUserStatus.Count(i => i.RoleId == preferredRole.Id
                                        //                && i.Deleted == false ) > 1
                                        //               ) ? prop.PropertyUserStatus.OrderByDescending(i => i.StatusChangedOn).FirstOrDefault(i => i.UserId == user.Id && i.RoleId == preferredRole.Id).PropertySubStatusMstr.SubStatus
                                        //              : prop.PropertyUserStatus.OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().PropertySubStatusMstr.SubStatus,

                                        Notes = (pumap.StatusId == null //updated
                                                         ) ?  prop.PropertyUserStatus.OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().Notes
                                        : pumap.Notes,
                                        // Update flag as true if status is updated as logged in role already
                                        // and status does not exists after the MPRN assignments.
                                        IsStatusUpdated = pumap.StatusId!=null? true:false,//status updated true..

                                        StatusChangedOn = (pumap.StatusId == null //updated
                                                         ) ?  prop.PropertyUserStatus.OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().StatusChangedOn 
                                        : pumap.StatusChangedOn,

                                        //prop.PropertyUserStatus
                                        //                    .FirstOrDefault(i => i.RoleId == preferredRole.Id
                                        //                    && i.Deleted == false
                                        //                    && i.PropertyUserMapsId == pumap.Id).StatusChangedOn,
                                        CreatedAt = prop.CreatedAt,
                                        Deleted = prop.Deleted,
                                        Id = pumap.Id,//As suggested by Abhijith,Sending the PropertyUsermapping id instead of propertyID
                                        PropertyId = prop.Id,
                                        UpdatedAt = 
                                        (prop.UpdatedAt >= prop.PropertyUserStatus.OrderByDescending(i => i.UpdatedAt).FirstOrDefault().UpdatedAt
                                                ? prop.UpdatedAt : prop.PropertyUserStatus.OrderByDescending(i => i.UpdatedAt).FirstOrDefault().UpdatedAt),
                                        //( prop.PropertyUserStatus // Abhijeet UpdatedAt null -12-10-2018
                                        //                    .FirstOrDefault(i => i.RoleId == preferredRole.Id
                                        //                    && i.Deleted == false
                                        //                    && i.PropertyUserMapsId == pumap.Id).StatusChangedOn== null? prop.CreatedAt: prop.PropertyUserStatus // Abhijeet UpdatedAt null
                                        //                    .FirstOrDefault(i => i.RoleId == preferredRole.Id
                                        //                    && i.Deleted == false
                                        //                    && i.PropertyUserMapsId == pumap.Id).StatusChangedOn) ,//prop.UpdatedAt,
                                        IsUnassigned = pumap.Deleted,
                                        Latitude = prop.Latitude,
                                        Longitude = prop.Longitude,
                                        IsIsolated = prop.IsIsolated,
                                        NotesCount = prop.PropertyUserStatus.Count(i => i.Deleted == false && !string.IsNullOrEmpty(i.Notes)),
                                        IsLastStatusUpdate = (pumap.Id == (prop.PropertyUserStatus.Where(i => i.RoleId == preferredRole.Id && i.UserId == user.Id && i.StatusId!=null
                                                                                                && i.Deleted == false).OrderByDescending(i => i.StatusChangedOn).FirstOrDefault().Id)) ? true : false
                                    });
                    return propList.AsQueryable().OrderBy(i => i.StatusChangedOn);
                }
                else
                {
                    if (assignedFlag == 1)
                    {
                        //var propAssignCount = context.PropertyUserStatus.Where(i => !i.Deleted && i.StatusId==null && i.Notes==null)
                        //    .GroupBy(i => new{ i.PropertyId,i.IncidentId}).Select(i => new {id=i.Key.PropertyId,i.Key.IncidentId, PropCount = i.Count()});
                                           
                        // for admin and other users
                        var propList = from prop in context.Properties.Where(i => !i.Deleted)
                                       select new PropertyResponse
                                       {
                                           BuildingName = prop.BuildingName,
                                           BuildingNumber = prop.BuildingNumber,
                                           Cell = prop.Cell,
                                           Country = (string.IsNullOrEmpty(prop.Country) ? "" : prop.Country), //Abhijeet-08-10-2018: this field does not show null value.
                                           CreatedBy = prop.CreatedBy,
                                           DependentLocality = prop.DependentLocality,
                                           DependentStreet = prop.DependentStreet,
                                           IncidentId = prop.IncidentId,
                                           IncidentName = prop.Incident.IncidentId,
                                           LocalityName = prop.LocalityName,
                                           MCBuildingName = prop.MCBuildingName,
                                           MCSubBuildingName = prop.MCSubBuildingName,
                                           ModifiedBy = prop.ModifiedBy,
                                           MPRN = prop.MPRN,
                                           Postcode = prop.Postcode,
                                           PostTown = prop.PostTown,
                                           PrincipalStreet = prop.PrincipalStreet,
                                           PriorityCustomer = prop.PriorityCustomer,
                                           Status = prop.Incident.Status,
                                           SubBuildingName = prop.SubBuildingName,
                                           Zone = prop.Zone,
                                           ZoneManagerId = prop.ZoneManagerId,
                                           ZoneManagerName= context.Users.Where(i=>i.Id==prop.ZoneManagerId).Select(i=> new {Name= i.FirstName +" "+ i.LastName}).FirstOrDefault().Name,
                                           CellManagerId = prop.CellManagerId,
                                           CellManagerName = context.Users.Where(i => i.Id == prop.CellManagerId).Select(i => new { Name = i.FirstName + " " + i.LastName }).FirstOrDefault().Name,
                                           //we can remove if UI can use AssignedResourcecount.
                                           //isMPRNAssigned = (from pum in prop.PropertyUserMaps.Where(i => i.Deleted == false)
                                           //                  join pus in prop.PropertyUserStatus.Where(i => i.Deleted == false)
                                           //                  on pum.Id equals pus.PropertyUserMapsId into temp
                                           //                  from lj in temp.DefaultIfEmpty()
                                           //                  where lj == null || lj.StatusId == null
                                           //                  select new { pum.PropertyId, pum.UserId, pum.RoleId } //Abhijeet added  pum.RoleId 25-10-2018
                                           //        ).Distinct().Count() > 0 ? true : false,
                                           // Consider only inprogress MPRNs dont consider completed count
                                           AssignedResourceCount = //propAssignCount.FirstOrDefault(i => i.id == prop.Id && i.IncidentId == prop.IncidentId)==null? 0: propAssignCount.FirstOrDefault(i=>i.id==prop.Id && i.IncidentId==prop.IncidentId).PropCount,
                                               (from pus in prop.PropertyUserStatus.Where(i => i.Deleted == false && i.StatusId == null && i.Notes == null)

                                                select new { pus.PropertyId, pus.UserId, pus.RoleId }//Abhijeet added  pum.RoleId 25-10-2018
                                               ).Distinct().Count(),
                                           LatestStatus = prop.PropertyStatusMstr.Status, //prop.PropertyUserStatus.OrderByDescending(i => i.CreatedAt).Take(1).FirstOrDefault().PropertyStatusMstr.Status,
                                           LatestStatusId = prop.StatusId,// prop.PropertyUserStatus.OrderByDescending(i => i.CreatedAt).Take(1).FirstOrDefault().PropertyStatusMstr.Id,
                                           LatestSubStatus = prop.PropertySubStatusMstr.SubStatus, // prop.PropertyUserStatus.OrderByDescending(i => i.CreatedAt).Take(1).FirstOrDefault().PropertySubStatusMstr.SubStatus,
                                           LatestSubStatusId = prop.SubStatusId, //prop.PropertyUserStatus.OrderByDescending(i => i.CreatedAt).Take(1).FirstOrDefault().PropertySubStatusMstr.Id,
                                                                                 // Notes = prop.PropertyUserStatus.OrderByDescending(i => i.CreatedAt).Take(1).FirstOrDefault().Notes,
                                           CreatedAt = prop.CreatedAt,
                                           Deleted = prop.Deleted,
                                           Id = prop.Id,
                                           UpdatedAt = prop.UpdatedAt,
                                           // Latitude= "18."+ran.Next(522100, 522193).ToString(),
                                           // Longitude = "-0." + ran.Next(115000, 115050).ToString()
                                           //IsUnassigned = (from pum in prop.PropertyUserMaps.Where(i => i.Deleted == false)
                                           //                join pus in prop.PropertyUserStatus.Where(i => i.Deleted == false)
                                           //                on pum.Id equals pus.PropertyUserMapsId into temp
                                           //                from lj in temp.DefaultIfEmpty()
                                           //                where lj == null || lj.StatusId == null
                                           //                select new { pum.PropertyId, pum.UserId, pum.RoleId }//Abhijeet added  pum.RoleId 25-10-2018
                                           //        ).Distinct().Count() > 0 ? false : true,
                                          // Latitude = prop.Latitude,
                                          // Longitude = prop.Longitude,
                                           IsIsolated = prop.IsIsolated,
                                           NotesCount = prop.PropertyUserStatus.Count(i => i.Deleted == false && !string.IsNullOrEmpty(i.Notes))
                                       };

                        return propList.AsQueryable();
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT, Features.RESOURCEMGT,Features.ASSIGNEDMPRN}, OperationType = OperationType.READ)]
        // GET tables/Property/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PropertyResponse> GetProperty(string id)
        {
            try
            {
                var propDb = from prop in context.Properties.Where(i => i.Id == id)
                             select new PropertyResponse
                             {
                                 BuildingName = prop.BuildingName,
                                 BuildingNumber = prop.BuildingNumber,
                                 Cell = prop.Cell,
                                 CellManagerId = prop.CellManagerId,
                                 Country = prop.Country,
                                 CreatedBy = prop.CreatedBy,
                                 DependentLocality = prop.DependentLocality,
                                 DependentStreet = prop.DependentStreet,
                                 IncidentId = prop.IncidentId,
                                 IncidentName = prop.Incident.IncidentId,
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
                                 LatestStatus = prop.PropertyStatusMstr.Status,
                                 LatestSubStatus = prop.PropertySubStatusMstr.SubStatus,
                                 Notes = prop.PropertyUserStatus.OrderByDescending(i => i.UpdatedAt).Take(1).FirstOrDefault().Notes,
                                 LatestStatusId = prop.StatusId,
                                 LatestSubStatusId = prop.SubStatusId,
                                 CreatedAt = prop.CreatedAt,
                                 Deleted = prop.Deleted,
                                 Id = prop.Id,
                                 UpdatedAt = prop.UpdatedAt,
                                // Latitude = string.IsNullOrEmpty(prop.Latitude) ? "51.52219" + prop.CreatedAt.Value.Second : prop.Latitude,
                              //   Longitude = string.IsNullOrEmpty(prop.Longitude) ? "-0.11505" + prop.CreatedAt.Value.Second : prop.Longitude,
                                 IsIsolated = prop.IsIsolated,
                                 NotesCount = prop.PropertyUserStatus.Count(i => i.Deleted == false && !string.IsNullOrEmpty(i.Notes))

                             };
                SingleResult<PropertyResponse> result = new SingleResult<PropertyResponse>(propDb);
                return result;
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
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT, Features.ASSIGNEDMPRN}, OperationType = OperationType.UPDATE)]
        // PATCH tables/Property/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<IHttpActionResult> PatchProperty(string id, Delta<PatchPropertyRequest> patch)
        {
            try
            {
               
                return Ok(patch);// UpdateAsync(id, patch);
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT}, OperationType = OperationType.CREATE)]
        // POST tables/Property
        public async Task<IHttpActionResult> PostProperty(Property item)
        {
            try
            {
                Property current = await InsertAsync(item);
                return CreatedAtRoute("Tables", new { id = current.Id }, current);
            }
            catch (Exception ex)
            {
                HttpUtilities.ServerError(ex, Request);
                return null;
            }
        }

        [Authorize]
        [CustomAuthorize(Module = new Features[] { Features.INCIDENTMGT}, OperationType = OperationType.DELETE)]
        // DELETE tables/Property/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteProperty(string id)
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
