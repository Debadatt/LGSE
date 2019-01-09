import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IncidentRoutingModule } from './incident-routing.module';
import { IncidentComponent } from './incident.component';
import { IncidentAddComponent } from './incident-add/incident-add.component';
import { IncidentEditComponent } from './incident-edit/incident-edit.component';
import { IncidentListComponent } from './incident-list/incident-list.component';
import { AppCommonModule } from '../../app-common.module';
import { IncidentShowPropertiesComponent } from './incident-show-properties/incident-show-properties.component';
import { IncidentUpdateStatusComponent } from './incident-update-status/incident-update-status.component';
import { IncidentService } from '../../services/incident.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { ShowMapComponent } from './show-map/show-map.component';
import { BingMapComponent } from 'src/app/modules/incident/bing-map/bing-map.component';
import { PapaParseModule } from 'ngx-papaparse';

@NgModule({
  imports: [
    CommonModule,
    IncidentRoutingModule,
    AppCommonModule,
    CommonModule,
    ReactiveFormsModule,
    PapaParseModule,
    TranslateModule,
    FormsModule,
    
  ],
  providers: [
    IncidentService
  ],
  declarations: [ 
    BingMapComponent,
    IncidentComponent, IncidentAddComponent, IncidentEditComponent, IncidentListComponent, IncidentShowPropertiesComponent, IncidentUpdateStatusComponent, ShowMapComponent]
  
})
export class IncidentModule { }

// npm install --save bingmaps
// npm install --save @types/bingmaps

