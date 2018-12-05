import { Component, OnInit, Input } from '@angular/core';
import { colorSets, customColors } from '../colorset'
import { DashbordService } from 'src/app/services/dashbord/dashbord.service';
import { ApiErrorService } from 'src/app/services/api-error.service';
import { PassIncident } from 'src/app/models/api/dashbord/pass-incident';
import { PassdataService } from 'src/app/services/passdata.service';
import { Subscription } from 'rxjs/internal/Subscription';
import { IncidentIdrequest } from 'src/app/models/api/dashbord/incidentid-request';
import { UserwiseIncidentResponse, UserwiseIncidentChart, UserwiseIncidentValue, UserwiseChartrequest } from 'src/app/models/api/dashbord/user-wise-incident';


@Component({
  selector: 'app-user-level-average-time-taken',
  templateUrl: './user-level-average-time-taken.component.html',
  styleUrls: ['./user-level-average-time-taken.component.css']
})
export class UserLevelAverageTimeTakenComponent implements OnInit {
  multi: any[];
  @Input() Incident: PassIncident;
  cells: string[];
  filtervalue: string;
  userwiseIncidentResponse: UserwiseIncidentResponse[] = [];
  userwiseIncidentChart: UserwiseIncidentChart[];
  constructor(private passdataservice: PassdataService,
    private dashboardservice: DashbordService,
    private apiErrorService: ApiErrorService) {

  }

  // options
  customColors = customColors;
  showXAxis = true;
  showYAxis = true;
  gradient = false;
  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel = 'User';
  showYAxisLabel = true;
  yAxisLabel = 'Avg Time [Min]';

  colorScheme = {
    domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA']
  };

  ngOnInit() {
    this.getCells(); // fucntion call for getting cells of incident
  }
  // fucntion for getting cells of incident
  getCells() {
    this.dashboardservice.getCellsAndZones(this.Incident.id).subscribe(
      mprnresponse => {
        console.log('cell response success');
        this.cells = [];
        this.cells = mprnresponse.cells;
        if (this.cells.length > 0) {
          this.filtervalue = this.cells[0];
          this.userWiseStatus();
        }
      },
      (error) => {
        this.apiErrorService.handleError(error);
      });
  }
  // end of function.
  //fucntion for filter chart

  filterChart() {
    console.log('sellected cells', this.filtervalue);
    this.userWiseStatus();
  }
  // end of fucntion

  userWiseStatus() {
    const incident = new UserwiseChartrequest();
    incident.IncidentId = this.Incident.id;
    incident.CellName = this.filtervalue;
    this.dashboardservice.getUserwiseAvarageTimetaken(incident).subscribe(
      incidentresponse => {
        console.log('statuswise incident data success');
        this.userwiseIncidentResponse = incidentresponse;
        this.reformatJSONForChart();
      },
      (error) => {
        console.log('statuswise incident data fail');
        this.apiErrorService.handleError(error);
      });
  }

  // code block for format json for chart
  reformatJSONForChart(): void {
    this.userwiseIncidentChart = [];
    console.log('this.userwiseIncidentResponse');
    console.log(this.userwiseIncidentResponse);
    for (let i = 0; i < this.userwiseIncidentResponse.length; i++) {
      if (this.userwiseIncidentResponse[i].series && this.userwiseIncidentResponse[i].series.length > 0) {
        const data = new UserwiseIncidentChart();
        data.name = this.userwiseIncidentResponse[i].name;
        data.series = [];
        for (let j = 0; j < this.userwiseIncidentResponse[i].series.length; j++) {
          const zonechart = new UserwiseIncidentValue();
          zonechart.name = this.userwiseIncidentResponse[i].series[j].name;
          // if value wants in hour format then use follwomg fucntion.
          //  zonechart.value = this.valueFormatting(this.userwiseIncidentResponse[i].series[j].value);
          if (this.userwiseIncidentResponse[i].series[j].value > 0) {
            zonechart.value = Math.round(Math.round(this.userwiseIncidentResponse[i].series[j].value) / 60);
          } else {
            zonechart.value = Math.round(this.userwiseIncidentResponse[i].series[j].value);

          }
          data.series.push(zonechart);
        }
        this.userwiseIncidentChart.push(data);

      }

    }
    console.log('user-level-average-time-taken.component');
    console.log(this.userwiseIncidentChart);
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