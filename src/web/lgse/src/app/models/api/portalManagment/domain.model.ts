//Domain List 
export class DomainListResponse {
    orgName: string;
    domainName: string;
    id: string;
    IsActive:boolean;
}
//Domain Add
export class DomainAddRequest {
    OrgName: string;
    DomainName: string;
    IsActive:boolean;
}
export class DomainEditRequest {
    orgName: string;
    isActive:boolean 
}

