import { Component, OnInit, ViewChild, ChangeDetectorRef, EventEmitter } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { PropertiesService } from '../../../services/properties.service';
import { PropertyListResponse, AssignedMprnResponse } from '../../../models/api/properties.model';
import { MAP_TYPE, PathType, FeatureNames } from '../../../app-common-constants';
import { TranslateService } from '@ngx-translate/core';
import { MapsService } from '../../../services/maps.service';
import { PassdataService } from '../../../services/passdata.service';
import { CustomSearchData, CustomSearchDataMain, SearchFilter } from 'src/app/models/search/custom-search-data';
import { Subscription } from 'rxjs/internal/Subscription';
import { merge, Observable } from 'rxjs';
import { startWith } from 'rxjs/operators';
import { switchMap } from 'rxjs/operators';
import { map } from 'rxjs/operators';
import { IncidentId, MapData } from 'src/app/models/api/map/incident-id';
import { MPRNDetails } from '../../../models/api/resources/paas-resources-data';
import { GetActiveMPRnList } from '../../../models/api/resources/get-active-mprn-response';
import { PassIncidentData } from 'src/app/models/api/incident.model';
import { BackButtonPath } from 'src/app/models/ui/back-btutton';
import { LocalstorageService } from 'src/app/services/localstorage.service';

