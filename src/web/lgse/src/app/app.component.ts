import { Component, OnInit, NgModule } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { MatInputModule, MatTableModule, MatToolbarModule, MatPaginatorModule, MatMenuModule, MatSidenavModule, MatSlideToggleModule } from '@angular/material';
import { LoginStatusProviderService } from './services/login-status-provider.service';
import { ActivatedRoute } from '@angular/router';
declare var $: any;
// import {} from ' ../../../src/assets/';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
@NgModule({
  imports: [

    MatInputModule, MatTableModule, MatToolbarModule, MatPaginatorModule, MatMenuModule, MatSidenavModule, MatSlideToggleModule
  ],
  exports: [
    MatInputModule, MatTableModule, MatToolbarModule, MatPaginatorModule, MatMenuModule, MatSidenavModule, MatSlideToggleModule
  ]
})

export class AppComponent implements OnInit {


  title = 'lgse';

  constructor(
    private router: Router,
    private loginStatusProviderService: LoginStatusProviderService,
    private translateService: TranslateService,
    private route: ActivatedRoute
  ) {

    translateService.setDefaultLang('en-US');
    translateService.use('en-US');

    // if (this.loginStatusProviderService.isLoggedIn === true) {
    //   this.router.navigate(['']);
    // }
    // else {
    //   this.router.navigate(['/auth/login']);
    // }
    // initialize app
    +function ($, window) {
      'use strict';
      window.app.init();
      window.app.menubar.init();
      window.app.navbar.init();
      window.app.customizer.init();
    };

  }
  ngOnInit() {
  }

}
