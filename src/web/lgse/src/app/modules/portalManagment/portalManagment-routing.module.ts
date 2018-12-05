import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoleListComponent } from './roles/role-list/role-list.component';
import { RoleAddEditComponent } from './roles/role-addedit/role-addedit.component';
import { WhiteListDomainAddEditComponent } from './whiteListDomains/whiteListDomain-addedit/whiteListDomain-addedit.component';
import { WhiteListDomainComponent } from './whiteListDomains/whiteListDomain-list/whiteListDomain-list.component';
import { UserAddEditComponent } from './users/user-addedit/user-addedit.component';
import { UserEditComponent } from './users/user-edit/user-edit.component';
import { UserListComponent } from './users/user-list/user-list.component';
import { RoleEditComponent } from './roles/role-edit/role-edit.component';
import { WhiteListDomainEditComponent } from './whiteListDomains/whiteListDomain-edit/whiteListDomain-edit.component';
import { RoleManagmentComponent } from './roleManagment/roleManagement.component';
import { UserManagmentComponent } from './userManagment/userManagment.component';
import { WelcomeTextComponent } from './welcome-text/welcome-text.component';
import {RoleAccessPermission} from './roleAccessPermission/roleAccessPermission.component';

const routes: Routes = [
  { path: 'roleList', component: RoleListComponent },
  { path: 'addRole', component: RoleAddEditComponent },
  {
    path: 'editRole/:id',
    component: RoleEditComponent
  },

  { path: 'userList', component: UserListComponent },
  { path: 'addUser', component: UserAddEditComponent },
  { path: 'editUser/:id', component: UserEditComponent },
  { path: 'whiteListDomain', component: WhiteListDomainComponent },
  
  { path: 'addWhiteListDomain', component: WhiteListDomainAddEditComponent },
  {
    path: 'editWhiteListDomain/:id',
    component: WhiteListDomainEditComponent
  },
  { path: 'assignRole', component: RoleManagmentComponent },
  { path: 'welcomeText', component: WelcomeTextComponent },
  { path: 'assignUserToRoles', component: UserManagmentComponent },
  { path: 'roleAccessPermission', component: RoleAccessPermission },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PortalManagmentRoutingModule { }
