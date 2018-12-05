import { Component, OnInit, Input } from '@angular/core';
import { colorSets, customColors } from '../colorset'
import { ZonewiseIncident, ZonewiseIncidentResponse, ZonewiseIncidentChart, ZonewiseIncidentValue } from 'src/app/models/api/dashbord/zonewise-incident';
import { DashbordService } from 'src/app/services/dashbord/dashbord.service';
import { ApiErrorService } from 'src/app/services/api-error.service';
import { IncidentIdrequest } from 'src/app/models/api/dashbord/incidentid-request';
import { CellwiseIncidentChart, CellwiseIncidentResponse } from 'src/app/models/api/dashbord/cellwise-incident-chart';
import { PassIncident } from 'src/app/models/api/dashbord/pass-incident';
import { PassdataService } from 'src/app/services/passdata.service';
import { Subscription } from 'rxjs/internal/Subscription';
import { StatuswiseIncidentResponse, StatuswiseIncidentChart, IncidentlevelavgtimeTaken } from 'src/app/models/api/dashbord/statuswise-incident-chart';
import { PipeTransform, Pipe } from '@angular/core';
@Component({
  selector: 'app-incident-level-average-time-taken',
  templateUrl: './incident-level-average-time-taken.component.html',
  styleUrls: ['./incident-level-average-time-taken.component.css']
})
export class IncidentLevelAverageTimeTakenComponent implements OnInit {
  @Input() Incident: PassIncident;
  single: any[];
  multi: any[];
  statuswiseIncidentResponse: IncidentlevelavgtimeTaken[] = [];
  statuswiseIncidentChart: StatuswiseIncidentChart[];
  customColors = customColors;
  constructor(
    private apiErrorService: ApiErrorService,
    private dashbordService: DashbordService) { }
  ngOnInit() {
    setTimeout(() => {    //<<<---    using ()=> syntax
      this.getStatuswiseIncident();
    }, 500);
  } // end of nginit

  // code block for getting statuswise incident list.
  getStatuswiseIncident(): void {
    const incident = new IncidentIdrequest();
    incident.IncidentId = this.Incident.id;
    this.dashbordService.getincidentLevelAvaragetimeTaken(incident).subscribe(
      incidentresponse => {
        console.log('statuswise incident data success');
        this.statuswiseIncidentResponse = incidentresponse;
        this.reformatJSONForChart();
      },
      (error) => {
        console.log('statuswise incident data fail');
        this.apiErrorService.handleError(error);
      });
  } // end of code block

  // code block for format json for chart
  reformatJSONForChart(): void {
    this.statuswiseIncidentChart = [];
    for (let i = 0; i < this.statuswiseIncidentResponse.length; i++) {
      const data = new StatuswiseIncidentChart();
      data.name = this.statuswiseIncidentResponse[i].name;
      if (this.statuswiseIncidentResponse[i].value > 0) {
        this.statuswiseIncidentResponse[i].value = Math.round(this.statuswiseIncidentResponse[i].value / 60);
      }
      data.value = Math.round(this.statuswiseIncidentResponse[i].value);

      this.statuswiseIncidentChart.push(data);

    }
   

  }// end  of function

  valueFormatting(d) {
    if (d > 0) {
      var h = Math.floor(d / 3600);
      var m = Math.floor(d % 3600 / 60);
      var s = Math.floor(d % 3600 % 60);
      //return h;
      return Number(('0' + h).slice(-2) + "." + ('0' + m).slice(-2));
    } else {
      return d
    }
  }
}
