import { Component, OnInit, ViewChild, NgModule, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AssignedMPRNResponse } from '../../../models/api/resources/assigned-mprn-response';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { GetAllResourcesResponse } from '../../../models/api/resources/get-all-resources-response';
import { ResourcesServiceService } from '../../../services/resources/resources-service.service';
import { forEach } from '@angular/router/src/utils/collection';
import { MPRNAssignmentUnassignment } from '../../../models/api/resources/assign-unassign-mprn-request';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { ApiErrorService } from '../../../services/api-error.service';
import { GetActiveMPRnList } from '../../../models/api/resources/get-active-mprn-response';
import { MPRNIdLists } from '../../../models/api/resources/mprn-id-list';
import { Router } from '@angular/router';
import { MPRNDetails } from '../../../models/api/resources/paas-resources-data';
import { PassdataService } from '../../../services/passdata.service';
import { CustomSearchData, CustomSearchDataMain, SearchFilter } from 'src/app/models/search/custom-search-data';
import { IncidentService } from 'src/app/services/incident.service';
import { IncidentListResponse, PassIncidentData } from 'src/app/models/api/incident.model';
import { APP_HOME, FEED, MAP_TYPE, PathType } from '../../../app-common-constants';
import { MapsService } from '../../../services/maps.service';
import { PassResourceData } from 'src/app/models/api/resources/paas-resources-data';
import { Subscription } from 'rxjs/internal/Subscription';
import { merge, Observable } from 'rxjs';
import { startWith } from 'rxjs/operators';
import { switchMap } from 'rxjs/operators';
import { map } from 'rxjs/operators';
import { FeatureNames } from 'src/app/app-common-constants';
import { LocalstorageService } from 'src/app/services/localstorage.service';
import { BackButtonPath } from 'src/app/models/ui/back-btutton';
import { MapData } from 'src/app/models/api/map/incident-id';




@Component({
  selector: 'app-assign-mprn',
  templateUrl: './assign-mprn.component.html',
  styleUrls: ['./assign-mprn.component.css']
})
export class AssignMprnComponent implements OnInit {

  // displayedColumns = ['select', 'mprn', 'buildingName', 'subBuildingName', 'buildingNumber', 'principalStreet', 'dependentStreet', 'postTown', 'localityName', 'dependentLocality', 'country', 'postcode', 'priorityCustomer', 'zone', 'cell', 'action'];


  displayedColumns = ['select', 'MPRN', 'BuildingName', 'dependentStreet', 'postTown', 'localityName', 'dependentLocality', 'PostCode', 'Zone', 'Cell', 'ZoneManagerName', 'CellManagerName', 'LatestStatus', 'status', 'action'];


  customSearchDataMain = new CustomSearchDataMain();
  dataSource;
  selectedresourcetype: string;
  customSearchData: CustomSearchData[] = [];
  isvisible = false;
  refreshTable: EventEmitter<any> = new EventEmitter();
  resultsLength = 0;
  selected: string[] = [];
  searchFilter: SearchFilter[] = [];
  customsearchsubscription: Subscription;
  feature = FeatureNames;
  selectall: boolean;
  count = 0;
  display = false;
  incidentid: string;
  address = [];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  getActiveMPRnList: GetActiveMPRnList[] = [];
  selectedmprn: GetActiveMPRnList[] = [];
  mprnAssignmentUnassignment: MPRNAssignmentUnassignment[] = [];
  constructor(
    private apiErrorService: ApiErrorService,
    private router: Router,
    private incidentService: IncidentService,
    private passdataservice: PassdataService,
    private resourcesServiceService: ResourcesServiceService,
    private appNotificationService: AppNotificationService,
    public localstorageservice: LocalstorageService
  ) { }

  ngOnInit() {
    this.searchKeyValue();
    this.getIncidentID();


    this.selectedresourcetype = '1';
    //  this.getResources(this.selectedresourcetype); // function call for getting resources
    // code block for  get custom serach data
    this.customsearchsubscription = this.passdataservice.customsearchvalue.subscribe((val) => {
      console.log('custom search data received');
      console.log(val);
      this.searchFilter = val;
      this.refreshTable.emit(null);
    });  //end
    // ==================================== paginator starts =============================
    // set default load parameters
    this.paginator.pageSize = 50;
    this.sort.active = 'MPRN';
    this.sort.direction = 'asc';


    //already commented
    //  this.getActiveMPRNList();
  } // end of nginit
  // pass search key data to custom search
  searchKeyValue(): void {
    this.customSearchData.push({
      searchkeydisplayname: 'MPRN',
      searchkey: 'MPRN',
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
      searchkeydisplayname: 'Dependent Street',
      searchkey: 'DependentStreet',
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
      searchkeydisplayname: 'Zone Manager',
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
        { displaytext: 'No', value: '1' },
      ]
    });
    this.customSearchData.push({
      searchkeydisplayname: 'Assigned',
      searchkey: 'AssignedResourceCount',
      controltype: 'Select',
      controlvalues: [
        { displaytext: 'Yes', value: '0' },
        { displaytext: 'No', value: '1' },
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
      ],
    });

