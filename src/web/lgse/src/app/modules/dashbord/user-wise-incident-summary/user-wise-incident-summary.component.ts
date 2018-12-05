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
  selector: 'app-user-wise-incident-summary',
  templateUrl: './user-wise-incident-summary.component.html',
  styleUrls: ['./user-wise-incident-summary.component.css']
})
export class UserWiseIncidentSummaryComponent implements OnInit {
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
  xAxisLabel = 'Users';
  showYAxisLabel = true;
  yAxisLabel = 'Status';

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
    this.dashboardservice.getUserwiseIncidentChartData(incident).subscribe(
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
          zonechart.value = this.userwiseIncidentResponse[i].series[j].value;
          data.series.push(zonechart);
        }
        this.userwiseIncidentChart.push(data);

      }

    }
  console.log('incidentwise chart');
  console.log(this.userwiseIncidentChart);
  }// end  of function
}
