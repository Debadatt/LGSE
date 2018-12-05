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
  selector: 'app-zone-level-average-time-taken',
  templateUrl: './zone-level-average-time-takan.component.html',
  styleUrls: ['./zone-level-average-time-takan.component.css']
})
export class ZoneLevelAverageTimeTakenComponent implements OnInit {
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
  showLegend = true;
  filtervalue: string;
  showXAxisLabel = true;
  xAxisLabel = 'Zones';
  showYAxisLabel = true;
  barMaxWidth = 12;
  yAxisLabel = 'Avg Time [Min]';

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
    this.fullscreensubject = this.passdataservice.fullscreensubject.subscribe(() => {
      this.zonewiseIncidentChartFilter = [];
      setTimeout(() => {    //<<<---    using ()=> syntax
        this.zonewiseIncidentChartFilter = this.zonewiseIncidentChart;
      }, 100);
    });
  }

  // code block for getting statuswise incident list.
  getStatuswiseIncident(): void {
    const incident = new IncidentIdrequest();
    incident.IncidentId = this.Incident.id;
    this.dashbordService.getZoneLevelAvarageTimetaken(incident).subscribe(
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
          // if result wants in hour format then use folllowing function.
          // zonechart.value = this.valueFormatting(this.zonewiseIncidentResponse[i].series[j].value);
          if (this.zonewiseIncidentResponse[i].series[j].value > 0) {
            zonechart.value = Math.round(Math.round(this.zonewiseIncidentResponse[i].series[j].value) / 60);
          } else {
            zonechart.value = Math.round(this.zonewiseIncidentResponse[i].series[j].value);
          }
          data.series.push(zonechart);
        }
        this.zonewiseIncidentChart.push(data);
      }

    }
    //this.multi = this.zonewiseIncidentChart;
    this.zonewiseIncidentChartFilter = this.zonewiseIncidentChart;
    console.log('zonewise time taken');
    console.log(this.zonewiseIncidentChartFilter)
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
}// end of class

export class DataFileter {
  name: string;
  value: string;
}