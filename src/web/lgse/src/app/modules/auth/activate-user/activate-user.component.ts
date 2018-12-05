import { Component, OnInit, OnDestroy } from '@angular/core';
import { TranslateService } from '../../../pipes/translate/translate.service';
import { FormGroup, FormControl, Validators } from '../../../../../node_modules/@angular/forms';
import { UserService } from '../../../services/user.service';
import { ActivatedRoute, Router } from '../../../../../node_modules/@angular/router';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { LOGIN } from '../../../app-common-constants';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-activate-user',
  templateUrl: './activate-user.component.html',
  styleUrls: ['./activate-user.component.css']
})
export class ActivateUserComponent implements OnInit, OnDestroy {

  resetPasswordForm: FormGroup;
  isShow = false;
  resetPasswordSubscription: Subscription;
  otpExpiredSubscription: Subscription;
  forgotPasswordMsgSubscription: Subscription;

  constructor(private translate: TranslateService,
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router,
    private appNotificationService: AppNotificationService ) {
  }

  ngOnInit() {
    this.resetPasswordForm = new FormGroup({
      'resetData': new FormGroup({
        otp: new FormControl(null, Validators.required),
        workemail: new FormControl(null, Validators.required),
        newPassword: new FormControl(null, [Validators.required, Validators.pattern('^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$')]),
        confirmPassword: new FormControl(null, [Validators.required, Validators.pattern('^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$')])
      }, this.checkIfMatchingPasswords),
    });
    this.resetPasswordSubscription = this.userService.getResetPasswordSuccessMessage().subscribe(message => {
      this.appNotificationService.success(message);
      this.navigateToLogin();
    });
    this.otpExpiredSubscription = this.userService.getOtpExpiredMessage().subscribe(message => {
      if(confirm('OTP has expired, Generate a new OTP?')) {
        this.userService.forgotPassword(this.resetPasswordForm.value.resetData.workemail);
      }
    });
    this.forgotPasswordMsgSubscription = this.userService.getForgotPasswordSuccessMessage().subscribe(message => {
      this.appNotificationService.success("New OTP has beed sent to : "+this.resetPasswordForm.value.resetData.workemail);
    });
    this.route.queryParams.subscribe(params => {
      if(params['workemail'] && params['code']) { 
        this.isShow = true;
      } else {
        this.isShow = false;
      }
      this.resetPasswordForm.setValue({
        'resetData': {
          'otp': params['code'] || '',
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
    this.userService.resetPassword(otp, workEmail, password, '/Account/ActivateUser');
  }

  ngOnDestroy() {
    if(this.resetPasswordSubscription) {
      this.resetPasswordSubscription.unsubscribe();
    }
    if(this.otpExpiredSubscription) {
      this.otpExpiredSubscription.unsubscribe();
    }
    if(this.forgotPasswordMsgSubscription) {
      this.forgotPasswordMsgSubscription.unsubscribe();
    }
  }
}
