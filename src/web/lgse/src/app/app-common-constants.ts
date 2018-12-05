export const SERVER_SYSTEM_ERROR_MAX_BOUNDARY = 10000;
export const APP_HOME = '/dashboard';
export const LOGIN = '/login';
export const SELECT_ROLE = '/auth/select-role';
export const ACTIVATE = '/auth/activate';
export const RESETPASSWORD = '/auth/reset-password';
export const FEED = '/feed';

// enum for selected resource type all resouces ,available resources.
export enum RESOURCES_TYPE {
  allresources = '1',
  availableresources = '2'
}
export enum MAP_TYPE {
  marker = 'marker',
  boundry = 'boundry'
}
export enum FeatureNames {
  DASHBOARD = 'DASHBOARD',
  RESOURCE_MANAGEMENT = 'RESOURCEMGT',
  INCIDENT_MANAGEMENT = 'INCIDENTMGT',
  PORTAL_MANAGEMENT = 'PORTALMANAGEMENT',
  ASSIGNED_MPRN = 'ASSIGNEDMPRN'
}
export enum PathType {
  WITHOUT_PARAM = 'withoutparam',
  WITH_PARAM = 'withparam'
}
export enum ResourcesType {
  ISOLATOR = 'Isolator',
  ENGINEER = 'Engineer'
}
export enum IncidentStatus {
  INPROGRESS = 0,
  COMPLETED = 1,
  CANCELED = 2
}
// end enum.
export const HistoryUrl = '/resources/mprn-history';
export const assignedproperties = '/incident/showProperties';
export const AssignedMprn = '/incident/showProperties';
export const CsvHeareds = ["BuildingName", "BuildingNumber", "Cell", "CellManager", "DependentLocality", "DependentStreet", "LocalityName", "MCBuildingName", "MCSubBuildingName", "MPRN", "PostTown", "PostcodeIncode", "PostcodeOutcode", "PrincipalStreet", "PriorityCustomer", "SubBuildingName", "Zone", "ZoneController"];






