import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule,Component } from '@angular/core';
import { TranslateModule, TranslateLoader  } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';

import { AppRoutingModule } from './app-routing.module';
import { LayoutModule } from './layouts/layout.module';
import { LoginStatusProviderService } from './services/login-status-provider.service';
import { AppCommonModule } from './app-common.module';
import { TranslateService } from './pipes/translate/translate.service';
import { ToastrModule } from 'ngx-toastr';
import { IncidentService } from './services/incident.service';
import { PropertiesService } from './services/properties.service';
import { DynamicWelcomeText } from './services/dynamicwelcometext.service';
import { ServerApiInterfaceService } from './services/server-api-interface.service';
import { ApiErrorService } from './services/api-error.service';

import { CustomSearchComponent } from 'src/app/custom/custom-search/custom-search.component';
import { LocalstorageService } from 'src/app/services/localstorage.service';
import { FacebookComponent } from './public/facebook/facebook.component';
import { TwitterComponent } from './public/twitter/twitter.component';
import { MapsComponent } from './public/maps/maps.component';
import { HeaderComponent } from './public/maps/header/header.component';
import { NoAccessComponent } from 'src/app/custom/no-access/no-access.component';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    AppComponent,
    FacebookComponent,
    TwitterComponent,
    MapsComponent,
    HeaderComponent,
    NoAccessComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AppCommonModule.forRoot(),
    LayoutModule, 
    HttpModule,
    HttpClientModule,
    BrowserAnimationsModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    ToastrModule.forRoot({
      closeButton: true,
      timeOut: 15000,
      extendedTimeOut: 5000,
      progressBar: true,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      tapToDismiss: false
    }),
  ],
  exports: [
    TranslateModule,
    CustomSearchComponent
  ],
  providers: [
    LoginStatusProviderService,
    LocalstorageService,
    TranslateService,
    IncidentService,
    DynamicWelcomeText,
    ServerApiInterfaceService,
    ApiErrorService,
    PropertiesService

  ],
  bootstrap: [AppComponent],
  
})

export class AppModule { }