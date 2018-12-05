
export class PropertyListResponse {
    id: string;
    mprn: string;
    buildingName: string;
    subBuildingName: string;
    buildingNumber: string;
    principalStreet: string;
    dependentStreet: string;
    postTown: string;
    localityName: string;
    dependentLocality: string;
    country: string;
    postcode: string;
    priorityCustomer: boolean;
    zone: string;
    cell: string;
    latitude: string;
    longitude: string;
    latestStatus:string;
    latestStatusId:string;
    latestSubStatus:string;
    latestSubStatusId:string;
    notes:string;
    notesCount:number;
    isIsolated:boolean;
    zoneManagerName:string;
    cellManagerName:string;
}
export class AssignedMprnResponse
{
    id:string;
    mprn: string;
    buildingName: string;
    subBuildingName: string;
    buildingNumber: string;
    principalStreet: string;
    dependentStreet: string;
    postTown: string;
    localityName: string;
    dependentLocality: string;
    country: string;
    postcode: string;
    priorityCustomer: boolean;
    zone: string;
    cell: string;
    latitude: string;
    longitude: string;
    latestStatus:string;
    latestStatusId:string;
    latestSubStatus:string;
    latestSubStatusId:string;
    notes:string;
    notesCount:number;
    incidentId:string;
    incidentName:string;
    status:number;
    propertyId:string;
    zoneManagerName:string;
    cellManagerName:string;
}
export class StausListResponse {
    id: string;
    status: string;
    displayOrder:number;
    shortText:string;
}
export class SubStausListResponse {
    id: string;
    subStatus: string;
}
export class UpdateMPRNStatusRequest {
    PropertyId: string;
    StatusId: string;
    PropertySubStatusMstrsId: string;
    Notes: string;
}
export class PublicMRPNList {
    categoryName: string;
    closingNotes: string;
    createdBy: string;
    description: string;
    endTime: string;
    id: string;
    incidentId: string;
    mprns: PropertyListResponse[];
    noOfPropsAffected: number;
    noOfPropsCompleted: number;
    notes: string;
    startTime: string;
    status: number;
}

