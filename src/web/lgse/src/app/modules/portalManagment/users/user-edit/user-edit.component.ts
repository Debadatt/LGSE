
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { RolesService } from '../../../../services/roles/roles.service';
import { UserService } from '../../../../services/user.service';
import { UserEditRequest, UserId } from '../../../../models/api/user.model';
import { ApiErrorService } from '../../../../services/api-error.service';
import { AppNotificationService } from '../../../../services/notification/app-notification.service';
import { PathType, FeatureNames } from 'src/app/app-common-constants';
import { PassdataService } from 'src/app/services/passdata.service';
import { LocalstorageService } from 'src/app/services/localstorage.service';


@Component({
  selector: 'app-user-Edit',
  templateUrl: './user-Edit.component.html',
  styleUrls: ['./user-Edit.component.css']
})

export class UserEditComponent implements OnInit {

  //Variable Declaration
  userAddForm: FormGroup;
  formRoles = [];
  feature = FeatureNames;
  mobnumPattern = "^(\\+)?[0-9]*";
  id: string;
  isActivated: boolean;
  otp: string;
  otplength: number
  backpath: boolean;

  constructor(private router: Router,
    private activtedroute: ActivatedRoute,
    private userService: UserService,
    private rolesService: RolesService,
    private apiErrorService: ApiErrorService,
    private passdataservice: PassdataService,
    private appNotificationService: AppNotificationService,
    public localstorageservice: LocalstorageService
  ) { }

  ngOnInit() {
    this.userAddForm = new FormGroup({
      'firstName': new FormControl(null, [Validators.required, Validators.maxLength(20), Validators.minLength(1), Validators.pattern('[a-zA-Z ]*')]),
      'lastName': new FormControl(null, [Validators.required, Validators.maxLength(20), Validators.minLength(1), Validators.pattern('[a-zA-Z ]*')]),
      'employeeId': new FormControl(null),
      'eusr': new FormControl(null, [Validators.maxLength(9), Validators.minLength(1), Validators.pattern('[0-9]*$')]),
      'email': new FormControl(null, [Validators.required, Validators.email]),
      'contactNo': new FormControl(null, [Validators.minLength(10), Validators.maxLength(13), Validators.pattern(this.mobnumPattern)]),
      'isActive': new FormControl(null),
    });
    this.id = this.activtedroute.snapshot.params.id;

    //Save Button Enable for Admin Role Only
    if (this.passdataservice.backpath.path == "/resources/all-resources") {
      this.backpath = false;
    }
    else {
      this.backpath = true;
    }
    this.getUserDetails(this.id);
  }

  //Get User Details for Perticular Id
  getUserDetails(id) {
    let isActive: boolean;
    this.userService.getSelectedUser(id).subscribe(payloadResponse => {
      this.userAddForm.get('firstName').setValue(payloadResponse['firstName']);
      this.userAddForm.get('lastName').setValue(payloadResponse['lastName']);
      this.userAddForm.get('employeeId').setValue(payloadResponse['employeeId']);
      this.userAddForm.get('eusr').setValue(payloadResponse['eusr']);
      this.userAddForm.get('email').setValue(payloadResponse['email']);
      this.userAddForm.get('contactNo').setValue(payloadResponse['contactNo']);
      
      isActive = payloadResponse['isActive'];
      this.isActivated = payloadResponse['isActivated'];
      this.otp = payloadResponse['activationOTP'];
      this.otplength = this.otp.length;
      
      if (isActive == true) {
        this.userAddForm.get('isActive').setValue("1");
      }
      else {
        this.userAddForm.get('isActive').setValue("0");
      }
    });
  }

  //Reset Password Functionality for admin
  resetPassword() {
    const user = new UserId();
    user.UserId = this.id;
    this.userService.editUserResetPassword(user).subscribe(
      response => {
        this.appNotificationService.success('Password Sent to Registered Mail!');
      },
      (error) => {
        this.apiErrorService.handleError(error, () => {
          if (error.error.message == "USER_NOT_ACVTD") {
            this.appNotificationService.error("User Not Activated!");
          }else if(error.error.message == "USER_DEACTIVATED_BY_ADMIN"){
            this.appNotificationService.error("User Is Inactivated By Admin!");
          }
        });
      });
  }

  //Send Activation Link when new User Added
  //Activation Button disable if it is Active Already
  sendAcivationLink() {
    const user = new UserId();
    user.UserId = this.id;
    this.userService.sendActivationLink(user).subscribe(
      response => {
        this.appNotificationService.success('Send Activation Link to Registered Mail!');
      },
      (error) => {

        //Error Handling
        this.apiErrorService.handleError(error, () => {
          if (error.error.message == "USER_ALREADY_ACTIVATED") {
            this.appNotificationService.error("User Already Activated!");
          }
          else if (error.error.message == "USER_NOT_ACVTD") {
            this.appNotificationService.error("User Not Activated!");
          }
          else {
            this.appNotificationService.error("User Failed To Activate!");
          }
        });
      });
  }

  cancel() {
    if (this.passdataservice.backpath.pathtype == PathType.WITHOUT_PARAM) {
      this.router.navigate([this.passdataservice.backpath.path]);
    } else {
      this.router.navigate([this.passdataservice.backpath.path, this.passdataservice.backpath.pathparams]);
    }
  }

  onSubmit() {
    var dataModel = new UserEditRequest();
    //Added data into data model
    dataModel.firstName = this.userAddForm.value.firstName;
    dataModel.lastName = this.userAddForm.value.lastName;
    dataModel.employeeId = this.userAddForm.value.employeeId;
    dataModel.eusr = this.userAddForm.value.eusr;
    dataModel.email = this.userAddForm.value.email;
    dataModel.contactNo = this.userAddForm.value.contactNo;
    if (this.userAddForm.get('isActive').value == 1) {
      dataModel.isActiveUser = true;
    }
    else {
      dataModel.isActiveUser = false;
    }

    //Call API for Role Adding Data
    this.userService.editUser(this.id, dataModel).subscribe(

      response => {
        this.router.navigate(['/portalManagment/userList']).then(this.appNotificationService.success('User Updated Successfully!'));
      },
      (error) => {

        //Error Handling
        this.apiErrorService.handleError(error, () => {
          if (error.error.message == "USER_EXISTS") {
            this.appNotificationService.error("User Already Exist!");
          }
          if (error.error.message == "UNASSIGN_MPRNs_BEFORE_DEACTIVATE") {
            this.appNotificationService.error("Unassing All The Assigned Jobs Before Deactivate The User!");
          }
        });
      });
  }
}






