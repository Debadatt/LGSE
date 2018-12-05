import { Component, OnInit, OnDestroy } from '@angular/core';
import { TranslateService } from '../../../pipes/translate/translate.service';
import { FormGroup, FormControl, Validators } from '../../../../../node_modules/@angular/forms';
import { UserService } from '../../../services/user.service';
import { DynamicWelcomeText } from '../../../services/dynamicwelcometext.service';
import { ActivatedRoute, Router } from '../../../../../node_modules/@angular/router';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { LOGIN } from '../../../app-common-constants';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit, OnDestroy {

  resetPasswordForm: FormGroup;
  isEmailPreset: boolean = false;
  resetPasswordSuccessMessageSubscription: Subscription;
  otpExpiredSubscription: Subscription;
  forgotPasswordMsgSubscription: Subscription;
  welcomeText:string;

  constructor(private translate: TranslateService,
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router,
    private dynamicWelcomeTextService:DynamicWelcomeText,
    private appNotificationService: AppNotificationService) { }

  ngOnInit() {
    this.resetPasswordForm = new FormGroup({
      'resetData': new FormGroup({
        otp: new FormControl(null, Validators.required),
        workemail: new FormControl(null, Validators.required),
        newPassword: new FormControl(null, [Validators.required, Validators.pattern('^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$')]),
        confirmPassword: new FormControl(null, [Validators.required, Validators.pattern('^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$')])
      }, this.checkIfMatchingPasswords),
    });
    this.dynamicWelcomeTextService.getDynamicWelcomeText().subscribe(DynmaicWelcomeTextResponse => {
      console.log("dynamic text response",DynmaicWelcomeTextResponse);
     if(DynmaicWelcomeTextResponse!=null)
     {
      this.welcomeText=DynmaicWelcomeTextResponse['description'];
     }
    });
    this.resetPasswordSuccessMessageSubscription = this.userService.getResetPasswordSuccessMessage().subscribe(message => {
      this.appNotificationService.success(message);
      this.navigateToLogin();
    });
    this.otpExpiredSubscription = this.userService.getOtpExpiredMessage().subscribe(message => {
      if (confirm('OTP has expired, Generate a new OTP?')) {
        this.userService.forgotPassword(this.resetPasswordForm.value.resetData.workemail);
      }
    });
    this.forgotPasswordMsgSubscription = this.userService.getForgotPasswordSuccessMessage().subscribe(message => {
      this.appNotificationService.success("New OTP has beed sent to : " + this.resetPasswordForm.value.resetData.workemail);
    });
    this.route.queryParams.subscribe(params => {
      if(params['workemail']) { 
        this.isEmailPreset = true;
      } else {
        this.isEmailPreset = false;
      }
      this.resetPasswordForm.setValue({
        'resetData': {
          'otp': '',
          'workemail': params['workemail'] || '',
          'newPassword': '',
          'confirmPassword': ''
        }
      });
    });
  }

  navigateToLogin() {
    setTimeout(() => {
      this.router.navigate([LOGIN]);
    }, 2 * 1000);
  }

  checkIfMatchingPasswords(g: FormGroup) {
    return g.get('newPassword').value === g.get('confirmPassword').value
      ? null : { 'mismatch': true };
  }

  onSendReset() {
    const otp = this.resetPasswordForm.value.resetData.otp;
    const workEmail = this.resetPasswordForm.value.resetData.workemail;
    const password = this.resetPasswordForm.value.resetData.newPassword;
    this.userService.resetPassword(otp, workEmail, password, '/Account/ForgotPassword');
  }

  ngOnDestroy() {
    if(this.resetPasswordSuccessMessageSubscription) {
      this.resetPasswordSuccessMessageSubscription.unsubscribe();
    }
    if(this.otpExpiredSubscription) {
      this.otpExpiredSubscription.unsubscribe();
    }
    if(this.forgotPasswordMsgSubscription) {
      this.forgotPasswordMsgSubscription.unsubscribe();
    }
  }
}
