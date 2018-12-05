
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RolesService } from '../../../../services/roles/roles.service';
 import { UserService } from '../../../../services/user.service';
 import { UserAddRequest } from '../../../../models/api/user.model';
import { ApiErrorService } from '../../../../services/api-error.service';
import { AppNotificationService } from '../../../../services/notification/app-notification.service';
import { FeatureNames } from 'src/app/app-common-constants';
import { LocalstorageService } from 'src/app/services/localstorage.service';

@Component({
  selector: 'app-user-addEdit',
  templateUrl: './user-addEdit.component.html',
  styleUrls: ['./user-addEdit.component.css']
})

export class UserAddEditComponent implements OnInit {

  //Variable Declaration
  userAddForm: FormGroup;
  formRoles = [];
  mobnumPattern = "^(\\+)?[0-9]*";
  isEngineer = false;
  feature = FeatureNames;
  roleName:string;


  constructor(private router: Router,
    private userService: UserService,
    private rolesService: RolesService,
    private apiErrorService: ApiErrorService,
    private appNotificationService: AppNotificationService,
    public localstorageservice: LocalstorageService
  ) { }

  ngOnInit() {
    this.userAddForm = new FormGroup({
      'firstName': new FormControl(null, [Validators.required, Validators.maxLength(20), Validators.minLength(1), Validators.pattern('[a-zA-Z ]*')]),
      'lastName': new FormControl(null, [Validators.required, Validators.maxLength(20), Validators.minLength(1), Validators.pattern('[a-zA-Z ]*')]),
      'employeeId': new FormControl(null),
      'eusr': new FormControl(null, [Validators.maxLength(9), Validators.minLength(1),Validators.pattern('[0-9]*$')]),
      'email': new FormControl(null, [Validators.required, Validators.email]),
      'contactNo': new FormControl(null, [ Validators.minLength(10), Validators.maxLength(13), Validators.pattern(this.mobnumPattern)]),
      'role': new FormControl(null, [Validators.required]),
    });
    this.getRoleList();
  }

  getRoleList() {
    this.rolesService.getRoleDataList().subscribe(RoleListResponse => {
      for (let role of RoleListResponse) {
        this.formRoles.push({
          id: role.id,
          name: role.roleName
        });
      }
    });
  }
  
  //On Role Section this method will get called
  selectChangeHandler(roleName) {
    //If Engineer Role- EUSR Field is mandatory
    //For Isolator Role-EUSR Filed is not mandatory
    this.roleName=roleName;
    const eusrControl = this.userAddForm.get('eusr');
    if (this.roleName === 'Engineer') {
      eusrControl.setValidators([Validators.required, Validators.minLength(1), Validators.maxLength(9), Validators.pattern('[0-9]*$')]);
    } else {
      eusrControl.clearValidators();
      eusrControl.setValidators([Validators.minLength(1), Validators.maxLength(9), Validators.pattern('[0-9]*$')]);
    }
    eusrControl.updateValueAndValidity();
  }
  
  onSubmit() {
    var dataModel = new UserAddRequest();

    //Added data into data model
    dataModel.firstName = this.userAddForm.value.firstName;
    dataModel.lastName = this.userAddForm.value.lastName;
    dataModel.employeeId = this.userAddForm.value.employeeId;
    dataModel.eusr = this.userAddForm.value.eusr;
    dataModel.email = this.userAddForm.value.email;
    dataModel.contactNo = this.userAddForm.value.contactNo;
    dataModel.roleId = this.userAddForm.value.role;
    dataModel.IsActiveUser = true;

    //Call API for Role Adding Data
    this.userService.addUser(dataModel).subscribe(
      response => {
        this.router.navigate(['/portalManagment/userList']).then(this.appNotificationService.success('User Created Successfully!'));
      },
      (error) => {
        //Error Handling
        this.apiErrorService.handleError(error, () => {
          if (error.error.message == "USER_EXISTS") {
            this.appNotificationService.error("User Already Exist!");
          }
          else if (error.error.message == "USER_NOT_ALLOWED") {
            this.appNotificationService.error("Domain Not Exist!");
          }
          else if (error.error.message == "USER_EXISTS_NOT_ACTIVATED") {
            this.appNotificationService.error("User Exist But Not Activated Yet!");
          } else if(error.error.message == "DOMAIN_IS_INACTIVE"){
            this.appNotificationService.error("Domain Is Inactive");
          }
        });
      });
  }
  }



 
  
  
  