import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { TranslatePipe } from '../pipes/translate/translate.pipe';
import { LoginStatusProviderService } from './login-status-provider.service';
import { AppNotificationService } from './notification/app-notification.service';
import { PayloadResponse } from '../models/api/payload-models';
//import { SERVER_SYSTEM_ERROR_MAX_BOUNDARY } from '../app-common-constants';
import { TranslateService } from '@ngx-translate/core';


@Injectable()
export class ApiErrorService {
    constructor(private appNotificationService: AppNotificationService,
        private router: Router,
        private loginStatusProviderService: LoginStatusProviderService,
        private translatePipe: TranslatePipe,
        private translateService: TranslateService) { }

    handleError(error, customErrorFunction?) {
        console.error('error', error); // log to console instead
        switch (error.status) {
            case 401:
                this.loginStatusProviderService.changeLoginStatus(false);
                this.translateService.get('RESPONSE-CODES.' + 'HTTP_ERROR_401').subscribe((res: string) => {
                    this.router.navigate(['/auth/login']).then( this.appNotificationService.error(res));
                 });               
                break;
            case 403:
                this.translateService.get('RESPONSE-CODES.' + 'HTTP_ERROR_403').subscribe((res: string) => {
                    this.appNotificationService.error(res);
                });
                break;
            case 404:
                this.translateService.get('RESPONSE-CODES.' + 'HTTP_ERROR_404').subscribe((res: string) => {
                    this.appNotificationService.error(res);
                });
            case 400:
                this.applicationErrorHandler(error, customErrorFunction);
                break;
            default:
                this.translateService.get('RESPONSE-CODES.' + 'SERVER_ERROR_UNKNOWN').subscribe((res: string) => {
                    this.appNotificationService.error(res);
                });
                break;
        }
    }

    applicationErrorHandler(error, customErrorFunction) {
        if (customErrorFunction) {
            customErrorFunction(error);
        }
        else {
            console.log('response code', error.error.message);
            this.translateService.get('RESPONSE-CODES.' + error.error.message).subscribe((errorMessage: string) => {
                if (errorMessage && errorMessage != '') {
                    this.appNotificationService.error(errorMessage);
                }
                else {
                    this.translateService.get('RESPONSE-CODES.' + 'SERVER_ERROR_UNKNOWN').subscribe((res: string) => {
                        this.appNotificationService.error(res);
                    });
                }
            });
        }
    }
}
