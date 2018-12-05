import { Component, OnInit, Input } from '@angular/core';
import { IncidentIdrequest } from 'src/app/models/api/dashbord/incidentid-request';
import { ActivatedRoute } from '@angular/router';
import { IncidentService } from 'src/app/services/incident.service';
import { ApiErrorService } from 'src/app/services/api-error.service';
import { PassIncident } from 'src/app/models/api/dashbord/pass-incident';
import { PassdataService } from 'src/app/services/passdata.service';
import { Subscription } from 'rxjs/internal/Subscription';

@Component({
  selector: 'app-dashbord-container',
  templateUrl: './dashbord-container.component.html',
  styleUrls: ['./dashbord-container.component.css']
})
export class DashbordContainerComponent implements OnInit {
  incidentid;
  passIncident: PassIncident;
  fullscreen = false;
  isdataavailable: boolean;
  buttonlabel = 'Full';
  status: number;
  fullwidthsubscription: Subscription
  ispannelopened = false;
  constructor(
    private activatedroute: ActivatedRoute,
    private apiErrorService: ApiErrorService,
    private passdataservice: PassdataService,
    private incidentservice: IncidentService,
  ) { }

  ngOnInit() {
    console.log('received params');
    this.passIncident = new PassIncident();
    console.log(this.passIncident);
    this.passIncident.id = this.activatedroute.snapshot.params.id;
    if (this.passIncident.id) {
      this.isdataavailable = true;
    }
    this.passIncident.incidentid = this.activatedroute.snapshot.params.incidentid;
    this.status = this.activatedroute.snapshot.params.status;
    this.passIncident.status = this.activatedroute.snapshot.params.status;
    // console.log(this.incidentid);
    if (this.passIncident.incidentid == undefined || this.passIncident.incidentid == '') {
      this.getStatuswiseIncident();
    }
    this.fullwidthsubscription = this.passdataservice.callfullscreen.subscribe(() => {
      this.Fullscreen();
    });
  }// end of nginit

  // code block for getting statuswise incident list.
  getStatuswiseIncident(): void {
    this.incidentservice.getCurrentIncident().subscribe(
      incidentresponse => {
        console.log('current incident data success');
        if (incidentresponse && incidentresponse !== null) {
          this.passIncident.id = incidentresponse.id;
          this.status = incidentresponse.status;
          this.passIncident.incidentid = incidentresponse.incidentId;
          this.passIncident.status = incidentresponse.status;
          if (this.passIncident.id) {
            this.isdataavailable = true;
          }else{
            this.isdataavailable = false;
          }
          console.log('api received incident id');
          console.log(this.incidentid);
        }
      },
      (error) => {
        console.log('current  incident data fail');
        this.apiErrorService.handleError(error);
      });
  } // end of code block
  Fullscreen() {
    this.fullscreen = !this.fullscreen;
    this.buttonlabel = 'Normal';
    this.passdataservice.setFullscreenDiv(this.fullscreen);
  }
  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    if (this.fullwidthsubscription) {
      this.fullwidthsubscription.unsubscribe();
    }
  }
  openFullscreen() {

    let elem = document.getElementById('fullscreen');
    console.log('fulll screennnnnnnnnnnnnnnnnnnn');
    console.log(elem);
    let methodToBeInvoked = elem.requestFullscreen ||
      elem.webkitRequestFullScreen || elem['mozRequestFullscreen']
      ||
      elem['msRequestFullscreen'];
    if (methodToBeInvoked) methodToBeInvoked.call(elem);
    document.body.className = document.body.className.replace(' no-scroll', '');

  }

  onExpand() {
    this.ispannelopened = true;
  }


} // end of class
