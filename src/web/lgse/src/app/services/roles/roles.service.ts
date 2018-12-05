import { Injectable } from '@angular/core';
import { ServerApiInterfaceService } from '../server-api-interface.service';
import { Observable } from 'rxjs';
import { RoleListResponse,RoleAddRequest,RoleEditRequest ,AssigningRolesToUserRequest,RolesForUserResponse,AssigningUsersToRoleRequest,UsersForRoleResponse,RoleAccessPermissionResponse} from '../../models/api/portalManagment/role.model';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginStatusProviderService } from '../login-status-provider.service';

@Injectable()
export class RolesService {
    constructor(
        private serverApiInterfaceService: ServerApiInterfaceService,
        private loginStatusProviderService: LoginStatusProviderService,
        private _http: HttpClient) {
    }
    
//Get API's
    getRoleDataList():
    Observable<RoleListResponse[]> {
    return this.serverApiInterfaceService.get(environment.baseurl + '/tables/Role?$orderby=CreatedAt desc');
}

//Get Selected Role 

getSelectedRole(id):
Observable<RoleListResponse[]> {
return this.serverApiInterfaceService.get(environment.baseurl + '/tables/Role/'+id);
}

//Assigning Multiples roles to single User
assigningRolesToUser(assigningRolesToUserRequest:AssigningRolesToUserRequest):
Observable<any> {
return this.serverApiInterfaceService.post(environment.baseurl + '/api/RoleCustom/AssignUnAssignRolesToUser',assigningRolesToUserRequest);
}

//Getting Assigned role for perticular user
gettingRolesForUser(roleId):
Observable<RolesForUserResponse[]> {
return this.serverApiInterfaceService.get(environment.baseurl + '/api/RoleCustom/GetUsers?roleId='+roleId);
}



assigningUsersToRole(assigningUsersToRoleRequest:AssigningUsersToRoleRequest):
Observable<any> {
return this.serverApiInterfaceService.post(environment.baseurl + '/api/RoleCustom/AssignUnAssignUsersToRole',assigningUsersToRoleRequest);
}

gettingUsersForRole(userId):
Observable<UsersForRoleResponse[]> {
return this.serverApiInterfaceService.get(environment.baseurl + '/api/RoleCustom/GetRoles?userId='+userId);
}

getRoleAccessPermission(id):
Observable<RoleAccessPermissionResponse[]> {
    return this.serverApiInterfaceService.get(environment.baseurl + '/api/RoleCustom/GetRolePermissions?roleId='+id);
    }

setRoleAccessPermission(roleAccessPermissionResponse:RoleAccessPermissionResponse[]):
Observable<any> {
return this.serverApiInterfaceService.post(environment.baseurl + '/api/RoleCustom/SetRolePermissions',roleAccessPermissionResponse);
}

//Post API's
addRole(roleAddRequest: RoleAddRequest):
        Observable<any> {
        return this.serverApiInterfaceService.post(environment.baseurl + '/tables/Role', roleAddRequest);
    }
    editRole(id,roleEditRequest: RoleEditRequest):
        Observable<any> {
        return this.serverApiInterfaceService.patch(environment.baseurl + '/api/Role/'+id, roleEditRequest);
    }

}
