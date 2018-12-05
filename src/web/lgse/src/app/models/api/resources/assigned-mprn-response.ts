export class AssignedMPRNResponse {
    id: string;
    status: number;
    incidentId: string;
    cell: string;
    zone: string;
    zoneManager:string;
    cellManager:string;
    priorityCustomer:boolean;
    postcode: string;
    country: string;
    dependentLocality: string;
    localityName: string;
    postTown: string;
    dependentStreet: string
    principalStreet: string;
    buildingNumber: string;
    subBuildingName: string;
    buildingName: string;
    mprn:string;
    checked:boolean;
}