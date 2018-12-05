import { Component, OnInit, OnDestroy } from '@angular/core';
import { TranslateService } from '../../../pipes/translate/translate.service';
import { UserService } from '../../../services/user.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { Router } from '@angular/router';
import { LOGIN } from '../../../app-common-constants';
import { Subscription } from 'rxjs';
import { DynamicWelcomeText } from '../../../services/dynamicwelcometext.service';

@Component({
  selector: 'app-new-password',
  templateUrl: './new-password.component.html',
  styleUrls: ['./new-password.component.css']
})
export class NewPasswordComponent implements OnInit, OnDestroy {

  resetPasswordForm: FormGroup;
  changePasswordSuccessMsgSubscription: Subscription;
  welcomeText:string;
  constructor(private translate: TranslateService,
  private userService: UserService,
  private appNotificationService: AppNotificationService,
  private dynamicWelcomeTextService:DynamicWelcomeText,
  private router: Router) { }

  ngOnInit() {
    this.resetPasswordForm = new FormGroup({
      oldPassword: new FormControl(null, [Validators.required]),
      newPassword: new FormControl(null, [Validators.required, Validators.pattern('^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$')]),
      confirmPassword: new FormControl(null, [Validators.required, Validators.pattern('^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$')])
    }, this.checkIfMatchingPasswords);
    
    this.resetPasswordForm.value.oldPassword="";
    this.dynamicWelcomeTextService.getDynamicWelcomeText().subscribe(DynmaicWelcomeTextResponse => {
      console.log("dynamic text response",DynmaicWelcomeTextResponse);
     if(DynmaicWelcomeTextResponse!=null)
     {
      this.welcomeText=DynmaicWelcomeTextResponse['description'];
     }
    });
   this.changePasswordSuccessMsgSubscription = this.userService.getChangePasswordSuccessMessage().subscribe(msg => {
    this.appNotificationService.success(msg);
    this.navigateToIncident();
   });
  }

  checkIfMatchingPasswords(g: FormGroup) {
    return g.get('newPassword').value === g.get('confirmPassword').value
      ? null : { 'mismatch': true };
  }

  onResetPassword(){
    const oldPassword = this.resetPasswordForm.value.oldPassword;
    const newPassword = this.resetPasswordForm.value.newPassword;
    this.userService.setNewPassword(oldPassword, newPassword);
  }

  ngOnDestroy() {
    if(this.changePasswordSuccessMsgSubscription) {
      this.changePasswordSuccessMsgSubscription.unsubscribe();
    }
  }

  navigateToIncident() {
    setTimeout(() => {
      this.router.navigate([LOGIN]);
    }, 2 * 1000);
  }
}
