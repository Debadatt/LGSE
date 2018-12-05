/// <reference path="../types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />
import { Injectable } from '@angular/core';
import { ServerApiInterfaceService } from './server-api-interface.service';
import { LoginStatusProviderService } from './login-status-provider.service';
import { PropertyListResponse, PublicMRPNList } from '../models/api/properties.model';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiErrorService } from './api-error.service';
import { LatLongDetail } from '../models/maps/lat-long-detail';
import { GeoDataRequestOptions } from '../models/maps/geo-data-request-options';
import { MAP_TYPE } from 'src/app/app-common-constants';
import { conditionallyCreateMapObjectLiteral } from '@angular/compiler/src/render3/view/util';
import { PassdataService } from 'src/app/services/passdata.service';
import { computeStyle } from '@angular/animations/browser/src/util';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class MapsService {

  latLongDetailsSubject: Subject<LatLongDetail[]>;
  initmapsubscriptions: Subject<any> = new Subject<any>();
  // postCodesSubject: Subject<string[]>;
  publicInfo: Subject<PublicMRPNList>;
  mapInstance;
  searchManager;
  callcount = 0;

  constructor(
    private serverApiInterfaceService: ServerApiInterfaceService,
    private passdataservice: PassdataService,
    private http: HttpClient,
    private loginStatusProviderService: LoginStatusProviderService,
    private apiErrorService: ApiErrorService
  ) {
    this.latLongDetailsSubject = new Subject<LatLongDetail[]>();
    this.publicInfo = new Subject<PublicMRPNList>();
    // this.postCodesSubject = new Subject<string[]>();
  }

  initMapReference(locMap) {
    this.mapInstance = locMap;
    // this.mapInstance.credentials = environment.bingMapsKey,
    console.log('mapInstance');
    console.log(this.mapInstance);
    Microsoft.Maps.loadModule(['Microsoft.Maps.SpatialDataService',
      'Microsoft.Maps.Search'], () => {
        this.searchManager = new Microsoft.Maps.Search.SearchManager(this.mapInstance);
      });
      // this.mapInstance.getCredentials(function(sessionId)
      // {
      //   console.log('sessionId',sessionId);
      // });
  }

  getLatLongDetailsSubject(): Observable<LatLongDetail[]> {
    return this.latLongDetailsSubject.asObservable();
  }

  // getpostCodesSubject(): Observable<string[]> {
  //   return this.postCodesSubject.asObservable();
  // }

  getLatLongToPlot(receivedId, maptype) {
    console.log("in getPropertyDataList " + receivedId);
    // var url = environment.baseurl + '/tables/Property?$filter=IncidentId eq ';
    var url = environment.baseurl + '/api/IncidentCustom/GetPostCodes?incidentId='
    // var url = environment.baseurl + '/api/IncidentCustom/Properties?$filter=IncidentId eq ';
    return this.serverApiInterfaceService.getProperty<PropertyListResponse[]>(url + receivedId.trim(), '1').subscribe(
      (response) => {
        this.clearOldMapsData();
        console.log(response);
        //  const propertiesList: PropertyListResponse[] = response;
        this.getLatLongValue(response, maptype);
      },
      (error) => {
        this.apiErrorService.handleError(error);
      }
    );
  }

  getLatLongToPlotWithQuotes(receivedId) {
    console.log("in getPropertyDataList " + receivedId);
    var url = environment.baseurl + '/tables/Property?$filter=IncidentId eq ';
    //var url = environment.baseurl + '/api/IncidentCustom/Properties?$filter=IncidentId eq ';
    return this.serverApiInterfaceService.getProperty<PropertyListResponse[]>(url + receivedId.trim(), '1').subscribe(
      (response) => {
        this.clearOldMapsData();
        const propertiesList: PropertyListResponse[] = response;
        this.getLatLongDetails(propertiesList);
      },
      (error) => {
        this.apiErrorService.handleError(error);
      }
    );
  }

  getLatLongDetails(propertiesList: PropertyListResponse[]) {
    let latLongDetails: LatLongDetail[] = [];
    let postCodes = [];
    for (let property of propertiesList) {
      let latLongRecord = new LatLongDetail();
      latLongRecord.lat = property.latitude;
      latLongRecord.long = property.longitude;
      latLongRecord.buildingName = property.buildingName;
      latLongRecord.buildingNumber = property.buildingNumber;
      latLongRecord.subBuildingName = property.subBuildingName;
      latLongRecord.principalStreet = property.principalStreet;
      latLongRecord.zone = property.zone;
      latLongRecord.country = property.country;
      latLongDetails.push(latLongRecord);
      postCodes.push(property.postcode);
    }
    this.sendLatLongDetails(latLongDetails);
    this.sendPostCodes(postCodes);
  }

  getPublicMRPSData() {
    var url = environment.baseurl + '/api/IncidentCustom/MPRNS';
    return this.serverApiInterfaceService.get<PublicMRPNList>(url).subscribe(
      (response) => {
        this.clearOldMapsData();
        this.extractDataSendToUI(response.mprns);
        this.sendRawDataToUI(response);
      },
      (error) => {
        this.apiErrorService.handleError(error);
      }
    );
  }

  sendRawDataToUI(publicInfo: PublicMRPNList) {
    this.publicInfo.next(publicInfo);
  }

  getPublicInfoObervable(): Observable<PublicMRPNList> {
    return this.publicInfo.asObservable();
  }

  clearOldMapsData() {
    for (var i = this.mapInstance.entities.getLength() - 1; i >= 0; i--) {
      var pushpin = this.mapInstance.entities.get(i);
      if (pushpin instanceof Microsoft.Maps.Pushpin || pushpin instanceof Microsoft.Maps.Polygon) {
        this.mapInstance.entities.removeAt(i);
      }
    }
  }

  private extractDataSendToUI(response: PropertyListResponse[]) {
    this.extractPostCodesAndSend(response);
    // this.extractTotalPropertiesAffected(response);
    // this.extractStartTime(response);
    // this.extractIncidentId(response);
    // this.extractStatus(response);
    // this.extractCompletedProperties(response);
  }

  extractPostCodesAndSend(response) {
    let postCodes = [];
    response.forEach(element => {
      element.postcode.includes(' ') ? (postCodes.indexOf(element.postcode.split(' ')[0]) === -1 ? postCodes.push(element.postcode.split(' ')[0]) : '') : (postCodes.indexOf(element.postcode.split(' ')[0]) === -1 ? postCodes.push(element.postcode) : '');
    });
    console.log(postCodes);
    this.sendPostCodes(postCodes);
  }

  sendLatLongDetails(latLongDetails: LatLongDetail[]) {
    this.latLongDetailsSubject.next(latLongDetails);
  }

  sendPostCodes(postCodes: string[]) {
    // this.postCodesSubject.next(postCodes);
    this.drawPostCodeBoundaries(postCodes);
  }

  drawPostCodeBoundaries(postCodes: string[]) {
    console.log(postCodes);
    let that = this;
    // let arr = ['G31','E14','B44','B98','AB45','BB11','B91', 'B30','B4','AB11','W8','EC2V','E11','SW6','DE55','AL8','BA20','BS1','N16','N17','CO4','SW1A','BH6','BT71','BT8','AL7','EC3N','EC1R','BN3','WC2E','SR5','BN1','WD3','BL9','BL1','BF1','SW15','EH15','LS1','NW3','CF31','G69','BH17','SW18','FY8','CH1','EH11','SO14','CH7','CA28'];
    let arr = ['E2', 'E15', 'E8', 'E14', 'E2', 'E7', 'E9'];
    let singlepostcode = [];



    for (let i = 0; i < postCodes.length; i++) {
      const newpostcode = postCodes[i].split(" ");
      singlepostcode.push(newpostcode[0]);
    }
    var filteredArray = singlepostcode.filter(function (item, pos) {
      return singlepostcode.indexOf(item) == pos;
    });
    console.log('filtered array');
    console.log(filteredArray);
    for (let i = 0; i < filteredArray.length; i++) {
      let geocodeRequest = {
        where: filteredArray[i],
        // where: '$$%%%',
        //  key: 'AqlBGa_q-saa0Ft8KSaHqB5OH-Se07IbksUWZUDhbnchHC-rfmd-4Ch4WJaFIGfy',
        callback: this.getBoundary,
        errorCallback: (e) => {
          console.log('bing map error callback');
          console.log(e)          
        },
        userData: { that: this }
      };
      //Make the geocode request.
      this.searchManager.geocode(geocodeRequest);
    }
    this.passdataservice.setMapvisible();
  }

  getBoundary(geocodeResult, userData) {
    let that = userData.that;
    that.callcount = 0;
    console.log('geocodeResult');
    console.log(geocodeResult);
    if (geocodeResult && geocodeResult.results && geocodeResult.results.length > 0) {
      //Zoom into the location.
      that.mapInstance.setView({ bounds: geocodeResult.results[0].bestView });

      //Create the request options for the GeoData API.
      let geoDataRequestOptions: GeoDataRequestOptions = {
        lod: 1,
        getAllPolygons: true
      };

      //Verify that the geocoded location has a supported entity type.
      switch (geocodeResult.results[0].entityType) {
        case "CountryRegion":
        case "AdminDivision1":
        case "AdminDivision2":
        case "Postcode1":
        case "Postcode2":
        case "Postcode3":
        case "Postcode4":
        case "Neighborhood":
        case "PopulatedPlace":
          geoDataRequestOptions.entityType = geocodeResult.results[0].entityType;
          break;
        default:
          return;
      }

      //Use the GeoData API manager to get the boundaries of the zip codes.
      Microsoft.Maps.SpatialDataService.GeoDataAPIManager.getBoundary(
        geocodeResult.results[0].location,
        geoDataRequestOptions,
        that.mapInstance,
        (data) => {
          //Add the polygons to the map.
          if (data.results && data.results.length > 0) {
            for (let i = 0, len = data.results[0].Polygons.length; i < len; i++) {
              var color;
              color = that.getRandomColor();
              data.results[0].Polygons[i].setOptions({
                strokeThickness: 1.5,
                fillColor: color,
              });
            }
            that.mapInstance.entities.push(data.results[0].Polygons);
          }
        });
    }

  }

  getRandomColor() {
    var o = Math.round, r = Math.random, s = 255;
    let rgb = 'rgba(' + o(r() * s) + ',' + o(r() * s) + ',' + o(r() * s) + ',' + 0.4 + ')';
    return (rgb);
  }

  // code by sarjerao
  getLatLongValue(postcodes, maptype) {
    // let latLongDetails: LatLongDetail[] = [];
    let postCodes = [];
    for (let postcode of postcodes) {
      // let latLongRecord = new LatLongDetail();
      // latLongRecord.lat = property.latitude;
      // latLongRecord.long = property.longitude;
      // latLongRecord.buildingName = property.buildingName;
      // latLongRecord.buildingNumber = property.buildingNumber;
      // latLongRecord.subBuildingName = property.subBuildingName;
      // latLongRecord.principalStreet = property.principalStreet;
      // latLongRecord.zone = property.zone;
      // latLongRecord.country = property.country;
      // latLongDetails.push(latLongRecord);
      postCodes.push(postcode);
    }
    if (maptype == MAP_TYPE.marker) {
      // this.sendLatLongDetails(latLongDetails);
    } else {
      this.sendPostCodes(postCodes);
    }

  } // end of functions 
  // fucntion for getting lat long
  getLantLongFromPostCode(postcode): Observable<any> {
    return this.http.get<any>('http://postcodes.io/postcodes/SW1A%201AA');
  }
  // end of function
  test(e) {
    if (this.callcount <= 1) {
      console.log('in test fucntion', e)
      this.initmapsubscriptions.next();
      setTimeout(() => {
        this.searchManager.geocode(e);
      }, 500);

      this.callcount++;
    }
  }

}
