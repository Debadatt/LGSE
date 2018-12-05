import { Injectable } from '@angular/core';
import { ServerApiInterfaceService } from '../server-api-interface.service';
import { Observable} from 'rxjs';
import { DomainListResponse, DomainAddRequest, DomainEditRequest } from '../../models/api/portalManagment/domain.model';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginStatusProviderService } from '../login-status-provider.service';



@Injectable()
export class DomainService {
    constructor(
        private serverApiInterfaceService: ServerApiInterfaceService,
        private loginStatusProviderService: LoginStatusProviderService,
        private _http: HttpClient) {
    }

    //Get API's
    //Domain List
    getDomainList():
        Observable<DomainListResponse[]> {
        return this.serverApiInterfaceService.get(environment.baseurl + '/api/Domain?$orderby=CreatedAt desc');
    }

    //Edit Domain--Get Existing information of selected Domain
    getSelectedDomain(id):
        Observable<DomainListResponse[]> {
        return this.serverApiInterfaceService.get(environment.baseurl + '/tables/Domain/' + id);
    }

    //Post API's
    addDomain(domainAddRequest: DomainAddRequest):
        Observable<any> {
        return this.serverApiInterfaceService.post(environment.baseurl + '/api/Domain', domainAddRequest);
    }

    //Edit Domain
    editDomain(id, domainEditRequest: DomainEditRequest):
        Observable<any> {
        return this.serverApiInterfaceService.patch(environment.baseurl + '/api/Domain/' + id, domainEditRequest);
    }

}
