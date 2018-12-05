import { Component, OnInit, ViewChild, NgModule } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AssignedMPRNResponse } from 'src/app/models/api/resources/assigned-mprn-response';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { GetAllResourcesResponse } from 'src/app/models/api/resources/get-all-resources-response';
import { ResourcesServiceService } from 'src/app/services/resources/resources-service.service';
import { forEach } from '@angular/router/src/utils/collection';
import { MPRNAssignmentUnassignment } from 'src/app/models/api/resources/assign-unassign-mprn-request';
import { AppNotificationService } from 'src/app/services/notification/app-notification.service';
import { ApiErrorService } from 'src/app/services/api-error.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs/internal/Subscription';
import { CustomSearchData, CustomSearchDataMain } from 'src/app/models/search/custom-search-data';
import { PassdataService } from 'src/app/services/passdata.service';
import { PropertyListResponse } from 'src/app/models/api/properties.model';
import { MapData } from 'src/app/models/api/map/incident-id';
import { MAP_TYPE, PathType, FeatureNames } from 'src/app/app-common-constants';
import { BackButtonPath } from 'src/app/models/ui/back-btutton';
import { PassResourceData } from 'src/app/models/api/resources/paas-resources-data';
import { LocalstorageService } from 'src/app/services/localstorage.service';
import { PassIncidentData } from 'src/app/models/api/incident.model';

@Component({
  selector: 'app-assigned-mprn-to-resources',
  templateUrl: './assigned-mprn-to-resources.component.html',
  styleUrls: ['./assigned-mprn-to-resources.component.css']
})
export class AssignedMprnToResourcesComponent implements OnInit {


  // displayedColumns = ['select', 'mprn', 'buildingName', 'subBuildingName', 'buildingNumber', 'principalStreet', 'dependentStreet', 'postTown', 'localityName', 'dependentLocality', 'country', 'postcode', 'priorityCustomer', 'zone', 'cell', 'action'];
  // displayedColumns = ['select', 'mprn', 'buildingName', 'buildingNumber', 'principalStreet', 'postTown', 'localityName', 'dependentLocality', 'country', 'action'];
  isvisible = false;
  displayedColumns = ['select', 'mprn', 'dependentStreet', 'postTown', 'localityName', 'dependentLocality', 'postcode', 'zone', 'cell', 'ZoneManager', 'CellManager', 'LatestStatus', 'status', 'action'];
  customSearchDataMain = new CustomSearchDataMain();
  selectedresourcetype: string;
  customSearchData: CustomSearchData[] = [];
  selected: string[] = [];
  username: string;
  feature = FeatureNames;
  roleid: string;
  dataSource;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  resourceseemail: string;
  resourceseid: string;
  selectall: boolean;
  selectmprnval: string;
  isinprogress = false;
  paramsubscriptions: Subscription;
  assignedMPRNResponse: AssignedMPRNResponse[] = [];
  mprnAssignmentUnassignment: MPRNAssignmentUnassignment[] = [];
  constructor(
    private activatedRoute: ActivatedRoute,
    private apiErrorService: ApiErrorService,
    private router: Router,
    private passdataservice: PassdataService,
    private resourcesServiceService: ResourcesServiceService,
    private appNotificationService: AppNotificationService,
    public localstorageservice: LocalstorageService
  ) { }

  ngOnInit() {
    this.searchKeyValue();
    // subscription for getting erouting params.
    this.paramsubscriptions = this.activatedRoute.params.subscribe(params => {
      this.resourceseemail = params.email;
      this.resourceseid = params.id;
      this.username = params.username;
      this.roleid = params.roleid;
      console.log('params');
      console.log(params);
    });// end of routing params.
    this.getAssignedMPRNList();
  } // end of ng init
  // pass search key data to custom search
  searchKeyValue(): void {
    // this.customSearchData.push({ searchkey: 'MPRN', searchvalue: 'MPRN' });
    // this.customSearchData.push({ searchkey: 'Dependent Street', searchvalue: 'Dependent Street' });
    // this.customSearchData.push({ searchkey: 'Post Town', searchvalue: 'Post Town' });
    // this.customSearchData.push({ searchkey: 'Locality Name', searchvalue: 'Locality Name' });
    // this.customSearchData.push({ searchkey: 'Dependent Locallity', searchvalue: 'Dependent Locallity' });
    // this.customSearchData.push({ searchkey: 'Post Code', searchvalue: 'Post Code' });

    this.selected.push('Post Code', 'MPRN');
    this.customSearchDataMain.selected = this.selected;
    this.customSearchDataMain.fieldlist = this.customSearchData;
  }// end of fucntion


  // fucntion fot apply filetr
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  //end filter

