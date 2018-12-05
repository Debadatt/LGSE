import { Injectable } from '@angular/core';
import { ServerApiInterfaceService } from './server-api-interface.service';
import { Observable } from 'rxjs';
import { PropertyListResponse, AssignedMprnResponse, StausListResponse, SubStausListResponse, UpdateMPRNStatusRequest } from '../models/api/properties.model';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginStatusProviderService } from './login-status-provider.service';
import { SearchFilter } from 'src/app/models/search/custom-search-data';
import { ListingHelper } from '../shared/listing.helper';
import { Angular5Csv } from '../../../node_modules/angular5-csv/Angular5-csv';

@Injectable()
export class PropertiesService {
  constructor(
    private loginStatusProviderService: LoginStatusProviderService,
    private serverApiInterfaceService: ServerApiInterfaceService,
    private _http: HttpClient) {
  }
  getAssignedMprn(pageIndex, pageSize, sortby, sortDirection, filter: SearchFilter[], implicitErrorHandling = true):
    Observable<AssignedMprnResponse[]> {
    console.log("in get assigned MPRN");
    console.log("PageIndex", pageIndex);
    console.log("pageSize", pageSize);
    console.log("sortby", sortby);
    console.log("sortDirection", sortDirection);
    console.log("filter", filter);
    let path;
    path = '/tables/Property?'
    //path='/api/IncidentCustom/Properties?'

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
          case 'LatestStatus':
            if (filter[i].searchvalue == 'null') {
              pathFilterpart += "LatestStatus eq " + filter[i].searchvalue;
            }
            else {
              pathFilterpart += "LatestStatus eq '" + filter[i].searchvalue + "'";
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
      path += ' and Deleted eq false and IsStatusUpdated eq false and IsUnassigned eq false ';
    }
    else {
      path += '&$filter=Deleted eq false and IsStatusUpdated eq false and IsUnassigned eq false';
    }
    console.log("assignedMPRN", environment.baseurl + path);
    return this.serverApiInterfaceService.getProperty(environment.baseurl + path, '0');
  }
  //Get All Properties for Selected Incident
  getPropertyDataList(receivedId, pageIndex, pageSize, sortby, sortDirection, filter: SearchFilter[], implicitErrorHandling = true):
    Observable<PropertyListResponse[]> {
    console.log('filter array in service');
    console.log(filter);
    let path;

    path = '/tables/Property?'
    //path='/api/IncidentCustom/Properties?'
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
          case 'LatestStatus':
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
      path += ' and Deleted eq false and IncidentId eq ' + receivedId;
    }
    else {
      path += '&$filter=Deleted eq false &$filter=' + 'IncidentId eq ' + receivedId;
    }
    return this.serverApiInterfaceService.getProperty(environment.baseurl + path, '1');
  }

  //Get MPRN Status
  getStatusDataList():
    Observable<StausListResponse[]> {
    return this.serverApiInterfaceService.get(environment.baseurl + '/api/PropertyStatusMstr?$filter=Deleted eq false &$orderby=DisplayOrder');
  }

  //Get MPRN Sub Status
  getSubStatusDataList(statusId):
    Observable<SubStausListResponse[]> {
    var url = environment.baseurl + '/api/PropertySubStatusMstr?$filter=PropertyStatusMstrsId eq ';
    return this.serverApiInterfaceService.get(url + statusId.trim() + ' and Deleted eq false');
  }

  //Getting data for MPRN and its status,substatus and Notes
  getMprnData(mprnId):
    Observable<PropertyListResponse[]> {
    return this.serverApiInterfaceService.get(environment.baseurl + '/api/Property/' + mprnId);
  }

  //Update MPRN Status,Substatus and Notes
  updateMprnStatus(updateMPRNStatusRequest: UpdateMPRNStatusRequest):
    Observable<any> {
    return this.serverApiInterfaceService.post(environment.baseurl + '/api/IncidentCustom/PropertyUserStatus', updateMPRNStatusRequest);
  }

  downloadCsv(incidentId): any {
    return this.serverApiInterfaceService.download(environment.baseurl + '/api/IncidentCustom/DownloadMPRN?incidentId=' + incidentId)
  }
}
