import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AuthRoutingModule } from './auth-routing.module';
import { LoginComponent } from './login/login.component';
import { AppCommonModule } from '../../app-common.module';
import { UserRegisterComponent } from './user-register/user-register.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { NewPasswordComponent } from './new-password/new-password.component';
import { SelectRoleComponent } from './select-role/select-role.component';
import { TranslateModule } from '@ngx-translate/core';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { ActivateUserComponent } from './activate-user/activate-user.component';
import { TermsAndConditionsComponent } from './user-register/terms-and-conditions/terms-and-conditions.component';
import { MatDialogModule } from '@angular/material';

@NgModule({
  imports: [
    CommonModule,
    AuthRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    AppCommonModule, 
    TranslateModule,
    MatDialogModule
  ],
  declarations: [
    LoginComponent,
    UserRegisterComponent,
    ForgotPasswordComponent,
    NewPasswordComponent,
    SelectRoleComponent,
    ResetPasswordComponent,
    ActivateUserComponent,
    TermsAndConditionsComponent
  ],
  providers: [ ],
  entryComponents: [TermsAndConditionsComponent]
})
export class AuthModule { }
