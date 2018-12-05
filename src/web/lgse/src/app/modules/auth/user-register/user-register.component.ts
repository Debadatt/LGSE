import { Component, OnInit, OnDestroy,ViewChild,ElementRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material';

import { TranslateService } from '@ngx-translate/core';
import { Register } from '../../../models/api/register.model';
import { UserService } from '../../../services/user.service';
import { DynamicWelcomeText } from '../../../services/dynamicwelcometext.service';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { Router } from '@angular/router';
import { ACTIVATE } from '../../../app-common-constants';
import { Subscription } from 'rxjs';
import { TermsAndConditionsComponent } from './terms-and-conditions/terms-and-conditions.component';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css'],
})
export class UserRegisterComponent implements OnInit, OnDestroy {

  welcomeText:string;
  registerForm: FormGroup;
  formRoles = [];
  mobnumPattern = "^(\\+)?[0-9]*";
  isEngineer = false;
  roleSubscription: Subscription;
  registerSuccessMessageSubscription: Subscription;
  
  // @ViewChild('role') role: ElementRef;
  constructor(private translate: TranslateService,
    private userService: UserService,
    private appNotificationService: AppNotificationService,
    private dynamicWelcomeTextService:DynamicWelcomeText,
    private router: Router,
    private dialog: MatDialog
  ) { }

  ngOnInit() {
    this.registerForm = new FormGroup({
      'registerData': new FormGroup({
        'workemail': new FormControl(null, [Validators.required, Validators.email]),
        'firstname': new FormControl(null, [Validators.required, Validators.maxLength(20), Validators.minLength(1), Validators.pattern('[a-zA-Z ]*')]),
        'lastname': new FormControl(null, [Validators.required, Validators.maxLength(20), Validators.minLength(1), Validators.pattern('[a-zA-Z ]*')]),
        'role': new FormControl(null, Validators.required),
        'employeeid': new FormControl(null, []),
        'eusr': new FormControl(null, [Validators.maxLength(9), Validators.minLength(1),Validators.pattern('[0-9]*$')]),
        'telnumber': new FormControl(null, [Validators.minLength(10), Validators.maxLength(13), Validators.pattern(this.mobnumPattern)]),
        'isAcceptTandC': new FormControl(false, Validators.pattern('true'))
      }),
    });
 
    this.dynamicWelcomeTextService.getDynamicWelcomeText().subscribe(DynmaicWelcomeTextResponse => {
      console.log("dynamic text response",DynmaicWelcomeTextResponse);
     if(DynmaicWelcomeTextResponse!=null)
     {
      this.welcomeText=DynmaicWelcomeTextResponse['description'];
     }
    });


    this.registerFormControlValueChanged();
    this.registerSuccessMessageSubscription = this.userService.getRegisterSuccessMessage().subscribe(message => {
      this.appNotificationService.success(message);
      this.navigateToActivatePage(this.registerForm.value.registerData.workemail);
    });
    this.userService.getRoles();
    this.roleSubscription = this.userService.getAllRoles().subscribe(roles => {
      for (let role of roles) {
        // need to be romoved after 
        if (role.roleName== 'Isolator' || role.roleName == 'Engineer') {
          this.formRoles.push({
            id: role.id,
            name: role.roleName
          });
        }
      }
    });
  }

  registerFormControlValueChanged() {
    this.registerForm.get('registerData').get('role').valueChanges.subscribe(
      (selRole: string) => {
        const eusrControl = this.registerForm.get('registerData').get('eusr');
        let selRoleName = this.getRoleFromID(selRole);
        if (selRoleName === 'Engineer') {
          eusrControl.setValidators([Validators.required, Validators.minLength(1), Validators.maxLength(9), Validators.pattern('[0-9]*$')]);
        } else {
          eusrControl.clearValidators();
          eusrControl.setValidators([Validators.minLength(1), Validators.maxLength(9), Validators.pattern('[0-9]*$')]);
        }
        eusrControl.updateValueAndValidity();
      }
    );

  }

  onSubmit() {
    if (this.getRoleFromID(this.registerForm.value.registerData.role) === 'Engineer') {
      if (this.registerForm.value.registerData.eusr) {
        this.submitRegistrationRequest();
      } else {
        this.showError();
      }
    } else {
      this.submitRegistrationRequest();
    }
  }

  private showError() {
    this.translate.get('REGISTER.valid.euser').subscribe((res: string) => {
      this.appNotificationService.error(res);
    });
  }

  private submitRegistrationRequest() {
    const registerData: Register = {
      "Email": this.registerForm.value.registerData.workemail,
      "FirstName": this.registerForm.value.registerData.firstname,
      "LastName": this.registerForm.value.registerData.lastname,
      "EmployeeId": this.registerForm.value.registerData.employeeid,
      "RoleId": this.registerForm.value.registerData.role,
      "EUSR": this.registerForm.value.registerData.eusr,
      "ContactNo": this.registerForm.value.registerData.telnumber,
    };
    this.userService.register(registerData);
  }

  navigateToActivatePage(email: string) {
    setTimeout(() => {
      this.router.navigate([ACTIVATE], { queryParams: { 'workemail': email } });
    }, 2 * 1000);
  }

  onSelectRole(roleId) {
    let selRoleName = this.getRoleFromID(roleId);
    if (selRoleName === 'Engineer') {
      this.isEngineer = true;
    } else {
      this.isEngineer = false;
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

  openTandCDialog() {
    this.dialog.open(TermsAndConditionsComponent);
  }

  ngOnDestroy() {
    if (this.registerSuccessMessageSubscription) {
      this.registerSuccessMessageSubscription.unsubscribe();
    }
    if (this.roleSubscription) {
      this.roleSubscription.unsubscribe();
    }
  }

}


