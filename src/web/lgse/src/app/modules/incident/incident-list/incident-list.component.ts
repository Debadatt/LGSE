import { Component, OnInit, ViewChild, EventEmitter } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { Router } from '@angular/router';
import { IncidentService } from '../../../services/incident.service';
import { IncidentListResponse, PassIncidentData, IncidentListResponseClone, PropertyStatusCount, IncidentPropertyStatusCount } from '../../../models/api/incident.model';
import { PropertiesService } from '../../../services/properties.service';
import { StausListResponse } from '../../../models/api/properties.model';
import { ApiErrorService } from '../../../services/api-error.service';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { MapsService } from '../../../services/maps.service';
import { PassdataService } from '../../../services/passdata.service';
import { CustomSearchData, CustomSearchSubSelectionValues, CustomSearchDataMain, SearchFilter } from 'src/app/models/search/custom-search-data';
import { Subscription } from 'rxjs/internal/Subscription';
import { merge, Observable } from 'rxjs';
import { startWith } from 'rxjs/operators';
import { switchMap } from 'rxjs/operators';
import { map } from 'rxjs/operators';
import { IncidentId, MapData } from 'src/app/models/api/map/incident-id';
import { MAP_TYPE, PathType, FeatureNames } from 'src/app/app-common-constants';
import { PassIncident } from 'src/app/models/api/dashbord/pass-incident';
import { BackButtonPath } from '../../../models/ui/back-btutton'
import { LocalstorageService } from 'src/app/services/localstorage.service';

@Component({
  selector: 'app-incident-list',
  templateUrl: './incident-list.component.html',
  styleUrls: ['./incident-list.component.css']
})

