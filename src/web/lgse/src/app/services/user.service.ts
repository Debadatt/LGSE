import { Injectable, OnInit } from '@angular/core';
import { RequestModel } from '../models/api/request.model';
import { Observable, Subject } from 'rxjs';
import { UserProfile } from '../models/api/user.profile.model';
import { Register } from '../models/api/register.model';
import { Role } from '../models/api/role.model';
import { ServerApiInterfaceService } from './server-api-interface.service';
import { environment } from '../../environments/environment';
import { TranslateService } from '@ngx-translate/core';
import { UserRole, UserRoleID } from '../models/api/user.role.model';

import { UserListResponse, UserAddRequest, UserEditRequest, UserId } from '../models/api/user.model';
import { ApiErrorService } from './api-error.service';
import { ApiSuccessResponse } from '../models/api/payload-models';
import { LoginRequest } from '../models/api/login.request.model';
import { ForgotPasswordRequest } from '../models/api/forgot.password.request.model';
import { ResetPasswordRequest } from '../models/api/reset.password.request';
import { NewPasswordRequest } from '../models/api/new.password.request.model';
import { PassdataService } from './passdata.service';

import { UserACLResponse } from 'src/app/models/ui/acl-response';
import { LocalstorageService } from 'src/app/services/localstorage.service';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class UserService implements OnInit {

  // base URL
  baseUrl = environment.baseurl + '/api';
  // login 
  loginUrl = this.baseUrl + '/Account/Login';
  loginSuccessMessage: Subject<string>;
  aclReceivedSubjects: Subject<boolean> = new Subject<boolean>();
  // register
  registerUrl = this.baseUrl + '/Account/Signup';
  regSuccessMessage: Subject<string>;
  // forgot password
  forgotPasswordUrl = this.baseUrl + '/Account/OTP';
  forgotPassSuccessMessage: Subject<string>;
  // reset password
  resetPasswordUrl = this.baseUrl + '/Account/ForgotPassword';
  resetPasswordSuccessMessage: Subject<string>;
  otpExpiredMessage: Subject<string>;
  // new password
  setNewPasswordUrl = this.baseUrl + '/Account/ChangePassword';
  changePasswordSuccessMessage: Subject<string>;
  // activate user
  activateUserUrl = this.baseUrl + '/Account/ActivateUser';
  //logout user
  logoutUserUrl = this.baseUrl + '/Account/Logout';
  logoutPassSuccessMessage: Subject<string>;
  // roles 
  rolesUrl = this.baseUrl + '/Role';
  allRoles: Subject<Role[]>;
  allUserRoles: Subject<UserRole[]>;

  // set preered Roles url
  prefRoleUrl = this.baseUrl + '/Account/UpdatePreferredRole';

  // user details
  userDetailsUrl = this.baseUrl + '/Account/UserProfile';

  // user list
  userList = this.baseUrl + '/User?$orderby=CreatedAt desc';


  constructor(private translateService: TranslateService,
    private passdataService: PassdataService,
    private localstorageservice: LocalstorageService,
    private router: Router,
    private serverApiInterfaceService: ServerApiInterfaceService,
    private apiErrorService: ApiErrorService) {
    this.forgotPassSuccessMessage = new Subject<string>();
    this.logoutPassSuccessMessage = new Subject<string>();
    this.allRoles = new Subject<Role[]>();
    this.allUserRoles = new Subject<UserRole[]>();
    this.resetPasswordSuccessMessage = new Subject<string>();
    this.otpExpiredMessage = new Subject<string>();
    this.changePasswordSuccessMessage = new Subject<string>();
    this.loginSuccessMessage = new Subject<string>();
    this.regSuccessMessage = new Subject<string>();
  }

  ngOnInit() {

  }

  getRoles() {
    this.serverApiInterfaceService.get<Role[]>(this.rolesUrl).subscribe(
      (response) => {
        const roles: Role[] = response;
        this.setRoles(roles);
      },
      (error) => {
        this.apiErrorService.handleError(error);
      }
    );
  }

  getAllRoles(): Observable<Role[]> {
    return this.allRoles.asObservable();
  }

  private setRoles(roles: Role[]) {
    this.allRoles.next(roles);
  }

  login(email: string, password: string) {
    let loginRequest = new LoginRequest();
    loginRequest.Email = email;
    loginRequest.Password = password;
    return this.serverApiInterfaceService.post<ApiSuccessResponse>(this.loginUrl, loginRequest).subscribe(
      (response) => {
        localStorage.setItem('accessToken', response.token);
        localStorage.setItem('username', response.username);
        localStorage.setItem('userId', response.userId);
        // login is successfull then bellow method will called and it will update to login component regarding with login is successfull.
        this.setLoginSuccessMessage("Login Successfull.");
      },
      (error) => {
        this.apiErrorService.handleError(error);
      }
    );
  }

  getLoginSuccessMessage(): Observable<string> {
    return this.loginSuccessMessage.asObservable();
  }

  private setLoginSuccessMessage(message: string) {
    // after login is successfull it will update to login component regarding with login is successfull.
    this.loginSuccessMessage.next(message);
  }

  register(registerData: Register) {
    var requestModel = new RequestModel();
    requestModel.url = this.registerUrl;
    requestModel.body = registerData;
    return this.serverApiInterfaceService.post<ApiSuccessResponse>(this.registerUrl, registerData).subscribe(
      (response) => {
        this.translateService.get('RESPONSE-CODES.' + response.message).subscribe((res: string) => {
          this.setRegisterSuccessMessage(res);
        });
      },
      (error) => {
        this.apiErrorService.handleError(error);
      }
    );
  }

  getRegisterSuccessMessage(): Observable<string> {
    return this.regSuccessMessage.asObservable();
  }

  private setRegisterSuccessMessage(message: string) {
    this.regSuccessMessage.next(message);
  }

  forgotPassword(userEmail: string) {
    let forgotPasswordReq: ForgotPasswordRequest = new ForgotPasswordRequest();
    forgotPasswordReq.Email = userEmail;
    return this.serverApiInterfaceService.post<ApiSuccessResponse>(this.forgotPasswordUrl, forgotPasswordReq).subscribe(
      (response) => {
        this.translateService.get('RESPONSE-CODES.' + response.message).subscribe((res: string) => {
          this.setForgotPasswordSuccessMessage(res + ' : ' + userEmail);
        });
      },
      (error) => {
        this.apiErrorService.handleError(error);
      }
    );
  }

  getForgotPasswordSuccessMessage(): Observable<string> {
    return this.forgotPassSuccessMessage.asObservable();
  }

  private setForgotPasswordSuccessMessage(message: string) {
    this.forgotPassSuccessMessage.next(message);
  }

  resetPassword(otp: string, email: string, password: string, url: string) {
    let resetPasswordReq: ResetPasswordRequest = new ResetPasswordRequest();
    resetPasswordReq.OTPCode = otp;
    resetPasswordReq.Email = email;
    resetPasswordReq.Password = password;
    return this.serverApiInterfaceService.post<ApiSuccessResponse>(this.baseUrl + url, resetPasswordReq).subscribe(
      (response) => {
        this.translateService.get('RESPONSE-CODES.' + response.message).subscribe((res: string) => {
          this.setResetPasswordSuccessMessage(res);
        });
      },
      (error) => {
        console.log('error message : ' + error.message);
        if (error.error.message === 'OTP_EXPIRED') {
          this.setOtpExpiredMessage('OTP_EXPIRED');
        } else {
          this.apiErrorService.handleError(error);
        }
      }
    );
  }

  getResetPasswordSuccessMessage(): Observable<string> {
    return this.resetPasswordSuccessMessage.asObservable();
  }

  private setResetPasswordSuccessMessage(message: string) {
    this.resetPasswordSuccessMessage.next(message);
  }

  getOtpExpiredMessage(): Observable<string> {
    return this.otpExpiredMessage.asObservable();
  }

  private setOtpExpiredMessage(message: string) {
    this.otpExpiredMessage.next(message);
  }

  setNewPassword(oldPassword: string, newPassword: string) {
    let newPasswordReq: NewPasswordRequest = new NewPasswordRequest();
    newPasswordReq.OldPassword = oldPassword;
    newPasswordReq.NewPassword = newPassword;
    return this.serverApiInterfaceService.post<ApiSuccessResponse>(this.setNewPasswordUrl, newPasswordReq).subscribe(
      (response) => {
        this.translateService.get('RESPONSE-CODES.' + response.message).subscribe((res: string) => {
          localStorage.clear();
          this.setChangePasswordSuccessMessage(res);
        });
      },
      (error) => {
        this.apiErrorService.handleError(error);
      });
  }

  getChangePasswordSuccessMessage(): Observable<string> {
    return this.changePasswordSuccessMessage.asObservable();
  }

  private setChangePasswordSuccessMessage(message: string) {
    this.changePasswordSuccessMessage.next(message);
  }
  // function for getting logged user details.
  getLoggedInUserDetails() {
    let loggedInUser = localStorage.getItem('username');
    this.serverApiInterfaceService.get<UserProfile>(this.userDetailsUrl).subscribe(
      (response) => {
        let userProfile: UserProfile = response;
        if (response) {
          this.passdataService.userName = response.firtName + " " + response.lastName;
          // this.passdataService.roleName=response.roles;
        }
        let userRoles: UserRole[] = userProfile.roles;
        // this fucntion will set logger user detaills.
        this.setUserRoles(userRoles);
      },
      (error) => {
        this.apiErrorService.handleError(error);
      }
    );
  }

  getUserRoles(): Observable<Role[]> {
    return this.allUserRoles.asObservable();
  }
  // fucntion for setting logged user details 
  private setUserRoles(roles: UserRole[]) {
    this.allUserRoles.next(roles);
  }

  setPreferedUserRole(roleId: string) {
    let roleToSet: UserRoleID = {
      'RoleId': roleId
    };
    this.serverApiInterfaceService.post<ApiSuccessResponse>(this.prefRoleUrl, roleToSet).subscribe(
      (response) => {
        console.log(response);
        this.GetUserAcl(roleId);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  //Get UserList
  getUserList():
    Observable<UserListResponse[]> {
    return this.serverApiInterfaceService.get(this.userList);
  }
  addUser(userAddRequest: UserAddRequest):
    Observable<any> {
    return this.serverApiInterfaceService.post(environment.baseurl + '/tables/User', userAddRequest);
  }

  editUser(id, userEditRequest: UserEditRequest):
    Observable<any> {
    return this.serverApiInterfaceService.patch(environment.baseurl + '/api/User/' + id, userEditRequest);
  }
  //get selected user
  getSelectedUser(id):
    Observable<UserEditRequest[]> {
    return this.serverApiInterfaceService.get(environment.baseurl + '/tables/User/' + id);
  }

  //Logout User
  // Logout(logoutRequest: LogoutRequest):
  //     Observable<any> {
  //       console.log("in userservicr",environment.baseurl + '/api/Account/Logout');
  //       alert("Test");
  //     return this.serverApiInterfaceService.post(environment.baseurl + '/api/Account/Logout',null);
  //   }


  Logout(isnavigate: boolean) {
    console.log("url", this.logoutUserUrl);
    return this.serverApiInterfaceService.post(this.logoutUserUrl, null).subscribe(
      (response) => {
        localStorage.removeItem('accessToken');
        localStorage.clear();
        this.localstorageservice.userACLResponseArray = [];
        this.localstorageservice.userACLResponseMap = null;
        if (isnavigate) {
          this.router.navigate(['/auth/login']);
        }
      },
      (error) => {
        this.apiErrorService.handleError(error);
      }
    );
  }
  // editUsetResetPassword(id) {
  //   console.log("url", this.editUserResetPassword);
  //   return this.serverApiInterfaceService.post(this.editUserResetPassword, id).subscribe(
  //     (response) => {
  //       localStorage.clear();
  //       this.localstorageservice.userACLResponseArray = [];
  //       this.localstorageservice.userACLResponseMap = null;
  //     },
  //     (error) => {
  //       this.apiErrorService.handleError(error);
  //     }
  //   );
  // }
  editUserResetPassword(user: UserId):
    Observable<any> {
    return this.serverApiInterfaceService.post(environment.baseurl + '/api/Account/ResetPassword', user);
  }

  GetUserAcl(roleid) {
    return this.serverApiInterfaceService.get(environment.baseurl + '/api/RoleCustom/GetRolePermissions?roleId=' + roleid).subscribe(
      (response) => {
        // localStorage.removeItem('acl');
        localStorage.setItem("acl", JSON.stringify(response));
        this.aclReceivedSubjects.next(true);
      },
      (error) => {
        this.apiErrorService.handleError(error);
      }
    );
  } // end of fucntion

  //Send Activation Link
  sendActivationLink(user: UserId):
    Observable<any> {
    // console.log("sendctivationLink",userId.UserId);
    return this.serverApiInterfaceService.post(environment.baseurl + '/api/Account/SendActivationLink', user);
  }


}// end of class 


