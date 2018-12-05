import { Component, OnInit } from '@angular/core';
import { PassdataService } from 'src/app/services/passdata.service';
import { Subscription } from 'rxjs/internal/Subscription';

@Component({
  selector: 'app-app-container',
  templateUrl: './app-container.component.html',
  styleUrls: ['./app-container.component.css']
})
export class AppContainerComponent implements OnInit {
  showClass = 'list-unstyled collapse show';
  collapseClass = 'list-unstyled collapse';
  isfullscreensubscription: Subscription;
  isfullscreen = false;
  // Portal Mangement Elements
  ariaExpand = false;
  showCollapseClass = 'list-unstyled collapse';

  //Resource Management Elements
  ariaExpandRM = false;
  showCollapseClassRM = 'list-unstyled collapse';
  constructor(private passdataservice: PassdataService) { }

  ngOnInit() {
    this.isfullscreensubscription = this.passdataservice.fullscreensubject.subscribe((value) => {
      this.isfullscreen = value; 
    });
  }
  togglePortalMgmt() {
    this.ariaExpand = !this.ariaExpand;
    this.ariaExpand ? this.showCollapseClass = this.showClass : this.showCollapseClass = this.collapseClass;
  }

  toggleResourceMgmt() {
    this.ariaExpandRM = !this.ariaExpandRM;
    this.ariaExpandRM ? this.showCollapseClassRM = this.showClass : this.showCollapseClassRM = this.collapseClass;
  }

}