  //code block for get assined mprn 
  getAssignedMPRNList(): void {
    this.resourcesServiceService.getAssinedMPRNList(this.resourceseemail, this.roleid).subscribe((getresourcePayloadresponse) => {
      this.assignedMPRNResponse = getresourcePayloadresponse;
      this.dataSource = new MatTableDataSource<AssignedMPRNResponse>(this.assignedMPRNResponse);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    });
  }// end of code block

  //fucntion for view map 
  viewSelctedresourcesMPRN(id) {
    console.log('mprn id', id);
  }
  selectMPRN(id): void {

  }
  //end of fucntion
  assignSelected(): void {
    this.mprnAssignmentUnassignment = [];
    for (let mprn of this.assignedMPRNResponse) {
      if (mprn.checked) {
        const mprnobject = new MPRNAssignmentUnassignment();
        mprnobject.IsUnAssign = true;
        mprnobject.UserId = this.resourceseid;
        mprnobject.PropertyId = mprn.id;
        mprnobject.RoleId = this.roleid;
        this.mprnAssignmentUnassignment.push(mprnobject);
      }
    }
    console.log('this.mprnAssignmentUnassignment');
    console.log(this.mprnAssignmentUnassignment);
    if (this.mprnAssignmentUnassignment.length <= 0) {
      this.appNotificationService.error("Select MPRN To Unassign");
      return;
    }
    console.log('mprn unassignment array of objects');
    console.log(this.mprnAssignmentUnassignment);
    this.isinprogress = true;
    this.resourcesServiceService.assignUnassignresources(this.mprnAssignmentUnassignment).subscribe(
      response => {
        this.isinprogress = false;
        this.router.navigate(['/resources/all-resources']).then(this.appNotificationService.success('MPRN Unassigned Successfully!'));
      },
      (error) => {
        this.isinprogress = false;
        this.apiErrorService.handleError(error);
      });

  } // end of fucntion.


  //fucntiuon for show hide resources
  hideShowSeacrh(): void {
    console.log('button clicked');
    this.isvisible = true;
    this.passdataservice.setOpenSearch(true);

  }
  //end of fucntion

  // code block for show map of mprn
  showPropertiesOnMap(element): void {
    const resource = new PassResourceData();
    resource.email = this.resourceseemail;
    resource.id = this.resourceseid;
    resource.username = this.username;
    resource.roleid = this.roleid;
    // set object in service for back button
    const backButtonPath = new BackButtonPath<PassResourceData>();
    backButtonPath.pathtype = PathType.WITH_PARAM;
    backButtonPath.path = 'resources/assignedmprn';
    backButtonPath.pathparams = resource;
    this.passdataservice.backpath = backButtonPath;
    // end 
    const mapdata = new MapData<PropertyListResponse>();
    mapdata.maptype = MAP_TYPE.marker
    mapdata.mapparam = element;
    this.passdataservice.bingMapdata = mapdata;
    this.router.navigate(['/incident/bingmap'], { skipLocationChange: true });
  }


  // code block for UpdateStatus.
  updateStatus(resource): void {

    const data = new PassResourceData();
    data.email = this.resourceseemail;
    data.id = this.resourceseid;
    data.username = this.username;
    data.roleid = this.roleid;
    // set object in service for back button
    const backButtonPath = new BackButtonPath<PassResourceData>();
    backButtonPath.pathtype = PathType.WITH_PARAM;
    backButtonPath.path = 'resources/assignedmprn';
    backButtonPath.pathparams = data;
    this.passdataservice.backpath = backButtonPath;
    // end 


    const incident = new PassIncidentData();
    incident.incidentid = resource.mprn
    incident.recordid = resource.incidentId;
    incident.propertyid = resource.id;
    // properties availabel in this page alwayes having status 0 now in this json response staus
    // is not available to  as per the disscussion with abhjit pass staus as 0 if not available
    if (resource.status == null || resource.status == undefined) {
      incident.status = 0;
    } else {
      incident.status = resource.status;
    }

    incident.urltype = PathType.WITH_PARAM;

    //    // set object in service for back button
    // const backButtonPath = new BackButtonPath<null>();
    // backButtonPath.pathtype = PathType.WITHOUT_PARAM;
    // backButtonPath.path = 'resources/assignedmprn';
    // this.passdataservice.backpath = backButtonPath;
    console.log('passed incident object');
    console.log(incident)
    this.router.navigate(['incident/updateStatus', incident]);
  }
  // code block for select all
  selectAlll() {
    console.log('select all', this.selectall);
    for (let mprn of this.assignedMPRNResponse) {
      mprn.checked = this.selectall;
    }

  }
  //end of block

  // code block for distroying data
  ngOnDestroy(): void {
    if (this.paramsubscriptions) { this.paramsubscriptions.unsubscribe; }
  }
  //end of code block

} // end of class
