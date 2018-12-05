import { Component, OnInit, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserService } from '../../../services/user.service';
import { DynamicWelcomeText } from '../../../services/dynamicwelcometext.service';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { ActivatedRoute, Router } from '../../../../../node_modules/@angular/router';
import { RESETPASSWORD } from '../../../app-common-constants';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit, OnDestroy {

  forgotPasswordForm: FormGroup;
  forgotPasswordMsgSubscription: Subscription;
  welcomeText:string;

  constructor(private translate: TranslateService,
    private userService: UserService,
    private appNotificationService: AppNotificationService,
    private dynamicWelcomeTextService:DynamicWelcomeText,
    private router: Router) { }

  ngOnInit() {
    this.forgotPasswordForm = new FormGroup({
      'workemail': new FormControl(null, [Validators.required, Validators.email])
    });
    this.dynamicWelcomeTextService.getDynamicWelcomeText().subscribe(DynmaicWelcomeTextResponse => {
      console.log("dynamic text response",DynmaicWelcomeTextResponse);
     if(DynmaicWelcomeTextResponse!=null)
     {
      this.welcomeText=DynmaicWelcomeTextResponse['description'];
     }
    });

    this.forgotPasswordMsgSubscription = this.userService.getForgotPasswordSuccessMessage().subscribe(message => {
      this.appNotificationService.success(message);
      this.goToResetPasswordPage(this.forgotPasswordForm.value.workemail);
    });
  }

  onSendReset() {
    this.userService.forgotPassword(this.forgotPasswordForm.value.workemail);
  }

  goToResetPasswordPage(email: string) {
    setTimeout(() => {
      this.router.navigate([RESETPASSWORD], { queryParams: { 'workemail': email } });
    }, 2 * 1000);
  }

  ngOnDestroy() {
    if (this.forgotPasswordMsgSubscription) {
      this.forgotPasswordMsgSubscription.unsubscribe();
    }
  }

}
