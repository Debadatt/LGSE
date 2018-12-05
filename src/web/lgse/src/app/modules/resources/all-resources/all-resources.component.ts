import { Component, OnInit, ViewChild, NgModule, EventEmitter } from '@angular/core';
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
import { FeatureNames, MAP_TYPE } from 'src/app/app-common-constants';

import { BackButtonPath } from 'src/app/models/ui/back-btutton';
import { PathType } from '../../../app-common-constants';
import { PassIncidentData } from 'src/app/models/api/incident.model';
import { MapData } from 'src/app/models/api/map/incident-id';
import { PropertyListResponse } from 'src/app/models/api/properties.model';
import { ApiErrorService } from 'src/app/services/api-error.service';

@Component({
  selector: 'app-all-resources',
  templateUrl: './all-resources.component.html',
  styleUrls: ['./all-resources.component.css']
})
export class AllResourcesComponent implements OnInit {
  @ViewChild(MatPaginator)
  paginator: MatPaginator;
  @ViewChild(MatSort)
  sort: MatSort;
  // process varibales 
  getAllResourcesResponse: GetAllResourcesResponse[] = [];
  //displayedColumns = ['name'];
  displayedColumns = ['FirstName', 'AssignedMPRNCount', 'Roles', 'Zones', 'Cells', 'Completed', 'action'];
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
  feature = FeatureNames;
  // end of process variables
  constructor(
    private resourcesServiceService: ResourcesServiceService,
    private router: Router,
    private apiErrorService: ApiErrorService,
    public localstorageservice: LocalstorageService,
    private passdataservice: PassdataService
  ) {

  }
  ngOnInit() {
    this.searchKeyValue();
    this.selectedresourcetype = '1';
    //  this.getResources(this.selectedresourcetype); // function call for getting resources
    // code block for  get custom serach data
    this.customsearchsubscription = this.passdataservice.customsearchvalue.subscribe((val) => {
      console.log('custom search data received');
      console.log(val);
      this.searchFilter = val;
      this.refreshTable.emit(null);
    })  //end
    // ==================================== paginator starts =============================
    // set default load parameters
    this.paginator.pageSize = 50;
    this.sort.active = 'FirstName';
    this.sort.direction = 'asc';

    // If the user changes the sort order, reset back to the first page.
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    this.refreshTable.subscribe(() => this.paginator.pageIndex = 0);
    merge(this.sort.sortChange, this.paginator.page, this.refreshTable)
      .pipe(
      startWith({}),
      switchMap(() => {
        console.log('in switch map');
        console.log(this.sort.active)
        return this.getResources();
      }),
      map(data => {
        // Flip flag to show that loading has finished.     
        return data;
      }),
    ).subscribe(
      resourcePayloadResponse => {
        console.log('response', resourcePayloadResponse);
        // this.resultsLength = bankListPayloadResponse.data.filteredrecords;
        //  this.dataSource = bankListPayloadResponse.data.records;
        if (resourcePayloadResponse && resourcePayloadResponse !== null) {
          this.resultsLength = resourcePayloadResponse.count;
          this.dataSource = new MatTableDataSource<GetAllResourcesResponse>(resourcePayloadResponse.results);
          console.log(this.dataSource);
        }
      }, (error) => {
        this.apiErrorService.handleError(error);
      });
  } // end of ng init

  getResources(): Observable<any> {
    console.log('get resources function called');
    return this.resourcesServiceService.getResourceList(this.paginator.pageIndex, this.paginator.pageSize, this.sort.active, this.sort.direction, this.searchFilter);
  }
  // fucntion fot apply filetr
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  //end filter
  // pass search key data to custom search
  searchKeyValue(): void {
    this.customSearchData.push({
      searchkeydisplayname: 'First Name',
      searchkey: 'FirstName',
      controltype: 'input',
      controlvalues: null
    });
    this.customSearchData.push({
      searchkeydisplayname: 'Last Name',
      searchkey: 'LastName',
      controltype: 'input',
      controlvalues: null
    });
    this.customSearchData.push({
      searchkeydisplayname: 'Status',
      searchkey: 'IsAssigned',
      controltype: 'Select',
      controlvalues: [
        { displaytext: 'All Logged In', value: '' },
        { displaytext: 'Assigned', value: '1' },
        { displaytext: 'Not Assigned', value: '2' }
      ]
    });
    const selectData = ['FirstName', 'LastName'];

    this.customSearchDataMain.selected = selectData;
    this.customSearchDataMain.fieldlist = this.customSearchData;
  }// end of fucntion


  // code block for view MPRN of selcted resources
  viewSelctedresourcesMPRN(resource: GetAllResourcesResponse): void {
    const passdata = new PassResourceData();
    passdata.email = resource.email;
    passdata.id = resource.id;
    passdata.roleid = resource.preferredRoleId;
    const usrname = resource.firstName + " " + resource.lastName;
    passdata.username = usrname;
    this.router.navigate(['resources/assignedmprn', passdata], { skipLocationChange: true });
    // this.router.navigate(['incident/showProperties', "'" + id + "'"], { skipLocationChange: true });
  }
  //end of code block
  //fucntiuon for show hide resources
  hideShowSeacrh(): void {
    console.log('button clicked');
    this.isvisible = true;
    this.passdataservice.setOpenSearch(true);

  }//end of fucntion

  // cleanup
  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    if (this.customsearchsubscription) {
      this.customsearchsubscription.unsubscribe();
    }
  }// end of fucntion

  // code block for redirecting to user details page 
  portalUserDetails(id) {
    const backButtonPath = new BackButtonPath<null>();
    backButtonPath.pathtype = PathType.WITHOUT_PARAM;
    backButtonPath.path = '/resources/all-resources';
    this.passdataservice.backpath = backButtonPath;
    this.router.navigate(['/portalManagment/editUser', id]);
  }
  // code block for Refresh Button
  refresh(): void {
    this.refreshTable.emit(null);
  }
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
