//Role List 
export class RoleListResponse {
    roleName: string;
    description: string;
    id: string;
}
//Getting Roles for Perticular User
export class RolesForUserResponse
{
    id: string;
    firstName: string;
    lastName: string;
    
}

//Getting User for Assigning Role
export class UsersForRoleResponse
{
    id: string;
    roleName: string;
    
}

//Role Add
export class RoleAddRequest {
    roleName: string;
    description: string;
}
export class RoleEditRequest {
    RoleName: string;
    Description: string;
}
export class AssigningRolesToUserRequest
{
    UserId:string;
    RoleIds:string[];
}
export class AssigningUsersToRoleRequest
{
    UserIds:string[];
    RoleId:string;
}
export class RoleAccessPermissionResponse
{
    roleId:string;
    featureText: string;
    featureId: string;
    createPermission: boolean;
    readPermission: boolean;
    updatePermission:boolean;
}
export class PassRoleData {
    roleId: string;
    roleName: string;
}

