/// <reference path="../../../types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />
import { Component, OnInit } from '@angular/core';
import { MapsService } from '../../../services/maps.service';
import { LatLongDetail } from '../../../models/maps/lat-long-detail';
import { environment } from '../../../../environments/environment';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/internal/Subscription';
import { MAP_TYPE, PathType } from 'src/app/app-common-constants';
import { PassdataService } from 'src/app/services/passdata.service';
import { PropertyListResponse } from 'src/app/models/api/properties.model';
import { Router } from '@angular/router';
import { filter } from 'rxjs/operators';
import { NavigationEnd } from '@angular/router';
import { Location } from '@angular/common';
import { validateVerticalPosition } from '@angular/cdk/overlay';
import { Subscribable } from 'rxjs';
import { ServerApiInterfaceService } from 'src/app/services/server-api-interface.service';

@Component({
  selector: 'app-bing-map',
  templateUrl: './bing-map.component.html',
  styleUrls: ['./bing-map.component.css']
})
export class BingMapComponent implements OnInit {
  searchManager: Microsoft.Maps.Search.SearchManager;
  incidentname: string;
  count: number;
  map;
  height: any;
  id: string;
  incidentid: string;
  maptype: string;
  incidentstatus: string;
  routescbscription: Subscription;
  mapsservicesubscribe: Subscription;
  mapvisible = false;
  mapvisiblesubscription: Subscription;
  initmapsubscriptions: Subscription;
  private history = [];
  constructor(private mapsService: MapsService,
    private mapsServcie: MapsService,
    private router: Router,
    private location: Location,
    private serverApiInterfaceService: ServerApiInterfaceService,
    private passdataservice: PassdataService,
    private activatedroute: ActivatedRoute) {
    this.height = (window.screen.height) + "px";
  }

  ngOnInit() {
    if (this.passdataservice.bingMapdata) {
      this.incidentname = this.passdataservice.bingMapdata.mapparam.incidentid;
      this.incidentstatus = this.passdataservice.bingMapdata.mapparam.status;
      console.log('this.passdataservice.bingMapdata.mapparam');
      console.log(this.passdataservice.bingMapdata.mapparam);
      if (this.passdataservice.bingMapdata.maptype == MAP_TYPE.boundry) {
        this.initMapsConfig();
        this.postcodeWithoutMarker(this.passdataservice.bingMapdata.mapparam.id, this.passdataservice.bingMapdata.mapparam.incident, this.passdataservice.bingMapdata.maptype);
      } else {
        const latlong = this.passdataservice.bingMapdata.mapparam; this.incidentname = this.passdataservice.bingMapdata.mapparam.incidentid;
        this.incidentname = this.passdataservice.bingMapdata.mapparam.incidentName;
        this.initMapsConfig();
        this.getMarker();
        // this.polatMarker(this.passdataservice.bingMapdata.mapparam, this.passdataservice.bingMapdata.maptype);
      }
      // subscription for setting map visible
      this.mapvisiblesubscription = this.passdataservice.ismapvisible.subscribe((value) => {
        setTimeout(() => {
          this.mapvisible = true;
        }, 500);

      });
      // en of subscription.


      // this.initMapsConfig();
      // this.initMapsConfig('51.50632', '-0.12714');
      // this.mapsService.initMapReference(this.map);
      this.mapsservicesubscribe = this.mapsService.getLatLongDetailsSubject().subscribe(
        latLongDetails => {
          this.clearOldMapsData();
          this.addPushPins(latLongDetails);
        });
    }
    // this.initmapsubscriptions = this.mapsServcie.initmapsubscriptions.subscribe((value) => {
    //   this.initMapsConfig();
    //   console.log('map inited');
    // });

  }// end of ng init

  postcodeWithoutMarker(id, incidentid, maptype) {
    console.log('in postcode with marker', id, incidentid, maptype)
    let loadDelay = 2 * 1000;
    if (this.count > 0) {
      loadDelay = 0;
    }
    setTimeout(() => {
      this.mapsServcie.getLatLongToPlot(id, maptype);
      this.count++;
    }, loadDelay);
  } // end of fucntion

  polatMarker(properties, maptype) {
    this.mapvisible = true;
    this.incidentname = properties.incidentName;
    let propertiesList: PropertyListResponse[] = [];
    propertiesList.push(properties);
    let loadDelay = 2 * 1000;
    if (this.count > 0) {
      loadDelay = 0;
    }
    setTimeout(() => {
      this.mapsService.getLatLongValue(propertiesList, maptype);
      this.count++;
    }, loadDelay);
  }

