import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppCommonModule } from '../../app-common.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { Routes, RouterModule } from '@angular/router';
import { RoleListComponent } from './roles/role-list/role-list.component';
import { RoleAddEditComponent } from './roles/role-addedit/role-addedit.component';
import { RolesService } from '../../services/roles/roles.service';
import { DomainService } from '../../services/domains/domain.service';
import { RoleEditComponent } from './roles/role-edit/role-edit.component';
import { UserAddEditComponent } from './users/user-addedit/user-addedit.component';
import { UserEditComponent } from './users/user-edit/user-edit.component';
import { UserListComponent } from './users/user-list/user-list.component';
import { WhiteListDomainAddEditComponent } from './whiteListDomains/whiteListDomain-addedit/whiteListDomain-addedit.component';
import { WhiteListDomainComponent } from './whiteListDomains/whiteListDomain-list/whiteListDomain-list.component';
import { PortalManagmentRoutingModule } from './portalManagment-routing.module';
import { WhiteListDomainEditComponent } from './whiteListDomains/whiteListDomain-edit/whiteListDomain-edit.component';
import { RoleManagmentComponent } from './roleManagment/roleManagement.component';
import { UserManagmentComponent } from './userManagment/userManagment.component';
import {RoleAccessPermission} from './roleAccessPermission/roleAccessPermission.component';
import { WelcomeTextComponent } from './welcome-text/welcome-text.component';

@NgModule({
  declarations: [RoleListComponent,
    RoleAddEditComponent,RoleEditComponent,UserAddEditComponent,UserEditComponent,UserListComponent,
    WhiteListDomainAddEditComponent,WhiteListDomainComponent,WhiteListDomainEditComponent,RoleManagmentComponent,UserManagmentComponent,RoleAccessPermission, WelcomeTextComponent
  ],
  imports: [
    CommonModule,
    AppCommonModule,
    CommonModule,
    ReactiveFormsModule,
    TranslateModule,
    FormsModule,
    PortalManagmentRoutingModule
  ],
  exports: [RouterModule],
  providers: [
    RolesService,
    DomainService
  ],
})
export class PortalManagmentModule { }





