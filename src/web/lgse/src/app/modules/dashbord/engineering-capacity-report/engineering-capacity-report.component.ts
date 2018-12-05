import { DashbordService } from 'src/app/services/dashbord/dashbord.service';
import { ApiErrorService } from 'src/app/services/api-error.service';
import { EngineeringCapacity } from 'src/app/models/api/dashbord/engineering-capacity';
import { IncidentIdrequest, EngineeringCapacityRequest } from 'src/app/models/api/dashbord/incidentid-request';
import { PassIncident } from 'src/app/models/api/dashbord/pass-incident';
import { Component, OnInit, Input, ViewChild, NgModule, EventEmitter } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { GetAllResourcesResponse } from 'src/app/models/api/resources/get-all-resources-response';
import { ResourcesServiceService } from 'src/app/services/resources/resources-service.service';
import { Router } from '@angular/router';
import { PassResourceData } from 'src/app/models/api/resources/paas-resources-data';
import { PassdataService } from 'src/app/services/passdata.service';
import { CustomSearchData, CustomSearchDataMain, SearchFilter } from 'src/app/models/search/custom-search-data';
import { Subscription } from 'rxjs/internal/Subscription';
import { merge, Observable } from 'rxjs';
import { startWith } from 'rxjs/operators';
import { switchMap } from 'rxjs/operators';
import { map } from 'rxjs/operators';


import { LocalstorageService } from 'src/app/services/localstorage.service';
import { FeatureNames } from 'src/app/app-common-constants';

import { BackButtonPath } from 'src/app/models/ui/back-btutton';

@Component({
  selector: 'app-engineering-capacity-report',
  templateUrl: './engineering-capacity-report.component.html',
  styleUrls: ['./engineering-capacity-report.component.css']
})
export class EngineeringCapacityReportComponent implements OnInit {
  engineeringCapacity: EngineeringCapacity[] = [];
  engineeringCapacitycsv: EngineeringCapacity[] = [];
  sortingtypt: string;
  displayedColumns = ['FirstName', 'Zones', 'Cells', 'Login', 'Logout'];
  dataSource;
  filtervalue: string;
  cells: string[];
  zones: string[];
  @Input() Incident: PassIncident;
  @ViewChild(MatPaginator)
  paginator: MatPaginator;
  @ViewChild(MatSort)
  sort: MatSort;
  // process varibales 
  constructor(private dashboardservice: DashbordService,
    private apiErrorService: ApiErrorService) { }

  ngOnInit() {
    // setTimeout(() => {    //<<<---    using ()=> syntax
    //   this.getEngCapacityReportData();
    // }, 100);
    // this.engineeringCapacity = [{ firstName: "Isolator", lastName: "LGSE", zones: ["1"], cells: ["1.1"], loggedin: "2018-09-12T10:23:29.669Z", loggedout: "2018-09-12T10:23:29.669Z" }, { firstName: "Isolator", lastName: "LGSE", zones: ["1"], cells: ["1.1"], loggedin: "2018-09-12T10:23:29.669Z", loggedout: "2018-09-12T10:23:29.669Z" }, { firstName: "Isolator", lastName: "LGSE", zones: ["1"], cells: ["1.1"], loggedin: "2018-09-12T10:23:29.669Z", loggedout: "2018-09-12T10:23:29.669Z" }, { firstName: "Isolator", lastName: "LGSE", zones: ["1"], cells: ["1.1"], loggedin: "2018-09-12T10:23:29.669Z", loggedout: "2018-09-12T10:23:29.669Z" }];
    // this.dataSource = new MatTableDataSource<EngineeringCapacity>(this.engineeringCapacity)
    this.getCells();
  }

  getEngCapacityReportData() {
    const incident = new EngineeringCapacityRequest();
    incident.IncidentId = this.Incident.id;
    incident.Value = this.filtervalue;
    incident.Type = this.sortingtypt;
    this.dashboardservice.getEngineeringCapacity(incident).subscribe(
      engineeringcapacityesponse => {
        this.engineeringCapacity = engineeringcapacityesponse;
        console.log(' this.engineeringCapacity');
        console.log(this.engineeringCapacity);
        this.dataSource = new MatTableDataSource<EngineeringCapacity>(this.engineeringCapacity);
        this.dataSource.paginator = this.paginator;
      },
      (error) => {
        this.apiErrorService.handleError(error);
      });
  }
  // end of fucnttion

  //DownloadCsvFile
  downloadReport() {
    const incident = new EngineeringCapacityRequest();
    incident.IncidentId = this.Incident.id;
    incident.Value = this.filtervalue;
    incident.Type = this.sortingtypt;
    this.dashboardservice.downloadCsv(incident).subscribe(payloadResponse => {
      console.log(payloadResponse);
      var today = new Date();
      var dd = today.getDate();
      var mm = today.getMonth() + 1;
      var yy = today.getFullYear();
      var date = dd.toString() + mm.toString() + yy.toString();

      let fileName = this.Incident.id + "_" + date + ".csv";
      console.log(fileName);
      this.saveFile(payloadResponse, this.Incident.id + "_" + dd.toString() + mm.toString() + yy.toString() + ".csv");
    });
  }


  saveFile(data: Blob, filename: string) {
    const blob = new Blob([data]);
    if (window.navigator && window.navigator.msSaveOrOpenBlob) { // for IE
      window.navigator.msSaveOrOpenBlob(blob, filename);
    }
    else {
      const url = window.URL.createObjectURL(blob);
      const anchor = document.createElement('a');
      anchor.download = filename;
      anchor.href = url;
      document.body.appendChild(anchor);   // added into dom
      anchor.click();
      anchor.remove();   // removed from dom
    }
  }
  // fucntion for getting cells of incident
  getCells() {
    this.dashboardservice.getCellsAndZones(this.Incident.id).subscribe(
      mprnresponse => {
        console.log('cell response success');
        this.cells = [];
        this.zones = [];
        this.cells = mprnresponse.cells;
        this.zones = mprnresponse.zones;
        if (this.zones.length > 0) {
          this.sortingtypt = 'zone';
          this.filtervalue = this.zones[0];
          this.getEngCapacityReportData();
        }
      },
      (error) => {
        this.apiErrorService.handleError(error);
      });
  }
  // end of function.
  // fucntion  for clear filters
  cleartFilter() {
    this.filtervalue = null;
    if (this.sortingtypt == 'all') {
      this.getEngCapacityReportData();
    }
  }
  // end of fucntion

  // filter chart 
  filterChart() {
    console.log('this.filtervalue', this.filtervalue);
   // if (this.filtervalue && this.filtervalue != null) {
      this.getEngCapacityReportData();
   // }
  }
  // end of function
   // fucntion for ploat cell and zone
   cellZone(Data: any): string {
    let datastring = '';
    for (let i = 0; i < Data.length; i++) {
      const val = Data[i].trim();
      if (i > 0 && val != '' && Data[i - 1].trim() != '') {
          datastring += ',' + val;
      } else {
        datastring += val;
      }
    }
    return datastring;
  }
  // end of fucntion

}