  initMapsConfig() {
    console.log('this.map');
    console.log(this.map);
    this.map = new Microsoft.Maps.Map('#showMap', {
      credentials: environment.bingMapsKey,
      // center: new Microsoft.Maps.Location(lat, long),
      // mapTypeId: Microsoft.Maps.MapTypeId.road,
      // zoom: 7,
      navigationBarMode: Microsoft.Maps.NavigationBarMode.compact,
      supportedMapTypes: [Microsoft.Maps.MapTypeId.road]
    });
    console.log('init config ');
    console.log(this.map);
    this.mapsService.initMapReference(this.map);
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
        description: 'Sub Building Name : ' + pushpinInfos[i].subBuildingName + ' ' + ' | Building Number : ' + pushpinInfos[i].buildingNumber + ' | Principal Street : ' + pushpinInfos[i].principalStreet + ' | Zone : ' + pushpinInfos[i].zone + ' | Counties  : ' + pushpinInfos[i].country,
      });
      infobox.setMap(this.map);
      Microsoft.Maps.Events.addHandler(pin, 'click', () => {
        infobox.setOptions({ visible: true });
      });
      this.map.entities.push(pin);
    }
  }

  clearOldMapsData() {
    if (this.map && this.map != null) {
      for (var i = this.map.entities.getLength() - 1; i >= 0; i--) {
        var pushpin = this.map.entities.get(i);
        if (pushpin instanceof Microsoft.Maps.Pushpin || pushpin instanceof Microsoft.Maps.Polygon) {
          this.map.entities.removeAt(i);
        }
      }
    }
  }// end of function
  closedInfoBox() {
    alert('texst');
  }
  //Called once, before the instance is destroyed.
  ngOnDestroy(): void {
    // if(this.map){this.map.dispose();}
    if (this.passdataservice.bingMapdata) {
      this.clearOldMapsData();
    }
    if(this.initmapsubscriptions){  this.initmapsubscriptions.unsubscribe();}
    if (this.routescbscription) { this.routescbscription.unsubscribe(); }
    if (this.mapsservicesubscribe) {
      this.mapsservicesubscribe.unsubscribe();
    }
    if (this.mapvisiblesubscription) { this.mapvisiblesubscription.unsubscribe(); }
  }
  Back() {
    console.log('this.passdataservice.backpath');
    console.log(this.passdataservice.backpath);
    if (this.passdataservice.backpath.pathtype == PathType.WITHOUT_PARAM) {
      this.router.navigate([this.passdataservice.backpath.path], { skipLocationChange: true });
    } else {
      this.router.navigate([this.passdataservice.backpath.path, this.passdataservice.backpath.pathparams], { skipLocationChange: true });
    }
  }
  // test fucn

  getMarker() {
    Microsoft.Maps.loadModule(['Microsoft.Maps.SpatialDataService',
      'Microsoft.Maps.Search'], () => {
        this.searchManager = new Microsoft.Maps.Search.SearchManager(this.map);
      });
    var url = environment.baseurl + '/tables/Property?$filter=IncidentId eq ';
    const data = this.passdataservice.bingMapdata.mapparam;
    const searchrequest = data.postcode;
    let geocodeRequest = {
      where: searchrequest,
      callback: this.generateMarker,
      errorCallback: (e) => {
      },
      userData: { that: this }
    };
    this.searchManager.geocode(geocodeRequest);
  }
  // end of fucntion

  // frucntion for generating marker
  generateMarker(geocodeResult, userData) {
    userData.that.clearOldMapsData();
    let that = userData.that;
    console.log('result data fucntion');
    console.log(geocodeResult);
    console.log('userData');
    console.log(userData.that);
    if (geocodeResult && geocodeResult.results && geocodeResult.results.length > 0) {
      let latLongDetails: LatLongDetail[] = [];
      for (const result of geocodeResult.results) {
        let latLongRecord = new LatLongDetail();
        latLongRecord.lat = result.locations[0].latitude;
        latLongRecord.long = result.locations[0].longitude;
        latLongDetails.push(latLongRecord);
      }
      for (var i = 0; i < 1; i++) {
        let loc = new Microsoft.Maps.Location(latLongDetails[i].lat, latLongDetails[i].long);
        let title = {
          title: latLongDetails[i].buildingName,
          icon: '../../assets/images/map_marker.png'
        }
        // custom map setview
        that.map.setView({
          mapTypeId: Microsoft.Maps.MapTypeId.road,
          center: new Microsoft.Maps.Location(latLongDetails[i].lat, latLongDetails[i].long),
          zoom: 7
        });
        const data = that.passdataservice.bingMapdata.mapparam;
        //  const searchrequest = data.buildingName + ", " + data.dependentLocality + "," + data.postcode;
        const searchrequest = data.postcode;
        let pin = new Microsoft.Maps.Pushpin(loc, title);
        var infobox = new Microsoft.Maps.Infobox(loc, {
          title: data.postcode,
          description: searchrequest,
          visible: false
        });
        infobox.setMap(that.map);
        Microsoft.Maps.Events.addHandler(pin, 'click', () => {
          infobox.setOptions({ visible: true });
        });
        that.map.entities.push(pin);
        that.mapvisible = true;
      }
    }
  } // end of fucntion

  // fucntion for getting lat long 
  getlatlong() {
    this.mapsServcie.getLantLongFromPostCode('123').subscribe((response) => {
      console.log('lat long data received++++++++++++++++++++++++++++++++');
      // if ((response.latitude && response.latitude !== null) && (response.longitude && response.longitude))       console.log('response');
      console.log(response);
      let pluslat = response.result.latitude + 0.005;
      let minuslat = response.result.latitude - 0.005;
      let pluslong = response.result.longitude + 0.005;
      let minuslong = response.result.longitude - 0.005;
      let url = "https://www.openstreetmap.org/export/embed.html?bbox=" + minuslong + "%2C" + minuslat + "%2C" + pluslong + "%2C" + pluslat + "&amp;layer=mapnik&amp;marker=" + response.result.latitude + "%2C" + response.result.longitude;
      console.log('url   ' + url);
      //  }
    });
  }

  // end of funtion.

}
