export interface User {
    token?;
    username?;
    userId?;
}

//User List 
export class UserListResponse {
    id : string;
    email: string;
    firstName: string;
    lastName: string;
    roles: [string];
    employeeId: string;
    eusr: string;
    contactNo: string;
    isActive:boolean;
}

export class UserAddRequest {
    firstName: string;
    lastName: string;
    employeeId: string;
    roleId: string;
    eusr: string;
    contactNo: string;
    isActive: boolean;
    email: string;
    IsActiveUser:boolean;
}
export class UserEditRequest
{
    firstName: string;
    lastName: string;
    employeeId: string;
    roleId: string;
    eusr: string;
    contactNo: string;
    isActiveUser: boolean;
    email: string;
    activationOTP:string;
}

export class UserId{
    UserId:string;
}
export class PassUserData
{
    id:string;
    firstName:string;
    lastName:string;
}

