import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IncidentListComponent } from './incident-list/incident-list.component';
import { IncidentAddComponent } from './incident-add/incident-add.component';
import { IncidentShowPropertiesComponent } from './incident-show-properties/incident-show-properties.component';
import { IncidentEditComponent } from './incident-edit/incident-edit.component';
import { IncidentUpdateStatusComponent } from './incident-update-status/incident-update-status.component';
import { BingMapComponent } from 'src/app/modules/incident/bing-map/bing-map.component';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: IncidentListComponent
      },
      {
        path: 'add',
        component: IncidentAddComponent
      },

      {
        path: 'showProperties',
        component: IncidentShowPropertiesComponent
      },
      {
        path: 'editIncident/:id',
        component: IncidentEditComponent
      },
      {
        path: 'updateStatus',
        component: IncidentUpdateStatusComponent
      },
      {
        path: 'bingmap',
        component: BingMapComponent
      }
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IncidentRoutingModule { }
