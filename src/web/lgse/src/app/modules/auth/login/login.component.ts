import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { TranslateService } from '@ngx-translate/core';
import { UserService } from '../../../services/user.service';
import { LoginStatusProviderService } from 'src/app/services/login-status-provider.service';
import { DynamicWelcomeText } from 'src/app/services/dynamicwelcometext.service';
import { SELECT_ROLE, APP_HOME } from '../../../app-common-constants';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { Subscription } from 'rxjs';
import { LocalstorageService } from 'src/app/services/localstorage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {
  welcomeText: string;
  loginForm: FormGroup;
  formRoles = [];
  loginSubscription: Subscription;
  roleSubscription: Subscription;
  aclSubscription: Subscription;
  aclreceived: boolean;

  constructor(private translate: TranslateService,
    private userService: UserService,
    private localstorageservice: LocalstorageService,
    private router: Router,
    private loginStatusProviderService: LoginStatusProviderService,
    private dynamicWelcomeTextService: DynamicWelcomeText,
    private appNotificationService: AppNotificationService) { }

  ngOnInit() {
    if (localStorage.getItem('accessToken')) {
           console.log('user already logged in redirected to homme page');
          this.localstorageservice.defaulHomePage();
      }
    this.loginForm = new FormGroup({
      'loginData': new FormGroup({
        workemail: new FormControl(null, [Validators.required, Validators.email]),
        password: new FormControl(null, [Validators.required])
      }),
    });
    this.dynamicWelcomeTextService.getDynamicWelcomeText().subscribe(DynmaicWelcomeTextResponse => {
      console.log("dynamic text response", DynmaicWelcomeTextResponse);
      if (DynmaicWelcomeTextResponse != null) {
        this.welcomeText = DynmaicWelcomeTextResponse['description'];
      }
    });

    //Get Dynamic Welcome text

    // this subject updates to login component after login is successfull.
    this.loginSubscription = this.userService.getLoginSuccessMessage().subscribe(message => {
      // this will get user details of logged user.
      this.userService.getLoggedInUserDetails();
    });

    // subscription for update this component once user roles data will  updated.
    this.roleSubscription = this.userService.getUserRoles().subscribe(roles => {
      for (let role of roles) {
        this.formRoles.push({
          id: role.id,
          name: role.roleName
        });
      }
      this.navigateToHomeOrRolePage(roles);
    });
    // scubscripion for get user acl if  acl received then it will naviate to landing page of applications.
    this.aclSubscription = this.userService.aclReceivedSubjects.subscribe((value) => {
      this.aclreceived = true;
      this.navigatToHomePage();
    });
    // end 
  }// end of ng init

  navigateToHomeOrRolePage(roles) {
    if (roles.length == 1) {
      let selRoleName = roles[0].roleName;
      let selRoleId = roles[0].id;
      this.loginStatusProviderService.setLoggedInUserRoleInfo(selRoleName, selRoleId);
      this.userService.setPreferedUserRole(selRoleId);
      // this.userService.GetUserAcl(selRoleId);
      this.navigatToHomePage();
    } else if (roles.length > 1) {
      this.navigatToSelectRolePage();
    }
  }

  navigatToHomePage() {
    // navigate to incident page
    this.loginStatusProviderService.isLoggedIn = true;
    //  this.router.navigate([APP_HOME]);
    //  console.log('this.localstorageservice.defaulHomePage()',this.localstorageservice.defaulHomePage());
    if (this.aclreceived) {
      this.localstorageservice.defaulHomePage();
    }
    // setTimeout(function(){  this.localstorageservice.defaulHomePage(); }, 3000);
    //  this.router.navigate([url])
  }

  onLogin() {
    if (!this.loginForm.valid)
      return;
    this.login();
  }
  // code block for login button click
  login() {
    // show progress bar here
    this.aclreceived = false;
    this.localstorageservice.userACLResponseArray = [];
    this.localstorageservice.userACLResponseMap = null;
    const workEmail = this.loginForm.value.loginData.workemail;
    const password = this.loginForm.value.loginData.password;
    this.userService.login(workEmail, password);
  }

  navigatToSelectRolePage() {
    this.loginStatusProviderService.isLoggedIn = true;
    this.router.navigate([SELECT_ROLE]);
  }

  // private getRoleFromID(roleId) {
  //   if(this.formRoles.length>0) {
  //     let selRole = this.formRoles.filter( role =>
  //       role.id === roleId
  //      );
  //      return selRole[0].roleName;
  //   } 
  //   return '';
  //  }

  ngOnDestroy() {
    if (this.loginSubscription) {
      this.loginSubscription.unsubscribe();
    }

    if (this.roleSubscription) {
      this.roleSubscription.unsubscribe();
    }
    if (this.aclSubscription) {
      this.aclSubscription.unsubscribe();
    }
  }
}
