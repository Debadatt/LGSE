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


@Component({
  selector: 'app-cellwise-incident-status',
  templateUrl: './cellwise-incident-status.component.html',
  styleUrls: ['./cellwise-incident-status.component.css']
})
export class CellwiseIncidentStatusComponent implements OnInit {
  cellwiseIncidentResponse: CellwiseIncidentResponse[] = [];
  cellwiseIncidentChart: CellwiseIncidentChart[];
  cellwiseIncidentChartFilter: CellwiseIncidentChart[];
  single: any[];
  multi: any[];
  dataFileter: DataFileter[] = [];
  filtervalue: string;
  fullscreensubject: Subscription;;
  @Input() Incident: PassIncident;
  ngOnInit() {
    this.filtervalue = 'all';
    // this.multi=[{"name":"Cell1","series":[{"name":"Isolated","value":34940000},{"name":"Restored","value":66940000},{"name":"No Gas","value":8940000},{"name":"No Connection","value":6940000}]},{"name":"Cell2","series":[{"name":"Isolated","value":300000},{"name":"Restored","value":3300000},{"name":"No Gas","value":3940000},{"name":"No Connection","value":43940000}]},{"name":"Cell3","series":[{"name":"Isolated","value":34300000},{"name":"Restored","value":34300000},{"name":"No Gas","value":43940000},{"name":"No Connection","value":34940000}]}];
    this.getCellwiseIncident();
    this.fullscreensubject = this.passdataservice.fullscreendivsubject.subscribe(() => {
      this.cellwiseIncidentChartFilter = [];
      setTimeout(() => {    //<<<---    using ()=> syntax
        this.cellwiseIncidentChartFilter = this.cellwiseIncidentChart;
      }, 100);
    });
  }
  constructor(private dashbordService: DashbordService,
    private passdataservice: PassdataService,
    private apiErrorService: ApiErrorService) {
    // Object.assign(this, {single, multi})   

  }
  // options
  customColors = customColors;
  showXAxis = true;
  showYAxis = true;
  gradient = false;
  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel = 'Cells';
  showYAxisLabel = true;
  yAxisLabel = 'Status';

  colorScheme = {
    domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA']
  };

  // code block for getting statuswise incident list.
  getCellwiseIncident(): void {
    const incident = new IncidentIdrequest();
    incident.IncidentId = this.Incident.id;
    this.dashbordService.getCellwiseIncidentChartData(incident).subscribe(
      incidentresponse => {
        console.log('statuswise incident data success');
        this.cellwiseIncidentResponse = incidentresponse;
        this.reformatJSONForChart();
      },
      (error) => {
        console.log('statuswise incident data fail');
        this.apiErrorService.handleError(error);
      });
  } // end of code block

  // code block for format json for chart
  reformatJSONForChart(): void {
    this.cellwiseIncidentChart = [];
    this.dataFileter = [];
    this.cellwiseIncidentChartFilter = [];
    for (let i = 0; i < this.cellwiseIncidentResponse.length; i++) {
      if (this.cellwiseIncidentResponse[i].series && this.cellwiseIncidentResponse[i].series.length > 0) {
        const data = new ZonewiseIncidentChart();
        data.name = this.cellwiseIncidentResponse[i].name;
        this.dataFileter.push({ name: data.name, value: data.name });
        data.series = [];
        for (let j = 0; j < this.cellwiseIncidentResponse[i].series.length; j++) {
          const zonechart = new ZonewiseIncidentValue();
          zonechart.name = this.cellwiseIncidentResponse[i].series[j].name;
          zonechart.value = this.cellwiseIncidentResponse[i].series[j].count;
          data.series.push(zonechart);
        }
        this.cellwiseIncidentChart.push(data);

      }

    }
    this.cellwiseIncidentChartFilter = this.cellwiseIncidentChart;
  }// end  of function
  filterChart() {
    console.log('this.filtervalue', this.filtervalue);
    this.cellwiseIncidentChartFilter = this.cellwiseIncidentChart;
    if (this.filtervalue !== 'all') {
      this.cellwiseIncidentChartFilter = this.cellwiseIncidentChart.filter(zone => zone.name == this.filtervalue);
    }
  }
  ngOnDestroy(): void {
    if (this.fullscreensubject) {
      this.fullscreensubject.unsubscribe();
    }
  }
  // fucntion for set chart as fullwidth
  fullWidthChart() {
    this.passdataservice.setFullWidth();
  }
}
export class DataFileter {
  name: string;
  value: string;
}