/// <reference path="../../../types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />
import { Component, OnInit } from '@angular/core';
import { MapsService } from '../../../services/maps.service';
import { LatLongDetail } from '../../../models/maps/lat-long-detail';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-show-map',
  templateUrl: './show-map.component.html',
  styleUrls: ['./show-map.component.css']
})
export class ShowMapComponent implements OnInit {

  map;

  constructor(private mapsService: MapsService) {
  }

  ngOnInit() {
    this.initMapsConfig();
    this.mapsService.initMapReference(this.map);
    this.mapsService.getLatLongDetailsSubject().subscribe(
      latLongDetails => {
        this.clearOldMapsData();
        this.addPushPins(latLongDetails);
      });
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

  addPushPins(pushpinInfos: LatLongDetail[]) {
    for (var i = 0; i < pushpinInfos.length; i++) {
      let loc = new Microsoft.Maps.Location(pushpinInfos[i].lat, pushpinInfos[i].long);
      let title = {
        title: pushpinInfos[i].buildingName,
        icon: '../../assets/images/map_marker.png'
      }
      let pin = new Microsoft.Maps.Pushpin(loc, title);
      var infobox = new Microsoft.Maps.Infobox(loc, {
        title: pushpinInfos[i].buildingName,
        description: pushpinInfos[i].subBuildingName + ' ' + pushpinInfos[i].buildingNumber + ', ' + pushpinInfos[i].principalStreet + ', ' + pushpinInfos[i].zone + ', ' + pushpinInfos[i].country,
        visible: false
      });
      infobox.setMap(this.map);
      Microsoft.Maps.Events.addHandler(pin, 'click', () => {
        infobox.setOptions({ visible: true });
      });
      this.map.entities.push(pin);
    }
  }

  clearOldMapsData() {
    for (var i = this.map.entities.getLength() - 1; i >= 0; i--) {
      var pushpin = this.map.entities.get(i);
      if (pushpin instanceof Microsoft.Maps.Pushpin || pushpin instanceof Microsoft.Maps.Polygon) {
        this.map.entities.removeAt(i);
      }
    }
  }

}

