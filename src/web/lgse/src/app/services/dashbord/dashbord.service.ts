import { Injectable } from '@angular/core';
import { ServerApiInterfaceService } from '../server-api-interface.service';
import { Observable } from 'rxjs';
import { PayloadResponse } from '../../models/api/payload-models';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginStatusProviderService } from '../login-status-provider.service';
import { GetAllResourcesResponse } from '../../models/api/resources/get-all-resources-response';
import { IncidentIdrequest, EngineeringCapacityRequest } from '../../models/api/dashbord/incidentid-request';
import { StatuswiseIncidentResponse, IncidentlevelavgtimeTaken } from '../../models/api/dashbord/statuswise-incident-chart';
import { ZonewiseIncidentResponse } from 'src/app/models/api/dashbord/zonewise-incident';
import { CellwiseIncidentResponse } from 'src/app/models/api/dashbord/cellwise-incident-chart';
import { ZoneCells } from 'src/app/models/api/dashbord/get-cells-zones-response';
import { UserwiseIncidentResponse, UserwiseChartrequest } from 'src/app/models/api/dashbord/user-wise-incident';
import { EngineeringCapacity } from 'src/app/models/api/dashbord/engineering-capacity';


@Injectable({
  providedIn: 'root'
})
export class DashbordService {

  constructor(
    private loginStatusProviderService: LoginStatusProviderService,
    private serverApiInterfaceService: ServerApiInterfaceService,
    private _http: HttpClient
  ) { }
  // service fucntion for satuswise dashbord chart
  getStatuswiseIncidentChartData(incidentrequest: IncidentIdrequest):
    Observable<StatuswiseIncidentResponse[]> {
    return this.serverApiInterfaceService.post(environment.baseurl + '/api/Report/IncidentStatus', incidentrequest);
  }// end of fucntions.

  // service fucntion for zonewise dashbord chart
  getZonewiseIncidentChartData(incidentrequest: IncidentIdrequest):
    Observable<ZonewiseIncidentResponse[]> {
    return this.serverApiInterfaceService.post(environment.baseurl + '/api/Report/IncidentStatusByZone', incidentrequest);
  }// end of fucntions.

  // service fucntion for zonewise dashbord chart
  getCellwiseIncidentChartData(incidentrequest: IncidentIdrequest):
    Observable<CellwiseIncidentResponse[]> {
    return this.serverApiInterfaceService.post(environment.baseurl + '/api/Report/IncidentStatusByCell', incidentrequest);
  }// end of fucntions.

  // service fucntion for user wise  dashbord chart
  getCellsAndZones(incidentid):
    Observable<ZoneCells> {
    return this.serverApiInterfaceService.get(environment.baseurl + '/api/Incident/' + incidentid);
  }// end of fucntions.

  // service fucntion for userwise dashbord chart
  getUserwiseIncidentChartData(userwiseChartrequest: UserwiseChartrequest):
    Observable<UserwiseIncidentResponse[]> {
    return this.serverApiInterfaceService.post(environment.baseurl + '/api/Report/UserWiseStatus', userwiseChartrequest);
  }// end of fucntions.

   // service fucntion for satuswise dashbord chart
   getincidentLevelAvaragetimeTaken(incidentrequest: IncidentIdrequest):
   Observable<IncidentlevelavgtimeTaken[]> {
   return this.serverApiInterfaceService.post(environment.baseurl + '/api/Report/AvgStatusTimeAtIncident', incidentrequest);
 }// end of fucntions.

   // service fucntion for zonewise avrage time taken dashbord chart
   getZoneLevelAvarageTimetaken(incidentrequest: IncidentIdrequest):
   Observable<ZonewiseIncidentResponse[]> {
   return this.serverApiInterfaceService.post(environment.baseurl + '/api/Report/AvgStatusTimeByZone', incidentrequest);
 }// end of fucntions.
  // service fucntion for zonewise dashbord chart
  getCellLevelAvarageTimeTaken(incidentrequest: IncidentIdrequest):
    Observable<CellwiseIncidentResponse[]> {
    return this.serverApiInterfaceService.post(environment.baseurl + '/api/Report/AvgStatusTimeByCell', incidentrequest);
  }// end of fucntions.
  
  // service fucntion for getting engineering capacity reportdata.
  getEngineeringCapacity(incidentrequest: EngineeringCapacityRequest):
    Observable<EngineeringCapacity[]> {
    return this.serverApiInterfaceService.post(environment.baseurl + '/api/Report/EngineeringCapacity',incidentrequest);
  }// end of fucntions.

  // service fucntion for userwise dashbord chart
  getUserwiseAvarageTimetaken(userwiseChartrequest: UserwiseChartrequest):
    Observable<UserwiseIncidentResponse[]> {
    return this.serverApiInterfaceService.post(environment.baseurl + '/api/Report/AvgStatusTimeTakenByUserLevel', userwiseChartrequest);
  }// end of fucntions.

  // download engineering capacity reports csv
  downloadCsv(incident:EngineeringCapacityRequest): any {
    return this.serverApiInterfaceService.download(environment.baseurl + '/api/Report/DownLoadEngineeringCapacity?IncidentId='+incident.IncidentId+'&Type='+incident.Type+'&value=' + incident.Value);  
  }
}
