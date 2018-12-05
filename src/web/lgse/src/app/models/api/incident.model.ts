import { StausListResponse } from "./properties.model";

//Incident List 
export class IncidentListResponse {
    id: string;
    incidentId: string;
    categoryName: string;
    description: string;
    noOfPropsAffected: number;
    startTime: string;
    endTime: string;
    status: number;
    noOfPropsIsolated: number;
    noOfPropsRepropsStatusCountsstored: number;
    propsStatusCounts: PropertyStatusCount[] = [];
    propertyAllStatusCounts: IncidentPropertyStatusCount[];

}
export class PropertyStatusCount {
    statusId: string;
    count: number;
    //shorttext:string;
}

export class IncidentPropertyStatusCount {
    propertyStatusMaster: StausListResponse;
    count: number;
}
export class IncidentListResponseClone {
    id: string;
    incidentId: string;
    categoryName: string;
    description: string;
    noOfPropsAffected: number;
    startTime: string;
    endTime: string;
    status: number;
    noOfPropsIsolated: number;
    noOfPropsRepropsStatusCountsstored: number;
    propsStatusCounts: PropertyStatusCountClone[] = [];
}
export class PropertyStatusCountClone {
    statusId: string;
    count: number;
    shorttext: string;
}
//Fetching the incident data for Edit
export class IncidentDataResponse {
    id: string;
    status: number;
    noOfCells: number;
    noOfZones: number;
    noOfPropsAffected: number;
    notes: string;
    description: string;
    categoriesMstrId: string;
    incidentId: string;
    noOfPropsRestored: string;
}

//Incident Add
export class IncidentAddRequest {
    //Incident : IncidentAddDetailsRequest;
    CategoriesMstrId: string;
    Description: string;
    Notes: string;
    NoOfPropsAffected: number;
    NoOfZones: number;
    NoOfCells: number;
    MPRNs: IncidentPropertiesRequest[] = [];
    ResolveUsers: IncidentResolveUsersRequest[] = [];
}


// export class IncidentAffectedAreaCountRequest {
//     MPRN: string;
//     Zone: string;
//     Cell: string;
// }
// export class IncidentAffectedAreaCountResponse {
//     noOfPropsAffected: string;
//     noOfZones: string;
//     noOfCells:string;
// }
//Incident Property
export class IncidentPropertiesRequest {
    MPRN: string;
    BuildingName: string;
    SubBuildingName: string;
    MCBuildingName: string;
    MCSubBuildingName: string;
    BuildingNumber: string;
    PrincipalStreet: string;
    DependentStreet: string;
    PostTown: string;
    LocalityName: string;
    DependentLocality: string;
    Country: string;
    Postcode: string;
    PriorityCustomer: boolean;
    Zone: string;
    Cell: string;
    IncidentId: string;
    ZoneManagerName: string;
    CellManagerName: string;
    ZoneController: string;
}
export class IncidentResolveUsersRequest {
    Name: string;
    Email: string;
}

//Incident Delete
export class IncidentDeleteRequest {

}

//Incident Edit
//Incident Add
export class IncidentEditRequest {
    //Incident : IncidentAddDetailsRequest;
    Id: string;
    CategoriesMstrId: string;
    Description: string;
    Notes: string;
    Status: number;
    MPRNs: IncidentPropertiesRequest[] = [];
    ResolveUsers: IncidentResolveUsersRequest[] = [];
}

//Incident Property


//  model for pass incident data


export class PassIncidentData {
    recordid: string;
    incidentid: string;
    propertyid: string;
    status: number;
    urltype: string;
}
