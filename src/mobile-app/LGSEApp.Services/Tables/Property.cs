using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LGSEApp.Services.Tables
{
    public class Property : BaseParameters
    {
        private string propertyId;
        [JsonProperty(PropertyName = "propertyId")]
        public string PropertyId
        {
            get { return propertyId; }
            set { propertyId = value; }
        }

        private string mPRN;
        [JsonProperty(PropertyName = "mPRN")]
        public string MPRN
        {
            get { return mPRN; }
            set { mPRN = value; }
        }
        private string buildingName;
        [JsonProperty(PropertyName = "buildingName")]
        public string BuildingName
        {
            get { return buildingName; }
            set { buildingName = value; }
        }
        private string subBuildingName;
        [JsonProperty(PropertyName = "subBuildingName")]
        public string SubBuildingName
        {
            get { return subBuildingName; }
            set { subBuildingName = value; }
        }
        private string mCBuildingName;
        [JsonProperty(PropertyName = "mCBuildingName")]
        public string MCBuildingName
        {
            get { return mCBuildingName; }
            set { mCBuildingName = value; }
        }
        private string mCSubBuildingName;
        [JsonProperty(PropertyName = "mCSubBuildingName")]
        public string MCSubBuildingName
        {
            get { return mCSubBuildingName; }
            set { mCSubBuildingName = value; }
        }
        private string buildingNumber;
        [JsonProperty(PropertyName = "buildingNumber")]
        public string BuildingNumber
        {
            get { return buildingNumber; }
            set { buildingNumber = value; }
        }
        private string principalStreet;
        [JsonProperty(PropertyName = "principalStreet")]
        public string PrincipalStreet
        {
            get { return principalStreet; }
            set { principalStreet = value; }
        }
        private string dependentStreet;
        [JsonProperty(PropertyName = "dependentStreet")]
        public string DependentStreet
        {
            get { return dependentStreet; }
            set { dependentStreet = value; }
        }
        private string postTown;
        [JsonProperty(PropertyName = "postTown")]
        public string PostTown
        {
            get { return postTown; }
            set { postTown = value; }
        }
        private string localityName;
        [JsonProperty(PropertyName = "localityName")]
        public string LocalityName
        {
            get { return localityName; }
            set { localityName = value; }
        }
        private string dependentLocality;
        [JsonProperty(PropertyName = "dependentLocality")]
        public string DependentLocality
        {
            get { return dependentLocality; }
            set { dependentLocality = value; }
        }
        private string country;
        [JsonProperty(PropertyName = "country")]
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        private string postcode;
        [JsonProperty(PropertyName = "postcode")]
        public string Postcode
        {
            get { return postcode; }
            set { postcode = value; }
        }
        private bool priorityCustomer;
        [JsonProperty(PropertyName = "priorityCustomer")]
        public bool PriorityCustomer
        {
            get { return priorityCustomer; }
            set { priorityCustomer = value; }
        }
        private string zone;
        [JsonProperty(PropertyName = "zone")]
        public string Zone
        {
            get { return zone; }
            set { zone = value; }
        }
        private string cell;
        [JsonProperty(PropertyName = "cell")]
        public string Cell
        {
            get { return cell; }
            set { cell = value; }
        }
        private string incidentId;
        [JsonProperty(PropertyName = "incidentId")]
        public string IncidentId
        {
            get { return incidentId; }
            set { incidentId = value; }
        }
        private string incidentName;
        [JsonProperty(PropertyName = "incidentName")]
        public string IncidentName
        {
            get { return incidentName; }
            set { incidentName = value; }
        }
        private string cellManagerId;
        [JsonProperty(PropertyName = "cellManagerId")]
        public string CellManagerId
        {
            get { return cellManagerId; }
            set { cellManagerId = value; }
        }
        private string zoneManagerId;
        [JsonProperty(PropertyName = "zoneManagerId")]
        public string ZoneManagerId
        {
            get { return zoneManagerId; }
            set { zoneManagerId = value; }
        }
        private int status;
        [JsonProperty(PropertyName = "status")]
        public int Status
        {
            get { return status; }
            set { status = value; }
        }
        private string latestStatus;
        [JsonProperty(PropertyName = "latestStatus")]
        public string LatestStatus
        {
            get { return latestStatus; }
            set { latestStatus = value; }
        }
        private string latestSubStatus;
        [JsonProperty(PropertyName = "latestSubStatus")]
        public string LatestSubStatus
        {
            get { return latestSubStatus; }
            set { latestSubStatus = value; }
        }
        private string notes;
        [JsonProperty(PropertyName = "notes")]
        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }
        private double latitude;
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }
        private double longitude;
        [JsonProperty(PropertyName = "longitude")]
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
        private bool isMPRNAssigned;
        [JsonProperty(PropertyName = "isMPRNAssigned")]
        public bool IsMPRNAssigned
        {
            get { return isMPRNAssigned; }
            set { isMPRNAssigned = value; }
        }
        private int assignedResourceCount;
        [JsonProperty(PropertyName = "assignedResourceCount")]
        public int AssignedResourceCount
        {
            get { return assignedResourceCount; }
            set { assignedResourceCount = value; }
        }
        private bool isStatusUpdated;
        [JsonProperty(PropertyName = "isStatusUpdated")]
        public bool IsStatusUpdated
        {
            get { return isStatusUpdated; }
            set { isStatusUpdated = value; }
        }
        private bool isUnassigned;
        [JsonProperty(PropertyName = "isUnassigned")]
        public bool IsUnassigned
        {
            get { return isUnassigned; }
            set { isUnassigned = value; }
        }
        private DateTime? statusChangedOn;
        [JsonProperty(PropertyName = "statusChangedOn")]
        public DateTime? StatusChangedOn
        {
            get { return statusChangedOn; }
            set { statusChangedOn = value; }
        }

        
              private bool isLastStatusUpdate;
        [JsonProperty(PropertyName = "isLastStatusUpdate")]
        public bool IsLastStatusUpdate
        {
            get { return isLastStatusUpdate; }
            set { isLastStatusUpdate = value; }
        }
        private bool isIsolated;
        [JsonProperty(PropertyName = "isIsolated")]
        public bool IsIsolated
        {
            get { return isIsolated; }
            set { isIsolated = value; }
        }
        
            private bool notesCount;
        [JsonProperty(PropertyName = "notesCount")]
        public bool NotesCount
        {
            get { return notesCount; }
            set { notesCount = value; }
        }
    }
}
