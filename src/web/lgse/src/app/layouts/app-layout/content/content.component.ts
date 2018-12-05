

import { Component, OnInit } from '@angular/core';
import { PassdataService } from 'src/app/services/passdata.service';
import { Subscription } from 'rxjs/internal/Subscription';
import { validateVerticalPosition } from '@angular/cdk/overlay';

@Component({
  selector: 'app-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.css']
})
export class ContentComponent implements OnInit {

  showClass = 'list-unstyled collapse show';
  collapseClass = 'list-unstyled collapse';

  // Portal Mangement Elements
  ariaExpand = false;
  showCollapseClass = 'list-unstyled collapse';
  isfullscreensubscription: Subscription;
  classmian: string;
  classsub: string;
  isfullscreen = false;
  //Resource Management Elements
  ariaExpandRM = false;
  showCollapseClassRM = 'list-unstyled collapse';

  constructor(private passdataservice: PassdataService) { }

  ngOnInit() {
    this.changeClasses();
    this.isfullscreensubscription = this.passdataservice.fullscreensubject.subscribe((value) => {
      this.isfullscreen = value;    
      this.changeClasses();
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

  ngOnDestroy(): void {
    if (this.isfullscreensubscription) { this.isfullscreensubscription.unsubscribe(); }
  }
  // fcuntiion for changing classes
  changeClasses(): void {
    if (this.isfullscreen) {
     document.body.style.overflow = 'hidden';
      this.classmian = 'test'
      this.classsub = 'test';
    } else {
     // alert('in else part');
     document.body.style.overflow = 'visible';  // firefox, chrome
      this.classmian = 'app-main in'
      this.classsub = 'wrap in';
    }
  }
}