    this.selected.push('Postcode', 'MPRN');
    this.customSearchDataMain.selected = this.selected;
    this.customSearchDataMain.fieldlist = this.customSearchData;
  }// end of fucntion
  // fucntion fot apply filetr
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  //end filter

  //code block for get assined mprn 
  // getActiveMPRNList(): void {
  //   this.resourcesServiceService.getActiveMPRNList(this.incidentid).subscribe((getresourcePayloadresponse) => {
  //     this.getActiveMPRnList = getresourcePayloadresponse;
  //     this.dataSource = new MatTableDataSource<GetActiveMPRnList>(this.getActiveMPRnList);
  //     this.dataSource.sort = this.sort;
  //     this.dataSource.paginator = this.paginator;
  //   });
  // }// end of code block

  //fucntion for view map 
  viewSelctedresourcesMPRN(id) {
    console.log('mprn id', id);
  }
  selectMPRN(id): void {

  }
  //end of fucntion
  assignResources(): void {
    this.selectedmprn = [];
    for (let mprn of this.getActiveMPRnList) {
      if (mprn.checked) {
        this.selectedmprn.push(mprn);
      }
    }
    // console.log('this.mprnAssignmentUnassignment');
    // console.log(mprnarray);
    if (this.selectedmprn.length <= 0) {
      this.appNotificationService.error("Select MPRN To Assign Resource");
      return;
    } else {
      this.passdataservice.selectedmprnllist = this.selectedmprn;
      this.router.navigate(['/resources/assign-mprn-resource'], { skipLocationChange: true });
    }
  }

  // fucntion for view assigned resources to specific mprn
  viewResourcesAssignedToMprn(property: GetActiveMPRnList): void {
    const mprn = new MPRNDetails();
    mprn.propertyid = property.id;
    mprn.mprn = property.mprn;
    this.router.navigate(['/resources/view-assigned-resource-to-mprn', mprn], { skipLocationChange: true });
  }// end of fucntion.

  //fucntiuon for show hide resources
  hideShowSeacrh(): void {
    console.log('button clicked');
    this.isvisible = true;
    this.passdataservice.setOpenSearch(true);

  }
  //end of fucntion

  //Commented by sneha
  // getIncidentList() {
  //   this.incidentService.getIncidentDataList().subscribe(payloadResponse => {
  //     this.incidentid = payloadResponse.filter(incident => incident.status == 0)[0].id;
  //     console.log('this.incidentid',this.incidentid);
  //      this.getActiveMPRNList();
  //   });
  // }

  getIncidentID() {
    console.log("in getActiveMPRNList");
    this.incidentService.getIncidentDataListForMPRN().subscribe(payloadResponse => {
      if (payloadResponse.length > 0) {
        this.incidentid = payloadResponse[0].id;
        console.log("incidentid in GetActiveMPRNList", this.incidentid);
        this.getActiveMprn();
      }
    });
    // 
  }
  getActiveMprn() {

    // If the user changes the sort order, reset back to the first page.
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    this.refreshTable.subscribe(() => this.paginator.pageIndex = 0);
    merge(this.sort.sortChange, this.paginator.page, this.refreshTable)
      .pipe(
      startWith({}),
      switchMap(() => {
        console.log('in switch map');
        console.log(this.sort.active)
        return this.getActiveMPRNList();
      }),
      map(data => {
        // Flip flag to show that loading has finished.     
        return data;
      }),
    ).subscribe(
      activeMPRNPayloadResponse => {
        console.log('response', activeMPRNPayloadResponse);
        console.log('Bank list  Response succeed');
        // this.resultsLength = bankListPayloadResponse.data.filteredrecords;
        //  this.dataSource = bankListPayloadResponse.data.records;
        let i: number;
        let generatedAddress;
        if (activeMPRNPayloadResponse && activeMPRNPayloadResponse !== null) {
          this.resultsLength = activeMPRNPayloadResponse.count;

          this.getActiveMPRnList = activeMPRNPayloadResponse.results;
          this.dataSource = new MatTableDataSource<GetActiveMPRnList>(this.getActiveMPRnList);


          this.getActiveMPRnList = activeMPRNPayloadResponse.results;
          for (let i = 0; i < this.getActiveMPRnList.length && this.getActiveMPRnList.length; i++) {
            this.address = [];
            if (this.getActiveMPRnList[i].buildingNumber.length > 0) {
              this.address.push(this.getActiveMPRnList[i].buildingNumber);
            }
            if (this.getActiveMPRnList[i].buildingName.length > 0) {
              this.address.push(this.getActiveMPRnList[i].buildingName);
            }
            if (this.getActiveMPRnList[i].subBuildingName.length > 0) {
              this.address.push(this.getActiveMPRnList[i].subBuildingName);
            }
            if (this.getActiveMPRnList[i].principalStreet.length > 0) {
              this.address.push(this.getActiveMPRnList[i].principalStreet);
            }
            console.log("address", this.address);
            generatedAddress = "";
            let j: number;

            for (j = 0; j < this.address.length; j++) {
              if (this.address.length - 1 - j > 0) {
                generatedAddress = generatedAddress + this.address[j] + ", ";
              }
              else {
                generatedAddress = generatedAddress + this.address[j];
              }
            }
            this.getActiveMPRnList[i].buildingName = generatedAddress;

            if (this.getActiveMPRnList[i].latestStatus != null && this.getActiveMPRnList[i].latestSubStatus != null && this.getActiveMPRnList[i].latestStatus.length > 0 && this.getActiveMPRnList[i].latestSubStatus.length > 0) {
              this.getActiveMPRnList[i].latestStatus = this.getActiveMPRnList[i].latestStatus + ", " + this.getActiveMPRnList[i].latestSubStatus
            }
          }
        }
        this.dataSource = new MatTableDataSource<GetActiveMPRnList>(this.getActiveMPRnList);

        console.log(this.dataSource);

      });
  }
  // code block for select all
  selectAlll() {
    console.log('select all', this.selectall);
    for (let mprn of this.getActiveMPRnList) {
      mprn.checked = this.selectall;
    }

  }
  //end of block

  getActiveMPRNList(): Observable<any> {

    return this.resourcesServiceService.getActiveMPRNList(this.incidentid, this.paginator.pageIndex, this.paginator.pageSize, this.sort.active, this.sort.direction, this.searchFilter);
    // test code
  }

  // code block for navigating to view mprns history.
  viewMPRNHistory(property: GetActiveMPRnList): void {
    const mprn = new MPRNDetails();
    mprn.propertyid = property.id;
    mprn.mprn = property.mprn;

    // set object in service for back button
    const backButtonPath = new BackButtonPath<null>();
    backButtonPath.pathtype = PathType.WITHOUT_PARAM;
    backButtonPath.path = '/resources/assign-mprn';
    this.passdataservice.backpath = backButtonPath;
    // end 
    console.log('view mprns history');
    this.router.navigate(['/resources/mprn-history', mprn], { skipLocationChange: true });

  }
  //end of code


  showPropertiesOnMap(element): void {

    const incidentdata = new PassIncidentData();
    incidentdata.recordid = element.id;
    incidentdata.incidentid = element.incidentid;
    incidentdata.status = element.status;
    // set object in service for back button
    const backButtonPath = new BackButtonPath<any>();
    backButtonPath.pathtype = PathType.WITHOUT_PARAM;
    backButtonPath.path = '/resources/assign-mprn';
    this.passdataservice.backpath = backButtonPath;
    // end 

    const mapdata = new MapData<GetAllResourcesResponse>();
    mapdata.maptype = MAP_TYPE.marker
    mapdata.mapparam = element;
    this.passdataservice.bingMapdata = mapdata;
    this.router.navigate(['/incident/bingmap']);
  }
  // fucntion for show incident map
  // fucntion for editing properties

  // code block for showProperties.
  showProperties(resource): void {
    console.log(resource);
    const incident = new PassIncidentData();
    incident.incidentid = resource.mprn
    incident.recordid = resource.incidentId;
    incident.propertyid = resource.id;
    incident.status = resource.status;
    incident.urltype = PathType.WITHOUT_PARAM;



    // set object in service for back button
    const backButtonPath = new BackButtonPath<null>();
    backButtonPath.pathtype = PathType.WITHOUT_PARAM;
    backButtonPath.path = '/resources/assign-mprn';
    this.passdataservice.backpath = backButtonPath;
    console.log('passed incident object');
    console.log(incident)
    // end 
    // console.log("In Show Properties",incident)
    this.router.navigate(['incident/updateStatus', incident]);
  }


  // end of function.


  // cleanup
  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    if (this.customsearchsubscription) {
      this.customsearchsubscription.unsubscribe();
    }
  }
  // code block for Refresh Button
  refresh(): void {
    this.refreshTable.emit(null);
  }

}
