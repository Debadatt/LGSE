import { Component, OnInit, Input } from '@angular/core';
import { colorSets, customColors } from '../colorset'
import { ZonewiseIncident, ZonewiseIncidentResponse, ZonewiseIncidentChart, ZonewiseIncidentValue } from 'src/app/models/api/dashbord/zonewise-incident';
import { DashbordService } from 'src/app/services/dashbord/dashbord.service';
import { ApiErrorService } from 'src/app/services/api-error.service';
import { IncidentIdrequest } from 'src/app/models/api/dashbord/incidentid-request';
import { PassIncident } from 'src/app/models/api/dashbord/pass-incident';
import { conditionallyCreateMapObjectLiteral } from '@angular/compiler/src/render3/view/util';
import { Subscription } from 'rxjs/internal/Subscription';
import { PassdataService } from 'src/app/services/passdata.service';

@Component({
  selector: 'app-zonewise-incident-status',
  templateUrl: './zonewise-incident-status.component.html',
  styleUrls: ['./zonewise-incident-status.component.css']
})
export class ZonewiseIncidentStatusComponent implements OnInit {
  @Input() Incident: PassIncident;
  single: any[];
  multi: any[];
  zonewiseIncidentResponse: ZonewiseIncidentResponse[] = [];
  zonewiseIncidentChart: ZonewiseIncidentChart[];
  zonewiseIncidentChartFilter: ZonewiseIncidentChart[];
  fullscreensubject: Subscription;
  showXAxis = true;
  showYAxis = true;
  gradient = false;
  fullscreen = false;
  showLegend = true;
  filtervalue: string;
  showXAxisLabel = true;
  xAxisLabel = 'Zones';
  showYAxisLabel = true;
  barMaxWidth = 12;
  yAxisLabel = 'Status';
  interval: any;
  modelfullscreen = false;

  view: any[] = [550, 400];
  customColors = customColors;
  dataFileter: DataFileter[] = [];
  // colorScheme = {
  //   domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA']
  // };
  colorScheme = colorSets.filter(colorname => colorname.name == 'vivid')[0];
  constructor(private dashbordService: DashbordService,
    private passdataservice: PassdataService,
    private apiErrorService: ApiErrorService) {
    // Object.assign(this, {single, multi})   

  }

  onSelect(event) {
    console.log(event);
  }

  ngOnInit() {
    this.getStatuswiseIncident();
    this.filtervalue = 'all';
    this.fullscreensubject = this.passdataservice.fullscreendivsubject.subscribe(() => {
      this.zonewiseIncidentChartFilter = [];
      setTimeout(() => {    //<<<---    using ()=> syntax
        this.zonewiseIncidentChartFilter = this.zonewiseIncidentChart;
      }, 100);
    });
  }

  // code block for getting statuswise incident list.
  getStatuswiseIncident(): void {
    this.zonewiseIncidentChart = [];
    const incident = new IncidentIdrequest();
    incident.IncidentId = this.Incident.id;
    this.dashbordService.getZonewiseIncidentChartData(incident).subscribe(
      incidentresponse => {
        console.log('statuswise incident data success');
        this.zonewiseIncidentResponse = incidentresponse;
        this.reformatJSONForChart();
      },
      (error) => {
        console.log('statuswise incident data fail');
        this.apiErrorService.handleError(error);
      });
  } // end of code block

  // code block for format json for chart
  reformatJSONForChart(): void {
    this.zonewiseIncidentChart = [];
    this.dataFileter = [];
    for (let i = 0; i < this.zonewiseIncidentResponse.length; i++) {
      if (this.zonewiseIncidentResponse[i].series && this.zonewiseIncidentResponse[i].series.length > 0) {
        const data = new ZonewiseIncidentChart();
        data.name = this.zonewiseIncidentResponse[i].name;
        this.dataFileter.push({ name: data.name, value: data.name });
        data.series = [];
        for (let j = 0; j < this.zonewiseIncidentResponse[i].series.length; j++) {
          const zonechart = new ZonewiseIncidentValue();
          zonechart.name = this.zonewiseIncidentResponse[i].series[j].name;
          zonechart.value = this.zonewiseIncidentResponse[i].series[j].count;
          data.series.push(zonechart);
        }
        this.zonewiseIncidentChart.push(data);

      }

    }
    //this.multi = this.zonewiseIncidentChart;
    this.zonewiseIncidentChartFilter = this.zonewiseIncidentChart;
  }// end  of function

  filterChart() {
    console.log('this.filtervalue', this.filtervalue);
    this.zonewiseIncidentChartFilter = this.zonewiseIncidentChart;
    if (this.filtervalue !== 'all') {
      this.zonewiseIncidentChartFilter = this.zonewiseIncidentChartFilter.filter(zone => zone.name == this.filtervalue);
    }
  }
  ngOnDestroy(): void {
    if (this.fullscreensubject) {
      this.fullscreensubject.unsubscribe();
    }
  }
  fullWidthChart() {
    this.passdataservice.setFullWidth();
  }

  openFullscreen() {
    this.fullscreen = !this.fullscreen;
    const test = this.zonewiseIncidentChartFilter;
    this.zonewiseIncidentChartFilter = [];
    if (this.fullscreen) {
      this.interval = setInterval(() => {
        this.getStatuswiseIncident();
      }, 30000);
      console.log('interval innter', this.interval);
    } else {
      console.log('interval', this.interval);
      clearInterval(this.interval);

    }
    setTimeout(() => {
      this.zonewiseIncidentChartFilter = test;
    }, 100);
    this.passdataservice.setFullscreen(this.fullscreen);

  }


}// end of class

export class DataFileter {
  name: string;
  value: string;
}
