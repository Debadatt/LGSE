import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NavigationStart, NavigationEnd, NavigationError, NavigationCancel, RoutesRecognized } from '@angular/router';
import { Subscription } from 'rxjs/internal/Subscription';
import { filter } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
import { Breadcrumbs } from 'src/app/models/breadcrumbs-struct';
import { UserService } from 'src/app/services/user.service';
import { LoginStatusProviderService } from 'src/app/services/login-status-provider.service';
import { concat } from 'rxjs/internal/operators/concat';
import { assignedproperties } from 'src/app/app-common-constants';
@Component({
  selector: 'app-top-header',
  templateUrl: './top-header.component.html',
  styleUrls: ['./top-header.component.css']
})
export class TopHeaderComponent implements OnInit {
  toggleOn: boolean = false;
  styles = 'hidden-lg hidden-md hidden-xs';
  rountersubscription: Subscription;
  breadcrumbs: Breadcrumbs[] = [];
  breadcrumbsmap: Map<string, string>;
  selectedmodule = '';
  logginStatuschangedsubscription: Subscription;
  styles1;
  username;
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private userService: UserService,
    private loginstatusservice: LoginStatusProviderService
  ) {
  }
  ngOnInit() {
    this.logginStatuschangedsubscription = this.loginstatusservice.logginStatusChanged.subscribe((value) => {
      if (!value) {
        this.logout(false);
      }
    });
    this.breadcrumbs.push({ key: 'dashboard', value: 'Dashboard' });
    this.breadcrumbs.push({ key: 'resources', value: 'Resource Management' });
    this.breadcrumbs.push({ key: 'incident', value: 'Incident Management' });
    this.breadcrumbs.push({ key: 'portalManagment', value: 'Portal Management' });
    this.breadcrumbs.push({ key: '/incident/showProperties', value: 'Assigned Properties' });
    this.breadcrumbsmap = new Map(this.breadcrumbs.map(x => [x.key, x.value] as [string, string]));

    this.rountersubscription = this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((val) => {
      this.setBreadCrum();
    });


    this.getUserTaskCount();
    this.setBreadCrum();
  }

  setBreadCrum() {
    const unformatedurl = <string>this.activatedRoute.root.snapshot['_routerState'].url;
    console.log('breadcum url');
    console.log(unformatedurl);
    let newurl = unformatedurl.replace(/\//g, ' ');
    let routelist = newurl.split(' ');
    if (routelist.length > 0 && routelist) {
      console.log('route list');
      console.log(routelist);
      if (unformatedurl == assignedproperties) {
        this.selectedmodule = this.breadcrumbsmap.get(assignedproperties);
      } else {
        this.selectedmodule = this.breadcrumbsmap.get(routelist[1]);
      }
    }
  }

  removeCss() {
    if (this.toggleOn === true) {
      this.styles = 'hidden-lg hidden-md hidden-xs';
      this.styles1 = '';
      this.toggleOn = false;
    }
    else {
      this.styles = ''
      this.styles1 = 'img-responsive hidden-sm';
      this.toggleOn = true;
    }
  }


  logout(islogout: boolean) {
    // alert('log out clicked')
    this.userService.Logout(islogout);
  }
  getUserTaskCount() {

  }
  test() {
    alert('test');
  }
  ngOnDestroy() {
    this.rountersubscription.unsubscribe();
    if (this.logginStatuschangedsubscription) { this.logginStatuschangedsubscription.unsubscribe(); }
  }

}