export class IncidentListComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  // process varibales 
  incidentListResponse: IncidentListResponse[] = [];
  incidentListResponsemap: Map<string, IncidentListResponse>;
  incidentListResponseClone: IncidentListResponseClone[] = [];
  displayedColumns = ['IncidentId', 'Description', 'NoOfPropsAffected', 'propsStatusCounts', 'CategoryName', 'StartTime', 'Status', 'EndTime', 'action'];
  dataSource;
  customSearchDataMain = new CustomSearchDataMain();
  selectedresourcetype: string;
  customSearchData: CustomSearchData[] = [];
  customSearchSubSelectionValues: CustomSearchSubSelectionValues[] = [];
  isvisible = false;
  refreshTable: EventEmitter<any> = new EventEmitter();
  resultsLength = 0;
  selected: string[] = [];
  searchFilter: SearchFilter[] = [];
  customsearchsubscription: Subscription;
  feature = FeatureNames;
  count = 0;
  display = false;
  selectedIncidentId: string;
  formCategories = [];
  formStatus1: formStatus[] = [];
  StatusListResponse: StausListResponse[] = [];
  formStatusMap: Map<string, formStatus>
  // end of process variables

  constructor(
    private router: Router,
    private incidentService: IncidentService,
    private propertiesService: PropertiesService, private apiErrorService: ApiErrorService,
    private appNotificationService: AppNotificationService,
    private mapsServcie: MapsService,
    private passdataservice: PassdataService,
    public localstorageservice: LocalstorageService

  ) { }

  ngOnInit() {
    this.getStatusList();
    this.searchKeyValue();
    this.getCategoryList();
    this.selectedresourcetype = '1';
    // code block for  get custom serach data
    this.customsearchsubscription = this.passdataservice.customsearchvalue.subscribe((val) => {
      this.searchFilter = val;
      if (this.paginator.pageSize === 1) {
        this.paginator.pageSize = 50;
      }
      this.refreshTable.emit(null);
    });  //end
    // ==================================== paginator starts =============================
    // set default load parameters
    this.paginator.pageSize = 1;
    this.sort.active = 'IncidentId';
    this.sort.direction = 'desc';

    // If the user changes the sort order, reset back to the first page.
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    this.refreshTable.subscribe(() => this.paginator.pageIndex = 0);
    merge(this.sort.sortChange, this.paginator.page, this.refreshTable)
      .pipe(
      startWith({}),
      switchMap(() => {
        return this.getIncidentList();
      }),
      map(data => {
        // Flip flag to show that loading has finished.     
        return data;
      }),
    ).subscribe(
      incidentPayloadResponse => {
        if (incidentPayloadResponse && incidentPayloadResponse !== null) {
          this.resultsLength = incidentPayloadResponse.count;
          this.incidentListResponse = incidentPayloadResponse.results;
          this.incidentListResponse.forEach(incident => {
            this.createPropertyStatusArray(incident);
            // Getting Count for Property status count
            incident.propsStatusCounts.forEach(statusCountWithId => {
              const filtercnt = incident.propertyAllStatusCounts.filter(a => a.propertyStatusMaster.id === statusCountWithId.statusId)[0];
              if (filtercnt) {
                filtercnt.count = statusCountWithId.count;
              }
            });
          });
          this.dataSource = new MatTableDataSource<IncidentListResponse>(this.incidentListResponse);
          // this.dataSource.sort = this.sort;
          // this.dataSource.paginator = this.paginator;
        }
      });
  }// end of ng init

  //Create Structure for Property Status Count
  createPropertyStatusArray(incident: IncidentListResponse) {
    incident.propertyAllStatusCounts = [];
    const statusCountItem = new IncidentPropertyStatusCount();
    this.StatusListResponse.forEach(item => {
      const statusCountItem = new IncidentPropertyStatusCount();
      //By Default Push 0 to StatusCount
      statusCountItem.count = 0;
      statusCountItem.propertyStatusMaster = item;
      incident.propertyAllStatusCounts.push(statusCountItem);
    });

  }

  getIncidentList(): Observable<any> {
    //get status list for merging status count
    // this.getStatusList();
    return this.incidentService.getIncidentDataList(this.paginator.pageIndex, this.paginator.pageSize, this.sort.active, this.sort.direction, this.searchFilter);
  }

  getStatusList() {
    this.propertiesService.getStatusDataList().subscribe(statusListResponse => {
      this.StatusListResponse = statusListResponse;
      //Added Hardcoded Status.Not Retriving from Database
      this.StatusListResponse.unshift({
        id: null,
        status: 'No Status',
        displayOrder: 4,
        shortText: 'NS',
      });
    });
  }


  // fucntion fot apply filetr
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  searchKeyValue(): void {
    this.customSearchData.push({
      searchkeydisplayname: 'Incident Id',
      searchkey: 'IncidentId',
      controltype: 'input',
      controlvalues: null
    });
    this.customSearchData.push({
      searchkeydisplayname: 'Status',
      searchkey: 'Status',
      controltype: 'Select',
      controlvalues: [
        { displaytext: 'In Progress', value: '0' },
        { displaytext: 'Complete', value: '1' },
        { displaytext: 'Cancel', value: '2' }
      ]
    });
    const selectData = ['IncidentId', 'Status'];
    this.customSearchDataMain.selected = selectData;
    this.customSearchDataMain.fieldlist = this.customSearchData;
  }// end of fucntion

  //function for show hide resources
  hideShowSeacrh(): void {
    this.isvisible = true;
    this.passdataservice.setOpenSearch(true);
  }
  //end of fucntion

  //check is any incident 'in Progress', If inProgress You can not create another Incident
  checkAnyIncidentIsInProgress() {
    let isIncidentInProgress: boolean;
    this.incidentService.checkAnyIncidentIsInProgress().subscribe(payloadResponse => {
      isIncidentInProgress = payloadResponse;
      if (isIncidentInProgress) {
        this.appNotificationService.info("New Incident cannot be added while there is an incident in progress.!");
      }
      else {
        this.router.navigate(['/incident/add']);
      }
    });
  }

  //DownloadCsvFile
  downloadReport(id, incidentId) {
    this.propertiesService.downloadCsv(id).subscribe(payloadResponse => {
      var today = new Date();
      var dd = today.getDate();
      var mm = today.getMonth() + 1;
      var yy = today.getFullYear();
      var date = dd.toString() + mm.toString() + yy.toString();
      let fileName = incidentId + "_" + date + ".csv";
      this.saveFile(payloadResponse, incidentId + "_" + dd.toString() + mm.toString() + yy.toString() + ".csv");
    });
  }

  //Save Downloaded File
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

  // code block for showProperties
  showProperties(id, incidentid, status): void {
    this.passdataservice.assignedMPRN = false;
    const incident = new PassIncidentData();
    incident.recordid = id;
    incident.incidentid = incidentid;
    incident.status = status;
    this.router.navigate(['incident/showProperties', incident]);
  }
  // end of block

  // code for Show Properties in Maps
  showPropertiesInMap(id, incidentId): void {
    this.display = true;
    this.selectedIncidentId = incidentId + '';
    this.sendShowLatLongRequest(id);
  }

  sendShowLatLongRequest(id) {
    let loadDelay = 2 * 1000;
    if (this.count > 0) {
      loadDelay = 0;
    }
    setTimeout(() => {
      this.count++;
    }, loadDelay);
  }

  // code block for Edit Incident
  editIncident(id): void {
    this.router.navigate(['incident/editIncident', id]);
  }
  // end of block

  // code block for Refresh Button
  refresh(): void {
    this.refreshTable.emit(null);
  }

  // code block for addIncident
  addIncident(): void {
    this.checkAnyIncidentIsInProgress();
  }
  // end of block

  // cleanup
  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    if (this.customsearchsubscription) {
      this.customsearchsubscription.unsubscribe();
    }
  }

  showBingMap(id, incidentId) {
    const mapdata = new MapData<IncidentId>();
    mapdata.maptype = MAP_TYPE.boundry;
    const incident = new IncidentId();
    incident.incidentid = incidentId;
    incident.id = id;
    mapdata.mapparam = incident;
    this.passdataservice.bingMapdata = mapdata;
    const backButtonPath = new BackButtonPath<null>();
    backButtonPath.pathtype = PathType.WITHOUT_PARAM;
    backButtonPath.path = '/incident';
    this.passdataservice.backpath = backButtonPath;
    this.router.navigate(['/incident/bingmap']);
  }

  //Get Category List
  getCategoryList() {
    this.incidentService.getCategoryDataList().subscribe(CategoryListResponse => {
      for (let category of CategoryListResponse) {
        this.formCategories.push({
          id: category.id,
          name: category.category
        });
      }
    });
  }

  viewDashbord(id, incidentid, status) {
    const incident = new PassIncident();
    incident.id = id;
    incident.incidentid = incidentid;
    incident.status = status;
    this.router.navigate(['/dashboard/values', incident]);
  }
}

class formStatus {
  id: string;
  status: string;
  displayOrder: number;
  shortText: string;
  count: number;
}