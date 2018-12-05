

import { Injectable } from '@angular/core';
import { ServerApiInterfaceService } from '../server-api-interface.service';
import { Observable } from 'rxjs';
import { PayloadResponse } from '../../models/api/payload-models';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginStatusProviderService } from '../login-status-provider.service';
import { GetAllResourcesResponse } from '../../models/api/resources/get-all-resources-response';
import { RESOURCES_TYPE } from '../../app-common-constants';
import { MPRNAssignmentUnassignment } from '../../models/api/resources/assign-unassign-mprn-request';
import { AssignedMPRNResponse } from '../../models/api/resources/assigned-mprn-response';
import { GetActiveMPRnList } from '../../models/api/resources/get-active-mprn-response';
import { MprnhistoryResponse } from 'src/app/models/api/resources/mprn-history-response';
import { SearchFilter } from 'src/app/models/search/custom-search-data';
import { ListingHelper } from '../../shared/listing.helper';

@Injectable()
export class ResourcesServiceService {

  constructor(
    private loginStatusProviderService: LoginStatusProviderService,
    private serverApiInterfaceService: ServerApiInterfaceService,
    private _http: HttpClient
  ) { }
  // service code block for getting rresources
  getResourceList(pageIndex, pageSize, sortby, sortDirection, filter: SearchFilter[]):
    Observable<GetAllResourcesResponse[]> {
    // based on selected categosy user data will fetch
    console.log('filter array in service');
    console.log(filter);
    let path;
    path = '/api/User?';
    path += ListingHelper.pagination(pageSize, pageIndex);
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
          case 'IsAssigned':
            if (filter[i].searchvalue == '1') {
              pathFilterpart += "AssignedMPRNCount gt 0";
            } else if (filter[i].searchvalue == '2') {
              pathFilterpart += "AssignedMPRNCount eq 0";
            }
            break;
          case 'Role':
            if (filter[i].searchvalue == '0') {
              pathFilterpart += "PreferredRole eq 'Engineer'";
            } else if (filter[i].searchvalue == '1') {
              pathFilterpart += "PreferredRole eq 'Isolator'";
            }
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
    if (pathWithFilters != '') {
      path += " and IsLoggedIn eq true and (PreferredRole eq 'Engineer' or PreferredRole eq 'Isolator')";
    }
    else {
      path += '&$filter=' + "IsLoggedIn eq true and (PreferredRole eq 'Engineer' or PreferredRole eq 'Isolator')";
    }

    return this.serverApiInterfaceService.getResources(environment.baseurl + path);
  }
  //end of code block
  // service fucntion for get assigned resourecs list 
  getAssinedResourceList(): Observable<GetAllResourcesResponse[]> {
    return this.serverApiInterfaceService.get(environment.baseurl + '/api/User?$filter=IsAssigned eq  true');
  }
  // end of code block
  assignUnassignresources(mprnassignmentUnassignment: MPRNAssignmentUnassignment[]):
    Observable<any> {
    return this.serverApiInterfaceService.post(environment.baseurl + '/api/IncidentCustom/MPRNsAssignment', mprnassignmentUnassignment);
  } // service fuction for assign and unassign mprns
  // service fucntion for get assigned MPRN list
  getAssinedMPRNList(email: string, roleid: string): Observable<AssignedMPRNResponse[]> {
    return this.serverApiInterfaceService.get(environment.baseurl + '/api/IncidentCustom/GetUserMPRNS?userEmail=' + email.trim() + "&roleid=" + roleid);
  } // end of fucntion

  // service function for getting active mprns
  getActiveMPRNList(incidentid, pageIndex, pageSize, sortby, sortDirection, filter: SearchFilter[]): Observable<GetActiveMPRnList[]> {

    console.log("incidentid-----", incidentid);
    console.log('filter array in service');
    console.log(filter);
    let path;
    // path = '/api/User?';
    path = '/api/Property?'

    path += ListingHelper.pagination(pageSize, pageIndex);
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
          case 'PriorityCustomer':
            if (filter[i].searchvalue == '0') {
              pathFilterpart += "PriorityCustomer eq true";
            } else if (filter[i].searchvalue == '1') {
              pathFilterpart += "PriorityCustomer eq false";
            }
            break;
          case 'AssignedResourceCount':
            if (filter[i].searchvalue == '0') {
              pathFilterpart += "AssignedResourceCount gt 0";
            } else if (filter[i].searchvalue == '1') {
              pathFilterpart += "AssignedResourceCount eq 0";
            }
            break;
          case 'LatestStatus':
            console.log("LatestStatus:", filter[i].searchvalue);
            if (filter[i].searchvalue == 'null') {
              pathFilterpart += "LatestStatus eq " + filter[i].searchvalue;
            }
            else {
              pathFilterpart += "LatestStatus eq '" + filter[i].searchvalue + "'";
            }
            break;
          case 'Cell':
            pathFilterpart += "Cell eq '" + filter[i].searchvalue+"'";
            break;
            case 'Zone':
            pathFilterpart += "Zone eq '" + filter[i].searchvalue+"'";
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

    if (pathWithFilters != '') {
      path += " and LatestStatus ne 'Restored' and Status ne 1 and Deleted eq false and IncidentId eq " + "'" + incidentid + "'";

    }
    else {

      path += "&$filter=Status ne 1 and LatestStatus ne 'Restored' and Deleted eq false and IncidentId eq " + "'" + incidentid + "'";


    }
    console.log("environment.baseurl + path", environment.baseurl + path);
    return this.serverApiInterfaceService.getProperty(environment.baseurl + path, '1');
    //return this.serverApiInterfaceService.get(environment.baseurl + "/api/Property?$filter=Status ne 1 and Deleted eq false and IncidentId eq'" + incidentid + "'");
  } // end of fucntions

  // service fucntion for get assigned resources to specific mprn
  getResourceListofMPRN(propertyid: string):
    Observable<GetAllResourcesResponse[]> {
    return this.serverApiInterfaceService.get(environment.baseurl + '/api/IncidentCustom/GetMPRNResources?PropertyId=' + propertyid.trim());
  }
  //end of fucntion.

  // service fucntion for get  MPRN list History
  getMPRNHistoryList(PropertyId: string): Observable<MprnhistoryResponse[]> {
    return this.serverApiInterfaceService.get(environment.baseurl + '/api/IncidentCustom/MPRNStatusHistory?propertyId=' + PropertyId.trim());
  } // end of fucntion.

}// end of service fucntion
