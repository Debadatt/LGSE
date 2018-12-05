import { Component, OnInit, Input } from '@angular/core';
import { colorSets, customColors } from '../colorset'
import { ApiErrorService } from 'src/app/services/api-error.service';
import { DashbordService } from 'src/app/services/dashbord/dashbord.service';
import { IncidentIdrequest } from '../../../models/api/dashbord/incidentid-request';
import { StatuswiseIncidentResponse, StatuswiseIncidentChart } from 'src/app/models/api/dashbord/statuswise-incident-chart';
import { PassIncident } from '../../../models/api/dashbord/pass-incident';
import { IncidentStatus } from 'src/app/app-common-constants';
import { PassdataService } from 'src/app/services/passdata.service';
import { HostListener } from '@angular/core';
import { Subscription } from 'rxjs/internal/Subscription';
import { Router } from '@angular/router';
import { filter } from 'rxjs/operators';
import { NavigationEnd } from '@angular/router';
@Component({
  selector: 'app-mprn-status-count',
  templateUrl: './mprn-status-count.component.html',
  styleUrls: ['./mprn-status-count.component.css']
})
export class MprnStatusCountComponent implements OnInit {

  single: any[];
  multi: any[];
  fullscreen = false;
  interval: any;
  incidentStatus = IncidentStatus;
  statuswiseIncidentResponse: StatuswiseIncidentResponse[] = [];
  statuswiseIncidentChart: StatuswiseIncidentChart[];
  status: number;
  rountersubscription: Subscription;
  @Input() Incident: PassIncident;
  constructor(
    private apiErrorService: ApiErrorService,
    private router: Router,
    private passdataservice: PassdataService,
    private dashbordService: DashbordService) {
    // Object.assign(this, {single, multi})   
  } // end of constructor

  // ng init 
  ngOnInit() {
    console.log('input received to status componeent');
    console.log('Incidentid', this.Incident.id);
    this.status = this.Incident.status;
    console.log('received ststus', this.status);
    setTimeout(() => {    //<<<---    using ()=> syntax
      this.getStatuswiseIncident();
    }, 500);
  }// end of ng  init
  color: {
    domain: ['#e8b200', '#60fe00', '#b8453a']
  }
  customColors = customColors;

  // colorScheme = {
  //   domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA']
  // };
  colorScheme = colorSets.filter(colorname => colorname.name == 'vivid')[0];


  onSelect(event) {
    console.log(event);
  } // end 

  // code block for getting statuswise incident list.
  getStatuswiseIncident(): void {
    const incident = new IncidentIdrequest();
    incident.IncidentId = this.Incident.id;
    this.statuswiseIncidentChart = [];
    this.dashbordService.getStatuswiseIncidentChartData(incident).subscribe(
      incidentresponse => {
        console.log('statuswise incident data success');
        this.statuswiseIncidentResponse = [];
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
      data.value = this.statuswiseIncidentResponse[i].count;
      this.statuswiseIncidentChart.push(data);
      console.log('this.statuswiseIncidentChart');
      console.log(this.statuswiseIncidentChart);

    }

  }// end  of function


  openFullscreen() {
    this.fullscreen = !this.fullscreen;
    const test = this.statuswiseIncidentChart;
    this.statuswiseIncidentChart = [];
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
      this.statuswiseIncidentChart = test;
    }, 100);



    this.passdataservice.setFullscreen(this.fullscreen);
    //   let elem = document.getElementById('fullscreen');
    //   console.log('fulll screennnnnnnnnnnnnnnnnnnn');
    //   console.log(elem);
    //   let methodToBeInvoked = elem.requestFullscreen ||
    //     elem.webkitRequestFullScreen || elem['mozRequestFullscreen']
    //     ||
    //     elem['msRequestFullscreen'];
    //   if (methodToBeInvoked) methodToBeInvoked.call(elem);

    //   document.addEventListener("fullscreenChange", function () {
    //     if (methodToBeInvoked != null) {
    //         alert("Went full screen");
    //     } else {
    //       alert("Exited full screen");              
    //     }
    // });
  }


  exitHandler() {
    // if (!document.fullscreenElement && !document.webkitIsFullScreen && !document.m && !document.msFullscreenElement) {
    //     ///fire your event
    //     this.fullscreen
    // }
  } 
  ngOnDestroy(): void {
    if (this.fullscreen) {
      this.fullscreen = false;
      clearInterval(this.interval);
      this.passdataservice.setFullscreen(this.fullscreen);
    }
 
  }
}
