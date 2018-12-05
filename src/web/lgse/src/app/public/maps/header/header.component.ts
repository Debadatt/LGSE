import { Component, OnInit, OnDestroy } from '@angular/core';
import { MapsService } from 'src/app/services/maps.service';
import { TranslateService } from 'src/app/pipes/translate/translate.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, OnDestroy {

  publicInfoSubscription: Subscription;
  countOfPropertiesAffected; 
  startTime;
  incidentId;
  status;
  completedProperties;

  constructor(private mapsService: MapsService,
    private translate: TranslateService) { }

  ngOnInit() {
    this.publicInfoSubscription = this.mapsService.getPublicInfoObervable().subscribe(publicInfo => {
        this.countOfPropertiesAffected = publicInfo.noOfPropsAffected;
        this.startTime = publicInfo.startTime;
        this.incidentId = publicInfo.incidentId;
        this.status = this.getStatusToDisplay(publicInfo.status);
        this.completedProperties = publicInfo.noOfPropsCompleted;
    });
  }

  private getStatusToDisplay(status: number) {
    if (status === 0) {
      return 'In Progress';
    }
    if (status === 1) {
      return 'Complete';
    }
    if (status === 2) {
      return 'Canceled';
    }
  }

  ngOnDestroy() { 
    if(this.publicInfoSubscription) {
      this.publicInfoSubscription.unsubscribe();
    }
  }

}
