import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { UserRoleSelected } from '../models/ui/user.role.selected';

@Injectable()
export class LoginStatusProviderService {
    isLoggedIn: boolean;
    authToken: string;
    userRoleName: string;
    roleId: string;    
    logginStatusChanged = new Subject<boolean>();    
    changeLoginStatus(value: boolean) {
        this.isLoggedIn = value;
        this.logginStatusChanged.next(value);
        if(!value) {
            localStorage.removeItem('accessToken');
            localStorage.clear();
        }
    }

    setLoggedInUserRoleInfo(userRole: string, id: string) {
        this.userRoleName = userRole;
        this.roleId = id;
        let selecteUserRole = new UserRoleSelected();
        selecteUserRole.roleId = this.roleId;
        selecteUserRole.roleName = this.userRoleName;
        localStorage.setItem('userRole',JSON.stringify(selecteUserRole));
    }

    getUserRoleInfo() {
        let localStorageUserRole = localStorage.getItem('userRole');
        let localStoreUserRole : UserRoleSelected = JSON.parse(localStorageUserRole);
        let selecteUserRole = new UserRoleSelected();
        selecteUserRole.roleId = this.roleId || localStoreUserRole.roleId;
        selecteUserRole.roleName = this.userRoleName || localStoreUserRole.roleName;
        return selecteUserRole;
    }
}

