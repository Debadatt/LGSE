import { Injectable } from '@angular/core';
import { ServerApiInterfaceService } from './server-api-interface.service';
import { Observable } from 'rxjs';
import { IncidentListResponse, IncidentAddRequest, IncidentDataResponse, IncidentEditRequest } from '../models/api/incident.model';
import { CategoryListResponse } from '../models/api/category.model';
import { ProcessRequest } from '../models/api/process.request.model';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginStatusProviderService } from './login-status-provider.service';
import { SearchFilter } from 'src/app/models/search/custom-search-data';
import { ListingHelper } from '../shared/listing.helper';


@Injectable()
export class IncidentService {
    constructor(
        private loginStatusProviderService: LoginStatusProviderService,
        private serverApiInterfaceService: ServerApiInterfaceService,
        private _http: HttpClient) {
    }

    //Get API's
    //Incident List
    // getIncidentDataList():
    //     Observable<IncidentListResponse[]> {
    //     //return this.serverApiInterfaceService.get(environment.baseurl + '/tables/Incident?$top=10&$skip=0');
    //     // return this.serverApiInterfaceService.get(environment.baseurl + '/tables/Incident?$top='+top+'&$skip='+skip+'');
        
    //     return this.serverApiInterfaceService.get(environment.baseurl + '/tables/Incident?$orderby=IncidentId desc');
    // }
    getIncidentDataListForMPRN():
    Observable<IncidentListResponse[]> {
    return this.serverApiInterfaceService.get(environment.baseurl + '/tables/Incident?$filter=Status eq 0');
}
      getIncidentDataList(pageIndex, pageSize, sortby, sortDirection, filter: SearchFilter[]):
        Observable<IncidentListResponse[]> {
          console.log('filter array in service');
            console.log(filter);
            let path;
           // path = '/api/User?';
            path='/tables/Incident?'
            path += ListingHelper.pagination(pageSize, pageIndex);
            // if (filter.length > 0) {
            // path += ListingHelper.pagination(pageSize, pageIndex);
            // }else{
            //   path += ListingHelper.pagination(1, pageIndex);
            // }
            path += '&' + ListingHelper.sort(sortby, sortDirection);
            // path += '&$filter=IsLoggedIn eq true';
            //  path += '&$filter=IsLoggedIn eq true';
            //path = '/api/User?$inlinecount=allPages&$top=' + pageSize + '&$skip=' + (pageIndex * pageSize) + '&$orderby=' + sortby + ' ' + sortDirection + '&$filter=IsLoggedIn eq true';
            console.log('SearchFilter.length', filter.length);
            let pathWithFilters = '';
            if (filter.length > 0) {
              path += '&$filter=';
              for (let i = 0; i < filter.length; i++) {
                let pathFilterpart = '';
                switch (filter[i].searchkey) {
                  case 'Status':
                    if (filter[i].searchvalue == '0') {
                      pathFilterpart += "Status eq 0";
                    } else if (filter[i].searchvalue == '1') {
                      pathFilterpart += "Status eq 1";
                    }
                    else if (filter[i].searchvalue == '2') {
                      pathFilterpart += "Status eq 2";
                    }
                    break;
                  case 'Category':
                    // if (filter[i].searchvalue == '0') {
                    //   pathFilterpart += "Status eq 0";
                    // } else if (filter[i].searchvalue == '1') {
                    //   pathFilterpart += "Status eq 1";
                    // }
                    // else if (filter[i].searchvalue == '2') {
                    //   pathFilterpart += "Status eq 2";
                    // }
                    //if(filter[i].searchvalue==)
                    break;
                  default:
                    pathFilterpart += "substringof('" + filter[i].searchvalue + "'," + filter[i].searchkey + ")";
                    break;
                }
        
                if (pathWithFilters == '') {
                  pathWithFilters += pathFilterpart;
                }
                else {
                  pathWithFilters += ' and ' + pathFilterpart;
                }
              }
        
              path += pathWithFilters;
            }
            
            // this is the default filtering for this API, append only if required
            // if (pathWithFilters != '') {      
            //   path += ' and IsLoggedIn eq true';
            // }
            // else {
            //  path += '&$filter=' + 'IsLoggedIn eq true';
            // }
        
            return this.serverApiInterfaceService.get(environment.baseurl + path);




        //return this.serverApiInterfaceService.get(environment.baseurl + '/tables/Incident?$orderby=IncidentId desc');
    }
    checkAnyIncidentIsInProgress():
        Observable<boolean> {
            return this.serverApiInterfaceService.get(environment.baseurl + '/api/IncidentCustom/IsAnyIncidentInprogress');
    }

    //Category data
    getCategoryDataList():
        Observable<CategoryListResponse[]> {
        return this.serverApiInterfaceService.get(environment.baseurl + '/api/CategoriesMstr?$orderby=DisplayOrder');
    }

    //get Incident Data for Perticular Id
    getIncidentData(incidentId, implicitErrorHandling = true):
        Observable<IncidentListResponse[]> {
        return this.serverApiInterfaceService.get(environment.baseurl + '/api/Incident/' + incidentId);
    }

    //Post API's
    addIncident(incidentAddRequest: IncidentAddRequest):
        Observable<any> {
        return this.serverApiInterfaceService.post(environment.baseurl + '/api/IncidentCustom/Create', incidentAddRequest);
    }

    editIncident(incidentEditRequest: IncidentEditRequest):
        Observable<any> {
        return this.serverApiInterfaceService.post(environment.baseurl + '/api/IncidentCustom/Edit', incidentEditRequest);
    }

    deleteIncident(processRequest: ProcessRequest):
        Observable<any> {
        return this.serverApiInterfaceService.delete(environment.baseurl + '/api/Incident/' + processRequest.id);
    }
    getCurrentIncident():

    Observable<IncidentListResponse> {

    return this.serverApiInterfaceService.get(environment.baseurl + '/api/IncidentCustom/GetInprogressIncident');

  }
}