@Component({
  selector: 'app-incident-show-properties',
  templateUrl: './incident-show-properties.component.html',
  styleUrls: ['./incident-show-properties.component.css']
})
export class IncidentShowPropertiesComponent implements OnInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  // process varibales 
  pathtypes = PathType;
  propertyListResponse: PropertyListResponse[] = [];
  assignedMprnResponse: AssignedMprnResponse[] = [];
  displayedColumns = ['isIsolated', 'MPRN', 'BuildingName', 'DependentStreet', 'PostTown', 'LocalityName', 'DependentLocality', 'Postcode', 'Zone', 'Cell', 'ZoneManagerName', 'CellManagerName', 'LatestStatus', 'action'];
  dataSource;
  customSearchDataMain = new CustomSearchDataMain();
  selectedresourcetype: string;
  customSearchData: CustomSearchData[] = [];
  isvisible = false;
  refreshTable: EventEmitter<any> = new EventEmitter();
  resultsLength = 0;
  selected: string[] = [];
  searchFilter: SearchFilter[] = [];
  customsearchsubscription: Subscription;
  display: boolean = false;
  address = [];
  receivedid: string;
  count = 0;
  isrecidavailable: boolean;
  incidentdbid: string;
  incidentidformap: string;
  assignedMPRN: boolean;
  incidentId: string;
  status: number;
  propertyid: string;
  feature = FeatureNames;


  constructor(
    private router: Router,
    private propertiesService: PropertiesService,
    private chdte: ChangeDetectorRef,
    private activtedroute: ActivatedRoute,
    private translate: TranslateService,
    public passdataservice: PassdataService,
    public localstorageservice: LocalstorageService,
    private mapsService: MapsService) { }

  ngOnInit() {
    this.searchKeyValue();
    this.receivedid = "'" + this.activtedroute.snapshot.params.recordid + "'";
    this.incidentdbid = this.activtedroute.snapshot.params.recordid;
    this.incidentidformap = this.activtedroute.snapshot.params.incidentid;
    this.incidentId = this.activtedroute.snapshot.params.incidentid;
    this.status = this.activtedroute.snapshot.params.status;
    this.assignedMPRN = this.passdataservice.assignedMPRN;
    console.log('this.passdataservice.assignedMPRN', this.passdataservice.assignedMPRN);

    //Assigned MPRN==True=====> only visible for Isolator and Engineer Role
    console.log('this.receivedid', this.receivedid);
    if (!this.activtedroute.snapshot.params.recordid) {
      this.isrecidavailable = false;
      // code block for  get custom serach data
      this.customsearchsubscription = this.passdataservice.customsearchvalue.subscribe((val) => {
        this.searchFilter = val;
        this.refreshTable.emit(null);
      });  //end
      // ==================================== paginator starts =============================
      // set default load parameters
      this.paginator.pageSize = 50;
      this.sort.active = 'IncidentId';
      this.sort.direction = 'desc';

      // If the user changes the sort order, reset back to the first page.
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      this.refreshTable.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page, this.refreshTable)
        .pipe(
        startWith({}),
        switchMap(() => {
          return this.getAssignedMprn();
        }),
        map(data => {
          // Flip flag to show that loading has finished.     
          return data;
        }),
      ).subscribe(
        assignedMprnResponse => {
          if (assignedMprnResponse && assignedMprnResponse !== null) {
            this.resultsLength = assignedMprnResponse.count;
            this.assignedMprnResponse = assignedMprnResponse.results;
            let i: number;
            let generatedAddress;
            this.receivedid = "'" + this.assignedMprnResponse[0].incidentId + "'";
            this.incidentdbid = this.assignedMprnResponse[0].incidentId;
            this.incidentidformap = this.assignedMprnResponse[0].incidentName;
            this.incidentId = this.assignedMprnResponse[0].incidentName;
            this.status = this.assignedMprnResponse[0].status;
            this.propertyid = this.assignedMprnResponse[0].propertyId;

            //Concatenate Address=BuildNo+BuildName+SubBuildingName+PricipleStreet
            for (i = 0; i < this.assignedMprnResponse.length && this.assignedMprnResponse.length; i++) {

              this.address = [];
              if (this.assignedMprnResponse[i].buildingNumber.length > 0) {
                this.address.push(this.assignedMprnResponse[i].buildingNumber);
              }
              if (this.assignedMprnResponse[i].buildingName.length > 0) {
                this.address.push(this.assignedMprnResponse[i].buildingName);
              }
              if (this.assignedMprnResponse[i].subBuildingName.length > 0) {
                this.address.push(this.assignedMprnResponse[i].subBuildingName);
              }
              if (this.assignedMprnResponse[i].principalStreet.length > 0) {
                this.address.push(this.assignedMprnResponse[i].principalStreet);
              }

              generatedAddress = "";
              let j: number;

              //Concatenated Address --Seprated by comma
              for (j = 0; j < this.address.length; j++) {
                if (this.address.length - 1 - j > 0) {
                  generatedAddress = generatedAddress + this.address[j] + ", ";
                }
                else {
                  generatedAddress = generatedAddress + this.address[j];
                }
              }

              this.assignedMprnResponse[i].buildingName = generatedAddress;
              if (this.assignedMprnResponse[i].latestStatus != null && this.assignedMprnResponse[i].latestSubStatus != null && this.assignedMprnResponse[i].latestStatus.length > 0 && this.assignedMprnResponse[i].latestSubStatus.length > 0) {
                this.assignedMprnResponse[i].latestStatus = this.assignedMprnResponse[i].latestStatus + ", " + this.assignedMprnResponse[i].latestSubStatus
              }
            }
            this.dataSource = new MatTableDataSource<AssignedMprnResponse>(assignedMprnResponse.results);
          } else {
            this.dataSource = new MatTableDataSource<AssignedMprnResponse>(assignedMprnResponse.results);
          }
        });
    }
    else {
      this.isrecidavailable = true;
      //Show Property Page---Visible to Admin,Incident Controller
      this.selectedresourcetype = '1';
      // code block for  get custom serach data
      this.customsearchsubscription = this.passdataservice.customsearchvalue.subscribe((val) => {
        this.searchFilter = val;
        this.refreshTable.emit(null);
      });  //end
      // ==================================== paginator starts =============================
      // set default load parameters
      this.paginator.pageSize = 50;
      this.sort.active = 'IncidentId';
      this.sort.direction = 'desc';

      // If the user changes the sort order, reset back to the first page.
      this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
      this.refreshTable.subscribe(() => this.paginator.pageIndex = 0);
      merge(this.sort.sortChange, this.paginator.page, this.refreshTable)
        .pipe(
        startWith({}),
        switchMap(() => {
          return this.getPropertyList();
        }),
        map(data => {
          // Flip flag to show that loading has finished.     
          return data;
        }),
      ).subscribe(
        propertiesPayloadResponse => {
          if (propertiesPayloadResponse && propertiesPayloadResponse !== null) {
            this.resultsLength = propertiesPayloadResponse.count;
            this.propertyListResponse = propertiesPayloadResponse.results;
            let i: number;
            let generatedAddress;

            //Concatenate Address=BuildNo+BuildName+SubBuildingName+PricipleStreet
            for (i = 0; i < this.propertyListResponse.length && this.propertyListResponse.length; i++) {

              this.address = [];
              if (this.propertyListResponse[i].buildingNumber.length > 0) {
                this.address.push(this.propertyListResponse[i].buildingNumber);
              }
              if (this.propertyListResponse[i].buildingName.length > 0) {
                this.address.push(this.propertyListResponse[i].buildingName);
              }
              if (this.propertyListResponse[i].subBuildingName.length > 0) {
                this.address.push(this.propertyListResponse[i].subBuildingName);
              }
              if (this.propertyListResponse[i].principalStreet.length > 0) {
                this.address.push(this.propertyListResponse[i].principalStreet);
              }
              generatedAddress = "";
              let j: number;

              //Concatenated Address Separated by comma
              for (j = 0; j < this.address.length; j++) {
                if (this.address.length - 1 - j > 0) {
                  generatedAddress = generatedAddress + this.address[j] + ", ";
                }
                else {
                  generatedAddress = generatedAddress + this.address[j];
                }
              }
              this.propertyListResponse[i].buildingName = generatedAddress;
              if (this.propertyListResponse[i].latestStatus != null && this.propertyListResponse[i].latestSubStatus != null && this.propertyListResponse[i].latestStatus.length > 0 && this.propertyListResponse[i].latestSubStatus.length > 0) {
                this.propertyListResponse[i].latestStatus = this.propertyListResponse[i].latestStatus + ", " + this.propertyListResponse[i].latestSubStatus
              }
            }
            this.dataSource = null;
            this.dataSource = new MatTableDataSource<PropertyListResponse>(propertiesPayloadResponse.results);
          }
        });
    }
  }// end of ng init
  // pass search key data to custom search
  searchKeyValue(): void {
    this.customSearchData.push({
      searchkeydisplayname: 'MPRN',
      searchkey: 'MPRN',
      controltype: 'input',
      controlvalues: null
    });
    // this.customSearchData.push({
    //   searchkeydisplayname: 'Building Name',
    //   searchkey: 'BuildingName',
    //   controltype: 'input',
    //   controlvalues: null
    // });
    // this.customSearchData.push({
    //   searchkeydisplayname: 'Building Number',
    //   searchkey: 'BuildingNumber',
    //   controltype: 'input',
    //   controlvalues: null
    // });
    // this.customSearchData.push({
    //   searchkeydisplayname: 'SubBuilding Name',
    //   searchkey: 'SubBuildingName',
    //   controltype: 'input',
    //   controlvalues: null
    // });
    // this.customSearchData.push({
    //   searchkeydisplayname: 'Principal Street',
    //   searchkey: 'PrincipalStreet',
    //   controltype: 'input',
    //   controlvalues: null
    // });
    this.customSearchData.push({
      searchkeydisplayname: 'Dependent Street',
      searchkey: 'DependentStreet',
      controltype: 'input',
      controlvalues: null
    });
    this.customSearchData.push({
      searchkeydisplayname: 'Post Town',
      searchkey: 'PostTown',
      controltype: 'input',
      controlvalues: null
    });
    this.customSearchData.push({
      searchkeydisplayname: 'Locality Name',
      searchkey: 'LocalityName',
      controltype: 'input',
      controlvalues: null
    });
    this.customSearchData.push({
      searchkeydisplayname: 'Dependent Locallity',
      searchkey: 'DependentLocality',
      controltype: 'input',
      controlvalues: null
    });

    this.customSearchData.push({
      searchkeydisplayname: 'Post Code',
      searchkey: 'Postcode',
      controltype: 'input',
      controlvalues: null
    });
    this.customSearchData.push({
      searchkeydisplayname: 'Zone',
      searchkey: 'Zone',
      controltype: 'input',
      controlvalues: null
    });
    this.customSearchData.push({
      searchkeydisplayname: 'Cell',
      searchkey: 'Cell',
      controltype: 'input',
      controlvalues: null
    });
    this.customSearchData.push({
      searchkeydisplayname: 'Zone Controller',
      searchkey: 'ZoneManagerName',
      controltype: 'input',
      controlvalues: null
    });
    this.customSearchData.push({
      searchkeydisplayname: 'Cell Manager',
      searchkey: 'CellManagerName',
      controltype: 'input',
      controlvalues: null
    });
    this.customSearchData.push({
      searchkeydisplayname: 'Priority Customer',
      searchkey: 'PriorityCustomer',
      controltype: 'Select',
      controlvalues: [
        { displaytext: 'Yes', value: '0' },
        { displaytext: 'No', value: '1' }
      ]
    });
    this.customSearchData.push({
      searchkeydisplayname: 'Status',
      searchkey: 'LatestStatus',
      controltype: 'Select',
      controlvalues: [
        { displaytext: 'Isolated', value: 'Isolated' },
        { displaytext: 'No Access', value: 'No Access' },
        { displaytext: 'No Gas Connection', value: 'No Gas Connection' },
        { displaytext: 'Restored', value: 'Restored' },
        { displaytext: 'No Status', value: 'null' }
      ]
    });
    this.selected.push('Postcode', 'MPRN');
    this.customSearchDataMain.selected = this.selected;
    this.customSearchDataMain.fieldlist = this.customSearchData;
  }// end of fucntion

  //function for show hide resources
  hideShowSeacrh(): void {
    this.isvisible = true;
    this.passdataservice.setOpenSearch(true);
  }
  //end of fucntion

  //Get Property Detail List------Incident Controller and Admin Can view
  getPropertyList(): Observable<any> {
    return this.propertiesService.getPropertyDataList(this.receivedid, this.paginator.pageIndex, this.paginator.pageSize, this.sort.active, this.sort.direction, this.searchFilter);
  }

  //Assigned Mprn List---Mainly Visible for Isolator and Engineer Role
  getAssignedMprn(): Observable<any> {
    return this.propertiesService.getAssignedMprn(this.paginator.pageIndex, this.paginator.pageSize, this.sort.active, this.sort.direction, this.searchFilter);
  }

  //Show Map
  onShowMap() {
    if (this.receivedid) {
      this.display = true;
      this.sendShowLatLongRequest(this.receivedid);
    }
  }

  sendShowLatLongRequest(id) {
    let loadDelay = 2 * 1000;
    if (this.count > 0) {
      loadDelay = 0;
    }
    setTimeout(() => {
      this.mapsService.getLatLongToPlotWithQuotes(this.receivedid);
      this.count++;
    }, loadDelay);
  }

  // code block for Update Status
  showProperties(id, type): void {
    const incident = new PassIncidentData();
    if (this.assignedMPRN) {
      incident.propertyid = this.propertyid;
    }
    else {
      incident.propertyid = id;
    }
    incident.incidentid = this.incidentId;
    incident.recordid = this.incidentdbid;
    incident.status = this.status


    // set object in service for back button
    const backButtonPath = new BackButtonPath<PassIncidentData>();
    backButtonPath.pathtype = PathType.WITH_PARAM;
    console.log('type', type);
    if (type == PathType.WITH_PARAM) {
      console.log('in param functions');
      backButtonPath.pathtype = PathType.WITH_PARAM;
      backButtonPath.pathparams = incident;
      incident.urltype = PathType.WITH_PARAM;
    } else {
      backButtonPath.pathtype = PathType.WITHOUT_PARAM;
      incident.urltype = PathType.WITHOUT_PARAM;
    }
    backButtonPath.path = '/incident/showProperties';

    this.passdataservice.backpath = backButtonPath;
    console.log('this.passdataservice.backpath');
    console.log(this.passdataservice.backpath);
    // end 
    this.router.navigate(['incident/updateStatus', incident]);
  }

  //code block for viewHistory
  viewMPRNHistory(property: GetActiveMPRnList, type: string): void {
    const mprn = new MPRNDetails();
    if (this.assignedMPRN) {
      mprn.propertyid = this.propertyid;
    }
    else {
      mprn.propertyid = property.id;
    }
    mprn.mprn = property.mprn;

    const incident = new PassIncidentData();
    incident.incidentid = this.incidentId;
    incident.recordid = this.incidentdbid;
    incident.status = this.status;

    // set object in service for back button
    const backButtonPath = new BackButtonPath<PassIncidentData>();
    if (type == PathType.WITH_PARAM) {
      backButtonPath.pathtype = PathType.WITH_PARAM;
      backButtonPath.pathparams = incident;
    } else {
      backButtonPath.pathtype = PathType.WITHOUT_PARAM;
    }
    backButtonPath.path = 'incident/showProperties';
    this.passdataservice.backpath = backButtonPath;
    // end 
    this.router.navigate(['/resources/mprn-history', mprn]);
  }
  // end of block

  showPropertiesOnMap(element, type: string): void {

    const incidentdata = new PassIncidentData();
    incidentdata.recordid = this.incidentdbid;
    incidentdata.incidentid = this.incidentidformap;
    incidentdata.status = this.status;
    // set object in service for back button
    const backButtonPath = new BackButtonPath<PassIncidentData>();
    console.log('type', type);
    if (type == PathType.WITH_PARAM) {
      console.log('in param functions');
      backButtonPath.pathtype = PathType.WITH_PARAM;
      backButtonPath.pathparams = incidentdata;
    } else {
      backButtonPath.pathtype = PathType.WITHOUT_PARAM;
    }
    backButtonPath.path = '/incident/showProperties';
    this.passdataservice.backpath = backButtonPath;
    console.log(' this.passdataservice.backpath', this.passdataservice.backpath);
    // end 

    const mapdata = new MapData<PropertyListResponse>();
    mapdata.maptype = MAP_TYPE.marker
    mapdata.mapparam = element;
    this.passdataservice.bingMapdata = mapdata;
    this.router.navigate(['/incident/bingmap']).then(
    );
  }
  // function for show incident map
  ShowIncidentMap() {
    const incidentdata = new PassIncidentData();
    incidentdata.recordid = this.incidentdbid;
    incidentdata.incidentid = this.incidentidformap;
    // set object in service for back button
    const backButtonPath = new BackButtonPath<PassIncidentData>();
    backButtonPath.pathtype = PathType.WITH_PARAM;
    backButtonPath.path = 'incident/showProperties';
    backButtonPath.pathparams = incidentdata;
    this.passdataservice.backpath = backButtonPath;
    // end 
    const mapdata = new MapData<IncidentId>();
    mapdata.maptype = MAP_TYPE.boundry;
    const incident = new IncidentId();
    incident.incidentid = this.incidentidformap;
    incident.id = this.incidentdbid;
    mapdata.mapparam = incident;
    this.passdataservice.bingMapdata = mapdata;
    this.router.navigate(['/incident/bingmap']);
  } // end of fucntion.

  // code block for Refresh Button
  refresh(): void {
    this.refreshTable.emit(null);
  }
  // cleanup
  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    if (this.customsearchsubscription) {
      this.customsearchsubscription.unsubscribe();
    }
  }
}
