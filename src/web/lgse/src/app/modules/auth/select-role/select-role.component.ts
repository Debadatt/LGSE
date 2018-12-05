import { Component, OnInit, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserService } from '../../../services/user.service';

import { Router } from '@angular/router';
import { LoginStatusProviderService } from 'src/app/services/login-status-provider.service';
import { APP_HOME } from '../../../app-common-constants';
import { Subscription } from 'rxjs';
import { UserRoleSelected } from '../../../models/ui/user.role.selected';
import { LocalstorageService } from 'src/app/services/localstorage.service';
import { AppNotificationService } from 'src/app/services/notification/app-notification.service';

@Component({
  selector: 'app-select-role',
  templateUrl: './select-role.component.html',
  styleUrls: ['./select-role.component.css']
})
export class SelectRoleComponent implements OnInit, OnDestroy {

  selectRoleForm: FormGroup;
  formRoles = [];
  userRolesSubscription: Subscription;
  aclSubscription: Subscription;
  aclreceived = false;
  select = 'select';
  selectedrole: string;
  constructor(private translate: TranslateService,
    private userService: UserService,
    private appnotificationservice: AppNotificationService,
    private localstorageservice: LocalstorageService,
    private loginStatusProviderService: LoginStatusProviderService,
    private router: Router) { }

  ngOnInit() {
    this.selectRoleForm = new FormGroup({
      'selectRoleData': new FormGroup({
        'role': new FormControl(null, Validators.required)
      }),
    });
    this.userRolesSubscription = this.userService.getUserRoles().subscribe(roles => {
      this.formRoles = [];
      for (let role of roles) {
        this.formRoles.push({
          id: role.id,
          name: role.roleName
        });
      }
      this.isNavigateToHome();
    });
    this.userService.getLoggedInUserDetails();
    this.aclSubscription = this.userService.aclReceivedSubjects.subscribe((value) => {
      this.aclreceived = true;
      this.navigatToHomePage();
    });
  } // ng init

  isNavigateToHome() {
    if (this.formRoles.length == 1) {
      // this.loginStatusProviderService.roleId = this.formRoles[0].roleId;
      this.loginStatusProviderService.setLoggedInUserRoleInfo(this.formRoles[0].roleName, this.formRoles[0].roleId);
      this.navigatToHomePage();
    }
  }

  // onSelectRole(roleId) {

  //   let selRoleName = this.getRoleFromID(roleId);
  //   let setRoleId = roleId;
  //   this.loginStatusProviderService.setLoggedInUserRoleInfo(selRoleName, setRoleId);
  // }

  onSelectRole() {
    let selRoleId = this.selectRoleForm.value.selectRoleData.role;
    this.selectedrole = selRoleId;
    console.log('selroleid');
    console.log(selRoleId);
    if (selRoleId && selRoleId != this.select) {
      let selRoleName = this.getRoleFromID(selRoleId);
      let setRoleId = selRoleId;
      this.loginStatusProviderService.setLoggedInUserRoleInfo(selRoleName, setRoleId);
    }
  }

  private getRoleFromID(roleId) {
    if (this.formRoles.length > 0) {
      let selRole = this.formRoles.filter(role =>
        role.id === roleId
      );
      return selRole[0].name;
    }
    return '';
  }

  onConfirm() {
    if (this.selectedrole != this.select) {
      this.sendRoleSelectionToServer();
      //   this.navigatToHomePage();
    } else {
      this.appnotificationservice.error('Select Role');
    }
  }

  navigatToHomePage() {
    // navigate to incident page
    this.loginStatusProviderService.isLoggedIn = true;
    // this.router.navigate([APP_HOME]);
    if (this.aclreceived) {
      this.localstorageservice.defaulHomePage();
    }
  }

  sendRoleSelectionToServer() {
    let localStoreUserRole: UserRoleSelected = this.loginStatusProviderService.getUserRoleInfo();
    this.userService.setPreferedUserRole(localStoreUserRole.roleId);
  }


  ngOnDestroy() {
    if (this.userRolesSubscription) {
      this.userRolesSubscription.unsubscribe();
    }
    if (this.userRolesSubscription) {
      this.userRolesSubscription.unsubscribe();
    }
  }

}
