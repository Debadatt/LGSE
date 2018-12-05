/// <reference path="../../types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />
import { Component, OnInit } from '@angular/core';
import { environment } from '../../../environments/environment';
import { MapsService } from '../../services/maps.service';

@Component({
  selector: 'app-maps',
  templateUrl: './maps.component.html',
  styleUrls: ['./maps.component.css']
})
export class MapsComponent implements OnInit {

  map;

  constructor(private mapsService: MapsService) { 

  }

  ngOnInit() {
    setTimeout(() => {
      this.initMapsConfig();
      this.getPublicMRPSData();
    }, 3000);

  }

  initMapsConfig() {
    this.map = new Microsoft.Maps.Map('#showMap', {
      credentials: environment.bingMapsKey,
      center: new Microsoft.Maps.Location(51.50632, -0.12714),
      mapTypeId: Microsoft.Maps.MapTypeId.road,
      zoom: 9,
      navigationBarMode: Microsoft.Maps.NavigationBarMode.compact,
      supportedMapTypes: [Microsoft.Maps.MapTypeId.road]
    });
  }

  getPublicMRPSData() {
    this.mapsService.initMapReference(this.map);
    this.mapsService.getPublicMRPSData();
    setInterval(() => {
      this.mapsService.getPublicMRPSData();
    }, 3*60*1000);
  }
}
