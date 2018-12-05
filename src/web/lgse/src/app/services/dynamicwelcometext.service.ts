import { Injectable } from '@angular/core';
import { ServerApiInterfaceService } from './server-api-interface.service';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginStatusProviderService } from './login-status-provider.service';
import { GetDynamicWelcomeTextResponse, WelcomeTextAddRequest } from '../models/api/dynamic.welcometext.model';


@Injectable()
export class DynamicWelcomeText {
    constructor(
        private loginStatusProviderService: LoginStatusProviderService,
        private serverApiInterfaceService: ServerApiInterfaceService,
        private _http: HttpClient) {
    }


    //Getting Dynamic Welcome Text for Login Screen 
    getDynamicWelcomeText():
        Observable<GetDynamicWelcomeTextResponse[]> {
        return this.serverApiInterfaceService.get(environment.baseurl + '/api/IncidentOverviewMstr');
    }

    //Getting Dynamic Welcome Text for Login Screen 
    getWelcomeText():
        Observable<GetDynamicWelcomeTextResponse[]> {
        return this.serverApiInterfaceService.get(environment.baseurl + '/api/IncidentOverviewMstr/E5D1C5AE-6CD1-4961-8692-3521433383F6');
    }
    //Add Welcome Text
    updateWelcomeText(welcomeTextAddRequest: WelcomeTextAddRequest, id):
        Observable<any> {
        return this.serverApiInterfaceService.patch(environment.baseurl + '/api/IncidentOverviewMstr/' + id, welcomeTextAddRequest);
    }


}
