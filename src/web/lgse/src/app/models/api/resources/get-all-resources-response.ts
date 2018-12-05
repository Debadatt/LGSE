export class GetAllResourcesResponse {
    id: string;
    firstName: String;
    lastName: string;
    role: [string];
    organization: string;
    employeeId: string;
    email: string;
    checked: boolean;
    mprn: string;
    isAssigned: boolean;
    zones: [string];
    cells: [string];
    assignedMPRNCount: number;
    preferredRole: string;
    preferredRoleId: string;
    completed: number;
    inprogress: number;
    propertyId: string; // added for view resources 
    roleId: string;
    roleName:string;
}